using Gameplay;
using UnityEngine;

public class CanvasUi : MonoBehaviour
{
    public GameObject Login;
    public GameObject SignUp;
    public GameObject Otp;
    public GameObject Forgot;
    public GameObject Setting;
    public GameObject Profile;
    public GameObject Home;
    public GameObject gameplay;
    public GameObject Loading;
    public GameObject TermsandCondition;
    public GameObject ContactUs;
    public GameObject ErrorPopUp;
    public GameObject SoloOption;
    public GameObject MuitiplayerOption;
    public GameObject JoinMultiplayer;
    public GameObject SearchingPlayer;
    public GameObject SearchInvite;
    public GameObject PrivacyPolicy;
    public GameObject Achivenment;
    public GameObject Purchase;
    public GameObject CoinByPopUp;
    public GameObject PremiumMemberPurchasePopUp;
    public GameObject PremiumMemberDetailsPopUp;
    public GameObject StartGame;
    public GameObject DailyFunGame;
    public GameObject DailyHardCore;
    public GameObject EnterGameCode;
    public GameObject OpenGame;
    public GameObject GameAccepting;
    public GameObject ExitGamePopUp;
    public GameObject DailyFunInfoPopUp;
    public GameObject HardCoreInfoPopUp;
    public GameObject ResetPswd;
    public GameObject ExitPanel;
    public GameObject CheckBtnPanel;
    public GameObject AvtarPanel;
    public GameObject GameBonous;
    public GameObject SelectBonous;
    public GameObject StackAnyLetterPanel;


    private void Awake()
    {
        Login.SetActive(false);
        SignUp.SetActive(false);
        Otp.SetActive(false);
        Forgot.SetActive(false);
        Setting.SetActive(false);
        Profile.SetActive(false);
        Home.SetActive(false);
        gameplay.SetActive(false);
        Loading.SetActive(false);
        TermsandCondition.SetActive(false);
        ContactUs.SetActive(false);
        ErrorPopUp.SetActive(false);
        DailyFunGame.SetActive(false);
        DailyHardCore.SetActive(false);
        EnterGameCode.SetActive(false);
        OpenGame.SetActive(false);
        GameAccepting.SetActive(false);
        ExitGamePopUp.SetActive(false);
        ResetPswd.SetActive(false);
        ExitPanel.SetActive(false);
        CheckBtnPanel.SetActive(false);
        AvtarPanel.SetActive(false);
        GameBonous.SetActive(false);
        SelectBonous.SetActive(false);
        StackAnyLetterPanel.SetActive(false);
    }

    public void DisableWaitingPanel()
    {
        FindObjectOfType<MultiplayerGameUI>().ToggleWaitingPanel(false);
    }
}