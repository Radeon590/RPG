using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeMenu : MonoBehaviour
{
    [SerializeField] GameObject TraderLootWindow;
    [SerializeField] GameObject PlayerLootWindow;

    List<GameObject> TradeLootWindow_strings;
    List<GameObject> PlayerLootWindow_strings;
    MenuMechs menuMechs;
    // Start is called before the first frame update
    void Start()
    {
        menuMechs = new MenuMechs();

        for(int i = 0; i < TraderLootWindow.transform.childCount; i++) 
        {
            TradeLootWindow_strings.Add(TraderLootWindow.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < TraderLootWindow.transform.childCount; i++) 
        {
            PlayerLootWindow_strings.Add(PlayerLootWindow.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
