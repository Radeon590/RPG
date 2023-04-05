using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIContr : MonoBehaviour
{
    [SerializeField] GameObject InvMenu;
    [SerializeField] GameObject[] ListEquipedWeapons;

    public static bool paused = false;

    bool pressed = false;
    public static bool alreadyInInv = false;

    float typeTimer = 0;

    int weaponInHand = 0;
    // Start is called before the first frame update
    void Start()
    {
        //weaponInHand = ListEquipedWeapons.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed) 
        {
            typeTimer += Time.deltaTime;

            if(typeTimer > 2.5f)//если игрок зажал кнопку, значит нужно открыть инвентарь
            {
                if (!alreadyInInv)
                    ActiveInv();
            }

            //если мы перестали нажимать I или мы нажимаем слишком долго
            if (Input.GetAxis("Inventory") == 0) 
            {
                if(typeTimer < 1) //если это был короткий тайп
                {
                    if (alreadyInInv)//то либо деактивируем меню инвентаря, если игрок сейчас в нем
                        DeactivateInv();
                    else//либо меняем итем в руке игрока
                        ChangeEquipedItems();
                }

                pressed = false;
                typeTimer = 0;
            }
        }
        else 
        {
            //флаг нажатия
            if(Input.GetAxis("Inventory") == 1) 
            {
                pressed = true;
            }
        }
    }

    void ChangeEquipedItems() 
    {
        ListEquipedWeapons[weaponInHand].SetActive(false);
        weaponInHand++;
        if (weaponInHand == ListEquipedWeapons.Length)
            weaponInHand = 0;
        ListEquipedWeapons[weaponInHand].SetActive(true);
    }

    void ActiveInv() 
    {
        InvMenu.SetActive(true);
        if (!paused)
            paused = true;
        alreadyInInv = true;
    }

    void DeactivateInv() 
    {
        InvMenu.SetActive(false);
        if (paused)
            paused = false;
        alreadyInInv = false;
    }
}
