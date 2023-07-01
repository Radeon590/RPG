using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMechs
{
    public delegate void action();

    public int currentLootMenuPunct = 0;

    bool pressed = false;

    /// <summary>
    /// function for choosing of puncts (strings) in menu
    /// </summary>
    /// <param name="a">action of disabling last chosen string in menu</param>
    /// <param name="a2">action of enabling new chosen string in menu</param>
    public void SelectionOfMenuItems(List<GameObject> items, action a = null, action a2 = null)
    {
        if (Input.GetAxis("DPAD_vertical") > 0 && !pressed) //если мы листнули вверх, то изменяем графически состояние строчек (в меню меняем подсвечиваемые итемы)
        {
            pressed = true;
            if(a != null)
                a.Invoke();

            if (currentLootMenuPunct == 0) //здесь просчитывается тот случай, если при обычном смещении currentPunct выйдет за границы
            {
                currentLootMenuPunct = items.Count - 1;
            }
            else
            {
                currentLootMenuPunct--;
            }

            if (a2 != null)
                a2.Invoke();
        }
        else if (Input.GetAxis("DPAD_vertical") < 0 && !pressed) //листаем вниз и делаем то же самое
        {
            pressed = true;
            if (a != null)
                a.Invoke();

            if (currentLootMenuPunct == items.Count - 1)//здесь просчитывается тот случай, если при обычном смещении currentPunct выйдет за границы
            {
                currentLootMenuPunct = 0;
            }
            else
            {
                currentLootMenuPunct++;
            }

            if (a2 != null)
                a2.Invoke();
        }
        else if (Input.GetAxis("DPAD_vertical") == 0 && pressed)//возвращение флага в обычное состояние для дальнейших нажатий
        {
            pressed = false;
        }
    }
}
