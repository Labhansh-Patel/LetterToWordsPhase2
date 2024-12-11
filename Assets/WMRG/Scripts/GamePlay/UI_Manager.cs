using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using APICalls;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// using PushNotifications;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager _instance;


    [SerializeField] public GameObject[] Screens;
    [SerializeField] public Sprite[] Avtar;


    [HideInInspector] public int CurrentScreenNo;
    [HideInInspector] public int LastScreenNo;
    [HideInInspector] public string User_Id;
    [HideInInspector] public string User_Token;
    [HideInInspector] public string User_Signup_otp;
    [HideInInspector] public string User_Email;
    [HideInInspector] public string User_Rank;
    [HideInInspector] public int Device_Type;
    [HideInInspector] public int User_GameId;
    [HideInInspector] public string Device_Token;
    [SerializeField] public GameObject loadingabr;

    [HideInInspector] public string User_Name;
    [HideInInspector] public string User_MobNumber;

    [HideInInspector] public Sprite userprofilesprite;
    [HideInInspector] public Sprite GetUserSprite;
    [HideInInspector] public List<Sprite> leadebaordProfileImageSprite = new List<Sprite>();

    [SerializeField] public InputField profileName;
    [SerializeField] public InputField ProfileEmail;
    [SerializeField] public InputField profileMobileNo;
    public Image ProfileImage;

    public GameObject StartLodingBar;


    public Text HomeProfileName_text;
    public Image HomeProfileImage;
    public Text HomeUserLevel_text;
    public TextMeshProUGUI PrivacyPolicyDescription_text;
    public TextMeshProUGUI TermsAndConditionDescription_text;


    public InputField SignEmailInput;
    public InputField SignPswdInput;

    public InputField SinUpNameInput;
    public InputField SinUpEmailInput;
    public InputField SinUpPswdInput;
    public InputField SinUpCnfPswdInput;
    public InputField ForgotEmailInput;
    public InputField OtpTextInput;
    public InputField OtpTextforgotInput;
    public InputField NewPswdInput;
    public InputField RePswdInput;
    public InputField ProfileNameInput;
    public InputField PrivateJoinInputField;


    public InputField SubjectInputField;
    public InputField MessegeInputField;
    public Text SubjectInputFieldPlaceholder;
    public Text MessegeInputFieldPlaceholder;

    public InputField SearchInputField;


    [SerializeField] public static string[] LeaderbaordProfileName;

    [SerializeField] public static string[] LeaderbaordProfileRank;

    [SerializeField] public static string[] LeaderbaordProfileImage;

    [SerializeField] public static string[] AvtarImage;
    [SerializeField] public Text errorPopUp_Text;
    [SerializeField] public GameObject ErrorPopup;
    [SerializeField] public GameObject ExitPopup;
    public GameObject LogOutExitPanel;
    [SerializeField] public GameObject WaitforOtherPanel;
    [SerializeField] public GameObject PlayerList;
    [SerializeField] public GameObject NotificationPoint;
    [SerializeField] public GameObject AvtarPanel;
    [SerializeField] public GameObject PrivateRoomJoinPanel;
    [SerializeField] public GameObject MultiplayerRoundPanel;
    [SerializeField] public GameObject InviteAndPlayButton;
    [SerializeField] public GameObject RoundTimmer;
    public GameObject JoinPrivateRoomPopup;

    [SerializeField] public GameObject SearchingPlayerPanel;
    [SerializeField] public TextMeshProUGUI Timmer_Text;
    [SerializeField] public TextMeshProUGUI SearchOpponentTimmer_Text;


    [SerializeField] public TextMeshProUGUI GameStartTimmer;

    [SerializeField] public TextMeshProUGUI FriendListMessege;

    [SerializeField] public TextMeshProUGUI searchFriendMessege;

    [SerializeField] public TextMeshProUGUI acquireTile;


    [SerializeField] public Button AddButton;

    [SerializeField] public Text[] MultiplayerPlayerName;
    private List<SearchFriend> PlayerFriendList = new List<SearchFriend>();
    private List<GameObject> PlayerFriendPrefab = new List<GameObject>();
    private List<GameObject> PlayerGameList = new List<GameObject>();
    private List<GameObject> GameDateList = new List<GameObject>();


    private List<GameObject> NotificationPlayerdPrefab = new List<GameObject>();

    private List<GameObject> FriendListPrefab = new List<GameObject>();
    private List<FriendList> MyFriendList = new List<FriendList>();
    private List<string> AddNotificationId = new List<string>();
    private List<GameObject> LeaderBoardPrefabList = new List<GameObject>();

    public GameObject searchPlayerAddPrefab;

    public GameObject AddFriendPanel;
    public GameObject AddGamelistPanel;
    public GameObject MyFriendListPrefab;
    public GameObject AddMyFriendListPanel;
    public GameObject PendingfriendPrefab;
    public GameObject GameListPrefab;
    public GameObject PendingFriendtPanel;
    public GameObject LeaderBoardPrefab;
    public GameObject LeaderboardParent;
    public Text LeaderBoardUserRank;

    public GameObject GameDateParent;
    public GameObject GameDatePrefab;


    public Text searchPlayerName_Text;

    [SerializeField] public GameObject _SinglePlayerResult;
    [SerializeField] public TextMeshProUGUI SingleplayerResultText;
    [SerializeField] public Text[] _multiplayerName;
    [SerializeField] public Text[] _MultiplayerPlayerScore;
    [SerializeField] public Text[] _multiplayerTurnName;
    [SerializeField] public Text[] _MultiplayerPlayerTurnScore;
    [SerializeField] public TextMeshProUGUI TurnLetter;
    [SerializeField] public TextMeshProUGUI TurnStartTimmer;

    [SerializeField] private List<GameObject> RoomNamePreFabList;

    public GameObject WorldResult;
    [SerializeField] public GameObject ShowPlayerNameButton;


    [SerializeField] public GameObject _multiplayerResult;

    [SerializeField] public GameObject RoomNamePreFab;

    [SerializeField] public GameObject RoomNameParent;

    public GameObject SearchPlayerNamePrefab;
    public GameObject SearchPlayerNameParent;
    public GameObject PrivateSearchPlayerParent;
    private List<GameObject> SearchPlayerGameObject = new List<GameObject>();
    private List<GameObject> PrivateSearchPlayerGameObject = new List<GameObject>();
    public TextMeshProUGUI SearchPrivateRoomName;
    public Button PrivateStartButton;
    public Button PrivateShareButton;
    public Button PlayerNameButton;


    private string Friendid;
    private string actionuserid;
    [SerializeField] private int AvtarId = 1;
    [SerializeField] public string PrivateRoomName;
    [SerializeField] public string JoinRoomName = string.Empty;

    public float _time = 30;
    private float _Minutes;
    private float _Seconds;
    [HideInInspector] public int NumberOfPlayerInvited = 0;

    public bool _IsTimmer = false;
    private bool islogedIn = false;
    public bool _IsTurnTimmer = false;

    private PhotonView photonView;


    public enum ScreenNames
    {
        login,
        SignUp,
        Forgotpassword,
        ContactUs,
        Profile,
        Setting,
        Home,
        Multichosse,
        Leaderbaord,
        friendList,
        Addfriend,
        Game,
        Privacy,
        term,
        ResetPassword,
        Otp,
        Notification,
        Gamelist,
        GameScreen,
        privateroomjoin,
        RoomList,
        selectPrivateMode,
        selectPrivateSearchPlayer,
        selectPublicMode
    }


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        // OneSignalManager.OnInitialized += OneSignalManager_OnInitialized;
        // OneSignalManager.initialize();
        // OneSignalManager.OnNotificationReceived += NotificationRecived;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
            OpenLoginScreen();
        }
        else
        {
            Device_Token = SystemInfo.deviceUniqueIdentifier;
            Debug.Log("User_Token" + Device_Token);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            if (photonView == null)
            {
                photonView = GetComponent<PhotonView>();
            }

            DeviceInfo();


            // islogin = PlayerPrefs.GetInt("loginstatus", 0);
            if (PlayerPrefs.GetInt("Loginstatus") == 1)
            {
                // CurrentScreenNo = 8;


                // User_Name =
                SearchInputField.onValueChanged.AddListener(HandlesearchInput);
                User_Id = PlayerPrefs.GetString("PlayerId");
                Debug.Log("User_Id...." + User_Id);

                // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallGetProfile ();
                ApiManager.GetProfile(User_Id, HandleProfileCall);
                islogedIn = true;
                OpenHomeScreen();
            }
            else
            {
                OpenLoginScreen();
                // CloseLogoutConfirmationPopup();
            }
        }
    }

    private void HandlesearchInput(string arg0)
    {
        Debug.Log(" changeValue........" + arg0);

        if (arg0 == "")
        {
            Debug.Log(" changeValue.." + PlayerFriendList.Count);
            for (int i = 0; i < PlayerFriendPrefab.Count; i++)
            {
                Destroy(PlayerFriendPrefab[i]);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        // ify)
        // {
        //     Debug.Log("OneSignalManager......,"+OneSignalManager.UserId);
        // }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            PrivateRoomName = "29";
            //PrivateRoomjoin(true);
        }
    }


    public void ClosePrivateRoom()
    {
        PrivateRoomjoin(false);
    }

    private void OneSignalManager_OnInitialized(bool asuccess)
    {
        if (asuccess)
            StartCoroutine(WaitUntilLogin());
    }

    private void NotificationRecived(string aTitle, string aBody, Dictionary<string, object> aPayload)
    {
        Debug.Log("RoomName" + aPayload["RoomName"]);
        PrivateRoomName = aPayload["RoomName"].ToString();
        ApiManager.GetProfile(User_Id, HandleProfileCall);
        PrivateRoomjoin(true);
    }

    public void PrivateRoomJoin()
    {
        if (PrivateJoinInputField.text.Length == 0)
        {
            ShowErrorPopup("Please Enter Room Name");
        }
        else
        {
            PrivateRoomName = PrivateJoinInputField.text;
            // ApiManager.GetProfile(User_Id, HandleProfileCall);
            //PrivateRoomjoin(true);
            CloseJoinPrivateRoomPopup();
            PhotonConnection._instance.MultiplayerPrivateJoin();
        }
    }

    public void PrivateRoomCreate()
    {
        if (User_Id.Length > 2)
        {
            PrivateRoomName = (User_Name).Substring(0, 2) + (User_Id).Substring(User_Id.Length - 2);
        }
        else
        {
            PrivateRoomName = (User_Name).Substring(0, 2) + User_Id;
        }

        PhotonConnection._instance.MultiplayerPrivateGame();
    }


    private IEnumerator WaitUntilLogin()
    {
        yield return new WaitUntil(() => islogedIn);
        //Updatenotification(OneSignalManager.UserId);
    }

    public void ActivateScreen(int no)
    {
        ClearAllInput();
        SoundManager.instance.PlayBtnSoud();
        for (int i = 0; i < Screens.Length; i++)
        {
            if (Screens[i].activeInHierarchy)
            {
                LastScreenNo = i;
            }
        }

        for (int i = 0; i < Screens.Length; i++)
        {
            Screens[i].SetActive(false);
        }

        Screens[no].SetActive(true);
        CurrentScreenNo = no;
        //Debug.Log("lastScreenNo........"+LastScreenNo+"currentScreenNo........"+CurrentScreenNo);
    }
    //............Device info............................//

    public void DeviceInfo()
    {
        // string deviceName = DeviceName.getDeviceName();
        //   Debug.Log("deviceName........................"+deviceName);
        //Device_Type = SystemInfo.deviceType ;
        if (Device.GetPlatform() == "Android")
        {
            Device_Type = 1;
        }
        else if (Device.GetPlatform() == "iOS")
        {
            Device_Type = 2;
        }
        else
        {
            Device_Type = 3;
        }

        // Debug.Log("device type............................"+ Device.GetPlatform());
    }

    //........Internet connection check...................//
    public bool CheckInternetConnection()
    {
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }
    //............Clear All Inputs..................//

    void ClearAllInput()
    {
        Debug.Log("celled");
        SignEmailInput.text = "";
        SignPswdInput.text = "";
        SinUpNameInput.text = "";
        SinUpEmailInput.text = "";
        SinUpPswdInput.text = "";
        SinUpCnfPswdInput.text = "";
        ForgotEmailInput.text = "";
        // OtpTextInput.text ="";
        // OtpTextforgotInput.text="";
        NewPswdInput.text = "";
        RePswdInput.text = "";
    }

    //.........Email Validate.............//
    bool EmailValidate(string _email)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        Match match = regex.Match(_email);
        if (match.Success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void CloseWordResult()
    {
        WorldResult.SetActive(false);
    }

    public void RoundStartTimmer(bool sucess)
    {
        RoundTimmer.SetActive(sucess);
    }

    public void OpenLoginScreen()
    {
        ActivateScreen((int)ScreenNames.login);
    }


    public void OpenSignupScreen()
    {
        ActivateScreen((int)ScreenNames.SignUp);
    }


    public void OpenForgotPasswordScreen()
    {
        ActivateScreen((int)ScreenNames.Forgotpassword);
    }

    public void OpenContactUsScreen()
    {
        ActivateScreen((int)ScreenNames.ContactUs);
    }


    public void OpenProfileScreen()
    {
        ActivateScreen((int)ScreenNames.Profile);
        //RewardedAdsScript._instance.ShowRewardedVideo();
    }


    public void OpenSettingScreen()
    {
        ActivateScreen((int)ScreenNames.Setting);
    }


    public void OpenHomeScreen()
    {
        ActivateScreen((int)ScreenNames.Home);
        ApiManager.PendingFriendList(User_Id, HandlePendingListCall);
    }


    public void OpenNOtificationScreen()
    {
        PendingFriendList();
        ActivateScreen((int)ScreenNames.Notification);
    }


    public void OpenGameListScreen()
    {
        GameController.gameType = GameController.GameType.SinglePlayer;
        ActivateScreen((int)ScreenNames.Gamelist);
        PlayerNameButton.gameObject.SetActive(false);
    }


    public void OpenMultichooseScreen()
    {
        //RewardedAdsScript._instance.ShowRewardedVideo();
        ActivateScreen((int)ScreenNames.Multichosse);
        PlayerNameButton.gameObject.SetActive(true);
    }


    public void OpenLeaderbaordScreen()
    {
        ActivateScreen((int)ScreenNames.Leaderbaord);
    }


    public void OpenFriendListScreen()
    {
        ShowPlayerFriendList();
        ActivateScreen((int)ScreenNames.friendList);
    }


    public void OpenAddFriedndScreen()
    {
        SearchInputField.text = " ";
        for (int i = 0; i < PlayerFriendPrefab.Count; i++)
        {
            Destroy(PlayerFriendPrefab[i]);
        }

        ActivateScreen((int)ScreenNames.Addfriend);
    }

    public void OpengameScreen()
    {
        ActivateScreen((int)ScreenNames.Game);
    }


    public void OpenTermScreen()
    {
        ActivateScreen((int)ScreenNames.term);
    }


    public void OpenPrivacyScreen()
    {
        ActivateScreen((int)ScreenNames.Privacy);
    }

    public void OpenResetPasswordScreen()
    {
        ActivateScreen((int)ScreenNames.ResetPassword);
    }

    public void LoadGameScreen()
    {
        Debug.Log("gamescene");
        ActivateScreen((int)ScreenNames.GameScreen);
    }


    public void OpenOtpScreen()
    {
        ActivateScreen((int)ScreenNames.Otp);
    }

    public void SelectPrivateMode()
    {
        ActivateScreen((int)ScreenNames.selectPrivateMode);
    }

    public void SelectPublicMode()
    {
        ActivateScreen((int)ScreenNames.selectPublicMode);
    }


    public void OpenSearchPrivatePlayer()
    {
        ActivateScreen((int)ScreenNames.selectPrivateSearchPlayer);
    }

    public void OpenJoinPrivateRoomPopup()
    {
        JoinPrivateRoomPopup.SetActive(true);
    }

    public void CloseJoinPrivateRoomPopup()
    {
        JoinPrivateRoomPopup.SetActive(false);
    }

    public void PrivateRoomjoin(bool sucess)
    {
        // ActivateScreen((int)ScreenNames.privateroomjoin);
        PrivateRoomJoinPanel.SetActive(sucess);
    }

    public void OpenRoomListScreen()
    {
        ActivateScreen((int)ScreenNames.RoomList);
        Debug.Log("ROOMLIST");
    }

    public void OpenLogOutExitPanel()
    {
        LogOutExitPanel.SetActive(true);
    }

    public void LogOutExitYesButton()
    {
        // SceneManager.LoadScene("UI");
        // Invoke("Load",0.1f);
        // ResetAllValue();
        LogOutExitPanel.SetActive(false);
        LogoutButtonPressed();
        //OpenHomeScreen();
    }

    public void LogOutExitNoButton()
    {
        LogOutExitPanel.SetActive(false);
    }

    public void openExitPopUp()
    {
        ExitPopup.SetActive(true);
    }

    public void ExitYesButton()
    {
        ExitPopup.SetActive(false);
        Application.Quit();
    }

    public void ExitNoButton()
    {
        ExitPopup.SetActive(false);
    }

    public void LoadingBarStatus(bool status)
    {
        if (status == true)
        {
            loadingabr.SetActive(true);
        }
        else
            loadingabr.SetActive(false);
    }


    public void ShowErrorPopup(string messege)
    {
        errorPopUp_Text.text = messege;
        ErrorPopup.SetActive(true);
    }


    public void TossLetterMessege()
    {
        ShowErrorPopup("Please drag and drop letter");
    }

    public void OpenAvtarPanel()
    {
        AvtarPanel.SetActive(true);
    }


    public void CloseAvtarPanel()
    {
        AvtarPanel.SetActive(false);
        ;
    }

    public void GetAvtarId(Button btn)
    {
        AvtarId = int.Parse(btn.name);
        ProfileImage.sprite = Avtar[AvtarId - 1];
        CloseAvtarPanel();
    }


    public void CloseErrorPopup()
    {
        ErrorPopup.SetActive(false);
    }


    public void ShowUserDetailOnHOme()
    {
        HomeProfileName_text.text = User_Name;
        profileName.text = User_Name;
        profileMobileNo.text = User_MobNumber;
        ProfileEmail.text = User_Email;
        HomeProfileImage.sprite = Avtar[AvtarId - 1];
        ProfileImage.sprite = Avtar[AvtarId - 1];
    }

    public void GetMultiplayerTurnPanel(bool susses)
    {
        MultiplayerRoundPanel.SetActive(susses);
    }


    //............Contact us ...................//

    public void FeedBackButtonPressed()
    {
        // GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayBgMusic();
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            if (SubjectInputField.text.Length == 0)
                ShowErrorPopup("\nPlease fill subject");

            else if (MessegeInputField.text.Length == 0)
                ShowErrorPopup("\n Please fill messege");

            else
            {
                // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallContactAdmin(SubjectInputField.text ,MessegeInputField.text); 
                LoadingBarStatus(true);

                ApiManager.ContactAdmin(SubjectInputField.text, MessegeInputField.text, User_Id, HandleContactAdminCall);
            }
        }
    }


    public void PrivacyPolicyPressed()
    {
        // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallPrivacyPolicy();
        ApiManager.PrivacyPolicy(HandlePrivacyCall);
    }


    public void Termconditionressed()
    {
        // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallTermsCondition();
        ApiManager.TermsCondition(HandleTermCall);
    }

    //...........Sin in ....................//
    public void SignInButtonPressed()
    {
        // GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayClip(1);
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            if (SignEmailInput.text.Length == 0)
                ShowErrorPopup("\nPlease fill email");

            else if (SignPswdInput.text.Length == 0)
                ShowErrorPopup("\n Please fill password");

            else if (!EmailValidate(SignEmailInput.text))
                ShowErrorPopup("\n Invalid email Format");

            else
            {
                User_Email = SignEmailInput.text;
                // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallSignIn( SignEmailInput.text ,SignPswdInput.text); 
                LoadingBarStatus(true);

                ApiManager.LoginUser(SignEmailInput.text, SignPswdInput.text, Device_Type.ToString(), Device_Token, HandleLoginCall);

                // ApiManager.LoginUser(SignEmailInput.text ,SignPswdInput.text,)
            }
        }
    }

    //............SinUp...................//
    public void SingUpButtonPressed()
    {
        // GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayClip(1);
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            if (SinUpNameInput.text.Length == 0)
                ShowErrorPopup(" Please Fill Name");
            else if (SinUpEmailInput.text.Length == 0) // && match.Success)
                ShowErrorPopup(" Please fill email");

            else if (SinUpPswdInput.text.Length == 0)
                ShowErrorPopup(" Please fill password");


            else if (SinUpCnfPswdInput.text.Length == 0)
                ShowErrorPopup(" Please fill confirm password");

            else if (!EmailValidate(SinUpEmailInput.text))
                ShowErrorPopup("\n Invalid email format");
            else if (SinUpPswdInput.text != SinUpCnfPswdInput.text)
                ShowErrorPopup("Password and Confirm password does not match");

            else
            {
                LoadingBarStatus(true);

                ApiManager.SingUp(SinUpNameInput.text, SinUpEmailInput.text, SinUpPswdInput.text, SinUpCnfPswdInput.text, Device_Type.ToString(), Device_Token, HandleSinUpCall);
                // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallSingUp(SinUpNameInput.text,SinUpEmailInput.text,SinUpPswdInput.text,SinUpCnfPswdInput.text );
            }
        }
    }
    //.................Forgot Password...............//

    public void ForgotPswdButtonPressed()
    {
        //GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayClip(1);

        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            if (ForgotEmailInput.text.Length == 0)
                ShowErrorPopup("\nPlease fill email");

            else if (!EmailValidate(ForgotEmailInput.text))
                ShowErrorPopup("\n Invalid email format");

            else
            {
                //GameObject.Find("Manager").GetComponent<WebServiceManager>().Call_ForgotPassword (ForgotEmailInput.text); 
                LoadingBarStatus(true);
                ApiManager.ForgotPassword(ForgotEmailInput.text, Device_Type.ToString(), Device_Token, HandleForgotPswdCall);
            }
        }
    }
    //.............Verify otp......................//

    public void VerifyOtpButtonPressed()
    {
        //GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayClip(1);
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            Debug.Log("otp " + User_Signup_otp);
            if (OtpTextInput.text.Length == 0)
                ShowErrorPopup("Please fill OTP");
            else
            {
                //GameObject.Find("Manager").GetComponent<WebServiceManager>().CallVerifyOtp (User_Email,OtpTextInput.text );
                Debug.Log("OtpTextInput.text.." + OtpTextInput.text);
                Debug.Log("OtpTextInput.text.." + User_Email);
                LoadingBarStatus(true);
                ApiManager.VerifyOtp(User_Email, OtpTextInput.text, HandleVerifyOtpCall);
            }
        }
    }

    //...........Change Password......................//
    public void ChangePasswordButtonPressed()
    {
        //GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayClip(1);

        if (CheckInternetConnection() == false)
        {
            Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            //   GameObject.Find("Manager").GetComponent<WebServiceManager>().CallChangePassword(NewPswdInput.text, RePswdInput.text);

            Debug.Log("enter  sdsd" + NewPswdInput.text);
            if (NewPswdInput.text.Length == 0)
                ShowErrorPopup(" Please fill new password");

            else if (RePswdInput.text.Length == 0)
                ShowErrorPopup(" Confirm password");
            else if (NewPswdInput.text != RePswdInput.text)
                ShowErrorPopup("Password and Confirm password does not match");

            else
            {
                //GameObject.Find("Manager").GetComponent<WebServiceManager>().CallChangePassword(NewPswdInput.text, RePswdInput.text);
                LoadingBarStatus(true);

                ApiManager.ChangePassword(NewPswdInput.text, RePswdInput.text, User_Id, Device_Type.ToString(), Device_Token, HandleChangePswdCall);
            }
        }
    }

    //....................LogOut.................//

    public void LogoutButtonPressed()
    {
        //GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayClip(1);
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallLogout ();
            LoadingBarStatus(true);

            ApiManager.Logout(User_Id, HandleLogoutCall);
        }
    }

    //.......Privacypolicy......................//
    public void PrivacyPolicyButtonPressed()
    {
        //GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayClip(1);;
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            //GameObject.Find("Manager").GetComponent<WebServiceManager>().CallPrivacyPolicy ();
            LoadingBarStatus(true);

            ApiManager.PrivacyPolicy(HandlePrivacyCall);
        }
    }

    //...............Tearms And Condition.........................//
    public void TermsConditionButtonPressed()
    {
        Debug.Log("IsRunning");
        // GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayClip(1);;
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            //GameObject.Find("Manager").GetComponent<WebServiceManager>().CallTermsCondition ();
            LoadingBarStatus(true);

            ApiManager.TermsCondition(HandleTermCall);
        }
    }

    //...................Profile open....................//
    public void ProfileButtonPressed()
    {
        // GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayClip(1);


        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            OpenProfileScreen();
            LoadingBarStatus(true);

            ApiManager.GetProfile(User_Id, HandleProfileCall);

            //GameObject.Find("Manager").GetComponent<WebServiceManager>().CallGetProfile ();

            // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallGetAvtarImage ();
        }
    }

    //..............Update ................................//
    public void UpdateSaveButtonPressed()
    {
        //GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayClip(1);;

        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            if (ProfileNameInput.text.Length == 0)
                ShowErrorPopup("\nPlease fill name");

            else
            {
                // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallUpdateProfile(ProfileNameInput.text,AvatarUrl);
                LoadingBarStatus(true);

                ApiManager.UpdateProfile(ProfileNameInput.text, profileMobileNo.text, AvtarId.ToString(), User_Id, Device_Type.ToString(), Device_Token, HandleUpdateProfileCall);
            }
        }
    }

    //..........User Rank.................//
    public void UserRankButtonPressed()
    {
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            // LoadingBarStatus(true);
            // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallGetUserRank();
            LoadingBarStatus(true);

            ApiManager.GetUserRank(HandleUseRanktCall);
        }
    }

    //............Resend Otp.............//

    public void ResendOtpButtonPressed()
    {
        // GameObject.Find("ButtonSound").GetComponent<ButtonSound>().PlayClip(1);
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallResendOtp (User_Email); 
            LoadingBarStatus(true);
            ApiManager.ResendOtp(User_Email, Device_Type.ToString(), Device_Token, HandleResendOtpCall);
        }
    }


    public void SearchFriend()
    {
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallResendOtp (User_Email);
            // PlayerFriendList.Clear();
            LoadingBarStatus(true);
            ApiManager.SearchFriend(searchPlayerName_Text.text, HandleSearchCall);
        }
    }

    public void ShowPlayerFriendList()
    {
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            LoadingBarStatus(true);

            ApiManager.MyFriendList(User_Id, HandleFriendListCall);
        }
    }


    public void SendFriendRequest(string friendid)
    {
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            LoadingBarStatus(true);

            ApiManager.SendFriendRequest(User_Id, friendid, User_Id, HandleSendFriendRequestCall);
        }
    }

    public void PendingFriendList()
    {
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            LoadingBarStatus(true);

            ApiManager.PendingFriendList(User_Id, HandlePendingListCall);
        }
    }

    public void AcceptFriendRequest()
    {
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            LoadingBarStatus(true);

            ApiManager.AcceptFriendRequest(Friendid, User_Id, "1", HandleAcceptFriendRequestCall);
        }
    }

    public void RejectFriendRequest()
    {
        if (CheckInternetConnection() == false)
        {
            // Debug.Log("not internet connection");
            ShowErrorPopup("\n No internet connection");
        }
        else
        {
            LoadingBarStatus(true);

            ApiManager.AcceptFriendRequest(Friendid, User_Id, "2", HandleAcceptFriendRequestCall);
        }
    }


    public void Updatenotification(string NotificationId)
    {
        LoadingBarStatus(true);
        ApiManager.UpdateNotification(User_Id, NotificationId, HandleUpdateNotification);
    }

    public void AddNotificationID(string _id)
    {
        Debug.Log("id.," + _id);
        if (NumberOfPlayerInvited < 5)
        {
            AddNotificationId.Add(_id);
        }
        else
        {
            ShowErrorPopup("Only five friend you have add");
        }
    }


    public void RemoveNotificationId(string _id)
    {
        AddNotificationId.Remove(_id);
    }


    public void InviteFriend()
    {
        if (AddNotificationId.Count > 0)
        {
            LoadingBarStatus(true);
            Dictionary<string, object> _invitefriend = new Dictionary<string, object>();
            _invitefriend.Add("RoomName", User_Id);

            //OneSignalManager.sendNotificationEN(AddNotificationId, User_Name + " Invite To Join Letter To Word", " Please join", _invitefriend);
            PhotonConnection._instance.MultiplayerPrivateGame();
            NumberOfPlayerInvited = AddNotificationId.Count;
            AddNotificationId.Clear();
        }
        else
        {
            ShowErrorPopup("Please select your friend");
        }
    }


    public void CreateGame(int gametype, string date)
    {
        DateTime time = System.DateTime.Now;
        Debug.Log("date.." + date);
        LoadingBarStatus(true);
        string game_mode = "1";
        ApiManager.CreateGame(User_Id, gametype.ToString(), time.ToString(), date, game_mode, 1, HandleCreateGame);
    }


    public void Solo()
    {
        // RewardedAdsScript._instance.ShowRewardedVideo();
        ShowDatesList();
        ShowGameList();
    }


    public void ShowGameList()
    {
        LoadingBarStatus(true);
        ApiManager.GameList(User_Id, HandleGameList);
    }

    public void ShowDatesList()
    {
        LoadingBarStatus(true);
        ApiManager.GetDates(HandleGetDates);
    }

    private void HandleGetDates(bool aSucess, GetDatesHeader callback)
    {
        UI_Manager._instance.LoadingBarStatus(false);
        if (aSucess)
        {
            for (int i = 0; i < GameDateList.Count; i++)
            {
                Destroy(GameDateList[i]);
            }

            if (callback.result.Count > 0)
            {
                float x = GameDateParent.transform.position.x;
                float y = GameDateParent.transform.position.y;


                for (int i = 0; i < callback.result.Count; i++)
                {
                    GameObject intrestdata = Instantiate(GameDatePrefab) as GameObject;

                    GameDateList.Add(intrestdata);
                    intrestdata.transform.SetParent(GameDateParent.transform);
                    intrestdata.transform.localScale = Vector3.one;
                    intrestdata.transform.localPosition = new Vector3(x, y);
                    intrestdata.GetComponent<DatesPrefab>().SetDataPrefab(callback.result[i]);

                    intrestdata.SetActive(true);
                    y -= 40;
                }
            }
        }
        else
        {
        }
    }

    public void ShowGameData(string GameId)
    {
        LoadingBarStatus(true);

        ApiManager.GameData(GameId, HandleGameData);
    }


    public void OpenNotificationPoint(bool _sucess)
    {
        NotificationPoint.SetActive(_sucess);
    }

    public void CallResultApi(String GameId, String UserId, String Win, int score)
    {
        LoadingBarStatus(true);

        ApiManager.GameComplete(GameId, UserId, Win, score, HandleGameComplete);
        // OpenHomeScreen();
    }


    public void CallLeaderBoard()
    {
        ApiManager.LeaderBoard(User_Id, HandLeaderBoard);
    }


    public void ShowActiveRoomName(List<RoomInfo> roomList)
    {
        for (int i = 0; i < RoomNamePreFabList.Count; i++)
        {
            Destroy(RoomNamePreFabList[i]);
        }

        float x = RoomNameParent.transform.position.x;
        float y = RoomNameParent.transform.position.y;

        foreach (RoomInfo item in roomList)
        {
            Debug.Log("room list name   " + item.Name);

            GameObject intrestdata = Instantiate(RoomNamePreFab) as GameObject;

            RoomNamePreFabList.Add(intrestdata);
            intrestdata.transform.SetParent(RoomNameParent.transform);
            intrestdata.transform.localScale = Vector3.one;
            intrestdata.transform.localPosition = new Vector3(x, y);
            intrestdata.GetComponent<RoomListPrefab>().SetDataPrefab(item);
            intrestdata.SetActive(true);
            y -= 40;
        }

        LoadingBarStatus(false);
    }


    private IEnumerator StartTimmer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            _time = 2;
        }

        Debug.Log("_time......." + _time);

        while (_time > 0)
        {
            yield return new WaitForSeconds(1);
            _time--;
            Timmer_Text.text = "You have  " + _time + " sec to left ";
            //Timmer_Text.text= _time.ToString() + " sec to left ";
        }

        Timmer_Text.text = " ";

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("36");
            PhotonNetwork.CurrentRoom.IsVisible = true;

            GameController.data.SearchHeighScore();
        }
    }


    public void Timmer()
    {
        if (_IsTimmer)
            return;
        Debug.Log("37");
        StartCoroutine(StartTimmer());
        _IsTimmer = true;
    }
    //.........Private search Back............//

    public void BackPrivateMode()
    {
        PhotonNetwork.Disconnect();
        SelectPrivateMode();
    }


    //......single player result........//
    public void singlePlayersesult(string messege)
    {
        SingleplayerResultText.text = messege;
        _SinglePlayerResult.SetActive(true);
    }


    public void ClosesinglePlayersesult()
    {
        _SinglePlayerResult.SetActive(false);
        _multiplayerResult.SetActive(false);
        OpenHomeScreen();
    }


    public void CloseMultiplayerResultPanel()
    {
        _multiplayerResult.SetActive(false);
        OpenHomeScreen();
        // PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }


    public void CloseAllPanel(bool sucess)
    {
        CloseMultiplayerResultPanel();
        MultiPlayerList(sucess);
        GetMultiplayerTurnPanel(sucess);
        _SinglePlayerResult.SetActive(false);
        WaitforOtherPanel.SetActive(false);
        Timmer_Text.text = " ";
    }

    //.......Play Solo Game...................//
    public void Load()
    {
        Debug.Log("Load");
        LoadGameScreen();
        GameController.data.StartGame();

        //Screens[17].SetActive(false);
    }

//....Wait for other Panel......//
    public void AfterCompleteWord(bool _bool)
    {
        WaitforOtherPanel.SetActive(_bool);
        // Timmer_Text.text=" ";
    }

//.........Searching Player..................//
    public void SearchPlayer(bool _bool)
    {
        Debug.Log("SearchPlayer");
        SearchingPlayerPanel.SetActive(_bool);
    }

//............MultiPlayer List......//
    public void MultiPlayerList(bool _bool)
    {
        ShowPlayerName();


        PlayerList.SetActive(_bool);
    }

    public void CanclePlayerList()
    {
        MultiPlayerList(false);
    }

    //..Show player in game ...//
    public void ShowNameInGame()
    {
        MultiPlayerList(true);
    }

    public void ShowPlayerName()
    {
        int i = 0;
        for (int j = 0; j < MultiplayerPlayerName.Length; j++)
        {
            MultiplayerPlayerName[j].text = " ";
        }

        GameStartTimmer.text = " ";
        Debug.Log(PhotonNetwork.PlayerList.Length);
        foreach (var p1 in PhotonNetwork.PlayerList)
        {
            if (p1.CustomProperties.ContainsKey("PlayerName"))
            {
                MultiplayerPlayerName[i].text = p1.NickName;
                i++;
            }
        }
    }


    public void PrivateSearchingPlayerName()
    {
        OpenSearchPrivatePlayer();

        foreach (GameObject item in PrivateSearchPlayerGameObject)
        {
            Destroy(item);
        }

        float x = PrivateSearchPlayerParent.transform.position.x;
        float y = PrivateSearchPlayerParent.transform.position.y;
        foreach (var p1 in PhotonNetwork.PlayerList)
        {
            GameObject intrestdata = Instantiate(SearchPlayerNamePrefab) as GameObject;
            intrestdata.transform.SetParent(PrivateSearchPlayerParent.transform);
            intrestdata.transform.localScale = Vector3.one;
            intrestdata.transform.localPosition = new Vector3(x, y);
            PrivateSearchPlayerGameObject.Add(intrestdata);

            intrestdata.transform.GetComponent<TextMeshProUGUI>().text = p1.NickName;
            intrestdata.SetActive(true);
        }

        SearchPrivateRoomName.text = "Room Name :- " + PrivateRoomName;
    }

    public void SearchingPlayerName()
    {
        foreach (GameObject item in SearchPlayerGameObject)
        {
            Destroy(item);
        }

        float x = SearchPlayerNameParent.transform.position.x;
        float y = SearchPlayerNameParent.transform.position.y;
        foreach (var p1 in PhotonNetwork.PlayerList)
        {
            GameObject intrestdata = Instantiate(SearchPlayerNamePrefab) as GameObject;
            intrestdata.transform.SetParent(SearchPlayerNameParent.transform);
            intrestdata.transform.localScale = Vector3.one;
            intrestdata.transform.localPosition = new Vector3(x, y);
            SearchPlayerGameObject.Add(intrestdata);

            intrestdata.transform.GetComponent<TextMeshProUGUI>().text = p1.NickName;
            intrestdata.SetActive(true);
        }
    }

    public void ShowMultiplayerResultPanel()
    {
        int i = 0;
        Debug.Log("PhotonNetwork.PlayerList......" + PhotonNetwork.CurrentRoom.PlayerCount);
        foreach (var p1 in PhotonNetwork.PlayerList)
        {
            if (p1.CustomProperties.ContainsKey("PlayerName"))
            {
                _multiplayerName[i].text = p1.NickName;
                int score = 0;
                GameController.data._MultiplayerWordScoreData.TryGetValue(p1.ActorNumber, out score);
                _MultiplayerPlayerScore[i].text = score.ToString();
                i++;
            }
        }

        _multiplayerResult.SetActive(true);
    }

    public void MultiplayerTurnResult(int winnerIndex)
    {
        int i = 0;
        int j = 1;

        GameController.data.CancelLetters();
        Debug.Log("PhotonNetwork.PlayerList......" + winnerIndex);
        foreach (var p1 in PhotonNetwork.PlayerList)
        {
            int score = 0;
            string word = " ";

            _multiplayerTurnName[i].text = p1.NickName;

            GameController.data._MultiplayerWordScoreData.TryGetValue(p1.ActorNumber, out score);
            _MultiplayerPlayerTurnScore[i].text = score.ToString();
            GameController.data._MultiplayerTurnWordData.TryGetValue(p1.ActorNumber, out word);
            _multiplayerTurnName[i].gameObject.transform.GetChild(1).GetComponent<Text>().text = word;

            i++;
        }


        for (int k = i; k < _multiplayerTurnName.Length; k++)
        {
            _multiplayerTurnName[k].text = " ";
            _MultiplayerPlayerTurnScore[k].text = " ";
            _multiplayerTurnName[k].gameObject.transform.GetChild(1).GetComponent<Text>().text = " ";
        }


        if (GameController.data._MultiplayerWordScoreData.Count > 0) //_MultiplayerWordData
        {
            int number;
            int Keynum;
            //GameController.data._MultiplayerWordScoreData.ElementAt(0).Value.score;
            //GameController.data._MultiplayerWordScoreData.ElementAt(0).Key;
            GameController.data._MultiplayerWordScoreData.TryGetValue(winnerIndex, out number);
            Debug.Log("NUMBER......." + number + "   " + winnerIndex);

            /*   for (int k = 1; k <= GameController.data._MultiplayerWordScoreData.Count; k++)
               {
                   int temp;
                   // GameController.data._MultiplayerWordData.ElementAt(k).Value.score;
                   GameController.data._MultiplayerWordScoreData.TryGetValue(k+1, out temp);
                   Debug.Log("temp... ."+ temp);

                   if (number < temp)
                   {
                       number = temp;
                       Debug.Log("temp......." + number);

                       // Keynum = GameController.data._MultiplayerWordData.ElementAt(k).Key;
                   }
               }*/


            int k = 0;
            int temp = 0;
            foreach (var p1 in PhotonNetwork.PlayerList)
            {
                GameController.data._MultiplayerWordScoreData.TryGetValue(p1.ActorNumber, out temp);

                Debug.Log(" number...." + number + "  temp...." + temp);
                if (temp == number)
                {
                    TurnLetter.text = p1.NickName + " win this round  Score " + number;
                }
            }
        }


        /*      foreach (string item in GameController.data.TurnLetter)
          {
              Debug.Log(item);
                TurnLetter.text +=item;
          }*/

        TurnTimmer(winnerIndex);
    }

    public void TurnTimmer(int _WinnerIndex)
    {
        if (_IsTurnTimmer)
            return;

        StartCoroutine(StartTurnTimmer(_WinnerIndex));
        _IsTurnTimmer = true;
    }

    private IEnumerator StartTurnTimmer(int _WinnerIndex)
    {
        int _time = 20;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }

        while (_time > 0)
        {
            TurnStartTimmer.text = _time.ToString();
            yield return new WaitForSeconds(1);
            _time--;
        }

        GameController.data.SetmultiplayerBoardData(_WinnerIndex);

        GetMultiplayerTurnPanel(false);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = true;
        }
    }


//..............Multiplayer start timmer text................//
    public void MultiplayerGameStartTimmer(int _time)
    {
        Debug.Log("Time,,,,,,,,,,," + _time);
        GameStartTimmer.text = "Game Start In : " + _time.ToString();
    }

    private void HandleLoginCall(bool asucess, LoginHeader callback)
    {
        LoadingBarStatus(false);

        Debug.Log("in this");
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result.id);
            if (callback.message == "Please verify your account first to login")
            {
                Debug.Log("in this");
                OpenOtpScreen();
                User_Id = callback.result.id;
                User_Token = callback.result.token;
                SignEmailInput.text = User_Email;
                ApiManager.ResendOtp(User_Email, Device_Type.ToString(), Device_Token, HandleResendOtpCall); //_email;
                // ResendOtp(SignEmailInput.text);
                //PlayerPrefs.SetInt("Loginstatus", 1);
                //PlayerPrefs.SetString("PlayerId", login.result.id);
                //PlayerPrefs.SetString("PlayerName", );
            }


            else
            {
                User_Id = callback.result.id;
                User_Token = callback.result.token;

                // UI_Manager._instance.OpenMapScreen();
                PlayerPrefs.SetInt("Loginstatus", 1);
                PlayerPrefs.SetString(Constant.playeridlocation, callback.result.id);
                ApiManager.GetProfile(User_Id, HandleProfileCall);
                // UI_Manager._instance.HomeProfileName_text.text = login.result.name;
                OpenHomeScreen();
                islogedIn = true;
            }
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
            if (callback.message == "Please verify your account first to login")
            {
                Debug.Log("in this");
                OpenOtpScreen();
                User_Id = callback.result.id;
                User_Token = callback.result.token;
                SignEmailInput.text = User_Email;
                ApiManager.ResendOtp(User_Email, Device_Type.ToString(), Device_Token, HandleResendOtpCall); //_email;
                // ResendOtp(SignEmailInput.text);
                //PlayerPrefs.SetInt("Loginstatus", 1);
                //PlayerPrefs.SetString("PlayerId", login.result.id);
                //PlayerPrefs.SetString("PlayerName", );
            }
        }
    }


    public void CallSingUp(string name, string email, string pass, string confPswd)
    {
        // ApiManager.SingUp (name, email, pass, confPswd , HandleSinUpCall);
    }

    private void HandleSinUpCall(bool asucess, SingUpHeader callback)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result.id);
            User_Signup_otp = callback.result.SingUp_Otp;
            User_Id = callback.result.id;
            User_Token = callback.result.token;
            User_Email = SinUpEmailInput.text;
            // PlayerPrefs.SetInt("Loginstatus", 1);
            // PlayerPrefs.SetString("PlayerId", callback.result.id);
            OpenOtpScreen();
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }


    public void Call_ForgotPassword(string email)
    {
        // ApiManager.ForgotPassword(email,HandleForgotPswdCall);
    }

    private void HandleForgotPswdCall(bool asucess, ForgotPasswordHeader callback)
    {
        LoadingBarStatus(false);
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result.id);
            UI_Manager._instance.OpenLoginScreen();
            PlayerPrefs.SetString("PlayerId", UI_Manager._instance.User_Id);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }

    private void HandleVerifyOtpCall(bool asucess, verifyOTPHeader callback)
    {
        LoadingBarStatus(false);
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
            User_Id = callback.result.userid;
            User_Token = callback.result.access_token;


            if (LastScreenNo == 2)
            {
                OpenResetPasswordScreen();
            }
            else
            {
                ApiManager.GetProfile(User_Id, HandleProfileCall);
                OpenHomeScreen();
                PlayerPrefs.SetString("PlayerId", callback.result.userid);
                PlayerPrefs.SetInt("Loginstatus", 1);


                islogedIn = true;
            }
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }

    private void HandleResendOtpCall(bool asucess, ResendOtpHeader callback)
    {
        LoadingBarStatus(false);
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);

            User_Email = callback.result.Email;
            //PlayerPrefs.SetString("PlayerId", callback.result.userid);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }

    private void HandleUpdateProfileCall(bool asucess, UpdateProfileHeader callback)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
            User_Id = callback.result.id;
            HomeProfileName_text.text = callback.result.name;
            User_Name = callback.result.name;
            User_Email = callback.result.email;
            User_MobNumber = callback.result.mobile;
            AvtarId = int.Parse(callback.result.avatar_id);


            ShowUserDetailOnHOme();
            // DownloadUserImage(callback.result.user_image);
            //yield return new WaitForSeconds(4);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }

    IEnumerator LoadUserProfileImage(string _url)
    {
        Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        WWW Link = new WWW(_url);
        yield return Link;
        Link.LoadImageIntoTexture(tex);
        // UI_Manager._instance.userprofilesprite.Add(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0)));
        UI_Manager._instance.userprofilesprite = (Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0)));
        Debug.Log("sdsad");
        // StartCoroutine("UserImage");
    }

    public void DownloadUserImage(string _url)
    {
        StartCoroutine(LoadUserProfileImage(_url));
    }

    private void HandlePrivacyCall(bool asucess, PrivacyPolicyHeader callback)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
            OpenPrivacyScreen();
            PrivacyPolicyDescription_text.text = callback.result.short_description;
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
        }
    }

    private void HandleTermCall(bool asucess, TermsConditionHeader callback)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
            OpenTermScreen();
            TermsAndConditionDescription_text.text = callback.result.short_description;
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
        }
    }

    private void HandleChangePswdCall(bool asucess, ChangePasswordHeader callback)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
            ShowErrorPopup(callback.message);
            PlayerPrefs.SetInt("Loginstatus", 1);
            PlayerPrefs.SetString("PlayerId", callback.result.id);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
        }
    }

    private void HandleLogoutCall(bool asucess, LogoutHeader callback)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.message);
            UI_Manager._instance.OpenLoginScreen();
            PlayerPrefs.DeleteAll();
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }

    private void HandleProfileCall(bool asucess, GetProfileHeader profileResponse)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", profileResponse.result.name);

//            // UI_Manager._instance.HomeProfileName_text.text = profile.result.name;
            User_Name = profileResponse.result.name;
            User_Email = profileResponse.result.email;
            User_MobNumber = profileResponse.result.mobile;
            AvtarId = int.Parse(profileResponse.result.avatar_id);


//            // SceneNevegation._instance.UnlockLevels();

//             //UI_Manager.GetuserimageUrl=profile.result.user_image;
//            // DownloadProfileImage(profile.result.user_image);
//             yield return new WaitForSeconds(1f);


            ShowUserDetailOnHOme();
            PlayerPrefs.SetString("PlayerName", profileResponse.result.name);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", profileResponse.message);
            // ShowErrorPopup(profileResponse.message);
            PlayerPrefs.DeleteAll();
            OpenLoginScreen();
        }
    }


    private void HandleUseRanktCall(bool asucess, GetUserRankeHeader rank)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}");
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", rank.message);
        }
    }


    private void HandleContactAdminCall(bool asucess, ContactAdmintHeader callback)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);

            ShowErrorPopup(callback.message);
            SubjectInputField.text = " ";
            MessegeInputField.text = " ";
            OpenSettingScreen();
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
        }
    }


    private void HandleGameStateCall(bool asucess, GameStateDataHeader callback)
    {
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
        }
    }


    private void HandleSendFriendRequestCall(bool asucess, SendFriendRequestHeader callback)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);

            Friendid = callback.result.FriendID;
            actionuserid = callback.result.ActionUserID;
            ShowErrorPopup(callback.message);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }

    private void HandleAcceptFriendRequestCall(bool asucess, AcceptFriendRequestHeader callback)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
            ShowErrorPopup(callback.message);
            ApiManager.PendingFriendList(User_Id, HandlePendingListCall);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }

    private void HandleRejectFriendRequestCall(bool asucess, RejectFriendRequestHeader callback)
    {
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
        }
    }

    private void HandleFriendListCall(bool asucess, MyFriendListHeader FriendList)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", FriendList.error);
            FriendListMessege.text = " ";

            for (int i = 0; i < FriendListPrefab.Count; i++)
            {
                Destroy(FriendListPrefab[i]);
            }

            NumberOfPlayerInvited = 0;
            float x = AddMyFriendListPanel.transform.position.x;
            float y = AddMyFriendListPanel.transform.position.y;

            if (FriendList.result.Count > 0)
            {
                InviteAndPlayButton.SetActive(true);
                for (int i = 0; i < FriendList.result.Count; i++)
                {
                    GameObject intrestdata = Instantiate(MyFriendListPrefab) as GameObject;
                    intrestdata.name = FriendList.result[i].friend.name;
                    intrestdata.transform.SetParent(AddMyFriendListPanel.transform);
                    intrestdata.transform.localScale = Vector3.one;
                    intrestdata.transform.localPosition = new Vector3(x, y);
                    FriendListPrefab.Add(intrestdata);
                    // intrestdata.transform.GetChild(0).GetComponent<Text>().text =  FriendList.result[i].friend.name;
                    intrestdata.GetComponent<MyFriendListPrefab>().SetDataPrefab(FriendList.result[i]);
                    intrestdata.SetActive(true);
                    y -= 40;
                }
            }
            else
            {
                FriendListMessege.text = "There is no Friend ";
                InviteAndPlayButton.SetActive(false);
            }
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", FriendList.message);
            ShowErrorPopup(FriendList.message);
            InviteAndPlayButton.SetActive(false);
        }
    }


    private void HandlePendingListCall(bool asucess, PendingFriendListHeader Pending)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            //LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}",Pending.result);
            for (int i = 0; i < NotificationPlayerdPrefab.Count; i++)
            {
                Destroy(NotificationPlayerdPrefab[i]);
            }

            float x = PendingFriendtPanel.transform.position.x;
            float y = PendingFriendtPanel.transform.position.y;
            if (Pending.result != null)
            {
                if (Pending.result.Count > 0)
                {
                    OpenNotificationPoint(true);
                    for (int i = 0; i < Pending.result.Count; i++)
                    {
                        GameObject intrestdata = Instantiate(PendingfriendPrefab) as GameObject;
                        intrestdata.name = Pending.result[i].user_data.id;
                        intrestdata.transform.SetParent(PendingFriendtPanel.transform);
                        intrestdata.transform.localScale = Vector3.one;
                        intrestdata.transform.localPosition = new Vector3(x, y);
                        NotificationPlayerdPrefab.Add(intrestdata);
                        intrestdata.transform.GetChild(0).GetComponent<Text>().text = Pending.result[i].user_data.name;
                        if (Pending.result[i].user_data.avatar_id != null)
                            intrestdata.transform.GetChild(3).GetComponent<Image>().sprite = Avtar[int.Parse(Pending.result[i].user_data.avatar_id)];

                        intrestdata.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
                        {
                            Debug.Log("Clicked !........" + intrestdata.name);
                            Friendid = intrestdata.name;
                            AcceptFriendRequest();
                        });
                        intrestdata.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                        {
                            Debug.Log("Clicked !........" + intrestdata.name);
                            Friendid = intrestdata.name;
                            RejectFriendRequest();
                        });

                        intrestdata.SetActive(true);
                        y -= 40;
                    }
                }
            }
            else
            {
                if (CurrentScreenNo == 16)
                {
                    ShowErrorPopup(Pending.message);
                }

                OpenNotificationPoint(false);
            }
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", Pending.message);
            if (CurrentScreenNo == 16)
            {
                ShowErrorPopup(Pending.message);
            }

            OpenNotificationPoint(false);
        }
    }


    private void HandleSearchCall(bool asucess, SearchFriendListHeader search)
    {
        LoadingBarStatus(false);
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", search.result[0].id);
            searchFriendMessege.text = " ";


            for (int i = 0; i < PlayerFriendPrefab.Count; i++)
            {
                Destroy(PlayerFriendPrefab[i]);
            }


            if (search.result.Count > 0)
            {
                float x = AddFriendPanel.transform.position.x;
                float y = AddFriendPanel.transform.position.y;

                for (int i = 0; i < search.result.Count; i++)
                {
                    GameObject intrestdata = Instantiate(searchPlayerAddPrefab) as GameObject;

                    PlayerFriendPrefab.Add(intrestdata);
                    intrestdata.transform.SetParent(AddFriendPanel.transform);
                    intrestdata.transform.localScale = Vector3.one;
                    intrestdata.transform.localPosition = new Vector3(x, y);
                    intrestdata.GetComponent<FriendListPrefab>().SetDataPrefab(search.result[i]);

                    /*  Button butt = AddButton.GetComponent<Button>();
                      butt.onClick.AddListener(() => {
                          Debug.Log("Clicked !");
                          SendFriendRequest();

                      });*/
                    intrestdata.SetActive(true);
                    y -= 40;
                }
            }
            else
            {
                searchFriendMessege.text = "There is no Friend ";
                ShowErrorPopup(search.message);
            }
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", search.message);
            ShowErrorPopup(search.message);
        }
    }


    private void HandleUpdateNotification(bool asucess, UpdateNotificationHeader callback)
    {
        LoadingBarStatus(false);
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "UpdateNotification  ID : {0}", callback.message);
            // ShowErrorPopup(callback.message);
        }
        else
        {
            LogSystem.LogColorEvent("red", "UpdateNotification  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }


    private void HandleCreateGame(bool asucess, CreateGameData callback)
    {
        LoadingBarStatus(false);
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "UpdateNotification  ID : {0}", callback.message);
            // ShowErrorPopup(callback.message);
            User_GameId = int.Parse(callback.game_id);
            // Load();
        }
        else
        {
            LogSystem.LogColorEvent("red", "UpdateNotification  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }

    private void HandleGameList(bool asucess, GameListHeader callback)
    {
        LoadingBarStatus(false);
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "UpdateNotification  ID : {0}", callback.message);
            // ShowErrorPopup(callback.message);

            for (int i = 0; i < PlayerGameList.Count; i++)
            {
                Destroy(PlayerGameList[i]);
            }

            if (callback.game_list.Count > 0)
            {
                float x = AddGamelistPanel.transform.position.x;
                float y = AddGamelistPanel.transform.position.y;


                for (int i = 0; i < callback.game_list.Count; i++)
                {
                    if (callback.game_list[i].game_type == "1")
                    {
                        GameObject intrestdata = Instantiate(GameListPrefab) as GameObject;

                        PlayerGameList.Add(intrestdata);
                        intrestdata.transform.SetParent(AddGamelistPanel.transform);
                        intrestdata.transform.localScale = Vector3.one;
                        intrestdata.transform.localPosition = new Vector3(x, y);
                        intrestdata.GetComponent<GameListPrefab>().SetDataPrefab(callback.game_list[i]);


                        /*  Button butt = AddButton.GetComponent<Button>();
                          butt.onClick.AddListener(() => {
                              Debug.Log("Clicked !");
                              SendFriendRequest();

                          });*/
                        intrestdata.SetActive(true);
                        y -= 40;
                    }
                }
            }

            OpenGameListScreen();
        }
        else
        {
            LogSystem.LogColorEvent("red", "UpdateNotification  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }

    private void HandleGameData(bool asucess, GameDataHeader callback)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "UpdateNotification  ID : {0}", callback.message);
            // ShowErrorPopup(callback.message);
            User_GameId = callback.game_data.id;
            if (callback.game_data.game_data != null)
            {
                //GameController.data.GetResumeData(callback.game_data.game_data);
                //LoadGameScreen();
            }
            else
            {
                Load();
            }

            // GameController.data.StartGame();
        }
        else
        {
            LogSystem.LogColorEvent("red", "UpdateNotification  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }

    private void HandleGameComplete(bool asucess, GameComplete callback)
    {
        LoadingBarStatus(false);

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "UpdateNotification  ID : {0}", callback.message);
            // ShowErrorPopup(callback.message);
        }
        else
        {
            LogSystem.LogColorEvent("red", "UpdateNotification  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }

    private void HandLeaderBoard(bool aSucess, LeaderBoardHeader callback)
    {
        LoadingBarStatus(false);

        if (aSucess)
        {
            LogSystem.LogColorEvent("green", "LeaderBoard  ID : {0}", callback.message);
            for (int i = 0; i < LeaderBoardPrefabList.Count; i++)
            {
                Destroy(LeaderBoardPrefabList[i]);
            }

            if (callback.data.Count > 0)
            {
                float x = LeaderboardParent.transform.position.x;
                float y = LeaderboardParent.transform.position.y;


                for (int i = 0; i < callback.data.Count; i++)
                {
                    GameObject intrestdata = Instantiate(LeaderBoardPrefab) as GameObject;

                    LeaderBoardPrefabList.Add(intrestdata);
                    intrestdata.transform.SetParent(LeaderboardParent.transform);
                    intrestdata.transform.localScale = Vector3.one;
                    intrestdata.transform.localPosition = new Vector3(x, y);
                    intrestdata.GetComponent<LeaderBoardPrefab>().SetDataPrefab(callback.data[i]);


                    intrestdata.SetActive(true);
                    y -= 40;
                }
            }
            // GameUi.instance.ShowLodingBar(false);
            // HandleEvents.ChangeStates(States.LeaderBoard);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LeaderBoard  ID : {0}", callback.message);
            ShowErrorPopup(callback.message);
        }
    }
}
//................End Ui_manager.......//

public class SearchFriend
{
    public string id;
    public string name;

    public SearchFriend(string _id, string _name)
    {
        id = _id;
        name = _name;
    }
}

public class FriendList
{
    public string id;
    public string user_id;
    public string friend_id;
    public string action_user_id;
    public string status;
    public string name;

    public FriendList(string _id, string _name, string _user_id, string _friend_id, string _action_user_id, string _status)
    {
        id = _id;
        name = _name;
        user_id = _user_id;
        friend_id = _friend_id;
        action_user_id = _action_user_id;
        status = _status;
    }
}