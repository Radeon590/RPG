using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedItems : MonoBehaviour
{
    List<GameObject> ListEquipedItems = new List<GameObject>();
    MenuMechs menuMechs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*menuMechs.SelectionOfMenuItems(
            () => ListEquipedItems[menuMechs.currentLootMenuPunct].SetActive(false),
            () => ListEquipedItems[menuMechs.currentLootMenuPunct].SetActive(true),
            ListEquipedItems);*/

        //if(Input.GetAxis())
    }
}
