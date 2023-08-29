using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PauseManager 
{
    private static bool pause;
    public static bool Pause 
    { 
        get => pause;
        set
        {
            OnSetPause?.Invoke(value);
            pause = value;
        }
    }

    public static event Action<bool> OnSetPause;

}
