using UnityEngine;

public class OpenLoginEventHandler : MonoBehaviour
{
    [SerializeField] private StateController stateControllerRef;

    private void OnDisable()
    {
        CheckInternet.InternetStatusChanged -= stateControllerRef.OpenLogin;
        Debug.Log("Desubscibed");
    }
}