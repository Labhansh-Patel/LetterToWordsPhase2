using System.Collections.Generic;

namespace Gameplay
{
    public class ValidLetterBlocks
    {
        public List<LetterBlockData> validWords = new List<LetterBlockData>();

        public int GetNumberofNewLetterBlocks()
        {
            int count = 0;
            foreach (var letterBlock in validWords)
            {
                if (letterBlock.TileSet.IsAvailable)
                {
                    count++;
                }
            }

            return count;
        }

        public List<LetterBlockData> GetNewLetterTiles()
        {
            List<LetterBlockData> newLetterTile = new List<LetterBlockData>();
            foreach (var letterBlock in validWords)
            {
                if (letterBlock.TileSet.IsAvailable)
                {
                    newLetterTile.Add(letterBlock);
                }
            }

            return newLetterTile;

        }

        public int GetWordLength()
        {
            string word = string.Empty;
            
            foreach (var letterBlock in validWords)
            {
                word += letterBlock.TileSet.BlockLetterString;
            }

            return word.Length;
        }


        public int GetWordScore()
        {
            int score = 0;
            int wordBonus = 0;
            foreach (var letterBlock in validWords)
            {
                score += letterBlock.TileSet.BlockScore;
                wordBonus += letterBlock.TileSet.WordBonus;
                
            }

            if (GetNumberofNewLetterBlocks()>= 7)
            {
                score += 50;
            }
            wordBonus = (wordBonus == 0) ? 1 : wordBonus;
            
            return (score * wordBonus) ;
        }

        public override string ToString()
        {

            string newLetterTile = string.Empty;
            foreach (var letterBlock in validWords)
            {
                newLetterTile += letterBlock.TileSet.BlockLetterString;
            }

            return newLetterTile;
        }
    }
}