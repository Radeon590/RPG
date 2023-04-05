using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainStart : MonoBehaviour
{
    public float rainpause = 0f;
    public float raintime = 0f;
    public float time = 0f;
    public GameObject rain;

    public bool israin = false;
    // Start is called before the first frame update
    void Start()
    {
        rainpause = Random.Range(300, 1200);
        //Debug.Log($"RainPause: {rainpause}");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (israin == false) 
        {
            if (time >= rainpause) 
            {
                israin = true;
                time = 0;
                raintime = Random.Range(60, 300);
                rain.SetActive(true);
                //Debug.Log($"RainTime: {raintime}");
            }
        }
        if (israin == true) 
        {
            if (time >= raintime) 
            {
                time = 0;
                israin = false;
                rainpause = Random.Range(300, 1200);
                rain.SetActive(false);
                //Debug.Log($"RainPause: {rainpause}");
            }
        }
    }
}
