using System.Text.RegularExpressions;
using APICalls;
using UnityEngine;

public class ForgotPassword : IState
{
    private GameUi gameUi;

    public ForgotPassword(GameUi gameUi)
    {
        this.gameUi = gameUi;
    }

    public void Enter()
    {
        gameUi._canvasUi.Forgot.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.Forgot.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.ForgotPasswordBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.NextBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.ForgotPasswordBackBtn.onClick.AddListener(ForgotPasswordBackBtnClick);
        gameUi._buttonUi.NextBtn.onClick.AddListener(NextBtnClick);
    }

    private void NextBtnClick()
    {
        if (CheckValidation())
        {
            string email = gameUi._inputFieldUi.ForgotEmailInput.text;
            ApiManager.ForgotPassword(email, GlobalData.DeviceType.ToString(), GlobalData.DeviceToken, HandleForgotPswdCall);
        }
    }

    private void ForgotPasswordBackBtnClick()
    {
        HandleEvents.ChangeStates(States.login);
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

        if (gameUi._inputFieldUi.ForgotEmailInput.text.Length == 0)
        {
            HandleEvents.PopoupErrorMsgOpen("Please enter emailId");
            status = false;
            return status;
        }
        else if (!EmailValidate(gameUi._inputFieldUi.ForgotEmailInput.text))
        {
            HandleEvents.PopoupErrorMsgOpen("Invalid email format");
            status = false;
            return status;
        }

        return status;
    }


    private void HandleForgotPswdCall(bool asucess, ForgotPasswordHeader callback)
    {
        if (asucess)
        {
            //LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result.id);
            HandleEvents.PopoupErrorMsgOpen(callback.message);

            PlayerPrefs.SetString("PlayerId", callback.result.id);
            HandleEvents.ChangeStates(States.login);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
        }
    }
}