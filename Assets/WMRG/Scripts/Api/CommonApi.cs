using System;
using APICalls;
using GameEvents;
using Gameplay;
using UnityEngine;

public static class CommonApi
{
    private static Action CallBack = null;

    public static void UserProfile(string userid, Action action = null)
    {
        CallBack = action;
        ApiManager.GetProfile(userid, HandleProfileCall);
    }

    private static void HandleProfileCall(bool asucess, GetProfileHeader profileResponse)
    {
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", profileResponse.result.name);

            GlobalData.UserName = profileResponse.result.name;
            GlobalData.UserEmail = profileResponse.result.email;
            GlobalData.UserMobileNumber = profileResponse.result.mobile;
            if (profileResponse.result.avatar_id != null)
            {
                GlobalData.UserAvtarId = int.Parse(profileResponse.result.avatar_id);
            }


            GlobalData.userData = profileResponse.result;

            PlayerPrefs.SetString("PlayerName", profileResponse.result.name);

            CallBack?.Invoke();
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", profileResponse.message);

            PlayerPrefs.DeleteAll();
            HandleEvents.ChangeStates(States.login);
        }
    }

    public static void CallDateWiseData(string date)
    {
        UI_Manager._instance.LoadingBarStatus(true);
        ApiManager.DateWiseData(date, HandleDateWiseData);
    }

    private static void HandleDateWiseData(bool aSucess, DateWiseDataHeader callback)
    {
        UI_Manager._instance.LoadingBarStatus(false);
        if (aSucess)
        {
            LogSystem.LogColorEvent("green", "Letter Count  ID : {0}", Alphabet.data.LetterStackList.Count);

            Alphabet.data.LetterStackList.Clear();
            for (int i = 0; i < callback.result.Count; i++)
            {
                LetterStack newdata = new LetterStack(callback.result[i].name, callback.result[i].score);
                Alphabet.data.LetterStackList.Add(newdata);
            }

            UI_Manager._instance.Load();
        }
        else
        {
        }
    }


    public static void CreateGame(string userid, string gametype, string time, string date, string game_mode, int multiplayerType, GamePlayController gamePlayController)
    {
        EventHandlerGame.EmitEvent(GameEventType.Loading, true);
        ApiManager.CreateGame(userid, gametype, time, date, game_mode, multiplayerType, (sucess, callback) => HandleCreateGame(sucess, gamePlayController, date, callback));
    }

    private static void HandleCreateGame(bool asucess, GamePlayController gamePlayController, string date, CreateGameData callback)
    {
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "CreateGame  ID : {0}", callback.letters.Count);
            GlobalData.GameId = int.Parse(callback.game_id);
            //Alphabet.data.LetterStackList.Clear();
            // for (int i = 0; i < callback.letters.Count; i++)
            // {
            //     LetterStack newdata = new LetterStack(callback.letters[i].name, callback.letters[i].score);
            //     Alphabet.data.LetterStackList.Add(newdata);
            //
            // }
            EventHandlerGame.EmitEvent(GameEventType.CreateGame, asucess, date, callback);
            //gamePlayController.CreateNewGame(asucess,date, callback);
            HandleEvents.ChangeStates(States.GamePlay);
            // GameController.data.StartGame();
        }
        else
        {
            LogSystem.LogColorEvent("red", "CreateGame ID : {0}", callback.message);
            HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
    }

    public static void CallResultApi(String GameId, String UserId, String Win, int score)
    {
        ApiManager.GameComplete(GameId, UserId, Win, score, HandleGameComplete);
    }

    private static void HandleGameComplete(bool asucess, GameComplete callback)
    {
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "UpdateNotification  ID : {0}", callback.message);
            HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
        else
        {
            LogSystem.LogColorEvent("red", "UpdateNotification  ID : {0}", callback.message);
            HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
    }
    public static void CallSetOutGameBonusApi(String UserId, String OutGameBonus)
    {
        ApiManager.OutGameBonus(UserId, OutGameBonus, HandleOutGameBonus);
    }

    private static void HandleOutGameBonus(bool asucess, OutGameBonus callback)
    {
        if (asucess)
        {
          //  LogSystem.LogColorEvent("green", "UpdateNotification  ID : {0}", callback.message);
          //  HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
        else
        {
        //    LogSystem.LogColorEvent("red", "UpdateNotification  ID : {0}", callback.message);
        //    HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
    }
    public static void CallGetUserPower()
    {
        ApiManager.GetUserPower(HandleGetUserPower);
    }

    private static void HandleGetUserPower(bool aScess, GetBonousPointResponse callback)
    {
        if (aScess)
        {
            LogSystem.LogColorEvent("green", "GetUserPower : {0}", callback.message);
            GlobalData.AnyLetter = callback.data[0].any_letter;
            GlobalData.ExtraTraySpace = callback.data[0].extra_tray_space;
            GlobalData.StackTile = callback.data[0].any_tile_from_stack;
            GlobalData.FreeGarbage = callback.data[0].free_garbage;
        }
        else
        {
            LogSystem.LogColorEvent("red", "GetUserPower : {0}", callback.message);
            HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
    }


    public static void CallUpdateUserPower(string power, string point, string sign)
    {
        ApiManager.UpdateUserPower(power, point, sign, HandleUpdateUserPower);
    }

    private static void HandleUpdateUserPower(bool aScess, UpdateBonousPointResponse callback)
    {
        if (!aScess)
        {
            LogSystem.LogColorEvent("green", "UpdateUserPower: {0}", callback.message);
            GlobalData.AnyLetter = callback.data.any_letter;
            GlobalData.ExtraTraySpace = callback.data.extra_tray_space;
            GlobalData.StackTile = callback.data.any_tile_from_stack;
            GlobalData.FreeGarbage = callback.data.free_garbage;
        }
        else
        {
            LogSystem.LogColorEvent("red", "UpdateUserPower : {0}", callback.message);
            HandleEvents.PopoupErrorMsgOpen(callback.message);
        }
    }
}