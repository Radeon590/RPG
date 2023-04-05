using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseSitting : MonoBehaviour
{
    public bool isRide = false;
    public bool CanRide = false;
    public GameObject hero;

    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.Find("Hero");

    }

    // Update is called once per frame
    void Update()
    {
        if (CanRide == true)
        {
            if (Input.GetKey(KeyCode.G))
            {
                isRide = true;
                hero.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                hero.transform.rotation = this.transform.rotation;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        CanRide = true;
    }

    private void OnTriggerExit(Collider other)
    {
        CanRide = false;
    }
}
