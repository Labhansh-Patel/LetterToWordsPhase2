using System;
using System.Collections.Generic;
using APICalls;
using GameEvents;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CalendarUI))]
public class Calendar : MonoBehaviour
{
    [SerializeField] private CalendarUI _calendarUI;

    [SerializeField] private GamePlayController _gamePlayController;

    private DateTime currentDateTime;

    private int gameType;
    private string PrefabDate;

    private int gamemode;

    private bool isCurrentMonth =>
        DateTime.Now.Month == currentDateTime.Month && DateTime.Now.Year == currentDateTime.Year;


    // Start is called before the first frame update
    void Start()
    {
        if (_calendarUI == null)
        {
            _calendarUI = GetComponent<CalendarUI>();
        }

        _calendarUI.DateGamePanel.SetActive(false);
    }

    public void InitializeCalendar(int gameType, int gameMode)
    {
        currentDateTime = DateTime.Now;
        this.gameType = gameType;
        gamemode = gameMode;
        RefreshCalendar();
        _calendarUI.DateGamePanel.SetActive(false);
        _calendarUI.plusArrow.onClick.RemoveAllListeners();
        _calendarUI.minusArrow.onClick.RemoveAllListeners();
        _calendarUI.CreateGame.onClick.RemoveAllListeners();


        _calendarUI.minusArrow.onClick.AddListener(PreviousMonth);
        _calendarUI.plusArrow.onClick.AddListener(NextMonth);
        _calendarUI.CreateGame.onClick.AddListener(() =>
        {
            if (GlobalData.userData.IsPremiumUser)
            {
                CreateGameBtnClick();
            }
            else
            {
                AdsManager.Instance.onAdFinised -= CreateGameBtnClick;
                AdsManager.Instance.onAdFinised += CreateGameBtnClick;
                AdsManager.Instance.PlayAdInterstitial();
            }
        });
    }

    private void CreateGameBtnClick()
    {
        string time = DateTime.Now.ToString("hh:mm");
        string game_mode;
        LogSystem.LogEvent("Globaldata Gamemode {0}", GlobalData.GameMode);
        if (GlobalData.GameMode == "daily")
        {
            game_mode = "1";
        }
        else
        {
            game_mode = "2";
        }

        GlobalData.GameDate = PrefabDate;


        CommonApi.CreateGame(GlobalData.UserId, "1", time, PrefabDate, game_mode, 1, _gamePlayController);
    }

    private void NextMonth()
    {
        if (isCurrentMonth) return; //  cannot go to the next month guard clause 

        currentDateTime = currentDateTime.AddMonths(1);
        RefreshCalendar();
    }

    private void RefreshCalendar()
    {
        GetMonthData(currentDateTime.Month, currentDateTime.Year, gamemode);
    }

    private void PreviousMonth()
    {
        currentDateTime = currentDateTime.AddMonths(-1);
        RefreshCalendar();
    }

    private void GetMonthData(int Month, int year, int gamemode)
    {
        _calendarUI.month.text = currentDateTime.ToString("MMM");
        GameUi.instance._canvasUi.Loading.SetActive(true);
        ApiManager.GetCalendarData(Month, year, gamemode, GlobalData.UserId, HandleCalendarData);
    }

    private void HandleCalendarData(bool asucess, List<CalendarData> calendardata)
    {
        if (asucess)
        {
            int i = 0;


            int numberofDays = DateTime.DaysInMonth(currentDateTime.Year, currentDateTime.Month);
            SetDatePositions(calendardata[0].date, numberofDays);
            foreach (var data in calendardata)
            {
                int daysAvailable = (isCurrentMonth) ? DateTime.Now.Day : numberofDays;

                if (i < daysAvailable)
                {
                    // todo: Change this order-----
                    int date = i + 1;
                    switch (data.game_status)
                    {
                        case 0:
                            _calendarUI.calendarTiles[i].sprite = _calendarUI.GameAvailable;

                            _calendarUI.calendarTiles[i].gameObject.SetActive(true);
                            break;
                        case 1:
                            _calendarUI.calendarTiles[i].sprite = _calendarUI.gameInProgress;
                            _calendarUI.calendarTiles[i].gameObject.SetActive(true);
                            break;
                        case 2:
                            _calendarUI.calendarTiles[i].sprite = _calendarUI.gameCompleted;
                            _calendarUI.calendarTiles[i].gameObject.SetActive(true);
                            break;
                    }

                    _calendarUI.calendarTiles[i].GetComponent<Button>().onClick.RemoveAllListeners();
                    _calendarUI.calendarTiles[i].GetComponent<Button>().onClick.AddListener((() => GetDataForDate(date)));
                }
                else
                {
                    _calendarUI.calendarTiles[i].sprite = _calendarUI.None;
                    _calendarUI.calendarTiles[i].gameObject.SetActive(true);
                    _calendarUI.calendarTiles[i].GetComponent<Button>().onClick.RemoveAllListeners();
                }

                i++;
            }


            LogSystem.LogEvent("Number of Days {0}", numberofDays);
            for (int j = numberofDays + 1; j <= _calendarUI.calendarTiles.Length; j++)
            {
                _calendarUI.calendarTiles[j - 1].gameObject.SetActive(false);
            }

            GameUi.instance._canvasUi.Loading.SetActive(false);
        }
    }

    private void SetDatePositions(DateTime startDate, int daysAvailable)
    {
        int dayofWeek = (int)startDate.DayOfWeek;

        int j = 0;
        int alldays = daysAvailable + dayofWeek;
        for (int i = dayofWeek; i < alldays; i++)
        {
            _calendarUI.calendarTiles[j].transform.position = _calendarUI.calendarPositions[i].position;

            LogSystem.LogEvent("i {0} j {1}", i, j);
            j++;
        }

        for (int i = alldays; i < _calendarUI.calendarPositions.Length; i++)
        {
            _calendarUI.calendarPositions[i].gameObject.SetActive(false);
        }
    }


    private void GetDataForDate(int date)
    {
        LogSystem.LogEvent("getDataForDate {0}", date);
        string datestring = currentDateTime.Year + "-" + currentDateTime.Month.ToString("00") + "-" + date.ToString("00");
        LogSystem.LogEvent("DateString {0}", datestring);
        PrefabDate = datestring;
        EventHandlerGame.EmitEvent(GameEventType.Loading, true);
        ApiManager.GetGameDataDate(GlobalData.UserId, currentDateTime.Month, datestring, gameType, gamemode, ((scess, data) => CompletedDateGameData(scess, data, date)));
    }

    private void CompletedDateGameData(bool ascess, DateGameData dategamedata, int date)
    {
        EventHandlerGame.EmitEvent(GameEventType.Loading, false);
        if (ascess)
        {
            foreach (Transform prefabT in _calendarUI.GameListContent)
            {
                Destroy(prefabT.gameObject);
            }

            foreach (var gamedata in dategamedata.game_list)
            {
                GameObject gamePrefab = Instantiate(_calendarUI.GameDataListPrefab, _calendarUI.GameListContent);
                var DateGame = gamePrefab.GetComponent<DateGamePrefab>();
                DateGame.SetGameData(gamedata, date);
                DateGame.OnCompleteDelete.RemoveAllListeners();
                DateGame.OnCompleteDelete.AddListener(GetDataForDate);
            }

            _calendarUI.GameListDate.text = date + " " + currentDateTime.ToString("MMMM") + " " + currentDateTime.Year;
            GlobalData.GameDate = date + " " + currentDateTime.ToString("MMMM") + " " + currentDateTime.Year;

            _calendarUI.DateGamePanel.SetActive(true);
        }
    }
}