public class MultiplayerOption : IState
{
    private GameUi gameUi;

    public MultiplayerOption(GameUi gameUi)
    {
        this.gameUi = gameUi;
    }


    public void Enter()
    {
        gameUi._canvasUi.MuitiplayerOption.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.MuitiplayerOption.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.MultiplayerBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.JoinBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.StartGame.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.MultiplayerBackBtn.onClick.AddListener(MultiplayerBackBtnClick);
        gameUi._buttonUi.JoinBtn.onClick.AddListener(JoinBtnClick);
        gameUi._buttonUi.StartGame.onClick.AddListener(StartGameClick);
    }

    private void StartGameClick()
    {
        gameUi._textUi.privateModeTxt.text = GlobalData.userData.IsPremiumUser ? "PRIVATE GAME" : "PRIVATE GAME (Premium only)";
        gameUi._imageUi.privateOptionStartGameHideImage.gameObject.SetActive(!GlobalData.userData.IsPremiumUser);
        HandleEvents.ChangeStates(States.startGame);
    }

    private void JoinBtnClick()
    {
      //  gameUi._imageUi.privateOptionJoinGameHideImage.gameObject.SetActive(!GlobalData.userData.IsPremiumUser);
        HandleEvents.ChangeStates(States.joinMultiplayGame);
    }

    private void MultiplayerBackBtnClick()
    {
        HandleEvents.ChangeStates(States.home);
    }
}