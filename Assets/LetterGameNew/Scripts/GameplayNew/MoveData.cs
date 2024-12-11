using System;

namespace Gameplay
{
    [Serializable]
    public class MoveData
    {
        public MoveType MoveType;
        public RemainingTile RemainingTile;
        public LetterTile LetterTile;
        public LetterBlock LastLetter;
        public TileSet  TileDropped;
        // public UISlots UISlots;
        // public BonusTile BonusTile;

    }
}