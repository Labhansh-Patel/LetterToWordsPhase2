using System.Collections;
using GameEvents;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject pivot;
    [SerializeField] private float timeoutTime;

    private Coroutine timeout;

    // Start is called before the first frame update
    void Start()
    {
        EventHandlerGame.Loading += EnableLoading;
        EnableLoading(false);
    }

    private void OnDestroy()
    {
        EventHandlerGame.Loading -= EnableLoading;
    }

    private void EnableLoading(bool active)
    {
        pivot.gameObject.SetActive(active);
        if (active)
        {
            timeout = StartCoroutine(TimeoutTimer());
        }
        else
        {
            HandleTimeoutRunner();
        }
    }

    private void HandleTimeoutRunner()
    {
        if (timeout != null)
        {
            StopCoroutine(timeout);
            timeout = null;
        }
    }

    private IEnumerator TimeoutTimer()
    {
        yield return new WaitForSeconds(timeoutTime);
        EnableLoading(false);
    }
}