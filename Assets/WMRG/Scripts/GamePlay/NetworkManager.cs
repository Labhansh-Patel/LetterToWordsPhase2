#define MGM_DEBUG
//#define UseAuthValues


using System;
using System.Collections.Generic;
using APICalls;
using ExitGames.Client.Photon;
using GameEvents;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Networking
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        #region Network Variables

        [SerializeField] private string appVersion = "0.1f";

        [SerializeField] private string photonRegion = "asia";

        #endregion

        #region EVENT CALLBACKS

        // Action Events for callback
        public event Action RoomConnectionSucess;
        public event Action RoomConnectionFailed;

        public event Action<Player> PlayerLeft;
        public event Action<Player> PlayerJoined;

        public event Action<DisconnectCause> Disconnect;

        public event Action MasterSwitched;

        #endregion

        public bool isPrivateJoin;
        private DateTime currentTime;

        private CreateMultiplayerData multiplayerData;

        private void Awake()
        {
            // ConnectPhoton();
        }


        void Start()
        {
            currentTime = System.DateTime.Now;
        }


        #region PhotonConnection

        public void ConnectPhoton(CreateMultiplayerData multiplayerData)
        {
            this.multiplayerData = multiplayerData;
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
            //PhotonNetwork.NetworkingClient.LoadBalancingPeer.SocketImplementationConfig[ConnectionProtocol.Udp] = typeof(ExitGames.Client.Photon.SocketUdpSrc);


            //PhotonNetwork.AutomaticallySyncScene = true;

            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = appVersion;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = photonRegion;

#if UseAuthValues
			PhotonNetwork.AuthValues = new AuthenticationValues();
			PhotonNetwork.AuthValues.UserId = "userID";
#endif

            PhotonNetwork.ConnectUsingSettings();
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }


        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
#if MGM_DEBUG
            Debug.LogFormat("Connected to Master");
#endif


            //TODO: change lobby name 
            string lobbyName = multiplayerData.MultiplayerMode.ToString(); // "Default";

            TypedLobby typedLobby = new TypedLobby(lobbyName, LobbyType.Default);
            PhotonNetwork.JoinLobby(typedLobby);
        }

        public void SetPlayerProperties()
        {
            //TODO: Get all properties of player from  profile
            PhotonNetwork.LocalPlayer.NickName = "PlayerName";
            //PhotonNetwork.LocalPlayer.SetCustomProperties(_gameNetworkInfo.playerProp);
        }

        public void SetPlayerCustomPropeties()
        {
            PhotonNetwork.NickName = GlobalData.UserName;
            Hashtable hash = new Hashtable();
            //hash.Add("PlayerName", UI_Manager._instance.User_Name);
            // hash.Add("GameType", GameController._multiplayergameType);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        private bool IsPrivateJoin => multiplayerData.MultiplayerAction == MultiplayerAction.JoinGame &&
                                      multiplayerData.MultiplayerMode == MultiplayerMode.Private;

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            //SetPlayerProperties();
            SetPlayerCustomPropeties();
            Debug.LogFormat("<color=red> RoomNumbers {0} LobbyName{1} </color>", PhotonNetwork.CountOfRooms, PhotonNetwork.CurrentLobby.Name);


            if (multiplayerData.MultiplayerAction == MultiplayerAction.CreateGame)
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 6;
                ExitGames.Client.Photon.Hashtable myhash = new ExitGames.Client.Photon.Hashtable();
                myhash.Add("gameData", JsonConvert.SerializeObject(multiplayerData));


                roomOptions.CustomRoomPropertiesForLobby = new[] { "gameData" };

                roomOptions.CustomRoomProperties = myhash;
                // roomOptions.CustomRoomProperties["gameData"] = JsonConvert.SerializeObject(multiplayerData);
                PhotonNetwork.CreateRoom(multiplayerData.roomName, roomOptions);
            }

            if (IsPrivateJoin)
            {
                PhotonNetwork.JoinRoom(multiplayerData.roomName);
            }
        }


        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
#if MGM_DEBUG
            Debug.LogFormat("JOINED ROOM  Failed Creating One {0} {1}", message, returnCode);
#endif


            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = (byte)5;
            roomOptions.PublishUserId = true;
            //if (_gameNetworkInfo.actiavetPlayerTTL) roomOptions.PlayerTtl = 300000;
            PhotonNetwork.CreateRoom(null, roomOptions, null);
        }

        public override void OnCreatedRoom()
        {
            //we created a room so we have to set the initial room properties for this room,
            //such as populating the team fill and score arrays

            //TODO ADD ROOM PROPERTIES HERE 

            //PhotonNetwork.CurrentRoom.SetCustomProperties(_gameNetworkInfo.roomProps);
        }


        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            LogSystem.LogEvent("{0}", (string)PhotonNetwork.CurrentRoom.CustomProperties["gameData"]);


#if MGM_DEBUG
            Debug.LogFormat("JOINED ROOM {0} {1} {2}", PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.IsOpen, PhotonNetwork.CurrentRoom.IsVisible);
#endif
            if (RoomConnectionSucess != null) RoomConnectionSucess();
        }


        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            LogSystem.LogEvent("JoinRoomFailed");
            base.OnJoinRoomFailed(returnCode, message);
#if MGM_DEBUG
            Debug.LogErrorFormat("JOIN ROOM FAILED {0}", message);

#endif
            if (RoomConnectionFailed != null) RoomConnectionFailed();
            PhotonNetwork.Disconnect();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
#if MGM_DEBUG
            Debug.LogFormat("NEW PLAYER JOINED  ROOM {0}", newPlayer.NickName);
#endif

            if (PlayerJoined != null) PlayerJoined(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
#if MGM_DEBUG
            Debug.LogFormat("PLAYER LEFT ROOM {0}", otherPlayer.NickName);
#endif

            if (PlayerLeft != null) PlayerLeft(otherPlayer);

            if (GameController._multiplayergameType == GameController.MultiplayerGameType.PublicMultiplayer) //|| GameController._multiplayergameType == GameController.MultiplayerGameType.PrivateMultiplayer)
            {
                /*if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
                {
                    UI_Manager._instance.CloseAllPanel(false);
                    UI_Manager._instance.ShowErrorPopup("You win this game because Opponent player left ");
                    UI_Manager._instance.CallResultApi(UI_Manager._instance.User_GameId.ToString(), UI_Manager._instance.User_Id, "1", GameController.data.WordScore);


                }*/
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
#if MGM_DEBUG
            Debug.LogFormat("PLAYER DISCONNECTED DUE TO {0}", cause);
#endif

            if (Disconnect != null) Disconnect(cause);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            if (MasterSwitched != null) MasterSwitched();
        }


        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            //UI_Manager._instance.ShowActiveRoomName(roomList);
            LogSystem.LogEvent("RoomListUpdate {0}", roomList.Count);
            

            foreach (var roomInfo in roomList)
            {
                LogSystem.LogEvent("INFO{0}", (string)roomInfo.CustomProperties["gameData"]);
            }

            EventHandlerGame.EmitEvent(GameEventType.RoomListUpdate, roomList);
        }

        #endregion
    }
}