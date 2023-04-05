using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Создание интерфейса
public interface I_Interact
{
    void Use(GameObject ObjHit, GameObject ObjRay);
}

public class Interacteble : MonoBehaviour
{
    public GameObject Hero;

    void Start()
    {
        if (Hero == null) {
            Hero = this.gameObject;
        }
    }


    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.forward,out hit, 10))
        {
            var it = hit.collider.gameObject.GetComponent<I_Interact>();
            if (Input.GetKey(KeyCode.E))
            {
                it?.Use(hit.collider.gameObject,Hero);
            }
        }
    }
}
