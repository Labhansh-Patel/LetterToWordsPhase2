using System;
using APICalls;
using UnityEngine;
using AudioSettings = Webmobril.AudioManager.AudioSettings;

public class Setting : IState
{
    private GameUi gameUi;

    public Setting(GameUi gameUi)
    {
        this.gameUi = gameUi;
    }

    public void Enter()
    {
        gameUi._canvasUi.Setting.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.Setting.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.SettingBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.sound.onValueChanged.RemoveAllListeners();
        gameUi._buttonUi.TermsBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.ContactUs.onClick.RemoveAllListeners();
        gameUi._buttonUi.PrivacyBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.LogOutBtn.onClick.RemoveAllListeners();

        gameUi._buttonUi.LogOutYesBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.LogOutNoBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.SettingBackBtn.onClick.AddListener(SettingBackButtonClick);
        gameUi._buttonUi.sound.onValueChanged.AddListener(soundClick);
        gameUi._buttonUi.TermsBtn.onClick.AddListener(TermsBtnClick);
        gameUi._buttonUi.ContactUs.onClick.AddListener(ContactUsClick);
        gameUi._buttonUi.PrivacyBtn.onClick.AddListener(PrivacyBtnClick);
        gameUi._buttonUi.LogOutBtn.onClick.AddListener(LogOutBtnClick);

        gameUi._buttonUi.LogOutYesBtn.onClick.AddListener(LogOutYesBtnClick);
        gameUi._buttonUi.LogOutNoBtn.onClick.AddListener(LogOutNoBtnClick);
    }

    private void LogOutNoBtnClick()
    {
        gameUi._canvasUi.ExitGamePopUp.SetActive(false);
    }

    private void LogOutYesBtnClick()
    {
        gameUi._canvasUi.ExitGamePopUp.SetActive(false);
        PlayerPrefs.DeleteAll();
        GlobalData.userData = null;
        HandleEvents.ChangeStates(States.login);
    }

    private void LogOutBtnClick()
    {
        gameUi._canvasUi.ExitGamePopUp.SetActive(true);
    }

    private void PrivacyBtnClick()
    {
        Application.OpenURL(Constant.PrivacyPolicyUrl);
        //HandleEvents.ChangeStates(States.privacyPolicy);
    }


    private void ContactUsClick()
    {
        Application.OpenURL(Constant.ContactUs);
    }

    private void TermsBtnClick()
    {
        Application.OpenURL(Constant.TermsCondition);
    }

    private void soundClick(bool arg0)
    {
        LogSystem.LogEvent("Sound {0}", arg0);
        PlayerPrefs.SetInt("Sound", Convert.ToInt32(arg0));
        AudioSettings.ToggleAudioSound(!arg0);
        AudioSettings.ToggleAudioMusic(!arg0);
    }

    private void SettingBackButtonClick()
    {
        HandleEvents.ChangeStates(States.home);
    }
}