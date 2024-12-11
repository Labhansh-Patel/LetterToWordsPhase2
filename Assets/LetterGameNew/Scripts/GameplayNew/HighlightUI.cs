using System;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class HighlightUI : MonoBehaviour
    {
        [SerializeField] private GameObject hightlight;

        private void Awake()
        {
            hightlight.gameObject.SetActive(false);
        }

        public void ActivateHighlight(CallBack callBack)
        {
            StartCoroutine(EnableHighlight(callBack));
        }

        private IEnumerator EnableHighlight(CallBack callBack)
        {
            hightlight.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            hightlight.gameObject.SetActive(false);
            callBack?.Invoke();
        }
    }
}