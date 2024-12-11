using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using APICalls;
using UnityEngine.UI;

public class Profile : IState
{
    private GameUi gameUi;
    public Profile(GameUi gameUi)
    {
        this.gameUi = gameUi;

    }

    public void Enter()
    {
        gameUi._canvasUi.Profile.SetActive(true);
        //gameUi.imageUi.ProfileImageBlockerEdit.gameObject.SetActive(true);
        //gameUi._buttonUi.ProfileSaveBtn.gameObject.SetActive(false);
        if (GlobalData.userData.login_type == 2)
        {
            gameUi._imageUi.ProfileImage.sprite = GlobalData.userData.socialAvatarImage;
          
        }
        else
        {
            gameUi._imageUi.ProfileImage.sprite = gameUi._imageUi.Avtar[GlobalData.UserAvtarId - 1];
            
        }
        
        
        gameUi._buttonUi.ResetBtn.gameObject.SetActive(!GlobalData.userData.IsSocialLogin);
        gameUi._buttonUi.ProfileEditBtn.gameObject.SetActive(!GlobalData.userData.IsSocialLogin);

        
        

        showInfoOnProfileOpen();
        RemoveListeners();
        AddAllListeners();
    }
    private void showInfoOnProfileOpen()
    {
        gameUi._inputFieldUi.ProfileNameInput.text = GlobalData.UserName;
        gameUi._inputFieldUi.ProfileEmailInput.text = GlobalData.UserEmail;
        gameUi._inputFieldUi.ProfileContactInput.text = GlobalData.UserMobileNumber;
    }
    public void Execute()
    {

    }

    public void Exit()
    {
        gameUi._canvasUi.Profile.SetActive(false);
        gameUi._canvasUi.AvtarPanel.SetActive(false);   
    }

    public void RemoveListeners()
    {


        gameUi._buttonUi.SaveBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.ProfileBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.ProfileEditBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.ResetBtn.onClick.RemoveAllListeners();

        gameUi._buttonUi.AvtarOneBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.AvtarTwoBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.AvtarThreeBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.AvtarFourBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.AvtarFiveBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.AvtarSixBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.AvtarSevenBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.AvtarEightBtn.onClick.RemoveAllListeners();


    }
    public void AddAllListeners()
    {

        gameUi._buttonUi.SaveBtn.onClick.AddListener(SaveBtnClick);
        gameUi._buttonUi.ProfileBackBtn.onClick.AddListener(ProfileBackBtnClick);
        gameUi._buttonUi.ProfileEditBtn.onClick.AddListener(ProfileEditBtnClick);
        gameUi._buttonUi.ResetBtn.onClick.AddListener(ResetBtnClick);

        gameUi._buttonUi.AvtarOneBtn.onClick.AddListener(AvtarOneBtnClick);
        gameUi._buttonUi.AvtarTwoBtn.onClick.AddListener(AvtarTwoBtnClick);
        gameUi._buttonUi.AvtarThreeBtn.onClick.AddListener(AvtarThreeBtnClick);
        gameUi._buttonUi.AvtarFourBtn.onClick.AddListener(AvtarFourBtnClick);
        gameUi._buttonUi.AvtarFiveBtn.onClick.AddListener(AvtarFiveBtnClick);
        gameUi._buttonUi.AvtarSixBtn.onClick.AddListener(AvtarSixBtnClick);
        gameUi._buttonUi.AvtarSevenBtn.onClick.AddListener(AvtarSevenBtnClick);
        gameUi._buttonUi.AvtarEightBtn.onClick.AddListener(AvtarEightBtnClick);
    }

    private void AvtarEightBtnClick()
    {
        GetAvtarId(8);
    }

    private void AvtarSevenBtnClick()
    {
        GetAvtarId(7);
    }

    private void AvtarSixBtnClick()
    {
        GetAvtarId(6);
    }

    private void AvtarFiveBtnClick()
    {
        GetAvtarId(5);
    }

    private void AvtarFourBtnClick()
    {
        GetAvtarId(4);
    }

    private void AvtarThreeBtnClick()
    {
        GetAvtarId(3);
    }

    private void AvtarTwoBtnClick()
    {
        GetAvtarId(2);
    }

    private void AvtarOneBtnClick()
    {
        GetAvtarId(1);
    }

    private void ResetBtnClick()
    {
        HandleEvents.ChangeStates(States.resetPswd);
    }

    private void SaveBtnClick()
    {
        string name = gameUi._inputFieldUi.ProfileNameInput.text;
        string mobileno = gameUi._inputFieldUi.ProfileContactInput.text;
        ApiManager.UpdateProfile(name, mobileno,GlobalData.UserAvtarId.ToString(),GlobalData.UserId, GlobalData.DeviceType.ToString(),GlobalData.DeviceToken, HandleUpdateProfileCall);

    }


    private void HandleUpdateProfileCall(bool asucess, UpdateProfileHeader callback)
    {
       

        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
            GlobalData.UserId = callback.result.id;
            GlobalData.UserName = callback.result.name;
            GlobalData.UserEmail = callback.result.email;
            GlobalData.UserMobileNumber = callback.result.mobile;
            GlobalData.UserAvtarId = int.Parse(callback.result.avatar_id);


          

        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
    }
    private void ProfileSaveClick()
    {
       /* if (CheckValidation())
        {
          *//*  string name = gameUi._inputFieldUi.ProfileUserNameIpf.text;
            string mobileno = gameUi._inputFieldUi.ProfileUserPhoneNumberIpf.text;
            string age = gameUi._inputFieldUi.ProfileUserAgeIpf.text;*//*
            gameUi._canvasUi.Loading.SetActive(true);
           // APICalls.ApiManager.UpdateUserProfile(name, " ", mobileno, age, HandleUpdateProfileCallback);
            // HandleEvents.PopoupErrorMsgOpen("***Work In Progress***");

        }*/
    }

    private void HandleUpdateProfileCallback(bool asucess, UpdateProfileHeader CallBack) 
    {
        if (asucess)
        {
            

        }
        else
        {

        }

        
    }

  

    private void ProfileBackBtnClick()
    {
        

        HandleEvents.ChangeStates(States.home);


    }


    public void GetAvtarId(int id)
    {
        GlobalData.UserAvtarId = id;
        if (GlobalData.userData.login_type == 2)
        {
            gameUi._imageUi.ProfileImage.sprite = GlobalData.userData.socialAvatarImage;
        }
        else
        {
            gameUi._imageUi.ProfileImage.sprite = gameUi._imageUi.Avtar[GlobalData.UserAvtarId - 1];
        }
     
        gameUi._canvasUi.AvtarPanel.SetActive(false);
    }
    private void ProfileEditBtnClick()
    {
        gameUi._canvasUi.AvtarPanel.SetActive(true);   
    }
    private void ProfileCalanderBtnClick()
    {
        HandleEvents.PopoupErrorMsgOpen(" Work In Progress");
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
 


}
