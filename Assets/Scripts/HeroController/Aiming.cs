using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


public class Aiming : MonoBehaviour
{
    #region postProcessing
    [SerializeField] PostProcessVolume thisPostProcessVolume;

    LensDistortion lensDistortion;
    DepthOfField depthOfField;
    #endregion

    #region ClassObj
    Transform scopeTarget;
    Transform headTransform;
    #endregion

    #region publicClassObj
    [SerializeField] GameObject BowObj;
    [Space]
    //
    [Header("Start arrow elements")]
    [SerializeField] GameObject CurrentArrowWeight;
    [SerializeField] GameObject CurrentArrow;
    [Space]
    //
    [Header("Prefabs of bow elements")]
    [SerializeField] GameObject ArrowPref;
    [SerializeField] GameObject ArrowWeightPref;
    [Space]
    //
    [Header("Postions for arrow elements")]
    [SerializeField] Transform ArrowWeightPattern;
    [SerializeField] Transform ArrowPattern;
    [SerializeField] Transform ArrowWeightPattern_start;
    [SerializeField] Transform ArrowPattern_start;
    [SerializeField] Transform ArrowWeightPattern_pulled;
    [SerializeField] Transform ArrowPattern_pulled;
    [Space]
    //
    [SerializeField] RectTransform scopeCircle_rectTransform;

    public MoveScript moveScript;

    //[HideInInspector] public GameObject LookingObj;
    //[HideInInspector] public List<GameObject> NearestObjects = new List<GameObject>();
    #endregion

    #region vars
    const float maxBowPower = 50;
    
    float maxScopeCircleSize = 350;
    float minScopeCircleSize = 35;
    float currentScopeCircleSize;

    float bowPower = 0;
    float distToAimingTarget = 5;
    float maxFocalLength = 1;

    float sensitivity = 3;

    float rotationX = 0;
    float rotationY = 0;

    float maxHeadY = 50;
    float minHeadY = -50;

    float shootCooldown = 0;

    bool scoping = false;
    bool realised = false;
    bool currentFocalLenghtIsMax = false;
    bool gamepadScoping = false;

    Vector2 arrowPatternPos_start;
    Vector2 arrowPatternPos_pulled;
    RaycastHit hit;
    #endregion

    #region publicVars
    public static Vector3 forwardTransformDirection;
    public static Vector3 rightTransformDirection;
    public static bool fixedMouse = false;
    //public static bool Entered

    public bool castRay = false;
    #endregion

    //List<>

    //Vector3 aimTarget;

    void Start()
    {
        //запоминаем параметры постобработки
        thisPostProcessVolume.profile.TryGetSettings(out lensDistortion);
        thisPostProcessVolume.profile.TryGetSettings(out depthOfField);
        //нужный для определения фокусного расстояние объект, в сторону которого мы будем запускать луч
        scopeTarget = transform.GetChild(transform.childCount - 1);
        //объект головы
        headTransform = this.transform;
        //определяем размеры для нашего прицела, чтобы не было проблем с адаптивностью
        maxScopeCircleSize = scopeCircle_rectTransform.rect.height;
        minScopeCircleSize = maxScopeCircleSize / 3;
    }

    void Update()
    {
        if (!GameUIContr.paused) 
        {
            //timer of activation of new arrow
            if (realised == true)
            {

                shootCooldown += Time.deltaTime;

                if (shootCooldown <= 1)
                {
                    //возвращаем прицел и параметры пост обработки к начальным параметрам
                    ChangeScopeSize(currentScopeCircleSize, maxScopeCircleSize, shootCooldown);
                    lensDistortion.intensity.value = Mathf.Lerp(40, 0, shootCooldown);
                    depthOfField.focalLength.value = Mathf.Lerp(maxFocalLength, 1, shootCooldown);
                    if (depthOfField.focusDistance.value > 100)
                        depthOfField.focusDistance.value = 100;
                }
                else if (shootCooldown > 3)
                {
                    //активиурем новую стрелу и обнуляем параметры кулдауна
                    currentFocalLenghtIsMax = false;
                    realised = false;
                    shootCooldown = 0;
                    CurrentArrowWeight.SetActive(true);
                    CurrentArrow.SetActive(true);
                }
            }
            else
            {
                AimWithBow();
            }

            //Mode of fixed camera 
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (fixedMouse == false)
                    fixedMouse = true;
                else
                    fixedMouse = false;
            }

            if (fixedMouse == false)
                MouseAiming();

            //Блок кода, отвечающий за выпуск лучей raycast
            if(scoping || LootWindowContainer.NearestObj.Count > 0) //условие для выпуска лучей - либо прицеливание, либо нахождение рядом с объектами лута (это нужно системе лута для того, чтобы не подбирать те предметы ,на которые игрок не смотрит)
            {
                hit = Raycasting();//выпускаем луч с помощью специальной функции и запоминаем hit-объекты

                if (LootWindowContainer.NearestObj.Count > 0 && hit.collider != null) //если рядом объекты лута и при этом игрок на что-то смотрит
                {
                    //то мы перебираем все находящиеся рядом объекты и проверяем: не на них ли смотрит игрок
                    foreach (GameObject obj in LootWindowContainer.NearestObj)
                    {
                        if (hit.collider.name == obj.name)
                        {
                            LootWindowContainer.LookingObj = hit.collider.gameObject;
                        }
                    }

                    //перепроверяем смотрит ли еще игрок на тот же лут-объект
                    if (LootWindowContainer.LookingObj != hit.collider.gameObject)
                        LootWindowContainer.LookingObj = null;
                }
                
            }
            else 
            {
                //если мы уже не находимся рядом с лут-объектами, то мы должны обнулить переменную LookingObj
                if (LootWindowContainer.LookingObj != null)
                    LootWindowContainer.LookingObj = null;
            }
        }
        
    }

    /// <summary>
    /// выпуск луча. возвращаемое значение - RaycastHit
    /// </summary>
    RaycastHit Raycasting() 
    {
        RaycastHit hit;
        Vector3 rayDirection = headTransform.TransformDirection(Vector3.forward);
        Ray ray = new Ray(headTransform.position, rayDirection);
        

        Physics.Raycast(ray, out hit);
        return hit;
    }

    void DetermineDistToCurrentTarget() 
    {
        /*выпускаем луч, и если объект, с которым он столкнется, не будет являться землей(террэином), 
        то тогда мы запоминанем дистанцию до этого объекта, чтобы потом приравнять этой дистанции фокусное расстояние*/
        //RaycastHit hit = Raycasting();

        if (hit.collider && hit.collider.gameObject.tag != "terrain")
        {
            distToAimingTarget = Vector3.Distance(headTransform.position, hit.collider.transform.position) - 1;
            //focal lenght should depend by this distance
            maxFocalLength = distToAimingTarget * 7;
            //aimTarget = hit.collider.transform.position;
            //hit.collider.transform.position = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + 10, hit.collider.transform.position.z);

            if (distToAimingTarget < 50) 
            {
                if (maxFocalLength > 100)
                    maxFocalLength = 100;
            }
            else 
            {
                if (maxFocalLength > 150)
                    maxFocalLength = 150;
            }
        }
    }

    void AimWithBow() 
    {
        if (BowObj.activeInHierarchy) //если лук в руках
        {
            //стрела и грузило должны быть активны
            CurrentArrow.SetActive(true);
            CurrentArrowWeight.SetActive(true);

            if (scoping == true) //если ЛКМ была нажата
            {
                //если герой не устал, то мы увеличиваем силу выстрела
                if (StaminaController.isHeroTired == false)
                {
                    //определяем дистанцию от игрока до объекта, на который он в этотм момент нацелен
                    DetermineDistToCurrentTarget();

                    if (bowPower < 3)
                    {
                        StaminaController.isAiming = true;
                        bowPower += Time.deltaTime * 0.35f;


                        //размер прицела нужно менять в зависимости от силы
                        ChangeScopeSize(maxScopeCircleSize, minScopeCircleSize, bowPower);
                        //натянутость тетивы
                        ArrowPattern.position = Vector3.Lerp(ArrowPattern_start.position, ArrowPattern_pulled.position, bowPower);
                        ChangeArrowPosToPatternPos();
                        //вибрация геймпада
                        if (bowPower < 1 && XInputControl.connected)
                            XInputControl.Vibrate(bowPower / 3, bowPower / 3);// / 3, bowPower / 3);
                                                                              //а также мы меняем некоторые эффекты пост обработки
                        lensDistortion.intensity.value = Mathf.Lerp(0, 40, bowPower);
                        depthOfField.focusDistance.value = distToAimingTarget;
                        if (currentFocalLenghtIsMax)//focal lenght должен меняться мгновенно, если игрок уже достаточно долго целится
                            depthOfField.focalLength.value = maxFocalLength;
                        else
                            depthOfField.focalLength.value = Mathf.Lerp(1, maxFocalLength, bowPower);
                        //we should to control focal lenght. If it equals maximum, we must remember that for moment of changing focus object
                        if (depthOfField.focalLength.value > maxFocalLength - 1)
                            currentFocalLenghtIsMax = true;
                        else
                            currentFocalLenghtIsMax = false;
                    }

                    //если отпустили ЛКМ, то выстреливаем
                    if (Input.GetAxis("RightTrigger") == 0 && gamepadScoping)
                    {
                        gamepadScoping = false;
                        RealiseArrow();
                    }


                    if (Input.GetMouseButtonUp(0))
                        RealiseArrow();
                }
                else
                {
                    //но если герой устал, то мы выстреливаем моментально, отнимая при этом еще больше стамины
                    StaminaController.stamina -= 10;
                    RealiseArrow();
                }
            }
            else
            {
                if (Input.GetAxis("RightTrigger") != 0)
                {
                    scoping = true;
                    gamepadScoping = true;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    scoping = true;
                }
            }
        }
        else 
        {
            CurrentArrow.SetActive(false);
            CurrentArrowWeight.SetActive(false);
        }
    }

    /// <summary>
    /// функция изменения размеров прицела
    /// </summary>
    /// <param name="max">максимальный размер прицела</param>
    /// <param name="min">минимальный размер прицела</param>
    /// <param name="progress">прогресс изменения размеров</param>
    void ChangeScopeSize(float max, float min, float progress) 
    {
        float scopeCircleSize_Var = Mathf.Lerp(max, min, progress);
        scopeCircle_rectTransform.sizeDelta = new Vector2(scopeCircleSize_Var, scopeCircleSize_Var);
    }

    void MouseAiming() 
    {
        //get mouse or stick movement
        float xInput = Input.GetAxis("Mouse X");
        if (xInput == 0)
            xInput = Input.GetAxis("HorizontalRstick") * 0.1f;
        float yInput = Input.GetAxis("Mouse Y");
        if (yInput == 0)
            yInput = Input.GetAxis("VerticalRstick") * 0.1f;

        xInput = xInput * Time.deltaTime * 150;
        yInput = yInput * Time.deltaTime * 150;

        //rotate head
        rotationX = headTransform.localEulerAngles.y + sensitivity * xInput;
        rotationY += yInput * sensitivity;
        rotationY = Mathf.Clamp(rotationY, minHeadY, maxHeadY);
        headTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

        //assign transform direction for player depended by head rotation
        forwardTransformDirection = headTransform.TransformDirection(Vector3.forward);
        rightTransformDirection = headTransform.TransformDirection(Vector3.right);

        //переставляем стрелу и "грузило" на изменившийся arrowPos
        ChangeArrowPosToPatternPos();
    }

    void ChangeArrowPosToPatternPos() 
    {
        CurrentArrowWeight.transform.position = ArrowWeightPattern.position;
        CurrentArrow.transform.position = ArrowPattern.position;
        CurrentArrow.transform.rotation = ArrowPattern.rotation;
    }

    void RealiseArrow() 
    {
        Rigidbody arrowWeight_rb = CurrentArrowWeight.GetComponent<Rigidbody>();
        float shootPower = Mathf.Lerp(0, maxBowPower, bowPower);

        //передаем выпускаемой стреле и грузу информацию о выстреле
        ArrowScript currentArrowScript = CurrentArrow.GetComponent<ArrowScript>();
        currentArrowScript.realised = true;
        currentArrowScript.bowPower = shootPower;
        CurrentArrowWeight.GetComponent<ArrowWeightScipt>().released = true;

        //определяем velocity для стрелы в зависимости от силы натяжения лука и от направления, куда смотрит наша голова
        Vector3 forwardDirection = headTransform.TransformDirection(Vector3.forward);
        Vector3 arrow_vel = new Vector3(forwardDirection.x * shootPower, forwardDirection.y * shootPower, forwardDirection.z * shootPower);

        //активируем действие физики на "грузиле" и запускаем его
        arrowWeight_rb.velocity = arrow_vel;
        arrowWeight_rb.isKinematic = false;

        //запоминаем и обнуляем всю информацию
        StaminaController.isAiming = false;
        scoping = false;
        realised = true;
        bowPower = 0;
        currentScopeCircleSize = scopeCircle_rectTransform.rect.height;
        XInputControl.Vibrate(bowPower, bowPower);

        //создаем новую стрелу и ее грузило, но игрок их пока что не должен видеть
        CurrentArrow = Instantiate(ArrowPref);
        CurrentArrow.SetActive(false);
        CurrentArrowWeight = Instantiate(ArrowWeightPref);
        CurrentArrowWeight.SetActive(false);
        CurrentArrow.GetComponent<ArrowScript>().ThisArrowWeight = CurrentArrowWeight.transform;

        //возвращаем тетиву на место
        ArrowPattern.position = ArrowPattern_start.position;
    }
}
