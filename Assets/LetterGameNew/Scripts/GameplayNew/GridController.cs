using System.Collections.Generic;
using System.Linq;
using APICalls;
using GameEvents;
using UnityEngine;

namespace Gameplay
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private int columnNumber;
        [SerializeField] private int rowNumber;

        [SerializeField] private TileSet[] allTiles;

        [SerializeField] private List<LetterBlockData> addedLetterBlock = new List<LetterBlockData>();

        public int GetAddedLetterCount => addedLetterBlock.Count;
        public int GetAddedNewLetterCount => AddedNewLetterCount();

        private int wordScore = 0;

        public int CurrentScore => wordScore;

        private LetterTile lastLetterBlock = null;

        private bool isValidWord;

        private List<ValidLetterBlocks> _validLetterBlocksList = new List<ValidLetterBlocks>();

        public List<ValidLetterBlocks> GetValidLetterBlocks => _validLetterBlocksList;

        public List<ValidLetterBlocks> NewValidLetterBlocksList => GetListNewWords();


        private bool hasVerticalWord = false;
        private bool hasHorizontalWord = false;

        private bool wrongWords;


        public bool IsValidForRound
        {
            get
            {
                if (_validLetterBlocksList.Count < 1)
                {
                    return false;
                }

                return isValidWord;
            }
        }


        public bool IsWrongWords => wrongWords;

        // Start is called before the first frame update
        void Start()
        {
            AddGridTiles();
            EventHandlerGame.TileAdded += AddedTile;
            EventHandlerGame.TileRemoved += RemovedTile;
            EventHandlerGame.CalculateScore += ValidateWord;
        }

        private void OnDestroy()
        {
            EventHandlerGame.TileAdded -= AddedTile;
            EventHandlerGame.TileRemoved -= RemovedTile;
            EventHandlerGame.CalculateScore -= ValidateWord;
        }

        public void ClearGame()
        {
            addedLetterBlock.Clear();
            foreach (var tileSet in allTiles)
            {
                tileSet.RemoveTile();
            }
        }

        public void SetLetterBlockColors(Color color)
        {
            foreach (var letterBlock in addedLetterBlock)
            {
                letterBlock.TileSet.SetBorderColor(color);
                // foreach (var VARIABLE in letterBlock.TileSet)
                // {
                //     
                // }
            }
        }


        private int AddedNewLetterCount()
        {
            int count = 0;
            foreach (var letterBlock in addedLetterBlock)
            {
                if (letterBlock.TileSet.IsAvailable)
                {
                    count++;
                }
            }

            return count;
        }

        public void ValidateWord()
        {
            List<ValidLetterBlocks> newValidLetterBlocks = new List<ValidLetterBlocks>();
            EventHandlerGame.EmitEvent(GameEventType.UpdateWordList, newValidLetterBlocks);
            EventHandlerGame.EmitEvent(GameEventType.CalculateRoundBonus);
            if (addedLetterBlock.Count == 0)
            {
                EventHandlerGame.EmitEvent(GameEventType.UpdateScoreUI, wordScore);
                return;
            }

            ResetForValidation();

            InitializeHorizontalWord();
            InitializeVerticalWord();
            ResetBlockColor();

            newValidLetterBlocks = GetListNewWords();
            if (newValidLetterBlocks.Count == 0)
            {
                EventHandlerGame.EmitEvent(GameEventType.UpdateScoreUI, 0);
                return;
            }

            LogSystem.LogEvent("NewValidLetterCount {0}", newValidLetterBlocks.Count);

            foreach (var validLetterBlock in newValidLetterBlocks)
            {
                string word = string.Empty;
                foreach (var letterBlock in validLetterBlock.validWords)
                {
                    word += letterBlock.TileSet.BlockLetterString;
                }

                // LogSystem.LogColorEvent("green", "New Word {0}", word);
            }


            CheckForValidConnection(newValidLetterBlocks);

            LogSystem.LogColorEvent("green", "ValidNewBlocks {0}", isValidWord);

            if (IsValidForRound)
            {
                EventHandlerGame.EmitEvent(GameEventType.UpdateWordList, GetListNewWords());
                CheckForValidWord(newValidLetterBlocks);


                // if (!isValidWord)
                // {
                //     return;
                // }


                // if (lastLetterBlock != null)
                // {
                //     lastLetterBlock.SetWordScore(wordScore);
                // }


                int turnScore = 0;


                foreach (var validLetterBlock in newValidLetterBlocks)
                {
                    // foreach (var letterBlock in validLetterBlock.validWords)
                    // {
                    turnScore += validLetterBlock.GetWordScore();
                    // }
                }


                EventHandlerGame.EmitEvent(GameEventType.UpdateScoreUI, turnScore);
            }
        }


        private void CheckForValidConnection(List<ValidLetterBlocks> newValidLetterBlocks)
        {
            if (isValidWord)
            {
                if (newValidLetterBlocks.Count > 1)
                {
                    isValidWord = CheckAllConnectedWord(newValidLetterBlocks);
                }

                // isValidWord = CheckForNewTileConnected(newValidLetterBlocks);
            }
        }

        private void ResetForValidation()
        {
            _validLetterBlocksList.Clear();
            wordScore = 0;
            lastLetterBlock = null;
            isValidWord = true;
            hasVerticalWord = false;
            hasVerticalWord = false;
        }

        private void CheckForValidWord(List<ValidLetterBlocks> newValidLetterBlocks)
        {
            wrongWords = false;
            foreach (var newValidLetter in newValidLetterBlocks)
            {
                string word = string.Empty;

                foreach (var letterBlock in newValidLetter.validWords)
                {
                    word += letterBlock.TileSet.BlockLetterString;
                }

                bool isValid = WordChecker.Instance.CheckForWord(word);


                //
                // if (GlobalData.userData.IsPremiumUser)
                // {
                if (isValid)
                {
                    foreach (var letterBlock in newValidLetter.validWords)
                    {
                        if (letterBlock.TileSet.CanSetGreenColorBorder && GlobalData.userData.IsPremiumUser)
                        {
                            letterBlock.TileSet.SetBorderColor(Color.green);
                        }
                    }
                }
                else
                {
#if !NoValidTest
                    isValidWord = false;
#endif

                    wrongWords = true;

                    foreach (var letterBlock in newValidLetter.validWords)
                    {
                        if (GlobalData.userData.IsPremiumUser)
                        {
                            letterBlock.TileSet.SetBorderColor(Color.red);
                        }
                    }
                }
                // }
            }
        }


        private List<LetterTile> availableLetter = new List<LetterTile>();

        public void CheckAddedTileConnected()
        {
            availableLetter.Clear();

            foreach (var validLetter in _validLetterBlocksList)
            {
                foreach (var letterBlockData in validLetter.validWords)
                {
                    if (!availableLetter.Contains(letterBlockData.TileSet))
                    {
                        availableLetter.Add(letterBlockData.TileSet);
                    }
                }
            }


            if (availableLetter.Count == 0)
            {
                return;
            }

            if (addedLetterBlock.Count != availableLetter.Count)
            {
                isValidWord = false;
                return;
            }


            GoThroughLetterTilesConnection(availableLetter[0]);

            // LogSystem.LogColorEvent("red", "ALLAvailableWordCount {0}", availableLetter.Count);

            foreach (var availableWord in availableLetter)
            {
                LogSystem.LogColorEvent("yellow", "AvailableWord {0}", availableWord.BlockLetterString);
            }


            if (availableLetter.Count > 0)
            {
                isValidWord = false;
            }
        }


        private void GoThroughLetterTilesConnection(LetterTile currentTile)
        {
            if (availableLetter.Contains(currentTile))
            {
                LogSystem.LogEvent("Removing Letter {0}", currentTile);
                availableLetter.Remove(currentTile);
                CheckForAdjoiningConnection(currentTile);
            }
            else
            {
                // LogSystem.LogEvent("Already Went ThroughLetter {0}", currentTile.BlockLetterString);
            }
        }


        private void CheckForAdjoiningConnection(LetterTile currentTile)
        {
            int columnNo = currentTile.CurrentTile.ColumnNo;
            int rowNo = currentTile.CurrentTile.RowNo;

            List<TileSet> adjacentTile = GetAdjacentTiles(columnNo, rowNo);

            foreach (var tileSet in adjacentTile)
            {
                if (tileSet.CurrentLetterTile != null)
                {
                    GoThroughLetterTilesConnection(tileSet.CurrentLetterTile);
                }
            }
        }


        private List<TileSet> GetAdjacentTiles(int columnNo, int rowNo)
        {
            List<TileSet> adjacentTiles = new List<TileSet>();

            int i = 0;
            while (i < 4)
            {
                var increment = (i == 0 || i == 2) ? 1 : -1;
                TileSet tileSet = null;
                if (i < 2)
                {
                    tileSet = GetTile(columnNo + increment, rowNo);
                }
                else
                {
                    tileSet = GetTile(columnNo, rowNo + increment);
                }

                if (tileSet != null)
                {
                    adjacentTiles.Add(tileSet);
                }

                i++;
            }

            foreach (var tileSet in adjacentTiles)
            {
                // LogSystem.LogEvent("tileSet C {0} R {1}", tileSet.ColumnNo , tileSet.RowNo);
            }


            return adjacentTiles;
        }


        public int testColumn, testRow;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                GetAdjacentTiles(testColumn, testRow);
            }
        }


        private bool CheckAllConnectedWord(List<ValidLetterBlocks> NewValidLetterBlocks)
        {
            List<ValidLetterBlocks> allWordAvailable = new List<ValidLetterBlocks>();

            foreach (var validLetter in NewValidLetterBlocks)
            {
                allWordAvailable.Add(validLetter);
            }

            ValidLetterBlocks previousLetterBlock = NewValidLetterBlocks[0];

            GoThroughNewLetterBlock(NewValidLetterBlocks, previousLetterBlock, allWordAvailable);

            // LogSystem.LogColorEvent("red", "AvailableWordCount {0}", allWordAvailable.Count);

            foreach (var availableWord in allWordAvailable)
            {
                // LogSystem.LogColorEvent("yellow", "AvailableWord {0}", availableWord.ToString());
            }

            if (allWordAvailable.Count > 0)
            {
                return false;
            }

            return true;
        }


        private void GoThroughNewLetterBlock(List<ValidLetterBlocks> NewValidLetterBlocks,
            ValidLetterBlocks previousLetterBlock, List<ValidLetterBlocks> allWordAvailable)
        {
            var ListLetterBlocks = previousLetterBlock.GetNewLetterTiles();

            allWordAvailable.Remove(previousLetterBlock);

            var currentSearchLetter = ListLetterBlocks[0];

            int i = 0;

            while (ListLetterBlocks.Count > 0)
            {
                ListLetterBlocks.Remove(currentSearchLetter);

                CheckForNewConnectedLetter(NewValidLetterBlocks, currentSearchLetter, allWordAvailable);

                if (ListLetterBlocks.Count > 0)
                {
                    currentSearchLetter = ListLetterBlocks[0];
                }
            }
        }


        private void CheckForNewConnectedLetter(List<ValidLetterBlocks> NewValidLetterBlocks,
            LetterBlockData currentSearchLetter, List<ValidLetterBlocks> allWordAvailable)
        {
            foreach (var validLetterBlock in NewValidLetterBlocks)
            {
                foreach (var vallidWords in validLetterBlock.validWords)
                {
                    if (vallidWords == currentSearchLetter)
                    {
                        if (allWordAvailable.Contains(validLetterBlock))
                        {
                            allWordAvailable.Remove(validLetterBlock);
                            if (validLetterBlock.GetNumberofNewLetterBlocks() > 1)
                            {
                                // LogSystem.LogColorEvent("yellow", "moreTransition {0}", validLetterBlock.ToString());
                                GoThroughNewLetterBlock(NewValidLetterBlocks, validLetterBlock, allWordAvailable);
                            }

                            allWordAvailable.Remove(validLetterBlock);
                        }
                    }
                }
            }
        }


        public List<ValidLetterBlocks> GetListNewWords()
        {
            List<ValidLetterBlocks> NewvalidLetterBlocksList = new List<ValidLetterBlocks>();

            foreach (var validletterBlock in _validLetterBlocksList)
            {
                // LogSystem.LogColorEvent("red", "---------");
                string worrrd = string.Empty;
                foreach (var words in validletterBlock.validWords)
                {
                    // worrrd += words.TileSet.BlockLetterString;
                    // LogSystem.LogColorEvent("yellow","Wordss {0}",words.TileSet.BlockLetterString);
                    if (words.TileSet.IsAvailable)
                    {
                        NewvalidLetterBlocksList.Add(validletterBlock);
                        break;
                    }
                }

                // LogSystem.LogColorEvent("red","---------");
            }

            return NewvalidLetterBlocksList;
        }


        private void ResetBlockColor()
        {
            foreach (var addedLetters in addedLetterBlock)
            {
                addedLetters.TileSet.SetBorderColor(Color.white);
            }
        }

        // private bool CheckAllTilesConnected()
        // {
        //     if (_validLetterBlocksList.Count > 0)
        //     {
        //         foreach (var validLetter in _validLetterBlocksList)
        //         {
        //             // LogSystem.LogColorEvent("green","------------");
        //             foreach (var words in validLetter.validWords)
        //             {
        //                 //   LogSystem.LogColorEvent("yellow","ValidWords {0}", words.TileSet.BlockLetterString);
        //             }
        //         }
        //
        //         foreach (var addedletters in addedLetterBlock)
        //         {
        //             bool isValid = false;
        //             foreach (var validLetter in _validLetterBlocksList)
        //             {
        //                 if (validLetter.validWords.Contains(addedletters))
        //                 {
        //                     // LogSystem.LogColorEvent("red","validWord {0}", addedletters.TileSet.BlockLetterString);
        //                     isValid = true;
        //                     break;
        //                 }
        //                 else
        //                 {
        //                     isValid = false;
        //                 }
        //             }
        //
        //             if (!isValid)
        //             {
        //                 return false;
        //             }
        //
        //             // if (addedLetterBlock.Contains(blockLetters.))
        //             // {
        //             //     
        //             // }
        //         }
        //
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }

        private void InitializeVerticalWord()
        {
            addedLetterBlock = addedLetterBlock.OrderBy((data => data.columnNumber))
                .ThenBy((data => data.rowNumber))
                .ToList();

            List<LetterDataSperated> columnSeperatedData = new List<LetterDataSperated>();
            int columnNumber = addedLetterBlock[0].columnNumber;
            Queue<LetterBlockData> currentColumn = new Queue<LetterBlockData>();
            int rowNo = addedLetterBlock[0].rowNumber;
            for (int i = 0; i < addedLetterBlock.Count; i++)
            {
                // LogSystem.LogColorEvent("green","Letter {0} CurrentColumn {1}",letterBlockData.columnNumber, columnNumber);
                if (addedLetterBlock[i].columnNumber == columnNumber && (rowNo == addedLetterBlock[i].rowNumber ||
                                                                         rowNo + 1 == addedLetterBlock[i].rowNumber))
                {
                    rowNo = addedLetterBlock[i].rowNumber;
                    currentColumn.Enqueue(addedLetterBlock[i]);
                    // LogSystem.LogColorEvent("green", "Letter {0} CurrentColumnCount {1}",
                    // addedLetterBlock[i].TileSet.BlockLetterString, currentColumn.Count);
                }
                else
                {
                    // LogSystem.LogColorEvent("green", "Letter {0} CurrentColumnCount {1}",
                    //     addedLetterBlock[i].TileSet.BlockLetterString, currentColumn.Count);
                    AddSeperatedData(currentColumn, columnSeperatedData);
                    if (i + 1 < addedLetterBlock.Count)
                    {
                        //currentColumn.Enqueue(addedLetterBlock[i]);
                        columnNumber = addedLetterBlock[i].columnNumber;
                    }

                    if (i < addedLetterBlock.Count - 1)
                    {
                        rowNo = addedLetterBlock[i].rowNumber;
                        i -= 1;
                    }
                }
            }

            AddSeperatedData(currentColumn, columnSeperatedData);

            // LogSystem.LogColorEvent("green", "CurrentLetterBlocks{0}", columnSeperatedData.Count);

            int coloumn = 0;

            if (columnSeperatedData.Count > 0)
            {
                coloumn = columnSeperatedData[0].seperatedData[0].columnNumber;
            }

            foreach (var Blocks in columnSeperatedData)
            {
                if (Blocks.seperatedData.Count == 0)
                {
                    return;
                }

                int score = 0;
                // if (Blocks.seperatedData.Count < 2)
                // {
                //     isValidWord = false;
                //     return;
                // }

                CheckVerticalWord(Blocks.seperatedData, out score);
                wordScore += score;
                int difference = Blocks.seperatedData[0].columnNumber - coloumn;
                if (difference == 0 || difference == 1)
                {
                    isValidWord = true;
                    coloumn = Blocks.seperatedData[0].columnNumber;
                }
                else
                {
                    isValidWord = false;
                    return;
                }
            }
        }

        private void InitializeHorizontalWord()
        {
            addedLetterBlock = addedLetterBlock.OrderBy((data => data.rowNumber))
                .ThenBy((data => data.columnNumber))
                .ToList();

            List<LetterDataSperated> columnSeperatedData = new List<LetterDataSperated>();
            int rowNumber = addedLetterBlock[0].rowNumber;
            Queue<LetterBlockData> currentRow = new Queue<LetterBlockData>();
            int columnNumber = addedLetterBlock[0].columnNumber;
            for (int i = 0; i < addedLetterBlock.Count; i++)
            {
                // LogSystem.LogColorEvent("green","Letter {0} CurrentColumn {1}",letterBlockData.columnNumber, columnNumber);
                if (addedLetterBlock[i].rowNumber == rowNumber && (columnNumber == addedLetterBlock[i].columnNumber ||
                                                                   columnNumber + 1 ==
                                                                   addedLetterBlock[i].columnNumber))
                {
                    columnNumber = addedLetterBlock[i].columnNumber;
                    currentRow.Enqueue(addedLetterBlock[i]);
                    //LogSystem.LogColorEvent("green", "Letter {0} CurrentColumnCount {1}",
                    // addedLetterBlock[i].TileSet.BlockLetterString, currentRow.Count);
                }
                else
                {
                    // LogSystem.LogColorEvent("green", "Letter {0} CurrentColumnCount {1}",
                    //     addedLetterBlock[i].TileSet.BlockLetterString, currentRow.Count);
                    AddSeperatedData(currentRow, columnSeperatedData);
                    if (i + 1 < addedLetterBlock.Count)
                    {
                        //currentRow.Enqueue(addedLetterBlock[i]);
                        rowNumber = addedLetterBlock[i].rowNumber;
                    }

                    if (i < addedLetterBlock.Count - 1)
                    {
                        columnNumber = addedLetterBlock[i].columnNumber;
                        i -= 1;
                    }
                }
            }

            AddSeperatedData(currentRow, columnSeperatedData);

            int row = 0;

            if (columnSeperatedData.Count > 0)
            {
                row = columnSeperatedData[0].seperatedData[0].rowNumber;
            }

            // LogSystem.LogColorEvent("green", "CurrentLetterBlocks{0}", columnSeperatedData.Count);

            foreach (var Blocks in columnSeperatedData)
            {
                if (Blocks.seperatedData.Count == 0)
                {
                    return;
                }

                int score = 0;
                // if (Blocks.seperatedData.Count < 2)
                // {
                //     isValidWord = false;
                //     return;
                // }

                CheckHorizontalWord(Blocks.seperatedData, out score);
                wordScore += score;
                int difference = Blocks.seperatedData[0].rowNumber - row;
                if (difference == 0 || difference == 1)
                {
                    isValidWord = true;
                    row = Blocks.seperatedData[0].rowNumber;
                }
                else
                {
                    isValidWord = false;
                    return;
                }
            }
        }

        private static void AddSeperatedData(Queue<LetterBlockData> currentColumn,
            List<LetterDataSperated> columnSeperatedData)
        {
            // if (currentColumn.Count > 1)
            // {
            // LogSystem.LogColorEvent("green", "CurrentColumn{0}", currentColumn.Count);
            LetterDataSperated newData = new LetterDataSperated();

            foreach (var data in currentColumn)
            {
                newData.seperatedData.Add(data);
            }

            columnSeperatedData.Add(newData);
            // }

            currentColumn.Clear();
            currentColumn.TrimExcess();
        }

        private void CheckVerticalWord(List<LetterBlockData> letterBlockDatas, out int wordScore)
        {
            string verticalWord = string.Empty;
            int score = 0;
            int wordBonus = 0;
            wordScore = score;
            if (letterBlockDatas.Count > 1)
            {
                // int column = addedLetterBlock[0].columnNumber;
                int row = letterBlockDatas[0].rowNumber;

                for (int i = 0; i < letterBlockDatas.Count; i++)
                {
                    if (row == letterBlockDatas[i].rowNumber)
                    {
                        letterBlockDatas[i].TileSet.EnableWordScore(false);
                        score += letterBlockDatas[i].TileSet.BlockScore;
                        wordBonus += letterBlockDatas[i].TileSet.WordBonus;
                        lastLetterBlock = letterBlockDatas[i].TileSet;
                        verticalWord += letterBlockDatas[i].TileSet.BlockLetterString;
                        row++;
                    }
                }

                if (verticalWord.Length > 1)
                {
                    // if (!(lastLetterBlock is null)) lastLetterBlock.SetWordScore(score);
                    wordBonus = (wordBonus == 0) ? 1 : wordBonus;
                    wordScore = score * wordBonus;
                    ValidLetterBlocks validLetterBlocks = new ValidLetterBlocks();
                    validLetterBlocks.validWords = letterBlockDatas;
                    _validLetterBlocksList.Add(validLetterBlocks);
                    hasVerticalWord = true;
                    // LogSystem.LogColorEvent("yellow", "VerticalWord {0}", verticalWord);
                }
            }
            else
            {
                // LogSystem.LogColorEvent("red", "VerticalWord One{0}", letterBlockDatas[0].TileSet.BlockLetterString);
            }
        }

        private void CheckHorizontalWord(List<LetterBlockData> letterBlockDatas, out int wordScore)
        {
            string horizontalWord = string.Empty;
            int score = 0;
            int wordBonus = 0;
            wordScore = score;
            if (letterBlockDatas.Count > 1)
            {
                int column = letterBlockDatas[0].columnNumber;

                for (int i = 0; i < letterBlockDatas.Count; i++)
                {
                    if (column == letterBlockDatas[i].columnNumber)
                    {
                        letterBlockDatas[i].TileSet.EnableWordScore(false);
                        score += letterBlockDatas[i].TileSet.BlockScore;
                        wordBonus += letterBlockDatas[i].TileSet.WordBonus;
                        lastLetterBlock = letterBlockDatas[i].TileSet;

                        horizontalWord += letterBlockDatas[i].TileSet.BlockLetterString;
                        column++;
                    }
                }

                //LogSystem.LogColorEvent("red","Horizontalword {0}", horizontalWord);
                if (horizontalWord.Length > 1)
                {
                    wordBonus = (wordBonus == 0) ? 1 : wordBonus;
                    wordScore = score * wordBonus;
                    ValidLetterBlocks validLetterBlocks = new ValidLetterBlocks();
                    validLetterBlocks.validWords = letterBlockDatas;
                    _validLetterBlocksList.Add(validLetterBlocks);
                    hasHorizontalWord = true;
                    // if (!(lastLetterBlock is null)) lastLetterBlock.SetWordScore(score);
                    // LogSystem.LogColorEvent("yellow", "HorizontalWord {0}", horizontalWord);
                }
            }
            else
            {
                //LogSystem.LogColorEvent("red", "HorizontalWord One{0}", letterBlockDatas[0].TileSet.BlockLetterString);
            }
        }

        private void RemovedTile(LetterBlockData letterBlockData)
        {
            if (addedLetterBlock.Contains(letterBlockData))
            {
                addedLetterBlock.Remove(letterBlockData);
                EventHandlerGame.EmitEvent(GameEventType.GridCountUpdate);
            }
        }

        private void AddedTile(LetterBlockData letterBlockData)
        {
            if (!addedLetterBlock.Contains(letterBlockData))
            {
                addedLetterBlock.Add(letterBlockData);
                ValidateWord();
                EventHandlerGame.EmitEvent(GameEventType.GridCountUpdate);
            }
        }

        private void AddGridTiles()
        {
            int row = 0;
            int column = 0;
            int index = 0;
            for (int i = 0; i < rowNumber; i++)
            {
                for (int j = 0; j < columnNumber; j++)
                {
                    allTiles[index].SetTileSetNumber(column, row);
                    index++;
                    column++;
                }

                row++;
                column = 0;
            }
        }

        public TileSet GetTile(int columnNumber, int rowNumber)
        {
            foreach (var tileSet in allTiles)
            {
                if (tileSet.CheckTileEquals(columnNumber, rowNumber))
                {
                    return tileSet;
                }
            }

            LogSystem.LogColorEvent("red", "Couldnot find the tileSet");
            return null;
        }
    }
}