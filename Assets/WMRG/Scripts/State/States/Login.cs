using System;
using System.Text.RegularExpressions;
using APICalls;
using GameEvents;
using UnityEngine;

public class Login : IState
{
    private GameUi gameUi;
    private FacebookHelper facebookHelper;
    private GoogleSignInHelper googleSignInHelper;
    private bool isSocialLogin = false;

    public Login(GameUi gameUi)
    {
        this.gameUi = gameUi;
        facebookHelper = new FacebookHelper();
        googleSignInHelper = new GoogleSignInHelper();
    }


    public void Enter()
    {
        gameUi._canvasUi.Login.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.Login.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.ForgotPasswordBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.SignUpBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.LoginBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.FacebookBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.GmailBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.ForgotPasswordBtn.onClick.AddListener(ForgotPasswordBtnClick);
        gameUi._buttonUi.SignUpBtn.onClick.AddListener(SignUpBtnClick);
        gameUi._buttonUi.LoginBtn.onClick.AddListener(LoginBtnClick);
        gameUi._buttonUi.FacebookBtn.onClick.AddListener(FacebookBtnClick);
        gameUi._buttonUi.GmailBtn.onClick.AddListener(GmailBtnClick);
    }

    private void ForgotPasswordBtnClick()
    {
        HandleEvents.ChangeStates(States.forgot);
    }

    private void SignUpBtnClick()
    {
        HandleEvents.ChangeStates(States.singUp);
    }

    private void LoginBtnClick()
    {
        if (CheckValidation())
        {
            EventHandlerGame.EmitEvent(GameEventType.Loading, true);
            string Email = gameUi._inputFieldUi.SignEmailInput.text;
            string passWord = gameUi._inputFieldUi.SignPswdInput.text;
            GlobalData.UserEmail = Email;

            ApiManager.LoginUser(Email, passWord, GlobalData.DeviceType.ToString(), GlobalData.DeviceToken, HandleLoginCall);
        }
    }

    private void FacebookBtnClick()
    {
        isSocialLogin = true;
        facebookHelper.LoginFacebookOperation(FacebookCallBack);
    }
    
    private void FacebookCallBack(FacebookHelper.FacebookData facebookData)
    {
        EventHandlerGame.EmitEvent(GameEventType.Loading, true);
        string userImage = $"https://graph.facebook.com/{facebookData.id}/picture?type=square";
        LogSystem.LogEvent("UserImage {0}", userImage);
        LogSystem.LogEvent("UserImage {0}", facebookData.name , facebookData.id);
        ApiManager.LoginUserSocial(facebookData.id, facebookData.name, userImage, string.Empty, 3, GlobalData.DeviceType.ToString(),
            GlobalData.DeviceToken, HandleLoginCall);
    }


    // public static GetProfile GetProfile(List<GetProfile> profiles, string id)
    // {
    //     return profiles.Select((profile => profile.avatar_id == id));
    // }
    private void GmailBtnClick()
    {
        isSocialLogin = true;
        Debug.Log("googleSignInHelper:");
        googleSignInHelper.OnSignIn(HandleGoogleSignIn);
    }

    private void HandleGoogleSignIn(GoogleSignInHelper.GoogleSignInData signInData)
    {
        LogSystem.LogEvent("GoogleData {0}", signInData.ToString());
        Debug.Log(signInData.DisplayName.ToString() + signInData.Email.ToString()+signInData.UserId.ToString());
        EventHandlerGame.EmitEvent(GameEventType.Loading, true);
        ApiManager.LoginUserSocial(signInData.UserId, signInData.DisplayName, signInData.ImageUrl, signInData.Email,
            3, GlobalData.DeviceType.ToString(),
            GlobalData.DeviceToken, HandleLoginCall);
    }
    
    //Validation
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

    private bool CheckValidation()
    {
        bool status = true;

        if (gameUi._inputFieldUi.SignEmailInput.text.Length == 0)
        {
            HandleEvents.PopoupErrorMsgOpen("Please enter emailId");
            status = false;
            return status;
        }
        else if (gameUi._inputFieldUi.SignPswdInput.text.Length == 0)
        {
            HandleEvents.PopoupErrorMsgOpen("Please enter password");
            status = false;
            return status;
        }
        else if (!EmailValidate(gameUi._inputFieldUi.SignEmailInput.text))
        {
            HandleEvents.PopoupErrorMsgOpen("Invalid email format");
            status = false;
            return status;
        }

        return status;
    }
    
    private void HandleLoginCall(bool asucess, LoginHeader callback)
    {
        EventHandlerGame.EmitEvent(GameEventType.Loading, false);

        if (asucess)
        {
            if (isSocialLogin)
            {
                gameUi._buttonUi.ResetBtn.gameObject.SetActive(false);
            }

            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result.id);
            if (callback.message == "Please verify your account first to login")
            {
                GlobalData.UserId = callback.result.id;
                GlobalData.UserToken = callback.result.token;

                ApiManager.ResendOtp(GlobalData.UserEmail, GlobalData.DeviceType.ToString(), GlobalData.DeviceToken, HandleResendOtpCall); //_email;
                HandleEvents.ChangeStates(States.otp);
                // ResendOtp(SignEmailInput.text);
                //PlayerPrefs.SetInt("Loginstatus", 1);
                //PlayerPrefs.SetString("PlayerId", login.result.id);
                //PlayerPrefs.SetString("PlayerName", );
            }
            else
            {
                GlobalData.UserId = callback.result.id;
                GlobalData.UserToken = callback.result.token;


                PlayerPrefs.SetInt("Loginstatus", 1);
                PlayerPrefs.SetString("PlayerId", callback.result.id);
                PlayerPrefs.SetString(Constant.playeridlocation, callback.result.id);
                // ApiManager.GetProfile(User_Id, HandleProfileCall);
                CommonApi.UserProfile(GlobalData.UserId, () => HandleEvents.ChangeStates(States.home));


                // islogedIn = true;
            }
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);

            if (callback.message == "Please verify your account first to login")
            {
                GlobalData.UserId = callback.result.id;
                GlobalData.UserToken = callback.result.token;

                ApiManager.ResendOtp(GlobalData.UserEmail, GlobalData.DeviceType.ToString(), GlobalData.DeviceToken, HandleResendOtpCall); //_email;
                HandleEvents.ChangeStates(States.otp);
            }
            else
            {
                HandleEvents.PopoupErrorMsgOpen(callback.message);
            }
        }
    }

    private void HandleResendOtpCall(bool aSucess, ResendOtpHeader callBack)
    {
        throw new NotImplementedException();
    }
}