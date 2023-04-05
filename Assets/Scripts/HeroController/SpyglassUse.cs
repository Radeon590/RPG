using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class SpyglassUse : MonoBehaviour
{
    enum State { 
        Normal = 0,
        Pipe
    }
    private State CurrentState = State.Normal;

    private Vignette vignette;
    public PostProcessVolume postprocess;
    // Start is called before the first frame update
    void Start()
    {
        CurrentState = State.Normal;
        postprocess.profile.TryGetSettings(out vignette);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (CurrentState == State.Normal)
            {
                CurrentState = State.Pipe;
            }
            else 
            {
                CurrentState = State.Normal;
            }

            if (CurrentState == State.Pipe)
            {
                vignette.intensity.Override(1);
                vignette.smoothness.Override(0.01f);
                Camera.main.fieldOfView = 20;
            }
            if (CurrentState == State.Normal)
            {
                vignette.intensity.Override(0.48f);
                vignette.smoothness.Override(0.187f);
                Camera.main.fieldOfView = 60;
            }
        }
    }
}
