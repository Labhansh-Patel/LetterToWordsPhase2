using System;

namespace Gameplay
{
    [Serializable]
    public class LetterBlockData
    {
        public int columnNumber;
        public int rowNumber;
        public LetterTile TileSet;
    }
}