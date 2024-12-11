using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CheckInternet : MonoBehaviour
{
    public static bool isConnected = false;

    [SerializeField] private GameObject internetPopup;

    private bool flag = false;
    private bool allowFirstTime = true;

    public static event Action<bool> InternetStatusChanged;

    void Start()
    {
        StartCoroutine(checkInternetConnection());
    }


    IEnumerator checkInternetConnection()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            UnityWebRequest www = new UnityWebRequest("http://google.com");
            www.timeout = 3;

            yield return www.SendWebRequest();
            isConnected = www.result == UnityWebRequest.Result.Success;

            if (!isConnected)
            {
                if (!flag)
                {
                    flag = true;
                    internetPopup.SetActive(true);

                    InternetStatusChanged?.Invoke(false);
                }
            }
            else
            {
                if (flag || allowFirstTime)
                {
                    allowFirstTime = false;
                    flag = false;
                    internetPopup.SetActive(false);
                    InternetStatusChanged?.Invoke(true);
                }
            }
        }
    }
}