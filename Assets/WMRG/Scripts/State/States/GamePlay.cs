using GameEvents;
using Gameplay;
using UnityEngine;

public class GamePlay : IState
{
    private GameUi gameUi;
    private GamePlayController _gamePlayController;
    public static bool StoreOutBonusCountReset = false;

    public GamePlay(GameUi gameUi, GamePlayController gamePlayController)
    {
        this.gameUi = gameUi;
        this._gamePlayController = gamePlayController;
        EventHandlerGame.ExitRoom += ExitFromGame;
    }


    public void Enter()
    {
        EventHandlerGame.EmitEvent(GameEventType.Loading, false);
        gameUi._canvasUi.gameplay.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.gameplay.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.GamePlayBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.GameExitYesBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.WaitingPanelBackBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.WaitingPanelBackBtn.onClick.AddListener(GamePlayBackBtnClick);
        gameUi._buttonUi.GamePlayBackBtn.onClick.AddListener(GamePlayBackBtnClick);
        gameUi._buttonUi.GameExitYesBtn.onClick.AddListener(ExitFromGame);
    }

    private void GamePlayBackBtnClick()
    {
        gameUi._canvasUi.ExitPanel.SetActive(true);
    }

    private void ExitFromGame()
    {
        StoreOutBonusCountReset = false;
        _gamePlayController.ClearGame();
        HandleEvents.ChangeStates(States.home);
        gameUi._canvasUi.ExitPanel.SetActive(false);

        // todo: check if it's printing for all type of games and modes. Otherwise outgamebonuspoints will not work properly
        Debug.Log("Game Exited!");
    }
}