using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUpdate : MonoBehaviour
{
    private Text Ui;
    // Start is called before the first frame update
    void Start()
    {
        Ui = GetComponent<Text>();
        Timer.GetCurrentTimer().UpdateTime += UpdateTime;
    }

    private void UpdateTime(float time)
    {
        float minutes = time / 60;
        float seconds = time % 60;
        Ui.text = Convert.ToString(Mathf.FloorToInt(minutes)) + ":" + Convert.ToString(Mathf.FloorToInt(seconds));
    }
}
