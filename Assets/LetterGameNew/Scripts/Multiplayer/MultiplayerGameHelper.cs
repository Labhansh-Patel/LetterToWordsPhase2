using System;
using System.Collections;
using System.Collections.Generic;
using APICalls;
using ExitGames.Client.Photon;
using GameEvents;
using Networking;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public partial class MultiplayerGameHelper : MonoBehaviour
    {
        [SerializeField] private GamePlayController _gamePlayController;
        [SerializeField] private NetworkManager _networkManager;
        public MultiplayerGameUI _multiplayerGameUI;
        [SerializeField] private int fastGameTime = 600;


        [FormerlySerializedAs("RequiredPlayers")] [SerializeField] [Range(1, 5)]
        private int requiredPlayers = 2;

        [SerializeField] private int scoreSubmitTime = 20;
        [SerializeField] private int extendedTimeCount = 20;

        private CreateMultiplayerData currentGameData = null;

        private Dictionary<Player, GameSyncData> recievedRoundScore = new Dictionary<Player, GameSyncData>();
        private Dictionary<Player, int> FinalScoreData = new Dictionary<Player, int>();
        private List<int> addExtraTimePlayers = new List<int>();
        private bool addedExtraTime = false;
        private bool blockExtraTime = false;

        private Coroutine scoreSubmitCoroutine;

        private MultiplayerGameState _multiplayerGameState;

        private void Start()
        {
            _networkManager.RoomConnectionSucess += OnRoomConnected;
            _networkManager.RoomConnectionFailed += OnRoomConnectionFailed;
            _networkManager.PlayerJoined += OnPlayerJoined;
            _networkManager.PlayerLeft += OnPlayerLeft;
            _networkManager.Disconnect += OnDisconnected;
            EventHandlerGame.ConnectMultiplayer += ConnectMultiplayer;
            EventHandlerGame.JoinRoom += JoinRoom;
            PhotonNetwork.NetworkingClient.EventReceived += OnEventRecieved;
            EventHandlerGame.ClearRoom += ClearRoom;
            EventHandlerGame.AddExtraTime += AddExtraTime;
            EventHandlerGame.BlockExtraTime += BlockExtraTime;
            _networkManager.MasterSwitched += MasterClientSwitched;
        }

        private void OnDestroy()
        {
            _networkManager.RoomConnectionSucess -= OnRoomConnected;
            _networkManager.RoomConnectionFailed -= OnRoomConnectionFailed;
            _networkManager.PlayerJoined -= OnPlayerJoined;
            _networkManager.PlayerLeft -= OnPlayerLeft;
            _networkManager.Disconnect -= OnDisconnected;
            _networkManager.MasterSwitched -= MasterClientSwitched;
            EventHandlerGame.ConnectMultiplayer -= ConnectMultiplayer;
            PhotonNetwork.NetworkingClient.EventReceived -= OnEventRecieved;
            EventHandlerGame.ClearRoom -= ClearRoom;
            EventHandlerGame.AddExtraTime -= AddExtraTime;
            EventHandlerGame.BlockExtraTime -= BlockExtraTime;
            EventHandlerGame.JoinRoom -= JoinRoom;
        }

        private void ClearRoom()
        {
            currentGameData = null;
            scoreSubmitCoroutine = null;
            recievedRoundScore.Clear();
            FinalScoreData.Clear();
            addExtraTimePlayers.Clear();
            PhotonNetwork.Disconnect();
            addedExtraTime = false;
            blockExtraTime = false;
        }

        public void SendNetworkEvents(NetworkCode EventCode, object Content, ReceiverGroup receiverGroup = ReceiverGroup.All)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = receiverGroup };
            LogSystem.LogEvent("SendNetwork Event {0}  Content {1}", EventCode, Content);
            PhotonNetwork.RaiseEvent((byte)EventCode, Content, raiseEventOptions, SendOptions.SendReliable);
        }

        private void SendDataToOthersOnGameJoin(object content)
        {
            //   object[] content = { inputMessage };
            SendNetworkEvents(NetworkCode.SendMultiplayerGameData, content);
        }

        private void OnEventRecieved(EventData eventData)
        {
            LogSystem.LogEvent("PhotonRaise Event {0}", eventData.Code);
            switch (eventData.Code)
            {
                case (byte)NetworkCode.SendRoundScoreData:
                    ManagePlayerRoundScore((string)eventData.CustomData);
                    break;
                case (byte)NetworkCode.GameSyncData:
                    HandleGameSyncData((string)eventData.CustomData);
                    break;
                case (byte)NetworkCode.StartGame:
                    StartMultiplayerGame((string)eventData.CustomData);
                    break;
                case (byte)NetworkCode.StartNextRound:
                    HandleStartNextRound();
                    break;
                case (byte)NetworkCode.ShowRoundScore:
                    StartCoroutine(ShowRoundResult(10));
                    break;
                case (byte)NetworkCode.ShowFinalResult:
                    ShowFinalScore();
                    break;
                case (byte)NetworkCode.SendAddExtraTime:
                    HandleAddExtraTime((int)eventData.CustomData);
                    break;
                case (byte)NetworkCode.RecieveAddExtraTime:
                    RecieveAddExtraTime((int)eventData.CustomData);
                    break;
                case (byte)NetworkCode.SendBlockExtraTime:
                    HandleBlockExtraTime((int)eventData.CustomData);
                    break;
                case (byte)NetworkCode.RecieveBlockExtraTime:
                    RecieveBlockExtraTime((int)eventData.CustomData);
                    break;
                case (byte)NetworkCode.SendFinalScoreData:
                    ReceiveFinalScoreData((string)eventData.CustomData);
                    break;
            }
        }

        private void ReceiveFinalScoreData(string eventDataCustomData)
        {
            LogSystem.LogEvent("FinalScoreData {0}", eventDataCustomData);

            _multiplayerGameState = MultiplayerGameState.FinalRound;
            List<FinalScoreDictData> finalScoreDictDatas =
                JsonConvert.DeserializeObject<List<FinalScoreDictData>>(eventDataCustomData);

            foreach (var finalScore in finalScoreDictDatas)
            {
                Player player = PhotonNetwork.CurrentRoom.GetPlayer(finalScore.pPhotonID);
                FinalScoreData[player] = finalScore.score;
            }

            //FinalScoreData = JsonConvert.DeserializeObject<Dictionary<Player, int>>(eventDataCustomData);
        }

        private void RecieveBlockExtraTime(int pPhotonID)
        {
            blockExtraTime = true;
            _multiplayerGameUI.IsExtraTimeBlocked(blockExtraTime);
        }

        private void HandleBlockExtraTime(int pPhotonID)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            blockExtraTime = true;
            _multiplayerGameUI.IsExtraTimeBlocked(blockExtraTime);
            SendNetworkEvents(NetworkCode.RecieveBlockExtraTime, pPhotonID, ReceiverGroup.Others);
        }

        private void RecieveAddExtraTime(int pPhotonID)
        {
            if (!addExtraTimePlayers.Contains(pPhotonID) && !blockExtraTime)
            {
                addExtraTimePlayers.Add(pPhotonID);
                if (pPhotonID == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                }
            }
        }

        private void HandleAddExtraTime(int pPhotonID)
        {
            if (!PhotonNetwork.IsMasterClient && blockExtraTime)
            {
                return;
            }

            if (!addExtraTimePlayers.Contains(pPhotonID))
            {
                addExtraTimePlayers.Add(pPhotonID);
                SendNetworkEvents(NetworkCode.RecieveAddExtraTime, pPhotonID, ReceiverGroup.Others);
            }
        }


        private void AddExtraTime()
        {
            if (recievedRoundScore.Count > 0)
            {
                if (blockExtraTime)
                {
                    EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.ExtraTimeBlocked);
                    _gamePlayController.BonusController.AddBonusCount(1);
                }
                else
                {
                    SendNetworkEvents(NetworkCode.SendAddExtraTime, PhotonNetwork.LocalPlayer.ActorNumber, ReceiverGroup.MasterClient);
                }
            }
            else
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.PowerUpOnlyUsableInMultiplayer);
                _gamePlayController.BonusController.AddBonusCount(1);
            }
        }

        private void BlockExtraTime()
        {
            if (recievedRoundScore.Count > 0)
            {
                if (blockExtraTime)
                {
                    EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.AlreadyBlockedForRound);
                    _gamePlayController.BonusController.AddBonusCount(1);
                    return;
                }

                SendNetworkEvents(NetworkCode.SendBlockExtraTime, PhotonNetwork.LocalPlayer.ActorNumber, ReceiverGroup.MasterClient);
            }
            else
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.PowerUpOnlyUsableInMultiplayer);
                _gamePlayController.BonusController.AddBonusCount(1);
            }
        }

        private void HandleStartNextRound()
        {
            _multiplayerGameState = MultiplayerGameState.RoundStarted;
            _multiplayerGameUI.ToggleRoundScorePanel(false);
        }

        private void StartMultiplayerGame(string gamesyncJson)
        {
            foreach (var pPhotonPlayer in PhotonNetwork.PlayerList)
            {
                LogSystem.LogEvent("AddedFinalScoreData");
                FinalScoreData.Add(pPhotonPlayer, 0);
            }

            EventHandlerGame.EmitEvent(GameEventType.ResetBonusCount);
            _multiplayerGameState = MultiplayerGameState.RoundStarted;
            HandleGameSyncData(gamesyncJson);
            _multiplayerGameUI.ToggleWaitingPanel(false);

            if (currentGameData.multiplayerType == MultiplayerType.FastGame)
            {
                StartCoroutine(StartFastGameTime());
            }
            else
            {
                _multiplayerGameUI.ToggleCountDownText(false);
            }
        }

        private IEnumerator StartFastGameTime()
        {
            int timeRemaining = fastGameTime;
            _multiplayerGameUI.SetShowCountDownTimer(timeRemaining, currentGameData.multiplayerType);
            _multiplayerGameUI.ToggleCountDownText(true);

            while (timeRemaining > 0)
            {
                yield return new WaitForSecondsRealtime(1);
                timeRemaining--;
                _multiplayerGameUI.SetShowCountDownTimer(timeRemaining, currentGameData.multiplayerType);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                SendNetworkEvents(NetworkCode.ShowFinalResult, null);
            }

            //_multiplayerGameUI.AddRoundScore(recievedRoundScore, FinalScoreData);
        }

        private void HandleGameSyncData(string eventDataCustomData)
        {
            GameSyncData gameSyncData = JsonConvert.DeserializeObject<GameSyncData>(eventDataCustomData);
            EventHandlerGame.EmitEvent(GameEventType.ResumeGame, gameSyncData);
            recievedRoundScore.Clear();
            addedExtraTime = false;
            blockExtraTime = false;
            addExtraTimePlayers.Clear();
        }

        private void ManagePlayerRoundScore(string roundJson)
        {
            LogSystem.LogEvent("PlayerRoundScoreJson {0}", roundJson);
            MultiplayerRoundData multiplayerRoundData = JsonConvert.DeserializeObject<MultiplayerRoundData>(roundJson);

            Player player = PhotonNetwork.CurrentRoom.GetPlayer(multiplayerRoundData.photonID);
            _gamePlayController.SetCurrentGameRoundData(multiplayerRoundData.GameSyncData);
            LogSystem.LogEvent("Player {0} Round Score {1} ", player.NickName, multiplayerRoundData.GameSyncData.TurnScore);
            if (recievedRoundScore.ContainsKey(player) && currentGameData.multiplayerType == MultiplayerType.WordToWord)
            {
                LogSystem.LogErrorEvent("The Player Score is already Added for the round");
                return;
            }

            multiplayerRoundData.GameSyncData.recievedDataTime = DateTime.Now;

            _multiplayerGameState = MultiplayerGameState.WaitingForScoreSubmission;
            if (currentGameData.multiplayerType == MultiplayerType.FastGame)
            {
                recievedRoundScore.Clear();
            }

            recievedRoundScore.Add(player, multiplayerRoundData.GameSyncData);
            FinalScoreData[player] += multiplayerRoundData.GameSyncData.TurnScore;


            if (currentGameData.multiplayerType == MultiplayerType.FastGame)
            {
                return;
            }

            HandleWordToWordGameRound();
        }

        private void HandleWordToWordGameRound()
        {
            if (recievedRoundScore.ContainsKey(PhotonNetwork.LocalPlayer))
            {
                _multiplayerGameUI.AddRoundScore(recievedRoundScore, FinalScoreData, currentGameData.multiplayerType);
            }

            //LogSystem.LogEvent("RoundScore Count {0} PlayerLength {1}", recievedRoundScore.Count, PhotonNetwork.PlayerList.Length);

            if (recievedRoundScore.Count >= PhotonNetwork.PlayerList.Length)
            {
                LogSystem.LogEvent("StartTheNextRound");

                StopCoroutine(scoreSubmitCoroutine);

                if (PhotonNetwork.IsMasterClient)
                {
                    SendNetworkEvents(NetworkCode.ShowRoundScore, null);
                }
            }

            if (recievedRoundScore.Count == 1)
            {
                scoreSubmitCoroutine = StartCoroutine(StartScoreSubmitTimer(scoreSubmitTime));
            }
        }

        private IEnumerator StartScoreSubmitTimer(int timer)
        {
            _multiplayerGameUI.SetShowCountDownTimer(timer, currentGameData.multiplayerType);
            _multiplayerGameUI.ToggleCountDownText(true);
            _multiplayerGameUI.ToggleBonusResult(true);
            _multiplayerGameUI.IsExtraTimeBlocked(blockExtraTime);
            while (timer > 0)
            {
                yield return new WaitForSecondsRealtime(1);
                timer--;

                _multiplayerGameUI.SetShowCountDownTimer(AddExtraTime(timer), currentGameData.multiplayerType);
            }

            CheckForExtraTime();
        }

        private int AddExtraTime(int time)
        {
            var temp = time;
            if (IsExtraTimeAvailableForPlayer)
            {
                return temp += extendedTimeCount;
            }
            else
            {
                return temp;
            }
        }

        public bool IsExtraTimeAvailableForPlayer =>
            addExtraTimePlayers.Contains(PhotonNetwork.LocalPlayer.ActorNumber) && !addedExtraTime;

        private void CheckForExtraTime()
        {
            if (addExtraTimePlayers.Count > 0 && !addedExtraTime)
            {
                addedExtraTime = true;
                scoreSubmitCoroutine = StartCoroutine(StartScoreSubmitTimer(extendedTimeCount));
                if (!addExtraTimePlayers.Contains(PhotonNetwork.LocalPlayer.ActorNumber) && !recievedRoundScore.ContainsKey(PhotonNetwork.LocalPlayer))
                {
                    _gamePlayController.RoundDone(true);
                    // GameSyncData gameSyncData = new GameSyncData();
                    // gameSyncData.TurnScore = 0;
                    // MultiplayerRoundData multiplayerRoundData =
                    //     new MultiplayerRoundData(PhotonNetwork.LocalPlayer.ActorNumber, gameSyncData);
                    // string roundDataJson = JsonConvert.SerializeObject(multiplayerRoundData);
                    // SendNetworkEvents(NetworkCode.SendRoundScoreData,roundDataJson);
                }
            }
            else
            {
                if (!recievedRoundScore.ContainsKey(PhotonNetwork.LocalPlayer))
                {
                    _gamePlayController.RoundDone(true);
                }

                ShowRoundScore();
            }
        }


        private void ShowRoundScore()
        {
            if (GlobalData.MultiplayerType == MultiplayerType.WordToWord)
            {
                GameUi.instance._canvasUi.GameBonous.SetActive(false);
            }
            
            _multiplayerGameUI.ToggleCountDownText(false);
            _multiplayerGameUI.ToggleBonusResult(false);
            LogSystem.LogEvent("Submit score countdownDone");
            if (PhotonNetwork.IsMasterClient)
            {
                LogSystem.LogEvent("SendRoundScore Events");
                string finalJson = MultiplayerJsonHelper.GetFinalScoreDictJson(FinalScoreData);
                SendNetworkEvents(NetworkCode.SendFinalScoreData, finalJson, ReceiverGroup.Others);
                SendNetworkEvents(NetworkCode.ShowRoundScore, null);
            }
        }

        private IEnumerator ShowRoundResult(int waitTime)
        {
            _multiplayerGameUI.ToggleCountDownText(false);
            _multiplayerGameUI.ToggleBonusResult(false);
            _multiplayerGameUI.AddRoundScore(recievedRoundScore, FinalScoreData, currentGameData.multiplayerType);
            yield return new WaitForSecondsRealtime(waitTime);
            CheckForGameEnd();
        }


        private void ShowFinalScore()
        {
            _multiplayerGameState = MultiplayerGameState.FinalRound;
            _multiplayerGameUI.AddRoundScore(recievedRoundScore, FinalScoreData, currentGameData.multiplayerType);
            _multiplayerGameUI.ToggleHomeBtn(true);
            _multiplayerGameUI.ToggleBonusResult(false);
        }


        private void CheckForGameEnd()
        {
            if (scoreSubmitCoroutine != null)
            {
                StopCoroutine(scoreSubmitCoroutine);
                scoreSubmitCoroutine = null;
            }

            bool isGameFinished = false;
            foreach (var roundScore in recievedRoundScore)
            {
                if (roundScore.Value.IsGameFinished)
                {
                    isGameFinished = true;
                    break;
                }
            }

            if (isGameFinished)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    string finalJson = MultiplayerJsonHelper.GetFinalScoreDictJson(FinalScoreData);
                    SendNetworkEvents(NetworkCode.SendFinalScoreData, finalJson,
                        ReceiverGroup.Others);
                    SendNetworkEvents(NetworkCode.ShowFinalResult, null);
                }
            }
            else
            {
                StartCoroutine(StartNextRound(5f));
            }
        }


        private IEnumerator StartNextRound(float waitTime)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            int highestScore = 0;
            GameSyncData gameSyncData = null;

            foreach (var roundScore in recievedRoundScore)
            {
                if (roundScore.Value.TurnScore > highestScore)
                {
                    highestScore = roundScore.Value.TurnScore;
                    gameSyncData = roundScore.Value;
                }
                else if (roundScore.Value.TurnScore == highestScore && gameSyncData != null)
                {
                    int comparevalue =
                        DateTime.Compare(gameSyncData.recievedDataTime, roundScore.Value.recievedDataTime);
                    if (comparevalue > 0)
                    {
                        highestScore = roundScore.Value.TurnScore;
                        gameSyncData = roundScore.Value;
                    }
                }
            }

            foreach (var roundScore in recievedRoundScore)
            {
                if (roundScore.Value.TurnScore == highestScore && roundScore.Key.NickName == GlobalData.userData.name)
                {
                    Debug.Log("Allow Bonus");
                    _gamePlayController.AllowForBonusUpdate();
                }
            }

            if (PhotonNetwork.IsMasterClient)
            {
                string finalJson = MultiplayerJsonHelper.GetFinalScoreDictJson(FinalScoreData);
                SendNetworkEvents(NetworkCode.SendFinalScoreData, finalJson,
                    ReceiverGroup.Others);
                StopAllCoroutines();

                DecideWinnerForRound(highestScore, gameSyncData);
            }
        }

        private void DecideWinnerForRound(int highestScore, GameSyncData gameSyncData)
        {
            LogSystem.LogEvent("HighestScoreForRound {0}", highestScore);
            SendGameSyncData(gameSyncData);
        }

        private void SendGameSyncData(GameSyncData gameSyncData)
        {
            string json = JsonConvert.SerializeObject(gameSyncData);

            SendNetworkEvents(NetworkCode.GameSyncData, json);
            SendNetworkEvents(NetworkCode.StartNextRound, null);
        }

        private void ConnectMultiplayer(CreateMultiplayerData gamedata)
        {
            currentGameData = gamedata;

            _networkManager.ConnectPhoton(currentGameData);
        }

        private void JoinRoom(CreateMultiplayerData data)
        {
            // string roomDataJson = (string) roomInfo.CustomProperties["gameData"];
            currentGameData = data;
            GlobalData.GameType = GameType.MultiPlayer;
            GlobalData.MultiplayerType = currentGameData.multiplayerType;
            _networkManager.JoinRoom(currentGameData.roomName);
        }
    }
}