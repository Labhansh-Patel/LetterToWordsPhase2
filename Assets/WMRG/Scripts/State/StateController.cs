using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [SerializeField] private GameUi gameUi;

    [SerializeField] private Calendar dailyFunCalendar, hardcoreCalendar;

    [SerializeField] private GamePlayController gamePlayController;


    private Login _login;
    private SignUp _signUp;
    private Otp _otp;
    private ForgotPassword _forgotPassword;
    private HomeScreen _home;
    private GamePlay _gameplay;
    private Setting _setting;
    private Profile _profile;
    private TermsandCondition _termsandCondition;
    private PrivacyPolicy _privacyPolicy;
    private ContactUs _contactUs;
    private SelectSolo _selectSolo;
    private DailyFunGame _dailyFunGame;
    private DailyHardGame _dailyHardGame;
    private MultiplayerOption _multiplayerOption;
    private JoinMultiplayerGame _joinMultiplayerGame;
    private SearchingPlayer _searchingPlayer;
    private StartGame _startGame;
    private Achivement _achivement;
    private PurchaseIAP _purchaseIAP;
    private PopUpMessage _ErrorPopUp;
    private OpenGame _openGame;
    private AcceptingGame _acceptGame;
    private ResetPassword _resetPassword;


    private StateMachine _stateMachine = new StateMachine();
    private Dictionary<States, IState> stateValues = new Dictionary<States, IState>();


    private void Awake()
    {
        HandleEvents.Changestate += CallStateChange;
        HandleEvents.BackToPreviousScreen += CallPreviousState;


        _login = new Login(gameUi);
        AddStatesToDictionary(States.login, _login);

        _signUp = new SignUp(gameUi);
        AddStatesToDictionary(States.singUp, _signUp);

        _otp = new Otp(gameUi);
        AddStatesToDictionary(States.otp, _otp);

        _forgotPassword = new ForgotPassword(gameUi);
        AddStatesToDictionary(States.forgot, _forgotPassword);

        _home = new HomeScreen(gameUi);
        AddStatesToDictionary(States.home, _home);

        _gameplay = new GamePlay(gameUi, gamePlayController);
        AddStatesToDictionary(States.GamePlay, _gameplay);

        _setting = new Setting(gameUi);
        AddStatesToDictionary(States.setting, _setting);

        _profile = new Profile(gameUi);
        AddStatesToDictionary(States.profile, _profile);

        _termsandCondition = new TermsandCondition(gameUi);
        AddStatesToDictionary(States.term, _termsandCondition);

        _privacyPolicy = new PrivacyPolicy(gameUi);
        AddStatesToDictionary(States.privacyPolicy, _privacyPolicy);

        _contactUs = new ContactUs(gameUi);
        AddStatesToDictionary(States.contactUs, _contactUs);

        _selectSolo = new SelectSolo(gameUi);
        AddStatesToDictionary(States.selectSolo, _selectSolo);

        _dailyFunGame = new DailyFunGame(gameUi, dailyFunCalendar, gamePlayController);
        AddStatesToDictionary(States.dailyFunGame, _dailyFunGame);

        _dailyHardGame = new DailyHardGame(gameUi, hardcoreCalendar, gamePlayController);
        AddStatesToDictionary(States.dailyHardCore, _dailyHardGame);

        _multiplayerOption = new MultiplayerOption(gameUi);
        AddStatesToDictionary(States.multiplayOption, _multiplayerOption);

        _joinMultiplayerGame = new JoinMultiplayerGame(gameUi);
        AddStatesToDictionary(States.joinMultiplayGame, _joinMultiplayerGame);

        _searchingPlayer = new SearchingPlayer(gameUi);
        AddStatesToDictionary(States.searchingPlayer, _searchingPlayer);

        _startGame = new StartGame(gameUi);
        AddStatesToDictionary(States.startGame, _startGame);

        _achivement = new Achivement(gameUi);
        AddStatesToDictionary(States.achivement, _achivement);

        _purchaseIAP = new PurchaseIAP(gameUi);
        AddStatesToDictionary(States.purchase, _purchaseIAP);

        _openGame = new OpenGame(gameUi);
        AddStatesToDictionary(States.openGame, _openGame);

        _acceptGame = new AcceptingGame(gameUi);
        AddStatesToDictionary(States.acceptGame, _acceptGame);

        _resetPassword = new ResetPassword(gameUi);
        AddStatesToDictionary(States.resetPswd, _resetPassword);

        _ErrorPopUp = new PopUpMessage(gameUi);

        CheckInternet.InternetStatusChanged += OpenLogin;
    }

    private void AddStatesToDictionary(States state, IState istate)
    {
        stateValues.Add(state, istate);
    }

    private IEnumerator Start()
    {
        GlobalData.DeviceToken = SystemInfo.deviceUniqueIdentifier;
        DeviceInfo();
        //PlayerPrefs.DeleteAll();
        yield return new WaitForEndOfFrame();
        //OpenLogin(CheckInternet.isConnected);
    }

    public void OpenLogin(bool isConnected)
    {
        if (PlayerPrefs.GetInt("Loginstatus") == 1 && isConnected)
        {
            CheckInternet.InternetStatusChanged -= OpenLogin;

            GlobalData.UserId = PlayerPrefs.GetString("PlayerId");
            CommonApi.UserProfile(GlobalData.UserId, () => HandleEvents.ChangeStates(States.home));
        }
        else
        {
            HandleEvents.ChangeStates(States.login);
        }
    }

    public void CallStateChange(States _state)
    {
        //ClearAllInputText();
        IState newstate = null;
        stateValues.TryGetValue(_state, out newstate);
        if (newstate == null)
        {
            Debug.LogErrorFormat("Coudn't find  {0} state", _state);
            return;
        }

        _stateMachine.ChangeState(newstate);
    }

    public void CallPreviousState()
    {
        _stateMachine.SwitchToPreviousState();
    }

    private void Update()
    {
        _stateMachine.ExecuteUpdateState();
    }

    public void DeviceInfo()
    {
        // string deviceName = DeviceName.getDeviceName();
        //   Debug.Log("deviceName........................"+deviceName);
        //Device_Type = SystemInfo.deviceType ;
        if (Device.GetPlatform() == "Android")
        {
            GlobalData.DeviceType = 1;
        }
        else if (Device.GetPlatform() == "iOS")
        {
            GlobalData.DeviceType = 2;
        }
        else
        {
            GlobalData.DeviceType = 3;
        }

    }
}