public class OpenGame : IState
{
    private GameUi gameUi;

    public OpenGame(GameUi gameUi)
    {
        this.gameUi = gameUi;
    }


    public void Enter()
    {
        gameUi._canvasUi.OpenGame.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.OpenGame.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.OpenGameBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.JoinGameBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.OpenGameBackBtn.onClick.AddListener(OpenGameBackBtnClick);
        gameUi._buttonUi.JoinGameBtn.onClick.AddListener(JoinGameBtnClick);
    }

    private void JoinGameBtnClick()
    {
        //HandleEvents.ChangeStates(States.GamePlay);
        HandleEvents.PopoupErrorMsgOpen("Work in progress");
    }

    private void OpenGameBackBtnClick()
    {
        HandleEvents.ChangeStates(States.joinMultiplayGame);
    }
}