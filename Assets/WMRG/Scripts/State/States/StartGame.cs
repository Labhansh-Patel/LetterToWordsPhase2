using System;
using GameEvents;
using Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

public class StartGame : IState
{
    private GameUi gameUi;

    public StartGame(GameUi gameUi)
    {
        this.gameUi = gameUi;
    }


    public void Enter()
    {
        gameUi._canvasUi.StartGame.SetActive(true);
        RemoveListeners();
        Debug.Log("Load Ad");
        AddAllListeners();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.StartGame.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.StartGameBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.Start_GameBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        Debug.Log("Load Ad");
        gameUi._buttonUi.StartGameBackBtn.onClick.AddListener(StartGameBackBtnClick);
        gameUi._buttonUi.Start_GameBtn.onClick.AddListener(() =>
        {
            if (GlobalData.userData.IsPremiumUser)
            {
                Start_GameBtnClick();
            }
            else
            {
                Debug.Log("Load Ad");
                AdsManager.Instance.onAdFinised -= Start_GameBtnClick;
                AdsManager.Instance.onAdFinised += Start_GameBtnClick;
                AdsManager.Instance.PlayAdInterstitial();
            }
        });
    }

    private void StartGameBackBtnClick()
    {
        HandleEvents.BackToPreviousState();
    }

    private void Start_GameBtnClick()
    {
        MultiplayerMode currentMode = (gameUi._buttonUi.PrivateToggle.isOn) ? MultiplayerMode.Private : MultiplayerMode.Public;

        if (!GlobalData.userData.IsPremiumUser && currentMode == MultiplayerMode.Private)
        {
            HandleEvents.PopoupErrorMsgOpen(GameMessages.CannotCreateRoomNonPremium);
            //  EventHandlerGame.EmitEvent(GameEventType.ShowPopupText,GameMessages.CannotCreateRoomNonPremium);
            return;
        }

        CreateMultiplayerData createMultiplayerData = new CreateMultiplayerData();
        var dateTime = DateTime.Now;

        int random = Random.Range(-5, 5);

        dateTime = dateTime.AddDays(random);

        var time = dateTime.ToString("hh:mm");
        createMultiplayerData.time = time;
        var date = dateTime.ToString("yyyy-MM-dd");

        createMultiplayerData.date = date;
        GlobalData.GameDate = createMultiplayerData.date;
        if (gameUi._buttonUi.FunGameToggle.isOn)
        {
            createMultiplayerData.gamemode = "1";
            GlobalData.GameMode = "daily";
        }
        else if (gameUi._buttonUi.HardCoreToggle.isOn)
        {
            createMultiplayerData.gamemode = "2";
            GlobalData.GameMode = "hardcore";
        }


        if (gameUi._buttonUi.WordForWordToggle.isOn)
        {
            createMultiplayerData.multiplayerType = MultiplayerType.WordToWord;
        }
        else if (gameUi._buttonUi.FastToggle)
        {
            createMultiplayerData.multiplayerType = MultiplayerType.FastGame;
        }

        GlobalData.MultiplayerType = createMultiplayerData.multiplayerType;
        createMultiplayerData.MultiplayerAction = MultiplayerAction.CreateGame;
        createMultiplayerData.roomName = (gameUi._buttonUi.PrivateToggle.isOn) ? GlobalData.userData.PrivateGameKey : GlobalData.userData.PublicGameKey;
        createMultiplayerData.MultiplayerMode = currentMode;
        createMultiplayerData.hostName = GlobalData.userData.name;
        EventHandlerGame.EmitEvent(GameEventType.ConnectMultiplayer, createMultiplayerData);
        EventHandlerGame.EmitEvent(GameEventType.Loading, true);
        // HandleEvents.ChangeStates(States.GamePlay);
        //HandleEvents.PopoupErrorMsgOpen("Work in progress");
    }
}