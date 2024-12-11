using GameEvents;
using Gameplay;

public class JoinMultiplayerGame : IState
{
    private GameUi gameUi;

    public JoinMultiplayerGame(GameUi gameUi)
    {
        this.gameUi = gameUi;
    }


    public void Enter()
    {
        gameUi._canvasUi.JoinMultiplayer.SetActive(true);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.JoinMultiplayer.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.JoinGame_BackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.AcceptingGameBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.ShowOpenBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.EnterGameBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.SubmitRoomNameBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.CloseRoomNameBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.JoinGame_BackBtn.onClick.AddListener(JoinGame_BackBtnClick);
        gameUi._buttonUi.AcceptingGameBtn.onClick.AddListener(AcceptingGameBtnClick);
        gameUi._buttonUi.ShowOpenBtn.onClick.AddListener(ShowOpenBtnClick);
        gameUi._buttonUi.EnterGameBtn.onClick.AddListener(EnterGameBtnClick);
        gameUi._buttonUi.SubmitRoomNameBtn.onClick.AddListener(SubmitRoomNameBtnClick);
        gameUi._buttonUi.CloseRoomNameBtn.onClick.AddListener(CloseRoomNameBtnClick);
    }
    
    private void JoinGame_BackBtnClick()
    {
        HandleEvents.ChangeStates(States.multiplayOption);
    }

    private void AcceptingGameBtnClick()
    {
        HandleEvents.ChangeStates(States.acceptGame);
    }

    private void ShowOpenBtnClick()
    {
        HandleEvents.ChangeStates(States.openGame);
    }

    private void EnterGameBtnClick()
    {
        // if (!GlobalData.userData.IsPremiumUser)
        // {
        //     HandleEvents.PopoupErrorMsgOpen(GameMessages.CannotJoinPrivateRoomNonPremium);
        //     //  EventHandlerGame.EmitEvent(GameEventType.ShowPopupText,GameMessages.CannotCreateRoomNonPremium);
        //     return;
        // }

        gameUi._canvasUi.EnterGameCode.SetActive(true);
    }

    private void SubmitRoomNameBtnClick()
    {
        if (string.IsNullOrEmpty(gameUi._inputFieldUi.roomCode.text))
        {
            HandleEvents.PopoupErrorMsgOpen(GameMessages.EnterRoomCode);
            //EventHandlerGame.EmitEvent(GameEventType.ShowPopupText,GameMessages.EnterRoomCode);
            return;
        }

        gameUi._canvasUi.EnterGameCode.SetActive(false);
        CreateMultiplayerData createMultiplayerData = new CreateMultiplayerData();
        createMultiplayerData.MultiplayerAction = MultiplayerAction.JoinGame;
        createMultiplayerData.MultiplayerMode = MultiplayerMode.Private;
        createMultiplayerData.roomName = gameUi._inputFieldUi.roomCode.text;
        EventHandlerGame.EmitEvent(GameEventType.ConnectMultiplayer, createMultiplayerData);
        EventHandlerGame.EmitEvent(GameEventType.Loading, true);
        // HandleEvents.ChangeStates(States.GamePlay);
    }

    private void CloseRoomNameBtnClick()
    {
        gameUi._canvasUi.EnterGameCode.SetActive(false);
    }
}