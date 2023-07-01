using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public Transform ArrowsContainer;
    public AnimalScript animalScript;

    public float damageMyltiplier = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            GainDamage(0.5f, true);
    }

    public void GainDamage(float damage, bool isItArrow) 
    {
        animalScript.HP_animal -= damage * damageMyltiplier;

        if (isItArrow)
            animalScript.AddArrowToLoot(1);
    }
}
