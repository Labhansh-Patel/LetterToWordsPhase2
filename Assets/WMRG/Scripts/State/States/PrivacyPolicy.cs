using System;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyPolicy : IState
{
    private GameUi gameUi;
    public PrivacyPolicy(GameUi gameUi)
    {
        this.gameUi = gameUi;

    }

    public void Enter()
    {
        gameUi._canvasUi.PrivacyPolicy.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        gameUi._canvasUi.PrivacyPolicy.SetActive(false);
    }

    public void RemoveListeners()
    {

        gameUi._buttonUi.PrivacyBackBtn.onClick.RemoveAllListeners();

    }
    public void AddAllListeners()
    {

       gameUi._buttonUi.PrivacyBackBtn.onClick.AddListener(PrivacyBackBtnClick);

    }

    private void PrivacyBackBtnClick()
    {
        HandleEvents.ChangeStates(States.setting);
    }

    

}
