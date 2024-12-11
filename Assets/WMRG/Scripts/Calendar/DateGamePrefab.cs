using APICalls;
using GameEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class DateEvent : UnityEvent<int>
{
}

public class DateGamePrefab : MonoBehaviour
{
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button deleteBtn;
    [SerializeField] private Button checkBtn;

    [SerializeField] private Text GameID;
    [SerializeField] private Text status;
    [SerializeField] private Text time;

    GameListDate _gamedata;

    private int gameDate;

    public DateEvent OnCompleteDelete = new DateEvent();


    public void SetGameData(GameListDate gamedata, int date)
    {
        _gamedata = gamedata;
        this.gameDate = date;
        GameID.text = "Game ID: " + gamedata.id;
        status.text = (gamedata.status == 1) ? "InProgress" : "Completed";

        time.text = gamedata.time;
        if (gamedata.status == 1)
        {
            resumeBtn.gameObject.SetActive(true);
            deleteBtn.gameObject.SetActive(true);
            checkBtn.gameObject.SetActive(false);
        }
        else
        {
            resumeBtn.gameObject.SetActive(false);
            deleteBtn.gameObject.SetActive(true);
            checkBtn.gameObject.SetActive(true);
        }

        RemoveListeners();
        AddAllListeners();
    }

    private void RemoveListeners()
    {
        deleteBtn.onClick.RemoveAllListeners();
        checkBtn.onClick.RemoveAllListeners();
        resumeBtn.onClick.RemoveAllListeners();
    }

    private void AddAllListeners()
    {
        deleteBtn.onClick.AddListener(DeleteBtnClick);
        checkBtn.onClick.AddListener(checkBtnClick);
        resumeBtn.onClick.AddListener(() =>
        {
            if (GlobalData.userData.IsPremiumUser)
            {
                resumeBtnClick();
            }
            else
            {
                AdsManager.Instance.onAdFinised -= resumeBtnClick;
                AdsManager.Instance.onAdFinised += resumeBtnClick;
                AdsManager.Instance.PlayAdInterstitial();
            }
        });
    }

    private void checkBtnClick()
    {
        GlobalData.isCheckBtnOn = true;
        ApiManager.GameData(_gamedata.id.ToString(), HandleGameData);
    }

    private void DeleteBtnClick()
    {
        ApiManager.DeleteGame(_gamedata.id.ToString(), HandleDeleteGame);
    }

    private void HandleDeleteGame(bool aScess, DeleteGameHeader callback)
    {
        if (aScess)
        {
            HandleEvents.PopoupErrorMsgOpen(callback.message);
            OnCompleteDelete?.Invoke(gameDate);
        }
        else
        {
            HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
    }

    private void resumeBtnClick()
    {
        EventHandlerGame.EmitEvent(GameEventType.Loading, true);
        ApiManager.GameData(_gamedata.id.ToString(), HandleGameData);
    }

    private void HandleGameData(bool asucess, GameDataHeader callback)
    {
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "UpdateNotification  ID : {0}", callback.message);

            GlobalData.GameId = callback.game_data.id;
            if (callback.game_data.game_data != null)
            {
                EventHandlerGame.EmitEvent(GameEventType.ResumeGame, callback.game_data.game_data);
                HandleEvents.ChangeStates(States.GamePlay);
                //GameController.data.GetResumeData(callback.game_data.game_data);
            }
            else
            {
                //GameController.data.StartGame();
            }
        }
        else
        {
            LogSystem.LogColorEvent("red", "UpdateNotification  ID : {0}", callback.message);
            HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
    }

    private void OnDestroy()
    {
        OnCompleteDelete.RemoveAllListeners();
    }
}