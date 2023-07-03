using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SideOfDetection {left, right, middle, middleExtra, up, down }

public enum AnimalBehaviour { walking, running, playing, hunting, backToHome}

public class AnimalScript : MonoBehaviour
{
    #region ClassObj for inspector
    [SerializeField] HitDetection[] HitDetectors;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject LootController;
    [SerializeField] GameObject LootWindowPref;
    #endregion

    #region ClassObj
    Animator animator;
    Transform playerTransform;
    #endregion

    #region publicMasssives
    public string[] startLoot;
    public int[] startLoot_numberOfElements;

    public List<(string, int)> loot = new List<(string, int)>();

    #endregion

    #region constVars
    const float walkSpeed = 2;
    const float speedWhileRotate = 1;
    const float remainingRotTimerConst = 0.25f;
    #endregion

    #region publicVars
    public AnimalBehaviour behaviour = AnimalBehaviour.walking;

    public float HP_animal = 1;

    [HideInInspector] public int down = 0;
    [HideInInspector] public int up = 0;
    [HideInInspector] public int middle = 0;
    [HideInInspector] public int middleExtra = 0;
    [HideInInspector] public int leftCount = 0;
    [HideInInspector] public int rightCount = 0;

    public bool remainingRot = false;
    public bool kill = false;
    #endregion

    #region vars
    float moveSpeed = 2;
    float rotateSpeed = 20;
    float remainingRotTimer = 0.5f;
    float sideOfRotChange_cooldownTimer = 0;
    float animationTimer = 0;

    int sideOfRot = 0;
    int indexOfArrows = -1;

    bool dead = false;
    bool lootAdded = false;
    #endregion

    void Start()
    {
        animator = this.GetComponent<Animator>();
        playerTransform = Player.transform;
        remainingRotTimer = remainingRotTimerConst;

        for(int i = 0; i < startLoot.Length; i++) 
        {
            loot.Add((startLoot[i], startLoot_numberOfElements[i]));
        }
    }

    void Update()
    {
        //зона для тестов
        if (Input.GetKeyDown(KeyCode.K))
            kill = true;

        if (kill)
            HP_animal = -100f;
        //

        sideOfRotChange_cooldownTimer -= Time.deltaTime;///кулдаун смены стороны поворота, чтобы она не менялась слишком часто

        if (HP_animal <= 0)//смерть если хп меньше 0
            Death();
        else //если изверь жив
        {
            //only if player is enough far away. Reason of this shit - optimization
            if (Vector3.Distance(transform.position, playerTransform.position) < 300)
            {
                //определяем поведение жвиотного
                if (behaviour == AnimalBehaviour.walking)
                    BehaviourMode_Walking();
                //else if()

                //moving
                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
                
            }
        }
    }

    /// <summary>
    /// режим спокойного передвижения животного
    /// </summary>
    void BehaviourMode_Walking() 
    {
        /*if (down < 0)
        {
            DetermineSideOfRot("walk");
            RotateAnimal(rotateSpeed * 3, 0);
        }*/
        

        if(middleExtra == 0 && up == 0 && middle == 0 && leftCount == 0 && rightCount == 0) //если у нрас нет никаких препятствий впереди
        {
            RemainingAnimalRot();
            moveSpeed = walkSpeed;
            animator.Play("run");
        }
        else if(middleExtra != 0) //если нужно экстренно повернуть (слишком близко препятствие)
        {
            //RemainingAnimalRot();
            RandomiseSideOfRot("walk");
            RotateAnimal(sideOfRot * rotateSpeed * 4, 0.1f);
        }
        else if (up != 0)//если сверху довольно близко находится высокое препятствие
        {
            //RemainingAnimalRot();
            RandomiseSideOfRot("walk");
            RotateAnimal(sideOfRot * rotateSpeed * 3, 0);
        }
        else if (middle != 0)//если прямо "по курсу" препятствие
        {
            //RemainingAnimalRot();
            RandomiseSideOfRot("walk");
            RotateAnimal(sideOfRot * rotateSpeed * 2, speedWhileRotate / 2);
            
        }
        else if (leftCount != 0 || rightCount != 0)//если препятсвие слева или справа
        {
            //RemainingAnimalRot();
            if (sideOfRot == 0)//если еще не определили куда поворачивать
            {
                animator.Play("walk");

                if (leftCount > rightCount)//если сигналов о препятствии слева больше, то поворот направо
                    sideOfRot = 1;
                else if (rightCount > leftCount)//если сигналов о препятствии справа больше, то поворачиваем налево
                    sideOfRot = -1;
            }
            RotateAnimal(sideOfRot * rotateSpeed, speedWhileRotate);//в итоге мы поворачиваем
        }
    }

    /// <summary>
    /// остаточное вращение животного после поворота
    /// </summary>
    void RemainingAnimalRot() 
    {
        if (sideOfRot != 0)
        {
            remainingRotTimer -= Time.deltaTime;
            if (remainingRotTimer > 0)
                RotateAnimal(sideOfRot * rotateSpeed, speedWhileRotate);
            else
            {
                //remainingRot = false;
                remainingRotTimer = remainingRotTimerConst;
                if(sideOfRotChange_cooldownTimer <= 0)
                sideOfRot = 0;
            }
        }
    }

    /// <summary>
    ///сторону поворота животного определеяем на рандом
    /// </summary>
    /// <param name="animation"></param>
    void RandomiseSideOfRot(string animation) 
    {
        //Debug.Log(sideOfRot);
        if(sideOfRot == 0 && sideOfRotChange_cooldownTimer <= 0) 
        {
            animator.Play(animation);
            sideOfRotChange_cooldownTimer = 5;

            sideOfRot = Random.Range(0, 10);
            if (sideOfRot < 5)
                sideOfRot = -1;
            else
                sideOfRot = 1;
        }
    }

    /// <summary>
    /// что тут можно еще сказать. Смэрт
    /// </summary>
    void Death() 
    {
        //RotateAnimal(1, 2);
        if(dead == false) 
        {
            /*foreach (var detector in HitDetectors) 
            {
                int arrowsInDetector = detector.ArrowsContainer.childCount;

                for (int i = 0; i < arrowsInDetector; i++) 
                {
                    Destroy(detector.ArrowsContainer.GetChild(i).gameObject);
                }
            }*/

            animator.Play("die");
            //Score.GetCurrentScore().AddScore(10);

            dead = true;
        }

        animationTimer += Time.deltaTime;

        if(animationTimer > 1.667f && !lootAdded) 
        {
            
            GameObject newLootWind = Instantiate(LootWindowPref);
            newLootWind.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);

            LootController.SetActive(true);
            LootInv lootInv = LootController.GetComponent<LootInv>();
            lootInv.LootWindow = newLootWind;
            Transform lookCanvas = newLootWind.transform.GetChild(0);
            lootInv.IndArea = lookCanvas.GetChild(0).gameObject.GetComponent<RectTransform>();
            lootInv.IndFillArea = lookCanvas.GetChild(1).gameObject.GetComponent<RectTransform>();
            lootInv.IndStartArea = lookCanvas.GetChild(2).gameObject.GetComponent<RectTransform>();
            if(lootInv.lastString == null)
                lootInv.lastString = newLootWind.transform.GetChild(newLootWind.transform.childCount - 1);

            Debug.Log(lootInv.lastString.gameObject.name);

            foreach ((string, int) element in loot)
            {
                lootInv.AddLootElement(element.Item1, element.Item2);
            }

            lootAdded = true;
            enabled = false;
        }
    }

    /// <summary>
    /// Эта функция позволяет вращать животное ы
    /// </summary>
    /// <param name="speedOfRot">скорость вращения</param>
    /// <param name="forwardMoveSpeed">скорость, с которой будет двигаться животное во время вращения</param>
    void RotateAnimal(float speedOfRot, float forwardMoveSpeed) 
    {
        moveSpeed = forwardMoveSpeed;
        transform.Rotate(0, speedOfRot * Time.deltaTime, 0);
    }

    /// <summary>
    /// добавить стрелы в лут с животоного
    /// </summary>
    /// <param name="number">количество стрел</param>
    public void AddArrowToLoot(int number) 
    {
        if(indexOfArrows != -1) 
        {
            loot[indexOfArrows] = (loot[indexOfArrows].Item1, loot[indexOfArrows].Item2 + number);
        }
        else
        {
            loot.Add(("arrow", number));
            indexOfArrows = loot.Count;
        }   
    }
}
