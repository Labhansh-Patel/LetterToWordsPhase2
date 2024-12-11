using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public static class SetPhotonID
{
    private static string photonID = "82b18024-bb2c-4de8-b619-9308ba1b8bd4";
    
     static  SetPhotonID()
     {
         PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = photonID;
     }
    
}
