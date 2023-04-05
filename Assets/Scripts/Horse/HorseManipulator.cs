using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseManipulator : MonoBehaviour,I_Interact
{
    // Start is called before the first frame updat
    public bool isRide = false;
    public GameObject Hero;
    public GameObject TriggerRide;
    public float HorseSpeed = 0.12f;
    public float HorseRunSpeed = 0.25f;
    public CharacterController _characterController;
    public float SpeedRotation = 3;
    public GameObject HorseTr;
    public bool isJump = false;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        TriggerRide = this.transform.Find("HorseTrigger").gameObject;
        Hero = GameObject.Find("Player");
    }

    //Функция залезания на лошадь
    public void RideSitting(GameObject obj) {
        Debug.Log("Hero Is RIDE!");
        Hero = obj;
        Hero.GetComponent<MoveScript>().enabled = false;
        Hero.transform.position = TriggerRide.transform.position;
        Hero.transform.SetParent(TriggerRide.transform);
        isRide = true;
    }

    //Функция слезания с лошади
    public void RideOut() {
        Debug.Log("Hero Is OUT!");
        TriggerRide.transform.DetachChildren();
        Hero.transform.position += new Vector3(5, 0, 0);
        Hero.GetComponent<MoveScript>().enabled = true;
        isRide = false;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.H) && isRide == true) {
            RideOut();
        }

        if (Input.GetKeyDown(KeyCode.G) && isRide == false)
        {
            RideSitting(Hero);
        }
        Move();


    }

    public void Move() 
    {

        if (isRide == true)
        {

        transform.Rotate(0, Input.GetAxis("Horizontal") * SpeedRotation, 0);
        }
    }

    // реализация метода Use
    public void Use(GameObject ObjHit, GameObject ObjRay)
    {
        RideSitting(ObjRay);
    }
}
