using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveScript : MonoBehaviour
{

    #region publicClassObj
    public AudioSource audio;
    public GameObject Sourse;
    public AudioClip Step;
    public GameObject Head;
    #endregion

    #region classObj
    CharacterController character;
    #endregion

    #region publicVars
    public static float moveSpeed = 3;
    
    public bool isMove = false;
    public bool isSnake = false;
    #endregion

    #region vars
    const float deltaStamina = 0.1f;
    const float runSpeed = 7;
    const float walkSpeed = 4;
    const float deltaSpeed = 0.75f;
    const float gravityDelta = 100;
    
    
    float sideStepsSpeed = 0.2f;
    float zMove = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Head = GameObject.Find("Head");
        Sourse = GameObject.Find("PlayerSteps");
        character = this.GetComponent<CharacterController>();
        audio = GameObject.Find("PlayerSteps").GetComponent<AudioSource>();
        Step = GameObject.Find("PlayerSteps").GetComponent<AudioClip>();
    }

    void Update()
    {

        if (isMove == false)
        {
            Sourse.SetActive(false);
        }
        else
        {
            Sourse.SetActive(true);
        }
        SnakeMode();
        Move();
    }

    void Move() 
    {
        //We can run if hero isnt tired
        if(StaminaController.isHeroTired == false) 
        {
            Running();
        }

        //шаги в бок должны быть медленнее ,чем обычный шаги
        sideStepsSpeed = moveSpeed / 3 * 2.5f;


        //MoveHero
        float translationZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float translationX = Input.GetAxis("Horizontal") * sideStepsSpeed * Time.deltaTime;
        character.Move(Vector3.ProjectOnPlane(Aiming.forwardTransformDirection, Vector3.up) * translationZ);
        character.Move(Vector3.ProjectOnPlane(Aiming.rightTransformDirection, Vector3.up) * translationX);
        isMove = true;

        if (isMove == true)
        {
            Sourse.SetActive(true);
        }
        else
        {
            Sourse.SetActive(false);
        }

        //Gravity
        character.Move(Vector3.down * gravityDelta * Time.deltaTime);
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }
    }

    void SnakeMode()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            moveSpeed = 1.5f;
            isSnake = true;
            Head.transform.Translate(Vector3.up * -1f);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            moveSpeed = 3;
            isSnake = false;
            Head.transform.Translate(Vector3.up * 1f);
        }   
    }
    void Running() 
    {
        bool moving = false;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) //если игрок не стоит на месте
            moving = true;
        else
            moving = false;

        if (Input.GetAxis("Run") != 0 && moving)//если мы нажали lShift, то мы ускоряемся
        {
            audio.pitch = 2;
            StaminaController.isRunning = true;
            if (moveSpeed < runSpeed)
            {
                moveSpeed += deltaSpeed * Time.deltaTime;
            }
            else
            {
                moveSpeed = runSpeed;
            }

        }
        else//если игрок не собирается бежать, то мы приравниваем скорость передвижения к константной скорости ходьбы
        {
            audio.pitch = 1;
            StaminaController.isRunning = false;
            if (moveSpeed < walkSpeed)
            {
                moveSpeed += deltaSpeed * Time.deltaTime;
            }
            if (moveSpeed > walkSpeed)
            {
                moveSpeed -= deltaSpeed * 1.5f * Time.deltaTime;
            }
        }
    }
}
