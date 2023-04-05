using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInv : MonoBehaviour
{
    public static List<(string, int)> playerInv = new List<(string, int)>();
    static List<GameObject> strings = new List<GameObject>();

    [SerializeField] RectTransform InventContent;
    [SerializeField] GameObject InvMenuStringPref;

    static RectTransform content; 
    static GameObject stringPref;

    MenuMechs menuMechs;

    // Start is called before the first frame update
    void Start()
    {
        menuMechs = new MenuMechs();
        content = InventContent;
        stringPref = InvMenuStringPref;
    }

    // Update is called once per frame
    void Update()
    {
        //part for testing
        if (Input.GetKeyDown(KeyCode.T))
            AddElement(("test", 4));

        //выбор строки в меню
        if(strings.Count > 0 && GameUIContr.alreadyInInv) 
        {
            menuMechs.SelectionOfMenuItems(strings,
            () => strings[menuMechs.currentLootMenuPunct].transform.GetChild(0).gameObject.SetActive(false), //GetComponent<Image>().color = Color.black,
            () => strings[menuMechs.currentLootMenuPunct].transform.GetChild(0).gameObject.SetActive(true)//GetComponent<Image>().color = Color.yellow);
            );
        }
        
    }

    public static void AddElement((string, int) element) 
    {
        //проверка на то, есть ли данный итем уже в меню
        for(int i = 0; i < playerInv.Count; i++) 
        {
            if(element.Item1 == playerInv[i].Item1) //если есть, то нужно реализовать другую функцию
            {
                AddToOld(element, i);
                return;//закончив выполнение этой
            }
        }
        //если же это новый итем, то тогда добавляем его как новый:

        //смещаем список вниз, добавляя место для нового итема
        content.sizeDelta = new Vector2(content.sizeDelta.x, content.sizeDelta.y + 5);
        //после изменения размеров окна нужно сместить все уже имеющиеся итемы выше (если они есть)
        for (int i = 0; i < content.childCount; i++)
        {
            Transform currentChild = content.GetChild(i);
            currentChild.localPosition = new Vector2(currentChild.localPosition.x, currentChild.localPosition.y + 2.5f);
        }
        //запоминаем последнюю строчку
        Transform lastString = content.GetChild(content.childCount - 1);
        //и создаем новую
        GameObject newString = Instantiate(stringPref);
        newString.transform.parent = content; //добавляя ей родителем скролл окно
        newString.transform.localScale = new Vector3(1, 1, 1);//настраиваем размеры
        newString.transform.localPosition = new Vector2(lastString.localPosition.x, lastString.localPosition.y - 30);//настраиваем его позицию при помощи инфы о последней строке, которую мы запомнили ранее
        newString.GetComponent<Text>().text = element.Item1 + " x" + element.Item2;//пишем характеристику итема
        if (content.childCount == 2) //если это была первая строка, то активируем ее рамку (визуально помечаем ее выбранной в меню)
            newString.transform.GetChild(0).gameObject.SetActive(true);
        strings.Add(newString);
        playerInv.Add(element);
    }

    /// <summary>
    /// функция, добавляющая новые итемы к уже существующему (складывающее количество итемов одного типа)
    /// </summary>
    /// <param name="element">какой элемент добюавляется</param>
    /// <param name="indexOfString">строка в сипске в меню</param>
    static void AddToOld((string, int) element, int indexOfString) 
    {
        playerInv[indexOfString] = (playerInv[indexOfString].Item1, playerInv[indexOfString].Item2 + element.Item2);
        strings[indexOfString].GetComponent<Text>().text = playerInv[indexOfString].Item1 + " x" + playerInv[indexOfString].Item2;
    }
}
