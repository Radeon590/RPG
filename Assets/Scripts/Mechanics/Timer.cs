using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float RemaningTime = 360f;
    private float CurrentTime = 0f;

    public delegate void completeTime();
    public event completeTime CompleteTime;
    
    public delegate void updateTime(float time);
    public event updateTime UpdateTime;

    public static Timer GetCurrentTimer() {
        return GameObject.Find("Timer").GetComponent<Timer>();
    }

    public void ResetTime()
    {
        CurrentTime = RemaningTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetTime();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime -= Time.deltaTime;
        UpdateTime.Invoke(CurrentTime);
        if (CurrentTime <= 0) 
        {
            CompleteTime.Invoke();
        }
    }
}
