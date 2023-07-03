using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTarget : MonoBehaviour
{
    [SerializeField] GameObject Animal;
    Transform animalTransform;
    AnimalScript animalScript;

    public SideOfDetection sideOfRay = SideOfDetection.left;

    bool obstacle = false;
    //string thisAnimalName;

    // Start is called before the first frame update
    void Start()
    {
        animalTransform = Animal.transform;
        animalScript = Animal.GetComponent<AnimalScript>();
        //thisAnimalName = Animal.name;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(animalTransform.position, transform.position);
        Physics.Raycast(ray, out hit);

        
        if(hit.collider 
            && hit.collider.gameObject.tag != "terrain" 
            && hit.collider.gameObject.name != Animal.name 
            && Vector3.Distance(animalTransform.position, hit.collider.transform.TransformPoint(Vector3.forward)) < 12) 
        {
            //Debug.Log(hit.collider.name);
            Debug.DrawLine(ray.origin, hit.collider.transform.position);
            if (!obstacle) 
            {
                if (sideOfRay == SideOfDetection.middle)
                    animalScript.middle++;
                else if (sideOfRay == SideOfDetection.left)
                    animalScript.leftCount++;
                else
                    animalScript.rightCount++;
                obstacle = true;
            }
        }
        if(hit.collider == null)
        {
            if (obstacle) 
            {
                //Debug.Log("minus");
                if (sideOfRay == SideOfDetection.middle)
                    animalScript.middle--;
                if (sideOfRay == SideOfDetection.left)
                    animalScript.leftCount--;
                else
                    animalScript.rightCount--;

                obstacle = false;
            }
        }
    }
}
