using System;
using System.Collections;
using System.Collections.Generic;
using APICalls;
using GameEvents;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class AcceptingGame : IState
{
    private GameUi gameUi;
    
    private Dictionary<string, RoomListPrefab> cachedRoomList = new Dictionary<string, RoomListPrefab>();

    public AcceptingGame(GameUi gameUi)
    {
        this.gameUi = gameUi;
        EventHandlerGame.RoomListUpdate += RoomListUpdate;
    }


    private void DeleteRoomList()
    {
        foreach (Transform prefab in gameUi._Prefabs.RoomListContent)
        {
            GameObject.Destroy(prefab.gameObject);
        }
    }
    
    private void RoomListUpdate(List<RoomInfo> roomList)
    {
      //  DeleteRoomList();
        	
      
      for(int i=0; i<roomList.Count; i++)
      {
          RoomInfo info = roomList[i];
          
          if (info.RemovedFromList)
          {
              GameObject.Destroy(cachedRoomList[info.Name].gameObject);
              cachedRoomList.Remove(info.Name);
          }
          else
          {
              if (cachedRoomList.ContainsKey(info.Name))
              {
                  // already have this room in our list 
              }
              else
              {
                  AddRoomInfoPrefab(info);
              }
          }
      }
      
        // foreach (var roomInfo in roomList)
        // {
        //     if (roomInfo.PlayerCount > 0)
        //     {
        //         AddRoomInfoPrefab(roomInfo);
        //     }
        //     
        //     LogSystem.LogEvent("INFO{0}", (string) roomInfo.CustomProperties["gameData"]);
        // }
    }

    private void AddRoomInfoPrefab(RoomInfo roomInfo)
    {
        var Prefab = GameObject.Instantiate(gameUi._Prefabs.RoomListPrefab, gameUi._Prefabs.RoomListContent);
        Prefab.SetDataPrefab(roomInfo);
        cachedRoomList . Add(roomInfo.Name,Prefab);
    }
    
    public void Enter()
    {
        DeleteRoomList();
        cachedRoomList.Clear();
        gameUi._canvasUi.GameAccepting.SetActive(true);
        RemoveListeners();
        AddAllListeners();
        CreateMultiplayerData createMultiplayerData = new CreateMultiplayerData();
        createMultiplayerData.MultiplayerAction = MultiplayerAction.JoinGame;
        createMultiplayerData.MultiplayerMode = MultiplayerMode.Public;
        EventHandlerGame.EmitEvent(GameEventType.ConnectMultiplayer, createMultiplayerData);
        // EventHandlerGame.EmitEvent(GameEventType.Loading,true);
    }

    public void Execute()
    {
       
    }

    public void Exit()
    {
        DeleteRoomList();
        cachedRoomList.Clear();
        gameUi._canvasUi.GameAccepting.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.GameAcceptingBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.AcceptBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.GameAcceptingBackBtn.onClick.AddListener(GameAcceptingBackBtnClick);
        gameUi._buttonUi.AcceptBtn.onClick.AddListener(AcceptBtnClick);
    }

    private void AcceptBtnClick()
    {
        HandleEvents.PopoupErrorMsgOpen("Work in progress");

        //HandleEvents.ChangeStates(States.GamePlay);
    }

    private void GameAcceptingBackBtnClick()
    {
        PhotonNetwork.Disconnect();
        HandleEvents.ChangeStates(States.joinMultiplayGame);
    }
}
