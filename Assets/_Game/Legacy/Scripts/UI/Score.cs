using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Score : MonoBehaviour
{
    public Text ui;
    public static int score = 0;
    public static Score GetCurrentScore() {
        return GameObject.Find("ScoreText").GetComponent<Score>();
    }

    public void AddScore(int sc)
    {
        score += sc;
        ui.text = "Score " + Convert.ToString(score);
    }

    private void Awake()
    {
        score = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        ui = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
