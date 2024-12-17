using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ChangeState(States state);

public delegate void CallBack();

public delegate void PopUpErrorMsg(string Msg);

public delegate void BackToPriviousScreen();


public static class HandleEvents
{
    public static event ChangeState Changestate;
    public static event PopUpErrorMsg PopoupErrorMsg;
    public static event BackToPriviousScreen BackToPreviousScreen;


    public static void BackToPreviousState()
    {
        BackToPreviousScreen?.Invoke();
    }

    public static void ChangeStates(States states)
    {
        Changestate?.Invoke(states);
    }

    public static void PopoupErrorMsgOpen(string Msg)
    {
        PopoupErrorMsg?.Invoke(Msg);
    }
}