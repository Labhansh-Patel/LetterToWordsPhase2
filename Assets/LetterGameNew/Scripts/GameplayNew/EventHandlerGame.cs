using System;
using System.Collections.Generic;
using APICalls;
using Gameplay;
using Photon.Realtime;

namespace GameEvents
{
    public enum GameEventType
    {
        AddedLetter,
        RemovedLetter,
        WordDone,
        TossLetter,
        LetterDone,
        UpdateScoreUI,
        CalculateScore,
        MoveAdded,
        AnyLetter,
        AnyLetterStack,
        SelectLetter,
        ResumeGame,
        CreateGame,
        GridCountUpdate,
        UpdateWordList,
        ConnectMultiplayer,
        ClearRoom,
        ExitGameRoom,
        CalculateRoundBonus,
        ShowPopupText,
        ResetBonusCount,
        AddBonus,
        AddExtraTime,
        BlockExtraTime,
        RoomListUpdate,
        JoinGame,
        Loading,
        RemoveExtraLetter
    }


    public class EventHandlerGame : BaseEventSystem
    {
        private static Dictionary<GameEventType, Delegate> allEvents = new Dictionary<GameEventType, Delegate>();
        public static event Callback<LetterBlockData> TileAdded;
        public static event Callback<LetterBlockData> TileRemoved;
        public static event Callback WordDone;
        public static event Callback<LetterTile> TossLetter;
        public static event Callback<LetterTile> LetterDone;
        public static event Callback<int> UpdateScore;
        public static event Callback CalculateScore;
        public static event Callback<MoveData> AddedMoveData;
        public static event Callback<LetterBlock> AnyLetter;
        public static event Callback<RemainingTile> AnyLetterStack;
        public static event Callback SelectTile;
        public static event Callback<GameSyncData> ResumeGame;
        public static event Callback<bool, string, CreateGameData> CreateGame;
        public static event Callback GridCountUpdate;
        public static event Callback<List<ValidLetterBlocks>> UpdateWordList;
        public static event Callback<CreateMultiplayerData> ConnectMultiplayer;
        public static event Callback<CreateMultiplayerData> JoinRoom;
        public static event Callback ClearRoom;
        public static event Callback ExitRoom;
        public static event Callback CalculateRoundBonus;
        public static event Callback<string> ShowPopup;
        public static event Callback ResetBonus;
        public static event Callback AddExtraTime;
        public static event Callback BlockExtraTime;
        public static event Callback<List<RoomInfo>> RoomListUpdate;
        public static event Callback<bool> Loading;
        public static event Callback<MoveData> RemoveExtraLetterTray;


        public static void RefreshDelegates()
        {
            allEvents.Clear();

            allEvents.Add(GameEventType.AddedLetter, TileAdded);
            allEvents.Add(GameEventType.RemovedLetter, TileRemoved);
            allEvents.Add(GameEventType.WordDone, WordDone);
            allEvents.Add(GameEventType.TossLetter, TossLetter);
            allEvents.Add(GameEventType.LetterDone, LetterDone);
            allEvents.Add(GameEventType.UpdateScoreUI, UpdateScore);
            allEvents.Add(GameEventType.CalculateScore, CalculateScore);
            allEvents.Add(GameEventType.MoveAdded, AddedMoveData);
            allEvents.Add(GameEventType.AnyLetter, AnyLetter);
            allEvents.Add(GameEventType.AnyLetterStack, AnyLetterStack);
            allEvents.Add(GameEventType.SelectLetter, SelectTile);
            allEvents.Add(GameEventType.ResumeGame, ResumeGame);
            allEvents.Add(GameEventType.CreateGame, CreateGame);
            allEvents.Add(GameEventType.GridCountUpdate, GridCountUpdate);
            allEvents.Add(GameEventType.UpdateWordList, UpdateWordList);
            allEvents.Add(GameEventType.ConnectMultiplayer, ConnectMultiplayer);
            allEvents.Add(GameEventType.ClearRoom, ClearRoom);
            allEvents.Add(GameEventType.ExitGameRoom, ExitRoom);
            allEvents.Add(GameEventType.CalculateRoundBonus, CalculateRoundBonus);
            allEvents.Add(GameEventType.ShowPopupText, ShowPopup);
            allEvents.Add(GameEventType.ResetBonusCount, ResetBonus);
            allEvents.Add(GameEventType.AddExtraTime, AddExtraTime);
            allEvents.Add(GameEventType.BlockExtraTime, BlockExtraTime);
            allEvents.Add(GameEventType.RoomListUpdate, RoomListUpdate);
            allEvents.Add(GameEventType.JoinGame, JoinRoom);
            allEvents.Add(GameEventType.Loading, Loading);
            allEvents.Add(GameEventType.RemoveExtraLetter, RemoveExtraLetterTray);
        }


        public static void EmitEvent(GameEventType gameEventType)
        {
            RefreshDelegates();

            Delegate eventcall = null;

            allEvents.TryGetValue(gameEventType, out eventcall);

            if (eventcall != null)
            {
                Callback callback = eventcall as Callback;

                callback?.Invoke();
            }
        }


        public static void EmitEvent<T>(GameEventType gameEventType, T arg1)
        {
            RefreshDelegates();
            Delegate eventcall = null;
            allEvents.TryGetValue(gameEventType, out eventcall);

            if (eventcall != null)
            {
                Callback<T> callback = eventcall as Callback<T>;
                //Debug.LogFormat("Callback {0}",eventcall.ToString());
                callback?.Invoke(arg1);
            }
        }

        public static void EmitEvent<T, T1>(GameEventType gameEventType, T arg1, T1 arg2)
        {
            RefreshDelegates();
            Delegate eventcall = null;
            allEvents.TryGetValue(gameEventType, out eventcall);

            if (eventcall != null)
            {
                Callback<T, T1> callback = eventcall as Callback<T, T1>;
                callback?.Invoke(arg1, arg2);
            }
        }

        public static void EmitEvent<T, T1, T2>(GameEventType gameEventType, T arg1, T1 arg2, T2 arg3)
        {
            RefreshDelegates();

            Delegate eventcall = null;

            allEvents.TryGetValue(gameEventType, out eventcall);

            if (eventcall != null)
            {
                Callback<T, T1, T2> callback = eventcall as Callback<T, T1, T2>;

                callback?.Invoke(arg1, arg2, arg3);
            }
        }

        public static void EmitEvent<T, T1, T2, T3>(GameEventType gameEventType, T arg1, T1 arg2, T2 arg3, T3 arg4)
        {
            RefreshDelegates();

            Delegate eventcall = null;

            allEvents.TryGetValue(gameEventType, out eventcall);

            if (eventcall != null)
            {
                Callback<T, T1, T2, T3> callback = eventcall as Callback<T, T1, T2, T3>;

                callback?.Invoke(arg1, arg2, arg3, arg4);
            }
        }
    }

    public class BaseEventSystem
    {
    }
}