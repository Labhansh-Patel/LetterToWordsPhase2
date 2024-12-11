using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactUs : IState
{
    private GameUi gameUi;
    public ContactUs(GameUi gameUi)
    {
        this.gameUi = gameUi;

    }
  

    public void Enter()
    {
        gameUi._canvasUi.ContactUs.SetActive(true);
                       RemoveListeners();
                       AddAllListeners();
    }

    public void Execute()
    {
       
    }

    public void Exit()
    {
        gameUi._canvasUi.ContactUs.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.ContactBackbtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.SubmitBtn.onClick.RemoveAllListeners();
    }
    public void AddAllListeners()
    {
        gameUi._buttonUi.ContactBackbtn.onClick.AddListener(ContactBackbtnClick);
        gameUi._buttonUi.SubmitBtn.onClick.AddListener(SubmitBtnClick);
    }

    private void ContactBackbtnClick()
    {
        HandleEvents.BackToPreviousState();
    }

    private void SubmitBtnClick()
    {
       
    }

}
