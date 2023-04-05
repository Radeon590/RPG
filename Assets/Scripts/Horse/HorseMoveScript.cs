using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseMoveScript : MonoBehaviour
{
    #region vars
    public float HorseSpeed = 0.12f;
    public float HorseRunSpeed = 0.25f;
    public CharacterController _characterController;
    public float SpeedRotation = 3;
    public static bool isRide = false;
    public Vector3 Forward;
    public Vector3 Right;
    public float HorseStamina;
    public GameObject HorseStaminaBar;
    #endregion
    void Start()
    {
        HorseStaminaBar = GameObject.Find("HorseStaminaBar");
        _characterController = GetComponent<CharacterController>();
        HorseSpeed = 0.12f;
        HorseStamina = 200f;

    }


    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        float translationZ = Input.GetAxis("Vertical") * HorseSpeed;
        float translationX = Input.GetAxis("Horizontal") * HorseSpeed;
        if (isRide == true)
        {
            
            if (Input.GetKey(KeyCode.W))
            {
                _characterController.Move(Forward * -HorseSpeed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                _characterController.Move(Forward * HorseSpeed);
            }
            _characterController.transform.Rotate(0, Input.GetAxis("Horizontal") * SpeedRotation, 0);
            transform.Rotate(0, Input.GetAxis("Horizontal") * SpeedRotation, 0);
        }
    }
}
