using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSolo : IState
{
    private GameUi gameUi;
    public SelectSolo(GameUi gameUi)
    {
        this.gameUi = gameUi;

    }
   

    public void Enter()
    {
        gameUi._canvasUi.SoloOption.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        gameUi._canvasUi.SoloOption.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.DailyFunBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.DailyHardBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.SoloBackBtn.onClick.RemoveAllListeners();
      
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.DailyFunBtn.onClick.AddListener(DailyFunBtnClick);
        gameUi._buttonUi.DailyHardBtn.onClick.AddListener(DailyHardBtnClick);
        gameUi._buttonUi.SoloBackBtn.onClick.AddListener(SoloBackBtnClick);
    }

    private void DailyFunBtnClick()
    {
        GlobalData.GameMode = "Fun";
        HandleEvents.ChangeStates(States.dailyFunGame); 
    }

    private void DailyHardBtnClick()
    {
        GlobalData.GameMode = "Hard";
        HandleEvents.ChangeStates(States.dailyHardCore);
    }

    private void SoloBackBtnClick()
    {
        HandleEvents.ChangeStates(States.home);
    }
}
