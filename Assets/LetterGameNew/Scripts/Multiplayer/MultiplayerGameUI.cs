using System.Collections.Generic;
using System.Linq;
using GameEvents;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay
{
    public class MultiplayerGameUI : MonoBehaviour
    {
        [SerializeField] private GameObject scorePanel;
        [SerializeField] private Text RoundScores;
        [SerializeField] private Text roundNames;
        [SerializeField] private Text totalScore;
        [SerializeField] private GameObject waitingPanel;
        [SerializeField] private Button homeBtn;

        [SerializeField] private Text connectdPlayerUI, roomCode;
        [SerializeField] private Button shareBtn, startGameBtn;
        [SerializeField] private Text countdownShowText;
        [SerializeField] private GameObject countDownDisplay;
        [SerializeField] private GameObject joinedPlayerPrefab;
        [SerializeField] private Transform joinPlayerParent;

        [SerializeField] private GameObject blockExtraTime;
        [SerializeField] private GameObject scorePanelBonus;
        [SerializeField] private GameObject addTimeExtra;
        [SerializeField] private GameObject roomDetails;
        [SerializeField] private GameObject turnScore;

        // [SerializeField] private Button shareBtn;

        private void Start()
        {
            RestMultiplayerRoom();
            homeBtn.onClick.RemoveAllListeners();
            homeBtn.onClick.AddListener(BackToHome);
            EventHandlerGame.ExitRoom += RestMultiplayerRoom;
        }

        private void OnDestroy()
        {
            EventHandlerGame.ExitRoom -= RestMultiplayerRoom;
        }

        private void RestMultiplayerRoom()
        {
            ToggleRoundScorePanel(false);
            ToggleWaitingPanel(false);
            ToggleHomeBtn(false);
            ToggleCountDownText(false);
        }

        private void BackToHome()
        {
            EventHandlerGame.EmitEvent(GameEventType.ExitGameRoom);
        }

        public void ToggleRoundScorePanel(bool active)
        {
            scorePanel.SetActive(active);
        }

        public void ToggleBonusResult(bool active)
        {
            scorePanelBonus.SetActive(active);
        }

        public void ToggleWaitingPanel(bool active)
        {
            waitingPanel.SetActive(active);
        }


        public void ToggleHomeBtn(bool active)
        {
            homeBtn.gameObject.SetActive(active);
        }

        public void AddRoundScore(Dictionary<Player, GameSyncData> recievedRoundScore, Dictionary<Player, int> totalFinalScore, MultiplayerType multiplayerType)
        {
            RoundScores.text = string.Empty;
            roundNames.text = string.Empty;
            totalScore.text = string.Empty;
            ToggleHomeBtn(false);

            var ordered = totalFinalScore.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            foreach (var Score in ordered)
            {
                string playerScoreText = string.Empty;

                GameSyncData syncData = null;
                recievedRoundScore.TryGetValue(Score.Key, out syncData);

                var turnScore = 0;

                if (syncData == null)
                {
                    turnScore = 0;
                }
                else
                {
                    turnScore = syncData.TurnScore;
                }

                if (multiplayerType == MultiplayerType.WordToWord)
                {
                    // playerScoreText =     $"{Score.Key.NickName} \t Turn Score : {turnScore} \t TotalScore : {Score.Value}\n";
                    RoundScores.text += turnScore + "\n";
                    totalScore.text += Score.Value + "\n";
                    roundNames.text += Score.Key.NickName + "\n";
                }
                else
                {
                    // playerScoreText =
                    //    $"{Score.Key.NickName} \t   TotalScore : {totalFinalScore[Score.Key]}\n";

                    RoundScores.text += turnScore + "\n";
                    totalScore.text += Score.Value + "\n";
                    roundNames.text += Score.Key.NickName + "\n";
                }

                // RoundScores.text += playerScoreText ;
            }

            turnScore.gameObject.SetActive(multiplayerType != MultiplayerType.FastGame);

            ToggleRoundScorePanel(true);
        }

        public void UpdateConnectedPlayers()
        {
            Debug.Log("Players Count: " + PhotonNetwork.PlayerList.Length);
            int playerCount = PhotonNetwork.PlayerList.Length;
            if (playerCount == 1)
            {
                startGameBtn.gameObject.SetActive(false);
            }
            else if (playerCount > 1)
            {
                if(PhotonNetwork.IsMasterClient)
                {
                    startGameBtn.gameObject.SetActive(true);
                }
            }

            foreach (Transform c in joinPlayerParent)
            {
                Destroy(c.gameObject);
            }

            connectdPlayerUI.text = "Players In Room:";

            foreach (var player in PhotonNetwork.PlayerList)
            {
                // connectdPlayerUI.text += "\t" + player.NickName + "\n";
                GameObject go = Instantiate(joinedPlayerPrefab, transform.position, Quaternion.identity);
                go.transform.GetChild(0).GetComponent<Text>().text = player.NickName;
                go.transform.SetParent(joinPlayerParent);
                go.transform.localScale = Vector3.one;
            }

            roomCode.text = "RoomCode :" + PhotonNetwork.CurrentRoom.Name;
        }

        public void TogglePrivateButton(bool active)
        {
            shareBtn.gameObject.SetActive(active);

            if (active)
            {
                if (PhotonNetwork.PlayerList.Length == 1)
                {
                    startGameBtn.gameObject.SetActive(false);
                }
                else
                {
                    startGameBtn.gameObject.SetActive(true);
                }
            }
            else
            {
                startGameBtn.gameObject.SetActive(false);
            }
        }


        public void UpdatePrivateOwner(UnityAction startGameCallBack,
            bool publicMode)
        {
            roomDetails.gameObject.SetActive(!publicMode);

            startGameBtn.onClick.RemoveAllListeners();
            startGameBtn.onClick.AddListener(startGameCallBack);
            shareBtn.onClick.AddListener(ShareRoomCode);
            ToggleButtons();
        }

        public void ToggleButtons()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("ShareButton True");
                TogglePrivateButton(true);
            }
            else
            {
                Debug.LogFormat("ShareButtonFalse");
                TogglePrivateButton(false);
            }
        }

        private void ShareRoomCode()
        {
            NativeShare nativeShare = new NativeShare();
            nativeShare.SetSubject("Join Private Game");
            nativeShare.SetTitle("Letter To Words Private Game");
            nativeShare.SetText(
                $"{GlobalData.userData.name} invited you to join his private game use the code  : {PhotonNetwork.CurrentRoom.Name} to join the game");
            // "Dowload test link : https://play.google.com/store/apps/details?id=com.supercell.clashroyale");
            nativeShare.Share();
        }

        public void ToggleCountDownText(bool active)
        {
            countDownDisplay.gameObject.SetActive(active);
        }

        public void IsExtraTimeBlocked(bool active)
        {
            blockExtraTime.SetActive(active);
        }

        public void SetShowCountDownTimer(int remainingTime, MultiplayerType gameType)
        {
            if (gameType == MultiplayerType.FastGame)
            {
                addTimeExtra.gameObject.SetActive(false);
            }
            else
            {
                addTimeExtra.gameObject.SetActive(true);
            }
            
        

            if (gameType == MultiplayerType.FastGame)
            {
                countdownShowText.color = Color.white;
            }
            else
            {
                countdownShowText.color = Color.black;
            }

            int minutes = remainingTime / 60;
            int seconds = remainingTime % 60;

            countdownShowText.text = $" {minutes:00} : {seconds:00}";
        }
    }
}