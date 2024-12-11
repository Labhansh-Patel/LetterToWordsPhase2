using System;
using UnityEngine;
using UnityEngine.UI;
using AudioSettings = Webmobril.AudioManager.AudioSettings;

public class ButtonUI : MonoBehaviour
{
    [Header("Login In")] public Button ForgotPasswordBtn;
    public Button SignUpBtn;
    public Button LoginBtn;
    public Button FacebookBtn;
    public Button GmailBtn;


    [Header("Sign Up")] public Button SignUpbtn;
    public Button SignUp_LoginBtn;


    [Header("Otp")] public Button Resendbtn;
    public Button DoneBtn;
    public Button OtpBackBtn;


    [Header("Forgot Password")] public Button NextBtn;
    public Button ForgotPasswordBackBtn;

    [Header("Contact Us")] public Button ContactBackbtn;
    public Button SubmitBtn;

    [Header("Profile")] public Button SaveBtn;
    public Button ProfileBackBtn;
    public Button ProfileEditBtn;
    public Button ResetBtn;


    [Header("Setting")] public Button SettingBackBtn;
    public Toggle sound;
    public Button TermsBtn;
    public Button ContactUs;
    public Button PrivacyBtn;
    public Button LogOutBtn;
    public Button LogOutYesBtn;
    public Button LogOutNoBtn;


    [Header("Home Screen")] public Button ProfileBtn;
    public Button SettingBtn;
    public Button SoloBtn;
    public Button MultiPlayerGameBtn;
    public Button Purchase;
    public Button ExitBtn;
    public Button AchivementBtn;

    [Header("Solo Game")] public Button DailyFunBtn;
    public Button DailyHardBtn;
    public Button SoloBackBtn;


    [Header("Daily Fun Game")] public Button DailyFunBackBtn;
    public Button DailyFunInfoBtn;
    public Button DailyFunCloseInfoBtn;
    public Button DailyFunStartTodayBtn;


    [Header("Daily Hard Core ")] public Button HardCoreBackBtn;
    public Button HardCoreInfoBtn;
    public Button HardCoreCloseInfoBtn;
    public Button DailyHardCoreStartTodayBtn;

    [Header("Multiplayer")] public Button MultiplayerBackBtn;
    public Button JoinBtn;
    public Button StartGame;

    [Header("Join Game")] public Button JoinGame_BackBtn;
    public Button AcceptingGameBtn;
    public Button ShowOpenBtn;
    public Button EnterGameBtn;
    public Button SubmitRoomNameBtn;
    public Button CloseRoomNameBtn;


    [Header("Open Game")] public Button OpenGameBackBtn;
    public Button JoinGameBtn;

    [Header("Game Accepting")] public Button GameAcceptingBackBtn;
    public Button AcceptBtn;

    [Header("Searching For Player")] public Button SearchingBackBtn;
    public Button StartGameBtn;
    public Button SearchInviteBtn;
    public Button ShareCodeBtn;

    [Header("Start Game")] public Button StartGameBackBtn;
    public Button Start_GameBtn;
    public Toggle FunGameToggle;
    public Toggle HardCoreToggle;
    public Toggle PublicToggle;
    public Toggle PrivateToggle;
    public Toggle WordForWordToggle;
    public Toggle FastToggle;


    [Header("Terms and Condition")] public Button TermsandConditionBackBtn;

    [Header("Privacy Policy")] public Button PrivacyBackBtn;

    [Header("Achivement")] public Button AchivementBackBtn;

    [Header("Purchase")] public Button PurchaseBackBtn;
    public Button BuyBtn;
    public Button PremiumMemberPurchaseBtn;
    public Button PremiumMemberDetailsBtn;
    public Button CloseCoinPopUpBtn;
    public Button FreeCoinsBtn;
    public Button ClosePremiumPurchasePopUpBtn;
    public Button ClosePremiumDetailsPopUpBtn;
    public Button UnlockPremiumBtn;
    public Button CanclePremiumBtn;

    [Header("Waiting Panel")] public Button WaitingPanelBackBtn;

    [Header("GamePlay")] public Button GamePlayBackBtn;
    public Button StarBtn;
    public Button CloseGameBonousBtn;
    public Button CloseSelectBonousBtn;
    //public Button confirmExit;//


    [Header("ErrorPopUp")] public Button ErrorOKBtn;

    [Header("Reset Pswd")] public Button ResetPswdBackBtn;
    public Button ResetPswdDoneBtn;

    [Header("Game ExitPanel")] public Button GameExitYesBtn;
    public Button GameExitNoBtn;


    [Header("Avtar")] public Button AvtarOneBtn;
    public Button AvtarTwoBtn;
    public Button AvtarThreeBtn;
    public Button AvtarFourBtn;
    public Button AvtarFiveBtn;
    public Button AvtarSixBtn;
    public Button AvtarSevenBtn;
    public Button AvtarEightBtn;

    [Header("Bonous Button")] public Button ExtraTraySpaceBtn;
    public Button AnyTileBtn;
    public Button AnyLetterBtn;
    public Button FreeGarbageBtn;
    public Button ExtendedTimeBtn;
    public Button BlockExtendedBtn;

    public Button GameExtraTraySpaceBtn;
    public Button GameAnyTileBtn;
    public Button GameAnyLetterBtn;
    public Button GameFreeGarbageBtn;
    public Button GameExtendedTimeBtn;
    public Button GameBlockExtendedBtn;


    private void Start()
    {
        int sound = PlayerPrefs.GetInt("Sound", 0);

        this.sound.isOn = Convert.ToBoolean(sound);
        AudioSettings.ToggleAudioMusic(!this.sound.isOn);
    }
}