using UnityEngine;

namespace Gameplay
{
    public class UISlots : MonoBehaviour
    {
        private bool isFree = true;

        public bool IsFree => isFree;

        public void ToggleFreeStatus(bool isFree)
        {
            this.isFree = isFree;
            if (isFree)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = Vector3.one * 1.3f;
            }
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.CompareTag("LetterTile"))
        //     {
        //         isFree = false;
        //     }
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     if (other.CompareTag("LetterTile"))
        //     {
        //         isFree = true;
        //     }
        // }
    }
}