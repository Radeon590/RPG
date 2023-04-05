using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
    [SerializeField] Material SkyMaterial;

    float dayTime = 0;
    float dayTime_border = 12;
    float dtDelta = 0.1f;

    bool firstPart = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SkyMaterial.SetColor("Top", Color.red);

        if (firstPart) 
        {
            if (dayTime < dayTime_border)
            {
                dayTime += dtDelta * Time.deltaTime;
            }
            else
            {
                firstPart = false;
            }
        }
        else 
        {
            if (dayTime > 0)
                dayTime -= dtDelta * Time.deltaTime;
            else 
            {
                firstPart = true;
            }
        }

        
        //Shader.Styli
    }

    void ChangeDayTime() 
    {

    }
}
