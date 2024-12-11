using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public  class PublishingSettings : MonoBehaviour
{
    private  static  string keystorePass = "letterwords";
    
    static PublishingSettings()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        
        PlayerSettings.Android.keystorePass = keystorePass;
        PlayerSettings.Android.keyaliasName = keystorePass;
        PlayerSettings.Android.keyaliasPass = keystorePass;
    }
}
