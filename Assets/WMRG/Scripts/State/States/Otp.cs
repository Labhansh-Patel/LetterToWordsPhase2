using APICalls;
using UnityEngine;

public class Otp : IState
{
    private GameUi gameUi;

    public Otp(GameUi gameUi)
    {
        this.gameUi = gameUi;
    }

    public void Enter()
    {
        gameUi._canvasUi.Otp.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.Otp.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.Resendbtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.DoneBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.OtpBackBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.Resendbtn.onClick.AddListener(OtpResendCodebtnClick);
        gameUi._buttonUi.DoneBtn.onClick.AddListener(VerifyBtnClick);
        gameUi._buttonUi.OtpBackBtn.onClick.AddListener(OtpBackBtnClick);
    }

    private void OtpResendCodebtnClick()
    {
    }

    private void VerifyBtnClick()
    {
        if (CheckValidation())
        {
            string otpipf = gameUi._inputFieldUi.OtpTextInput.text;
            string email = GlobalData.UserEmail;

            ApiManager.VerifyOtp(email, otpipf, HandleVerifyOtp);
        }
    }

    private void HandleVerifyOtp(bool aSucess, verifyOTPHeader callback)
    {
        if (aSucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
            GlobalData.UserId = callback.result.userid;
            GlobalData.UserToken = callback.result.access_token;
            PlayerPrefs.SetString("PlayerId", callback.result.userid);
            PlayerPrefs.SetInt("Loginstatus", 1);
            CommonApi.UserProfile(GlobalData.UserId, () => HandleEvents.ChangeStates(States.home));


            // islogedIn = true;
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
    }

    private void OtpBackBtnClick()
    {
        HandleEvents.BackToPreviousState();
    }

    private bool CheckValidation()
    {
        bool status = true;

        if (gameUi._inputFieldUi.OtpTextInput.text.Length == 0)
        {
            HandleEvents.PopoupErrorMsgOpen("Please enter email");
            status = false;
            return status;
        }

        return status;
    }
}