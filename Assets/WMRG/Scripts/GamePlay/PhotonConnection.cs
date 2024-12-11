using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Networking;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonConnection : MonoBehaviour

{
    public static PhotonConnection _instance;

    public PhotonView photonView;
    private string JoinRoomName;
    private string CreateRoomName;
    private string roomName; //string for saving room name
    private bool isConnecting;
    private bool _isTimmer;
    private Coroutine timmerCorutine;
    private NetworkManager Manager;


    private int roomSize = 2; //int for saving room size
    private List<RoomInfo> roomListings = new List<RoomInfo>(); //list of current rooms
    [SerializeField] private Transform roomsContainer; //container for holding all the room listings

    private GameObject roomListingPrefab;

    // Action Events for callback
    public event Action RoomConnectionSucess;
    public event Action RoomConnectionFailed;
    public event Action<Player> PlayerLeft;
    public event Action<Player> PlayerJoined;
    public event Action<DisconnectCause> Disconnect;

    public event Action MasterSwitched;

    private bool _IsTimmer = false;
    private float WaitTime;
    private bool isStartGame = false;
    private string _currentDate;

    public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    private string mLobbyName;

    public string SetLobbyName
    {
        get { return mLobbyName; }
        set { mLobbyName = value; }
    }

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        Manager = GetComponent<NetworkManager>();
        SubscribeNetwork();
    }

    // Update is called once per frame

    private void SubscribeNetwork()
    {
        Manager.RoomConnectionSucess += OnJoinedRoom;
        Manager.PlayerJoined += OnPlayerEnteredRoom;
        Manager.PlayerLeft += OnPlayerLeftRoom;
        Manager.Disconnect += OnDisconnected;
    }

    public void MultiplayerPublicGame()
    {
        PhotonNetwork.Disconnect();
        SetLobbyName = "Public";

        UI_Manager._instance.LoadingBarStatus(true);
        GameController._multiplayergameType = GameController.MultiplayerGameType.PublicMultiplayer;
        ConnectPhotonServer();
        WaitTime = 30;
        _IsTimmer = false;
    }

    public void MultiplayerPrivateGame()
    {
        PhotonNetwork.Disconnect();
        SetLobbyName = "Private";
        Manager.isPrivateJoin = false;
        GameController._multiplayergameType = GameController.MultiplayerGameType.PrivateMultiplayer;
        ConnectPhotonServer();
        WaitTime = 120;
        _IsTimmer = false;
        UI_Manager._instance.PrivateStartButton.gameObject.SetActive(true);
        UI_Manager._instance.PrivateShareButton.gameObject.SetActive(true);
    }

    public void MultiplayerPrivateJoin()
    {
        UI_Manager._instance.LoadingBarStatus(true);

        PhotonNetwork.Disconnect();
        SetLobbyName = "Private";
        Manager.isPrivateJoin = true;
        GameController._multiplayergameType = GameController.MultiplayerGameType.PrivateMultiplayer;
        ConnectPhotonServer();
        WaitTime = 120;
        _IsTimmer = false;
        UI_Manager._instance.PrivateStartButton.gameObject.SetActive(false);
        UI_Manager._instance.PrivateShareButton.gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        PhotonNetwork.Disconnect();
        SetLobbyName = "CreateJoin";
        UI_Manager._instance.LoadingBarStatus(true);
        GameController._multiplayergameType = GameController.MultiplayerGameType.JoinRoomMultiplayer;
        ConnectPhotonServer();
        UI_Manager._instance.OpenRoomListScreen();
        WaitTime = 0;
        _IsTimmer = false;
        timmerCorutine = null;
    }


    public void CreateMultiplayerGame()
    {
        PhotonNetwork.Disconnect();
        SetLobbyName = "CreateJoin";
        UI_Manager._instance.LoadingBarStatus(true);
        GameController._multiplayergameType = GameController.MultiplayerGameType.CreateMultiplayer;
        ConnectPhotonServer();
        WaitTime = 30;
        _IsTimmer = false;
    }

    public void ShowRoomList()
    {
        PhotonNetwork.Disconnect();
        SetLobbyName = "CreateJoin";
        UI_Manager._instance.LoadingBarStatus(true);
        GameController._multiplayergameType = GameController.MultiplayerGameType.nulls;
        ConnectPhotonServer();
        UI_Manager._instance.OpenRoomListScreen();
    }


    public void ConnectPhotonServer()
    {
        // UI_Manager._instance.LoadingBarStatus(false);
        // PhotonNetwork.NickName = 
        // SetPlayerCustomPropeties();

        GameController.gameType = GameController.GameType.Multiplayer;
        // Debug.Log(" GameController.gameType........."+ GameController.gameType);
        isStartGame = false;
        //Manager.ConnectPhoton();
    }


    /* public override void OnJoinedLobby()
     {
         base.OnJoinedLobby();
         Debug.Log("Join to  Lobby.......................");
         //Join the layer in lobby
        if(  GameController._multiplayergameType ==GameController.MultiplayerGameType.PublicMultiplayer)
        {
              Debug.Log("public.......................");
           PhotonNetwork.JoinRandomRoom();
        }


        // UIManager._instance.OpenPlayerMatchScreen();
         if ( GameController._multiplayergameType==GameController.MultiplayerGameType.PrivateMultiplayer)
         {
             if(UI_Manager._instance.PrivateRoomName==null)
             {
               PhotonNetwork.JoinRoom(UI_Manager._instance.User_Id);
             }
             else{

                  PhotonNetwork.JoinRoom(UI_Manager._instance.User_Id);
                  Debug.Log("........... "+UI_Manager._instance.PrivateRoomName);
               }



                 Debug.Log("private.......................");

         }

     }
    */

    IEnumerator CheckIfPlayerNotFound()
    {
        UI_Manager._instance.LoadingBarStatus(false);
        UI_Manager._instance.SearchPlayer(true);
        UI_Manager._instance.SearchOpponentTimmer_Text.text = "Please wait for opponent - " + (int)WaitTime / 60 + ":" + (WaitTime % 60).ToString();

        while (WaitTime > 0)
        {
            yield return new WaitForSeconds(1);
            WaitTime--;
            UI_Manager._instance.SearchOpponentTimmer_Text.text = "Please wait for opponent - " + (int)WaitTime / 60 + ":" + (WaitTime % 60);
        }
        // yield return new WaitForSeconds (WaitTime);


        if (GameController._multiplayergameType == GameController.MultiplayerGameType.PublicMultiplayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
            {
                UI_Manager._instance.SearchPlayer(false);
                UI_Manager._instance.ShowErrorPopup("Player Not Found");

                PhotonNetwork.Disconnect();
            }
            else
            {
                SetTimmerBoolean(true);
            }
        }
        else
        {
            SetTimmerBoolean(true);
        }
    }

    private void SetTimmerBoolean(bool _timmer)
    {
        _isTimmer = _timmer;


        if (_isTimmer == true && PhotonNetwork.CurrentRoom.PlayerCount < 6 && PhotonNetwork.CurrentRoom.PlayerCount >= 1 && PhotonNetwork.IsMasterClient)
        {
            if (timmerCorutine != null)
            {
                StopCoroutine(timmerCorutine);
                Debug.Log("Stop corutine  player not found");
            }

            if (GameController._multiplayergameType == GameController.MultiplayerGameType.PublicMultiplayer)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                // PhotonNetwork.CurrentRoom.IsVisible = false;
            }

            _currentDate = System.DateTime.Now.ToString("yyyy/MM/dd");
            Debug.Log(_currentDate);

            photonView.RPC("StartGameCall", RpcTarget.All, _currentDate);
        }
    }


    private IEnumerator StartTimmer()
    {
        int _time = 10;
        Debug.Log("OnPlayerEnteredRoom......................." + _time);
        timmerCorutine = null;

        while (_time > 0)
        {
            UI_Manager._instance.MultiplayerGameStartTimmer(_time);
            yield return new WaitForSeconds(1);
            _time--;
        }


        if (_time == 0)
        {
            UI_Manager._instance.MultiPlayerList(false);
        }
    }


    public void Timmer()
    {
        if (_IsTimmer)
            return;

        StartCoroutine(StartTimmer());
        _IsTimmer = true;
    }


    public void OnJoinRandomFailed(short so, string s)
    {
        OnClick_JoinRoom();
    }


    public void OnJoinedRoom()
    {
        Debug.LogFormat("JOINED ROOM {0} {1} {2}", PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.IsOpen, PhotonNetwork.CurrentRoom.IsVisible);

        if (RoomConnectionSucess != null)
            RoomConnectionSucess();


        if (GameController._multiplayergameType != GameController.MultiplayerGameType.PrivateMultiplayer)
        {
            SetTimmerBoolean(false);
            timmerCorutine = StartCoroutine(CheckIfPlayerNotFound());
        }
        else
        {
            UI_Manager._instance.PrivateSearchingPlayerName();
        }


        Debug.Log(" PhotonNetwork.CurrentRoom.PlayerCount;........." + PhotonNetwork.CurrentRoom.PlayerCount);
        if (GameController._multiplayergameType == GameController.MultiplayerGameType.PrivateMultiplayer)
        {
            // PhotonNetwork.CurrentRoom.IsVisible = false;
        }

        UI_Manager._instance.SearchingPlayerName();
    }


    public void OnClick_JoinRoom()
    {
        RoomOptions ro = new RoomOptions { IsOpen = true, MaxPlayers = (byte)2 };
        PhotonNetwork.CreateRoom(null, ro, TypedLobby.Default);
    }


    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("NEW PLAYER JOINED  ROOM {0}", newPlayer.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            if (GameController._multiplayergameType == GameController.MultiplayerGameType.PrivateMultiplayer && isStartGame == false)
            {
                //  Debug.Log(" private players name........" + PhotonNetwork.LocalPlayer.NickName);
                //  Debug.Log("PlayerCount..." + PhotonNetwork.CurrentRoom.PlayerCount + "... NumberOfPlayerInvited......" + UI_Manager._instance.NumberOfPlayerInvited);
                //  if ((PhotonNetwork.CurrentRoom.PlayerCount - 1) == UI_Manager._instance.NumberOfPlayerInvited)
                //{
                //  SetTimmerBoolean(true);
                UI_Manager._instance.PrivateSearchingPlayerName();

                Debug.Log("players name........" + PhotonNetwork.LocalPlayer.NickName);
            }


            if (timmerCorutine == null && isStartGame == true)
            {
                // string json = JsonConvert.SerializeObject(GameController.data.Previousdata, jsonSettings);
                // Debug.LogFormat("Json {0}", json);
                Debug.Log("players name........" + GameController.data.Previousdata);
                // byte[] bytes = Encoding.ASCII.GetBytes(GameController.data.Previousdata);
                photonView.RPC("CallPlayerjoinRoom", RpcTarget.Others, newPlayer, GameController.data.Previousdata, timmerCorutine, _currentDate);

                //  photonView.RPC("Test", RpcTarget.All, newPlayer);
            }
        }

        UI_Manager._instance.SearchingPlayerName();
    }


    public void CallPrivatePlayerRpc()
    {
        photonView.RPC("StartGameCall", RpcTarget.All);
    }


    [PunRPC]
    public void StartGameCall(string currentDate)
    {
        UI_Manager._instance.SearchPlayer(false);
        UI_Manager._instance.MultiPlayerList(true);
        UI_Manager._instance.LoadingBarStatus(false);

        Debug.LogFormat(" Start Game Function Called ");
        Debug.LogFormat(" PhotonNetwork.IsMasterClient " + PhotonNetwork.IsMasterClient);
        _isTimmer = false;
        Timmer();
        isStartGame = true;

        CommonApi.CallDateWiseData(currentDate);
    }


    [PunRPC]
    public void Test(Player newPlayer)
    {
        Debug.Log("newPlayer  ," + newPlayer.NickName);
    }

    [PunRPC]
    public void CallPlayerjoinRoom(Player newPlayer, byte[] bytesData, Coroutine _coroutine, string date)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newPlayer.ActorNumber)
        {
            Debug.Log("players name........");
            string json = Encoding.ASCII.GetString(bytesData);
            Debug.LogFormat("JsonRecieved {0}", json);
            GameStateData _Newdata = JsonConvert.DeserializeObject<GameStateData>(json);
            if (json != string.Empty)
            {
                GameController.data.GetResumeData(_Newdata);
                UI_Manager._instance.LoadingBarStatus(false);
                UI_Manager._instance.SearchPlayer(false);
            }
            else
            {
                // _IsTimmer = true;
                StartGameCall(date);
            }
        }
    }


    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PlayerLeft != null)
            PlayerLeft(otherPlayer);
    }

    public void OnPhotonPlayerConnected(Player player)
    {
        Debug.Log("Player Connected " + player.NickName);
    }


    public void OnDisconnectedFromPhoton(Player player)
    {
        Debug.Log("Player Disconnected " + player.NickName);
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        if (MasterSwitched != null) MasterSwitched();
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Write device info
            //stream.SendNext (deviceUID);
            // stream.SendNext (deviceModel);
            //stream.SendNext (deviceName);
        }
        else
        {
            // Read device info
            //deviceUID = (string)stream.ReceiveNext ();
            // deviceModel = (string)stream.ReceiveNext ();
            //deviceName = (string)stream.ReceiveNext ();
        }
    }


    public void GetPlayerCustomPropeties()
    {
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;

        Debug.LogError("Disconnected. Please check your Internet connection." + cause);
        // UI_Manager._instance.ShowErrorPopup("Disconnected. Please check your Internet connection." + cause);
    }
}