namespace Gameplay
{
    public enum NetworkCode : byte
    {
        StartGame = 19,
        SendRoundScoreData = 20,
        GameSyncData = 21,
        StartNextRound = 22,
        ShowRoundScore = 23,
        ShowFinalResult = 24,
        SendAddExtraTime = 25,
        RecieveAddExtraTime = 26,
        SendBlockExtraTime = 27,
        RecieveBlockExtraTime = 28,
        SendFinalScoreData = 29,
        SendMultiplayerGameData = 30
    }
}