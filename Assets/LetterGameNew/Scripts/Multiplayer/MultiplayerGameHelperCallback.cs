using System.Collections;
using APICalls;
using GameEvents;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Gameplay
{
    public partial class MultiplayerGameHelper
    {
        #region PhotonConnectionCallBack

        [SerializeField] private int matchMakingWaitTime = 30;

        private void OnRoomConnected()
        {
            Debug.LogFormat("OnRoomConnected {0}", (string)PhotonNetwork.CurrentRoom.CustomProperties["gameData"]);
            _multiplayerGameUI.ToggleRoundScorePanel(false);
            string roomDataJson = (string)PhotonNetwork.CurrentRoom.CustomProperties["gameData"];
            currentGameData = JsonConvert.DeserializeObject<CreateMultiplayerData>(roomDataJson);
            GlobalData.MultiplayerType = currentGameData.multiplayerType;
            _multiplayerGameUI.ToggleWaitingPanel(true);

            CommonApi.CreateGame(GlobalData.UserId, "2", currentGameData.time, currentGameData.date, currentGameData.gamemode, (int)currentGameData.multiplayerType, _gamePlayController);
            
            // CheckForRequiredPlayers();

            _multiplayerGameUI.UpdateConnectedPlayers();

            if (currentGameData.MultiplayerMode == MultiplayerMode.Public)
            {
                // StartCoroutine(WaitForPlayersConnect(matchMakingWaitTime));

                _multiplayerGameUI.UpdatePrivateOwner(CheckForRequiredPlayers, true);
                // _multiplayerGameUI.TogglePrivateButton(false);
            }
            else if (PhotonNetwork.CurrentRoom.Name == GlobalData.userData.PrivateGameKey)
            {
                Debug.LogFormat("--------------");
                _multiplayerGameUI.UpdatePrivateOwner
                    (CheckForRequiredPlayers, false);
            }


            if (currentGameData.MultiplayerMode == MultiplayerMode.Private)
            {
                _multiplayerGameUI.ToggleButtons();
            }
        }


        private IEnumerator WaitForPlayersConnect(int waitTime)
        {
            _multiplayerGameState = MultiplayerGameState.MatchMaking;

            yield return new WaitForSecondsRealtime(waitTime);

            CheckForRequiredPlayers();
        }

        private void OnPlayerJoined(Player player)
        {
            LogSystem.LogEvent("OnPlayerJoined");

            // CheckForRequiredPlayers();
            _multiplayerGameUI.UpdateConnectedPlayers();
        }

        private void CheckForRequiredPlayers()
        {
            LogSystem.LogEvent("CheckForRequiredPlayer");
            if (PhotonNetwork.PlayerList.Length >= requiredPlayers)
            {
                LogSystem.LogEvent("RequiredPlayersYes");
                PhotonNetwork.CurrentRoom.IsOpen = false;


                if (!PhotonNetwork.IsMasterClient) return;

                GameSyncData gameSyncData = _gamePlayController.ConstructGameSyncData();
                LogSystem.LogEvent("GameSyncData {0}", gameSyncData);
                string gameSyncJson = JsonConvert.SerializeObject(gameSyncData);
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                SendNetworkEvents(NetworkCode.StartGame, gameSyncJson);
            }
            else
            {
                LogSystem.LogEvent("RequiredPlayersNo");
                HandleEvents.PopoupErrorMsgOpen(GameMessages.CannotFindPlayers);
                EventHandlerGame.EmitEvent(GameEventType.ExitGameRoom);
            }
        }


        private void OnPlayerLeft(Player player)
        {
            if (_multiplayerGameState == MultiplayerGameState.MatchMaking)
            {
                _multiplayerGameUI.UpdateConnectedPlayers();
                return;
            }

            if (_multiplayerGameState == MultiplayerGameState.FinalRound)
            {
                return;
            }

            if (PhotonNetwork.PlayerList.Length == 1)
            {
                HandleEvents.PopoupErrorMsgOpen(GameMessages.AllPlayersDisconnected);
                SendNetworkEvents(NetworkCode.ShowFinalResult, null);
            }
        }

        private void OnDisconnected(DisconnectCause cause)
        {
            LogSystem.LogEvent("Disconnected Cause {0}", cause);
            if (cause != DisconnectCause.DisconnectByClientLogic)
            {
                HandleEvents.PopoupErrorMsgOpen(GameMessages.Disconnected);
                EventHandlerGame.EmitEvent(GameEventType.ExitGameRoom);
            }
        }


        private void OnRoomConnectionFailed()
        {
            EventHandlerGame.EmitEvent(GameEventType.Loading, false);
            HandleEvents.PopoupErrorMsgOpen(GameMessages.CannotFindRoom);
        }

        private void MasterClientSwitched()
        {
        }

        #endregion
    }
}