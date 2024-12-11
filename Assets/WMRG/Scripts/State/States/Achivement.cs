using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achivement : IState
{
    private GameUi gameUi;
    public Achivement(GameUi gameUi)
    {
        this.gameUi = gameUi;

    }
  

    public void Enter()
    {
        gameUi._canvasUi.Achivenment.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
       
    }

    public void Exit()
    {
        gameUi._canvasUi.Achivenment.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.AchivementBackBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.AchivementBackBtn.onClick.AddListener(AchivementBackBtnClick);
    }

    private void AchivementBackBtnClick()
    {
        HandleEvents.BackToPreviousState();
    }
}
