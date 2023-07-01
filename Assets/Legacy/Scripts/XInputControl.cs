using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class XInputControl : MonoBehaviour
{
    #region XInput
    static PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    #endregion

    public static bool connected = false;

    // Update is called once per frame
    void Update()
    {
        CheckControllerConnection();
    }

    public static void Vibrate(float left, float right) 
    {
        if(connected)
        GamePad.SetVibration(playerIndex, left, right);
    }

    void CheckControllerConnection() 
    {
        if(!connected && Input.GetJoystickNames().Length > 0) 
        {
            connected = true;
        }
    }
}
