using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepDistance : MonoBehaviour
{

    public GameObject Player;
    public bool fear = false;
    public bool isSnak;
    public float fearChance = 0.5f;

    private float timeCheckFear = 0;
    void Start()
    {
        Player = GameObject.Find("Player");
    }


    void Update()
    {
        timeCheckFear += Time.deltaTime;
        isSnak = Player.GetComponent<MoveScript>().isSnake;

        if (isSnak == false)
        {
            if (Vector3.Distance(Player.transform.position, transform.position) <= 10)
            {
                fearChance = 1.3f;
                if (timeCheckFear > 0.2f)
                {
                    timeCheckFear = 0;
                    if (Random.Range(0f, 100f) <= fearChance)
                    {
                        fear = true;
                        //Debug.Log("fear");
                    }
                }
            }     
            else
            {
                fear = false;
            }
        }






        if (isSnak == true)
        {
            if (Vector3.Distance(Player.transform.position, transform.position) <= 5)
            {
                fearChance = 1.2f;
                if (timeCheckFear > 0.2f)
                {
                    timeCheckFear = 0;
                    if (Random.Range(0f, 100f) <= fearChance)
                    {
                        fear = true;
                        Debug.Log("fear Snake");
                    }
                }
            }
            else
            {
                fear = false;
            }
        }


        if (isSnak == true)
        {
            if (Vector3.Distance(Player.transform.position, transform.position) <= 4)
            {
                fearChance = 1.3f;
                if (timeCheckFear > 0.2f)
                {
                    timeCheckFear = 0;
                    if (Random.Range(0f, 100f) <= fearChance)
                    {
                        fear = true;
                        Debug.Log("fear Snake");
                    }
                }
            }
            else
            {
                fear = false;
            }


            if (isSnak == true)
            {
                if (Vector3.Distance(Player.transform.position, transform.position) <= 3)
                {
                    fearChance = 1.4f;
                    if (timeCheckFear > 0.2f)
                    {
                        timeCheckFear = 0;
                        if (Random.Range(0f, 100f) <= fearChance)
                        {
                            fear = true;
                            Debug.Log("fear Snake");
                        }
                    }
                }
                else
                {
                    fear = false;
                }

            if (isSnak == true)
                {
                    if (Vector3.Distance(Player.transform.position, transform.position) <= 2)
                    {
                        fearChance = 1.5f;
                        if (Random.Range(0f, 100f) <= fearChance)
                        {
                            fear = true;
                            Debug.Log("fear Snake");
                        }
                    }
                    else
                    {
                        fear = false;
                    }
                }
            }


            if (isSnak == true)
            {
                if (Vector3.Distance(Player.transform.position, transform.position) <= 1)
                {
                    fearChance = 1.6f;
                    if (timeCheckFear > 0.2f)
                    {
                        timeCheckFear = 0;
                        if (Random.Range(0f, 100f) <= fearChance)
                        {
                            fear = true;
                            Debug.Log("fear Snake");
                        }
                    }
                }
                else
                {
                    fear = false;
                }
            }

        }
    }
    

}
