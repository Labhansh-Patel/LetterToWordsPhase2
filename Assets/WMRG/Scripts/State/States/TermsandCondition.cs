using System;
using System.Collections.Generic;
using UnityEngine;

public class TermsandCondition : IState
{
    private GameUi gameUi;
    public TermsandCondition(GameUi gameUi)
    {
        this.gameUi = gameUi;

    }

    public void Enter()
    {
        gameUi._canvasUi.TermsandCondition.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        gameUi._canvasUi.TermsandCondition.SetActive(false);
    }

    public void RemoveListeners()
    {

        gameUi._buttonUi.TermsandConditionBackBtn.onClick.RemoveAllListeners();

    }
    public void AddAllListeners()
    {

        gameUi._buttonUi.TermsandConditionBackBtn.onClick.AddListener(TermsandConditionBackBtnClick);

    }

    private void TermsandConditionBackBtnClick()
    {
        HandleEvents.ChangeStates(States.setting);
    }



}
