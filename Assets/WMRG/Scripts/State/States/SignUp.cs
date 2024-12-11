using System.Text.RegularExpressions;
using APICalls;

public class SignUp : IState
{
    private GameUi gameUi;

    public SignUp(GameUi gameUi)
    {
        this.gameUi = gameUi;
    }


    public void Enter()
    {
        gameUi._canvasUi.SignUp.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.SignUp.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.SignUpbtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.SignUp_LoginBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.SignUpbtn.onClick.AddListener(SignUpbtnClick);
        gameUi._buttonUi.SignUp_LoginBtn.onClick.AddListener(SignUp_LoginBtnClick);
    }

    private void SignUp_LoginBtnClick()
    {
        HandleEvents.ChangeStates(States.login);
    }

    private void SignUpbtnClick()
    {
        if (CheckValidation())
        {
            string name = gameUi._inputFieldUi.SinUpNameInput.text;
            string email = gameUi._inputFieldUi.SinUpEmailInput.text;
            string pswd = gameUi._inputFieldUi.SinUpPswdInput.text;
            string cpswd = gameUi._inputFieldUi.SinUpCnfPswdInput.text;
            GlobalData.UserEmail = gameUi._inputFieldUi.SinUpEmailInput.text;

            ApiManager.SingUp(name, email, pswd, cpswd, GlobalData.DeviceType.ToString(), GlobalData.DeviceToken, HandleSinUpCall);
        }
    }

    private void HandleSinUpCall(bool aSucess, SingUpHeader callback)
    {
        if (aSucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result.id);
            // User_Signup_otp = callback.result.SingUp_Otp;
            GlobalData.UserId = callback.result.id;
            GlobalData.UserToken = callback.result.token;

            HandleEvents.ChangeStates(States.otp);
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
    }

    private bool CheckValidation()
    {
        bool status = true;

        if (gameUi._inputFieldUi.SinUpNameInput.text.Length == 0)
        {
            HandleEvents.PopoupErrorMsgOpen("Please enter name");
            status = false;
            return status;
        }
        else if (gameUi._inputFieldUi.SinUpEmailInput.text.Length == 0)
        {
            HandleEvents.PopoupErrorMsgOpen("Please enter email");
            status = false;
            return status;
        }

        if (gameUi._inputFieldUi.SinUpPswdInput.text.Length == 0)
        {
            HandleEvents.PopoupErrorMsgOpen("Please enter password");
            status = false;
            return status;
        }
        else if (gameUi._inputFieldUi.SinUpCnfPswdInput.text.Length == 0)
        {
            HandleEvents.PopoupErrorMsgOpen("Please enter confirm password");
            status = false;
            return status;
        }
        else if (!EmailValidate(gameUi._inputFieldUi.SinUpEmailInput.text))
        {
            HandleEvents.PopoupErrorMsgOpen("Invalid email format");
            status = false;
            return status;
        }
        else if (gameUi._inputFieldUi.SinUpCnfPswdInput.text != gameUi._inputFieldUi.SinUpPswdInput.text)
        {
            HandleEvents.PopoupErrorMsgOpen("Password and Confirm password did not matched");
            status = false;
            return status;
        }

        return status;
    }

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
}