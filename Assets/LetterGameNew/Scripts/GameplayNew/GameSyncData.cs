using System;
using System.Collections.Generic;

namespace Gameplay
{
    [Serializable]
    public class GameSyncData
    {
        public List<LetterTileData> LetterTiles;
        public List<LetterBlock> _trayDatas;
        public List<LetterBlock> tossList;
        public List<int> trayemptyIndex;
        public int MinusScore;
        public int LastRoundScore;
        public int TwoXlCount;
        public int ThreeXlCount;
        public int TwoXwCount;
        public int ThreeXwCount;
        public int TurnScore;
        public int WordScore;
        public int BonusCount;
        public bool IsGameFinished;
        public DateTime recievedDataTime;
    }

    public class LetterTileData
    {
        public LetterBlock LetterBlock;
        public PowerUpType PowerUpType;
        public TileData collisionObj;
        public bool IsAvailable;
        public int pos;
    }

    public class TileData
    {
        public int columnNumber;
        public int rowNumber;
    }
}