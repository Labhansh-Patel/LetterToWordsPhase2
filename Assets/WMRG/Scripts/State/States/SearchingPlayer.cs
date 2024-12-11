using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingPlayer : IState
{
    private GameUi gameUi;
    public SearchingPlayer(GameUi gameUi)
    {
        this.gameUi = gameUi;

    }
    public void AddAllListeners()
    {
        throw new System.NotImplementedException();
    }

    public void Enter()
    {
        gameUi._canvasUi.SearchingPlayer.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        gameUi._canvasUi.SearchingPlayer.SetActive(false);
    }

    public void RemoveListeners()
    {
        throw new System.NotImplementedException();
    }
}
