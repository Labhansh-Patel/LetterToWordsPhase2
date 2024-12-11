using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using APICalls;
using System;

public class GameListPrefab : MonoBehaviour
{
    private GameList GameListData;
    [SerializeField] private Text Date;
    [SerializeField] private Text GameDate;
    [SerializeField] private Text Status;
   [SerializeField] private Button Play;
    [SerializeField] private Button End;



    public void SetDataPrefab(GameList friend)
    {
        GameListData = friend;
        SetUidata();
    }
    private void SetUidata()
    {
        Date.text = GameListData.time;
        GameDate.text = GameListData.date;
        if (GameListData.status=="1")
        {
            Status.text = "In-Progress";
        }



        Play.onClick.RemoveAllListeners();
        Play.onClick.AddListener(CalGameDataApi);
        End.onClick.RemoveAllListeners();
        End.onClick.AddListener(EndButtonClick);
    }

    private void EndButtonClick()
    {
        ApiManager.GameComplete(GameListData.id,UI_Manager._instance.User_Id,"0", 0, HandleGameComplete);
    }

    private void HandleGameComplete(bool aSucess, GameComplete callback)
    {
        if (aSucess)
        {
            UI_Manager._instance.ShowGameList();
        }
        else
        {

        }
    }

    private void CalGameDataApi()
    {
      UI_Manager._instance.ShowGameData(GameListData.id);
    }

}
