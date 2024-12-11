using System;
using APICalls;
using Gameplay;

public class DailyFunGame : IState
{
    private GameUi gameUi;
    private Calendar _calendar;
    private GamePlayController _gamePlayController;

    public DailyFunGame(GameUi gameUi, Calendar calendar, GamePlayController gamePlayController)
    {
        this.gameUi = gameUi;
        this._calendar = calendar;
        _gamePlayController = gamePlayController;
    }

    public void Enter()
    {
        gameUi._canvasUi.DailyFunGame.SetActive(true);
        GlobalData.GameMode = "daily";
        RemoveListeners();
        AddAllListeners();
        _calendar.InitializeCalendar(1, 1);
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.DailyFunGame.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.DailyFunBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.DailyFunInfoBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.DailyFunCloseInfoBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.DailyFunStartTodayBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.DailyFunBackBtn.onClick.AddListener(DailyFunBackBtnClick);
        gameUi._buttonUi.DailyFunInfoBtn.onClick.AddListener(DailyFunInfoBtnClick);
        gameUi._buttonUi.DailyFunCloseInfoBtn.onClick.AddListener(DailyFunCloseInfoBtnClick);

        if (GlobalData.userData.IsPremiumUser)
        {
            gameUi._buttonUi.DailyFunStartTodayBtn.onClick.AddListener(PlayTodayBtnClick);
        }
        else
        {
            gameUi._buttonUi.DailyFunStartTodayBtn.onClick.AddListener(AdsManager.Instance.PlayAdInterstitial);
            AdsManager.Instance.onAdFinised -= PlayTodayBtnClick;
            AdsManager.Instance.onAdFinised += PlayTodayBtnClick;
        }
    }

    private void DailyFunCloseInfoBtnClick()
    {
        gameUi._canvasUi.DailyFunInfoPopUp.SetActive(false);
    }

    private void DailyFunInfoBtnClick()
    {
        gameUi._canvasUi.DailyFunInfoPopUp.SetActive(true);
    }

    private void PlayTodayBtnClick()
    {
        string time = DateTime.Now.ToString("hh:mm");

        string date = DateTime.Now.ToString("yyyy-MM-dd");
        GlobalData.GameDate = date;
        GlobalData.GameMode = "daily";
        string game_mode = "1";

        CommonApi.CreateGame(GlobalData.UserId, "1", time, date, game_mode, 1, _gamePlayController);

        // ApiManager.CreateGame(GlobalData.UserId, "1", time, date, game_mode, HandleCreateGame);


        // HandleEvents.ChangeStates(States.GamePlay);
    }

    private void DailyFunBackBtnClick()
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