using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public Transform ThisArrowWeight;

    ArrowWeightScipt thisArrowWeightScipt;

    Transform PlayerTransform;

    public float bowPower = 0;
    public bool realised = false;

    float afterShotTimer = 0;

    bool approach = true;

    void Start()
    {
        thisArrowWeightScipt = ThisArrowWeight.gameObject.GetComponent<ArrowWeightScipt>();
    }

    void Update()
    {
        //После выстрела  
        if (realised == true)
        {
            //Проверяем не произошло ли столкновение с другими объектами
            if (thisArrowWeightScipt.col)
            {
                if (thisArrowWeightScipt.hit == true) //если это было попадание по животному
                {
                    transform.SetParent(thisArrowWeightScipt.gameObject.transform.parent);/*то мы делаем стрелу дочерним объектом специальному контейнеру, чтобы потом ее можно было забрать как лут. А также чтобы она двигалась вслед за анимациями животного*/
                    //thisArrowWeightScipt.hitDetection.GainDamage(3);//и наносим животному урон
                }

                approach = false;
                realised = false;
                PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                if (thisArrowWeightScipt.gameObject != null) //уничтожаем ненужное больше грузило
                    Destroy(thisArrowWeightScipt.gameObject);
            }
            else //Если столкновений не было, то мы двигаем стрелу вслед за летящим "грузилом"
            {
                if (bowPower > 5) //Нам не нужно, чтобы стрела вращалась вслед за "грузилом", если расстояние полета будет слишком маленьким
                    this.transform.LookAt(ThisArrowWeight);

                // this.transform.position = new Vector3(ThisArrowWeight.position.x, ThisArrowWeight.position.y, ThisArrowWeight.position.z); 
                this.transform.position = Vector3.MoveTowards(this.transform.position, ThisArrowWeight.position, 1f);
            }
        }

        //Убираем стрелу со сцены после приземления, если прошло достаточно времени и игрок достаточно далеко
        if (approach == false) 
        {
            afterShotTimer += Time.deltaTime;

            if(afterShotTimer > 180) 
            {
                if(Vector2.Distance(transform.position, PlayerTransform.position) > 100) 
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    /*void Update()
    {
        
    }*/
}
