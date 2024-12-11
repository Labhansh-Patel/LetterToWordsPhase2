using System;
using APICalls;
using Gameplay;

public class DailyHardGame : IState
{
    private GameUi gameUi;
    private Calendar _calendar;
    private GamePlayController _gamePlayController;

    public DailyHardGame(GameUi gameUi, Calendar calendar, GamePlayController gamePlayController)
    {
        this.gameUi = gameUi;
        this._calendar = calendar;
        _gamePlayController = gamePlayController;
    }


    public void Enter()
    {
        gameUi._canvasUi.DailyHardCore.SetActive(true);
        GlobalData.GameMode = "hardcore";
        RemoveListeners();
        AddAllListeners();
        _calendar.InitializeCalendar(1, 2);
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.DailyHardCore.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.HardCoreBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.HardCoreInfoBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.HardCoreCloseInfoBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.DailyHardCoreStartTodayBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.HardCoreBackBtn.onClick.AddListener(HardCoreBackBtnClick);
        gameUi._buttonUi.HardCoreInfoBtn.onClick.AddListener(HardCoreInfoBtnClick);
        gameUi._buttonUi.HardCoreCloseInfoBtn.onClick.AddListener(HardCoreCloseInfoBtnClick);

        if (GlobalData.userData.IsPremiumUser)
        {
            gameUi._buttonUi.DailyHardCoreStartTodayBtn.onClick.AddListener(HardCore_PlayTodayBtnClick);
        }
        else
        {
            AdsManager.Instance.onAdFinised -= HardCore_PlayTodayBtnClick;
            AdsManager.Instance.onAdFinised += HardCore_PlayTodayBtnClick;
        }
    }

    private void HardCoreCloseInfoBtnClick()
    {
        gameUi._canvasUi.HardCoreInfoPopUp.SetActive(false);
    }

    private void HardCoreInfoBtnClick()
    {
        gameUi._canvasUi.HardCoreInfoPopUp.SetActive(true);
    }

    private void HardCore_PlayTodayBtnClick()
    {
        string time = DateTime.Now.ToString("hh:mm");

        string date = DateTime.Now.ToString("yyyy-MM-dd");
        GlobalData.GameDate = date;
        GlobalData.GameMode = "hardcore";
        string game_mode = "2";
        CommonApi.CreateGame(GlobalData.UserId, "1", time, date, game_mode, 1, _gamePlayController);

        // ApiManager.CreateGame(GlobalData.UserId, "1", time, date, game_mode, HandleCreateGame);
    }

    private void HardCoreBackBtnClick()
    {
        HandleEvents.BackToPreviousState();
    }

    private void HandleCreateGame(bool asucess, CreateGameData callback)
    {
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "CreateGame  ID : {0}", callback.letters.Count);
            GlobalData.GameId = int.Parse(callback.game_id);
            Alphabet.data.LetterStackList.Clear();
            for (int i = 0; i < callback.letters.Count; i++)
            {
                LetterStack newdata = new LetterStack(callback.letters[i].name, callback.letters[i].score);
                Alphabet.data.LetterStackList.Add(newdata);
            }

            GameController.data.StartGame();
        }
        else
        {
            LogSystem.LogColorEvent("red", "CreateGame ID : {0}", callback.message);
            HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
    }
}