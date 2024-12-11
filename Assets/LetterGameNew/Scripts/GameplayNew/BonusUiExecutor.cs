using System;
using UnityEngine;

namespace Gameplay
{
    public class BonusUiExecutor : MonoBehaviour
    {
        [SerializeField] private GameObject extendedTime;
        [SerializeField] private GameObject blockExtraTime;


        private void OnEnable()
        {
            // if (GlobalData.GameMode == "hardcore" )
            // {
            //     gameObject.SetActive(false);
            // }
            EnableGamePanels(GlobalData.GameType);
        }

        public void EnableGamePanels(GameType gameType)
        {
            
            extendedTime.gameObject.SetActive(gameType == GameType.MultiPlayer);
            blockExtraTime.gameObject.SetActive(gameType == GameType.MultiPlayer);
        }
        
    }
}