using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowWeightScipt : MonoBehaviour
{
    //public GameObject AnimalBody;
    public HitDetection hitDetection;

    public bool released = false;
    public bool col = false;
    public bool hit = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (released) //если грузило столкнулось с каким-нибудь объектом после того, как стрела была выпущена
        {
            if (collision.gameObject.tag != "detector" && collision.gameObject.tag != "animal" && collision.gameObject.tag != "animalAreaBoard" && collision.gameObject.tag != "LootRaycastTrigger") //если это не детектор препятствий животного и не его "двигательный" колайдер
            {
                Debug.Log("Arrow collision");
                //то мы останавливаем грузило
                col = true;
                Rigidbody rb = this.GetComponent<Rigidbody>();
                Destroy(rb);
            }

            if (collision.gameObject.tag == "animalHitCol" && hit == false) //если объект, с которым произошло столкновение был детектором повреждений животного
            {
                //то мы запоминаем его, чтобы нанести урон, а также делаем грузило дочерним объектом специальному контейнеру (а затем и стрелу)
                Debug.Log("hit");
                hitDetection = collision.gameObject.GetComponent<HitDetection>();
                hitDetection.GainDamage(1, true);
                this.transform.SetParent(hitDetection.ArrowsContainer);
                hit = true;
            }
        }
    }
}
