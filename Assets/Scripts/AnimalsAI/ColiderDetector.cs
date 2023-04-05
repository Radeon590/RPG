using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderDetector : MonoBehaviour
{
    public SideOfDetection side = SideOfDetection.middle;

    [SerializeField] GameObject ThisAnimal;
    AnimalScript animalScript;

    GameObject CollisionObject;
    GameObject StayingObject;

    bool obstacle = false;
    // Start is called before the first frame update
    void Start()
    {
        animalScript = ThisAnimal.GetComponent<AnimalScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool CheckColObj(string tag) 
    {
        if (tag == "terrain" || tag == "animalAreaBoard") return true;
        else return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == CollisionObject)
        StayingObject = other.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (animalScript.down != 0 && side == SideOfDetection.down && other.gameObject.tag == "terrain")
        {
            animalScript.down++;
        }
        else*/
        if (obstacle == false && other.gameObject.name != ThisAnimal.name && other.gameObject.tag != "detector" && other.gameObject.tag != "arrow" && other.gameObject.tag != "arrowWeight" && other.gameObject.tag != "animalHitCol") 
        {
            if(side == SideOfDetection.middleExtra && !CheckColObj(other.gameObject.tag)) 
            {
                animalScript.middleExtra++;
            }
            else if (side == SideOfDetection.up && CheckColObj(other.gameObject.tag)) 
            {
                animalScript.up++;
            }
            else if(!CheckColObj(other.gameObject.tag))
            {
                if (side == SideOfDetection.middle)
                {
                    animalScript.middle++;
                }
                else
                {
                    if (side == SideOfDetection.right)
                    {
                        animalScript.rightCount++;
                    }
                    else
                    {
                        animalScript.leftCount++;
                    }
                }
            }

            obstacle = true;
            CollisionObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (side == SideOfDetection.down && other.gameObject.tag == "terrain")
        {
            animalScript.down--;
        }
        else*/
        if (obstacle == true && other.gameObject.name == CollisionObject.name) 
        {
            /*if(CollisionObject != StayingObject) 
            {
                
            }*/

            if (side == SideOfDetection.middleExtra && !CheckColObj(other.gameObject.tag))
            {
                animalScript.middleExtra--;
            }
            else if (side == SideOfDetection.up && CheckColObj(other.gameObject.tag))
            {
                animalScript.up--;
            }
            else if (!CheckColObj(other.gameObject.tag))
            {
                if (side == SideOfDetection.middle)
                {
                    animalScript.middle--;
                }
                else
                {
                    if (side == SideOfDetection.right)
                    {
                        animalScript.rightCount--;
                    }
                    else
                    {
                        animalScript.leftCount--;
                    }
                }
            }

            //animalScript.remainingRot = true;
            obstacle = false;
            CollisionObject = other.gameObject;
        }
    }
}
