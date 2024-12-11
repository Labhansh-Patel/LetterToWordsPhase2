using APICalls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPassword : IState
{
    private GameUi gameUi;
    public ResetPassword(GameUi gameUi)
    {
        this.gameUi = gameUi;

    }

    public void Enter()
    {
        gameUi._canvasUi.ResetPswd.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        gameUi._canvasUi.ResetPswd.SetActive(false);

    }

    public void RemoveListeners()
    {

        gameUi._buttonUi.ResetPswdBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.ResetPswdDoneBtn.onClick.RemoveAllListeners();
    }
    public void AddAllListeners()
    {

        gameUi._buttonUi.ResetPswdBackBtn.onClick.AddListener(ResetPswdBackBtnClick);
        gameUi._buttonUi.ResetPswdDoneBtn.onClick.AddListener(ResetPswdDoneBtnClick);
    }

    private void ResetPswdDoneBtnClick()
    {
        if(CheckValidation())
        {
            string nPswd = gameUi._inputFieldUi.NewPswdInput.text;
            string cPswd = gameUi._inputFieldUi.ConfPswdInput.text;

            ApiManager.ChangePassword(nPswd, cPswd,GlobalData.UserId, GlobalData.DeviceType.ToString(),GlobalData.DeviceToken, HandleChangePswdCall);

        }


    }

    private void ResetPswdBackBtnClick()
    {
        HandleEvents.ChangeStates(States.profile);
    }

    private void HandleChangePswdCall(bool asucess, ChangePasswordHeader callback)
    {
     

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
            HandleEvents.PopoupErrorMsgOpen(callback.message);
            PlayerPrefs.SetInt("Loginstatus", 1);
            PlayerPrefs.SetString("PlayerId", callback.result.id);
            HandleEvents.ChangeStates(States.profile);
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

        if (gameUi._inputFieldUi.NewPswdInput.text.Length == 0)
        {
            HandleEvents.PopoupErrorMsgOpen("Please enter new password");
            status = false;
            return status;
        }
        else if (gameUi._inputFieldUi.ConfPswdInput.text.Length == 0)
        {
            HandleEvents.PopoupErrorMsgOpen("Please enter confirm password");
            status = false;
            return status;
        }
      

        return status;
    }


}
