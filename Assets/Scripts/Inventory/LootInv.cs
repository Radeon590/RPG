using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootInv : MonoBehaviour
{
    #region PublicClassObjects
    [SerializeField] GameObject StringPattern;
    public GameObject LootWindow;
    public RectTransform IndArea;
    public RectTransform IndFillArea;
    public RectTransform IndStartArea;
    public Transform lastString;
    #endregion

    #region Classobjects
    MenuMechs menuMechs;

    Transform PlayerTransform;

    List<(string, int)> lootElements = new List<(string, int)>();
    List<GameObject> lootStrings = new List<GameObject>();
    #endregion

    #region PublicVars
    public int namespaceIndex = -5;
    #endregion

    #region vars
    float choosedTimer = 0;

    bool choosed = false;
    bool inventoryShowed = false;
    bool inList = false;
    //bool addedTolootlist = false;
    #endregion

    void Start()
    {
        menuMechs = new MenuMechs();

        PlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        //нам онадобится знать положение последней строки для того, чтобы при добавлении новых строк можно было рассчитать их положение
        /*namespaceIndex = LootWindow.transform.childCount - 1;
        lastString = LootWindow.transform.GetChild(namespaceIndex);*/

        //несколько строк для тестов
        /*AddLootElement("arrow", 4);
        AddLootElement("meat", 4);
        AddLootElement("bones", 8);
        AddLootElement("test", 5);*/
    }

    void Update()
    {
        if (!GameUIContr.paused) 
        {
            if(Vector3.Distance(transform.position, PlayerTransform.position) < 5) //если игрок находится рядом с данным лут-объектом
            {
                if(!inList) //если данный лут-объект все еще не внесен в список находящихся рядом с игроком, то мы его добавляем
                {
                    LootWindowContainer.NearestObj.Add(this.gameObject);
                    inList = true;
                }
                else//если же все подготовления сделаны, то мы осуществляем логику определения показывать лут-окно или нет
                {
                    if(LootWindowContainer.LookingObj == this.gameObject) //если игрок смотрит на этот лут-объект, то:
                    {
                        if (!inventoryShowed) //если мы все еще не активировали окно лута, то мы его активируем
                            ShowingInventory(true);
                        //окно лута всегда должно быть повернуто в сторону игрока
                        LootWindow.transform.LookAt(PlayerTransform.position);

                        if(lootStrings.Count > 0) //если в окне лута есть элементы
                        {
                            //игрок может выбирать элементы в списке лута
                            menuMechs.SelectionOfMenuItems(lootStrings,
                                                           () => lootStrings[menuMechs.currentLootMenuPunct].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.black,
                                                           () => lootStrings[menuMechs.currentLootMenuPunct].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.yellow
                                                           );
                        }

                        //если игрок нажмет "submit" и при этом сипсок лута не будет пуст, то мы поднимем флаг, чтобы далее определить добавление итема в инвентарь
                        if (Input.GetAxis("Submit") != 0 && !choosed && lootStrings.Count != 0)
                        {
                            choosed = true;
                        }
                    }
                    else //если же игрок не смотрит на данный лут-объект, то нам не нужно его показывать
                    {
                        if (inventoryShowed)
                            ShowingInventory(false);
                    }
                }
            }
            else//если игрок отдаляется от лут-объекта, то
            {
                if (inList)//если лут-объект все еще в списке рядом находящихся объектов, мы его удаляем
                {
                    LootWindowContainer.NearestObj.Remove(this.gameObject);
                    inList = false;
                }

                //а лут-окно перестаем показывать
                if (inventoryShowed)
                    ShowingInventory(false);
            }

            if(choosed == true) //если у нас было нажатие "submit"
            {
                choosedTimer += Time.deltaTime;//считаем специальный таймер, определяющий длину нажатия

                if(choosedTimer > 1) //если игрок жмет больше одной секунды, значит, он зажимает
                {
                    FillIndikator();//нужно заполнять индикатор (визуально)
                }
                if (choosedTimer > 3) //если игрок держал больше 3 секунд, значит он хочет взять все итемы из этого лут-объекта
                {
                    IndFillArea.sizeDelta = new Vector2(IndArea.sizeDelta.x, IndFillArea.sizeDelta.y);//обнуляем индикатор
                    AddAllElementsToInv();//добавляем все итемы в инвентарь
                    choosed = false;
                    choosedTimer = 0;
                }

                if (Input.GetAxis("Submit") == 0)//если игрок перестал нажимать
                {
                    choosed = false;

                    if (choosedTimer < 1)//если это было короткое нажатие, то добавляем один элемент в инвентарь
                        AddElementToYourInventory(menuMechs.currentLootMenuPunct);

                    choosedTimer = 0;
                }
            }
            else //если игрок не нажимает
            {
                if(IndFillArea.sizeDelta.x > IndStartArea.sizeDelta.x) //проверяем индикатор "зажатия" (заполняемый когда берется весь лут), и в случае чего сбрасываем его
                {
                    IndFillArea.position = IndArea.position;
                    IndFillArea.sizeDelta = new Vector2(IndFillArea.sizeDelta.x - 3 * Time.deltaTime, IndFillArea.sizeDelta.y);
                }
            }  
        }
    }

    /// <summary>
    /// заполнение индикатора при зажатии (игрок берет весь лут)
    /// </summary>
    void FillIndikator() 
    {
        if(IndFillArea.sizeDelta.x < IndArea.sizeDelta.x) 
        {
            IndFillArea.position = IndArea.position;
            IndFillArea.sizeDelta = new Vector2(IndFillArea.sizeDelta.x + 1 * Time.deltaTime, IndFillArea.sizeDelta.y);
        }
    }

    /// <summary>
    /// добавление всех элементов лут-меню в инвентарь игрока
    /// </summary>
    void AddAllElementsToInv() 
    {
        if(lootElements.Count > 0) 
        {
            for (int i = 0; i < lootElements.Count;)
            {
                //добавляем этот итем в список инвентаря игрока
                PlayerInv.AddElement(lootElements[i]);
                //и убираем этот итем из:
                Destroy(lootStrings[i]);//из видимого игроком меню лута
                lootElements.RemoveAt(i);//из списка характеристик итемов
                lootStrings.RemoveAt(i);//из списка строчек меню лута убираем опустевшую строчку
            }

            ReplaceLootStrings(-0.3f);//все меню теперь должно переместиться на пункт ниже
        }
    }

    /// <summary>
    /// добавление одного элемента лут-меню в инвентарь игрока
    /// </summary>
    /// <param name="element">какой конкретно элемент собирается взять игрок</param>
    void AddElementToYourInventory(int element) //если мы выбрали добавить элемент в свой инвентарь
    {
        if(lootElements.Count > 0) 
        {
            //добавляем этот итем в список инвентаря игрока
            PlayerInv.AddElement(lootElements[element]);
            //и убираем этот итем из:
            Destroy(lootStrings[element]);//из видимого игроком меню лута
            lootElements.RemoveAt(element);//из списка характеристик итемов
            lootStrings.RemoveAt(element);//из списка строчек мменю лута убираем опустевшую строчку

            ReplaceLootStrings(-0.3f);//все меню теперь должно переместиться на пункт ниже

            //первую оставшуюся в этом меню строчку приравниваем первой
            if(lootElements.Count > 0) 
            {
                menuMechs.currentLootMenuPunct = 0;
                lootStrings[menuMechs.currentLootMenuPunct].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }

    /// <summary>
    /// добавление нового элемента в лут
    /// </summary>
    /// <param name="name">название лута</param>
    /// <param name="number">количество этого лута</param>
    public void AddLootElement(string name, int number) //функция, добавляющая новый элемент в меню лута
    {
        if (lastString != null)//как только мы назначили последнюю строку у нас еще неизвестен индекс последней строки
            namespaceIndex = 1;

        //создаем новый итем списка характеристик (добавляем в список)
        (string, int) lootElement_new = (name, number);
        lootElements.Add(lootElement_new);

        //после добавления нужно перенести все уже существующие пункты с учетом нового (освободить для него место)
        ReplaceLootStrings(0.3f);

        GameObject newString = Instantiate(StringPattern);//создаем новый итем для меню
        newString.transform.position = new Vector3(lastString.position.x, lastString.position.y - 0.3f, lastString.position.z);//меняем ему позицию, которая чуть ниже последнего предыдущего итема
        newString.transform.parent = LootWindow.transform;//назначаем итему родителем окно
        newString.GetComponent<TextMesh>().text = name + " x" + number;//визуализируем характеристики
        lastString = newString.transform;//запоминаем последний итем как последнюю строчку
        lootStrings.Add(newString);//и добавляем его в список строк итемов

        //если это первая строчка, то мы с самого начала должны сделать ее выбранной в меню
        if(lootStrings.Count == 1)
            newString.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public void ShowingInventory(bool condition) //функция вкл/выкл отображения меню
    {
        LootWindow.SetActive(condition);

        inventoryShowed = condition;
    }

    /// <summary>
    /// визуально смезает все строки в лут-меню вниз на y-delta (используется когда меняется количество строк в меню)
    /// </summary>
    /// <param name="y_delta">на какой y сместить строки</param>
    void ReplaceLootStrings(float y_delta) //функция перемещения итемов в меню
    {
        LootWindow.transform.position = new Vector3(LootWindow.transform.position.x, LootWindow.transform.position.y + y_delta, LootWindow.transform.position.z);//меняем позицию самого объекта окна

        Transform LastString_reinstalation = LootWindow.transform.GetChild(namespaceIndex);//запоминаем первую строчку как последнюю перемещенную
        foreach (GameObject i in lootStrings)//перебираем все строчки, перемещая их на delta ниже ,чем предыдущая строка (которую запомнили выше)
        {
            i.transform.position = new Vector3(LastString_reinstalation.position.x, LastString_reinstalation.position.y - Mathf.Abs(y_delta), LastString_reinstalation.position.z);
            LastString_reinstalation = i.transform;//последнюю строку нужно каждый раз запоминать снова
        }
    }
}
