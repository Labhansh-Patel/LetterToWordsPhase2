//#define TestGamePlay

using System.Collections;
using System.Collections.Generic;
using APICalls;
using GameEvents;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class GamePlayController : MonoBehaviour
    {
        [SerializeField] private string testJson;

        [SerializeField] private int InitailLetterBlocks;

        [SerializeField] private LetterTile _letterBlock;

        [SerializeField] private RemainingTile _remainingTile;

        [SerializeField] private Transform parentLetterBlock, parentRemainingBlock;

        [SerializeField] private UISlots[] letterPos;

        [SerializeField] private SelectTilePrefab _selectTilePrefab;

        private GameData _gameDataHeader;

        private readonly Stack<LetterBlock> availableLetters = new Stack<LetterBlock>();

        private Stack<RemainingTile> _trayDatas = new Stack<RemainingTile>();

        private readonly Stack<MoveData> allMoveDatas = new Stack<MoveData>();

        private Dictionary<LetterTile, int> _letterTiles = new Dictionary<LetterTile, int>();

        private List<LetterBlock> tossedLetters = new List<LetterBlock>();

        private List<LetterBlock> currentRoundTossedTile = new List<LetterBlock>();

        [SerializeField] private GridController _gridController;
        [SerializeField] private BonusController _bonusController;
        [SerializeField] private MultiplayerGameHelper _multiplayerGameHelper;
        [SerializeField] private GamePlayControllerUI _controllerUI;

        //private BonusExecuteController _bonusExecuteController = null;

        [Header("BonusTile")] [SerializeField] private BonusTile TwoXL, ThreeXL, TwoXW, ThreeXW;

        [SerializeField] private List<int> BonusColumnNo;
        [SerializeField] private List<int> BonusRowNo;
        private List<int> tossedBinIndex = new List<int>();
        private bool _noTossPenalty = false;
        private static bool gameCompleted = false;
        private int _minusScore = 0;
        private int lastRoundScore = 0;
        private int wordScore = 0;
        private int _turnScore = 0;
        private string currentGameRoundData = null;

        private bool gameDone;

        private bool forceGameEnd = false;

        //todo: Remove this after testing
        [SerializeField] private TMP_InputField noOfTilesInput;
        private int noOfTilesToWin = 61;

        public static bool GameCompleted => gameCompleted;
        public static GamePlayController Instance;

        #region GetterVariables

        public int MinusScore => _minusScore;
        public int WordScore => wordScore;

        public int TurnScore => _turnScore;

        public BonusController BonusController => _bonusController;
        public GridController GridController => _gridController;
        public List<LetterBlock> TossedLetters => tossedLetters;
        public List<LetterBlock> TossedLettersCurrentRound => currentRoundTossedTile;

        public Stack<RemainingTile> TrayDatas => _trayDatas;
        public Dictionary<LetterTile, int> LetterTiles => _letterTiles;

        public List<int> TossedBinIndex => tossedBinIndex;

        #endregion

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            GamePlayData.fingerTileOffset = Screen.height / 14.5f;
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
#if TestGamePlay
            ApiManager.TodayStack(GetTodaysData);

            //_gameDataHeader = DataSaver.loadData<GameDataHeader>("GameData");
            //_gameDataHeader = JsonConvert.DeserializeObject<GameDataHeader>(testJson);
            //InitializeLetterBlock();

#endif

            _controllerUI.undoBtn.onClick.RemoveAllListeners();
            _controllerUI.undoBtn.onClick.AddListener(DoUndoMove);

            _controllerUI.undoAllBtn.onClick.RemoveAllListeners();
            _controllerUI.undoAllBtn.onClick.AddListener(UndoAll);

            _controllerUI.homeBtn.onClick.RemoveAllListeners();
            _controllerUI.homeBtn.onClick.AddListener(BackToHome);
            SubscribeGameEvents();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void RemoveExtraLetterTry(MoveData moveData)
        {
            letterPos[7].gameObject.SetActive(false);
            //   Debug.Log("yo yo");

            if (!moveData.LetterTile.IsOnGrid)
            {
                Destroy(moveData.LetterTile.gameObject);
                //     moveData.RemainingTile.GetLetterBlock.UndoLetter();
                //    _trayDatas.Push(moveData.RemainingTile);
                //     moveData.RemainingTile.gameObject.SetActive(true);


                moveData.LetterTile.SetUISlotsArray(letterPos);
                moveData.LetterTile.SetUISlot(letterPos[7]);

                _letterTiles.Remove(moveData.LetterTile);
                //  tossedBinIndex.Remove(moveData.RemainingTile.transform.GetSiblingIndex());
                _trayDatas.Push(moveData.RemainingTile);
            }
        }

        private void Update()
        {
            // GameSyncData gameSyncData = JsonConvert.DeserializeObject<GameSyncData>(currentGameRoundData);
            // gameSyncData.LetterTiles.ForEach(((letterTileData) => { Debug.Log("ZZZ: " + letterTileData.LetterBlock.letter); }));
        }

        private void BackToHome()
        {
            ClearGame();
            HandleEvents.ChangeStates(States.home);
            _controllerUI.gameOverPanel.SetActive(false);
        }

        private void SubscribeGameEvents()
        {
            EventHandlerGame.TossLetter += TossLetter;
            EventHandlerGame.LetterDone += LetterDone;
            EventHandlerGame.UpdateScore += UpdateScore;
            EventHandlerGame.AddedMoveData += AddMoveSet;
            EventHandlerGame.ResumeGame += RecreateGameSyncData;
            EventHandlerGame.CreateGame += CreateNewGame;
            EventHandlerGame.CalculateRoundBonus += AddBonus;
            EventHandlerGame.ResetBonus += ResetBonus;
            EventHandlerGame.RemoveExtraLetterTray += RemoveExtraLetterTry;
        }

        private void ResetBonus()
        {
            LogSystem.LogEvent("ResetBonus Called");
            TwoXL.SetSyncData(0);
            TwoXW.SetSyncData(0);
            ThreeXL.SetSyncData(0);
            ThreeXW.SetSyncData(0);
        }

        private void UnSubscribeGameEvents()
        {
            EventHandlerGame.TossLetter -= TossLetter;
            EventHandlerGame.LetterDone -= LetterDone;
            EventHandlerGame.UpdateScore -= UpdateScore;
            EventHandlerGame.AddedMoveData -= AddMoveSet;
            EventHandlerGame.ResumeGame -= RecreateGameSyncData;
            EventHandlerGame.CreateGame -= CreateNewGame;
            EventHandlerGame.CalculateRoundBonus -= AddBonus;
            EventHandlerGame.RemoveExtraLetterTray -= RemoveExtraLetterTry;
        }

        private void OnDestroy()
        {
            UnSubscribeGameEvents();
        }

        private void UpdateScore(int turnScore)
        {
            turnScore -= MinusScore;
            _turnScore = turnScore;
        }

        public int GetReplaceTileSetCount()
        {
            int count = 0;
            foreach (var tile in _letterTiles)
            {
                if (!tile.Key.IsPlacedInTile)
                {
                    count++;
                }
            }

            return count;
        }

        private void LetterDone(LetterTile letterTile)
        {
            int i = 0;
            if (_trayDatas.Count > 0)
            {
                LetterTile newLetterTile = Instantiate(_letterBlock, parentLetterBlock);
                // newLetterTile.transform.position = letterPos[i].transform.position;
                RemainingTile remainingTile = _trayDatas.Pop();
                remainingTile.gameObject.SetActive(false);
                tossedBinIndex.Add(remainingTile.transform.GetSiblingIndex());
                newLetterTile.SetUISlotsArray(letterPos);
                newLetterTile.SetInitialPos(null);
                newLetterTile.SetLetterBlock(remainingTile.GetLetterBlock);
                //TossLetter(newLetterTile);
                _letterTiles.Add(newLetterTile, i);
            }
        }

        private void TossLetter(LetterTile letterTile)
        {
            // _trayDatas.Clear();
            if (_trayDatas.Count <= 0)
            {
                // HandleRoundDone();
                _controllerUI.ForceGameEnd();
                return;
            }

            RemainingTile remainingTile = _trayDatas.Pop();

            MoveData moveData = new MoveData();
            moveData.MoveType = MoveType.TossLetter;
            moveData.RemainingTile = remainingTile;
            moveData.LetterTile = letterTile;
            moveData.LastLetter = letterTile.GetBlockLetter;
            tossedLetters.Add(moveData.LastLetter);

            currentRoundTossedTile.Add(moveData.LastLetter);
            AddMoveSet(moveData);
            if (_noTossPenalty)
            {
                LogSystem.LogEvent("TossPenalty {0}", _noTossPenalty);
            }
            else
            {
                _minusScore += letterTile.BlockScore;
                LogSystem.LogEvent("MinusScore {0}", _minusScore);
            }

            letterTile.SetLetterBlock(remainingTile.GetLetterBlock);
            remainingTile.gameObject.SetActive(false);
            tossedBinIndex.Add(remainingTile.transform.GetSiblingIndex());
            _gridController.ValidateWord();
        }

        public void AddMoveSet(MoveData moveData)
        {
            allMoveDatas.Push(moveData);
        }

        public void SetNoTossPenalty(bool active)
        {
            _noTossPenalty = active;
        }

        private bool CheckIfLetterTileAvailable()
        {
            int count = 0;
            foreach (var letter in _letterTiles)
            {
                if (letter.Key.IsAvailable)
                {
                    count++;
                }
            }

            return count > 0;
        }

        #region UndoRegion

        private void UndoAll()
        {
            if (allMoveDatas.Count <= 0)
            {
                return;
            }

            //   Debug.Log(allMoveDatas.Count);
            int count = allMoveDatas.Count;
            for (int i = 0; i < count; i++)
            {
                SingleUndo();
            }

            //   Debug.Log("Undo pressed");
            ClearBonusRound();
            GameSyncData gameSyncData = JsonConvert.DeserializeObject<GameSyncData>(currentGameRoundData);
            RecreateGameSyncData(gameSyncData);
            currentRoundTossedTile.Clear();
            LetterGameAudioHolder.Instance.PlayUndoAllSound();

            _controllerUI.ClearScoreWordsList();
            // int length = allMoveDatas.Count;
            //
            // for (int i = 0; i < length; i++)
            // {
            //     DoUndoMove();
            // }
        }

        private void ClearBonusRound()
        {
            TwoXL.ClearRoom();
            ThreeXL.ClearRoom();
            TwoXW.ClearRoom();
            ThreeXW.ClearRoom();
        }

        private void DoUndoMove()
        {
            if (allMoveDatas.Count == 0)
            {
                return;
            }

            SingleUndo();

            LetterGameAudioHolder.Instance.PlayUndoSound();

            _gridController.ValidateWord();
        }

        #endregion

        private void SingleUndo()
        {
            MoveData moveData = allMoveDatas.Pop();

            //  Debug.Log("MoveData: " + moveData.MoveType);

            switch (moveData.MoveType)
            {
                case MoveType.TossLetter:
                case MoveType.AnyLetterStack:
                    moveData.LetterTile.SetLetterBlock(moveData.LastLetter);
                    _minusScore -= moveData.LastLetter.score;
                    moveData.RemainingTile.GetLetterBlock.UndoLetter();
                    _trayDatas.Push(moveData.RemainingTile);
                    moveData.RemainingTile.gameObject.SetActive(true);
                    tossedBinIndex.Remove(moveData.RemainingTile.transform.GetSiblingIndex());
                    tossedLetters.Remove(moveData.LastLetter);
                    currentRoundTossedTile.Remove(moveData.LastLetter);
                    _bonusController.AddBonusCount(1);
                    break;

                case MoveType.AddedLetter:
                    moveData.LetterTile.RemoveMove(moveData.TileDropped);
                    break;

                case MoveType.AddedBonus:
                    moveData.LetterTile.RemoveBonus();
                    break;

                case MoveType.ExtraLetter:
                    Destroy(moveData.LetterTile.gameObject);
                    moveData.RemainingTile.GetLetterBlock.UndoLetter();
                    _trayDatas.Push(moveData.RemainingTile);
                    moveData.RemainingTile.gameObject.SetActive(true);
                    tossedBinIndex.Remove(moveData.RemainingTile.transform.GetSiblingIndex());
                    letterPos[7].gameObject.SetActive(false);
                    _bonusController.AddBonusCount(1);
                    break;

                case MoveType.NoPenalty:
                    _noTossPenalty = false;
                    _bonusController.AddBonusCount(1);
                    break;

                case MoveType.AnyLetter:
                    moveData.LetterTile.SetLetterBlock(moveData.LastLetter);
                    _bonusController.AddBonusCount(1);
                    // moveData.RemainingTile.GetLetterBlock.UndoLetter();
                    // _trayDatas.Push(moveData.RemainingTile);
                    // moveData.RemainingTile.gameObject.SetActive(true);
                    break;
            }
        }

        public void CreateNewGame(bool asucess, string date, CreateGameData stack)
        {
            _controllerUI.Initialize();
            UpdateScore(0);
            wordScore = 0;
            _controllerUI.SetWordScore();
            Debug.Log(GlobalData.GameMode);

            if (GlobalData.GameMode == "hardcore")
            {
                _bonusController.SetBonusForHardCore();
            }
            else
            {
                _bonusController.SetBonusForFun();
            }

            if (asucess)
            {
                List<LetterBlock> PlayerHandData = new List<LetterBlock>();
                List<LetterBlock> TrayData = new List<LetterBlock>();

                for (int i = 0; i < stack.letters.Count; i++)
                {
                    if (i < InitailLetterBlocks)
                    {
                        LetterBlock letterBlock = new LetterBlock();
                        letterBlock.LetterBlocks(stack.letters[i].name, stack.letters[i].score);
                        PlayerHandData.Add(letterBlock);
                    }
                    else
                    {
                        LetterBlock trayBlock = new LetterBlock();
                        trayBlock.LetterBlocks(stack.letters[i].name, stack.letters[i].score);
                        TrayData.Add(trayBlock);
                    }
                }

                _gameDataHeader = new GameData();
                _gameDataHeader._letterBlocks = PlayerHandData;
                _gameDataHeader._trayData = TrayData;

                _gameDataHeader._trayData.Reverse();

                InitializeLetterBlock();
                SetRemainingTile();
                EventHandlerGame.EmitEvent(GameEventType.GridCountUpdate);
                StartCoroutine(WaitAndSendGameSyncData());
                EventHandlerGame.EmitEvent(GameEventType.UpdateScoreUI, wordScore);
            }
        }

        private IEnumerator WaitAndSendGameSyncData()
        {
            yield return new WaitForEndOfFrame();
            if (GlobalData.GameType == GameType.SoloPlayer)
            {
                GameSyncData gameSyncData = ConstructGameSyncData();
                SaveGameSyncData(gameSyncData);
            }

            parentRemainingBlock.GetComponent<GridLayoutGroup>().enabled = false;
        }

        #region RoundValidation

        public void RoundDone(bool autoSubmitScore = false)
        {
            if (!autoSubmitScore)
            {
                if (gameCompleted || _gridController.GetAddedNewLetterCount <= 0)
                {
                    string text = (gameCompleted) ? GameMessages.GameCompleted : GameMessages.PleaseAddATile;
                    EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, text);
                    return;
                }
            }

            _gridController.CheckAddedTileConnected();
            if (_gridController.IsValidForRound)
            {
                //GameSyncData gameSyncData = new GameSyncData();
                bool tileInMidBlock = false;

                tileInMidBlock = CheckIFMidTileAdded(tileInMidBlock);

                if (!tileInMidBlock)
                {
                    EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.CentreTileMissing);
                    return;
                }

                foreach (var moveData in allMoveDatas)
                {
                    if (moveData.MoveType == MoveType.ExtraLetter)
                    {
                        EventHandlerGame.EmitEvent(GameEventType.RemoveExtraLetter, moveData);
                    }
                }

                HandleRoundDone();
            }
            else
            {
                if (autoSubmitScore)
                {
                    EndRoundMultiplayer();
                }
                else
                {
                    LogSystem.LogEvent("Invalid Cnnectionn");

                    string message = (_gridController.IsWrongWords)
                        ? GameMessages.InvalidWord
                        : GameMessages.InvalidConnection;

                    EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, message);
                }
            }
        }

        private void EndRoundMultiplayer()
        {
            GameSyncData gameSyncData = ConstructGameSyncData();
            gameSyncData.TurnScore = 0;
            LogSystem.LogEvent("TurnScore {0}", gameSyncData.TurnScore);
            LogSystem.LogEvent("RoundWordScore {0}", gameSyncData.TurnScore);
            SendMultiplayerTurnScore(gameSyncData);
            LetterGameAudioHolder.Instance.PlayEndOfRoundSound(false);
        }

        private void HandleRoundDone()
        {
            var data = _trayDatas.ToArray();
            // _trayDatas.Clear();
            // _trayDatas.Push(data[0]);
            // _trayDatas.Push(data[1]);
            // _trayDatas.Push(data[2]);
            // _trayDatas.Push(data[3]);
            ShowRoundEndDetails();
            RoundEndDataUpdate();
            _controllerUI.SetWordScore();
            Debug.Log("bonus on round end: " + _bonusController.OutGameBonusCount);
            _bonusController.OutGameBonusInNewRound = _bonusController.OutGameBonusCount;
        }

        private bool updateBonusPoint = false;
        private bool isBonusTile = false;

        public void AllowForBonusUpdate()
        {
            updateBonusPoint = true;
        }

        private void RoundEndDataUpdate()
        {
            if (CheckForExtraBonus())
            {
                if (GlobalData.MultiplayerType != MultiplayerType.WordToWord)
                {
                    _bonusController.AddBonusCount(1, true);
                }
                else
                {
                    isBonusTile = true;
                }
            }

            _noTossPenalty = false;
            wordScore += _turnScore;
            _minusScore = 0;

            lastRoundScore = wordScore;
            allMoveDatas.Clear();
            currentRoundTossedTile.Clear();
            //AddBonus();

            List<ValidLetterBlocks> nullList = null;
            EventHandlerGame.EmitEvent(GameEventType.UpdateWordList, nullList);
            //_gridController.ValidateWord();
            EventHandlerGame.EmitEvent(GameEventType.WordDone);

            GameSyncData gameSyncData = ConstructGameSyncData();

            LogSystem.LogEvent("TurnScore {0}", gameSyncData.TurnScore);
            LogSystem.LogEvent("RoundWordScore {0}", gameSyncData.TurnScore);

            if (GlobalData.GameType == GameType.SoloPlayer)
            {
                SaveGameSyncData(gameSyncData);
                EventHandlerGame.EmitEvent(GameEventType.CalculateScore);
            }
            else
            {
                SendMultiplayerTurnScore(gameSyncData);
            }

            _gridController.SetLetterBlockColors(Color.white);
            LetterGameAudioHolder.Instance.PlayEndOfRoundSound(true);
        }


        private void ShowRoundEndDetails()
        {
            _controllerUI.ShowRoundEndScore(_gridController.NewValidLetterBlocksList, currentRoundTossedTile);
        }

        private void SendMultiplayerTurnScore(GameSyncData gameSyncData)
        {
            MultiplayerRoundData multiplayerRoundData =
                new MultiplayerRoundData(PhotonNetwork.LocalPlayer.ActorNumber, gameSyncData);

            string roundDataJson = JsonConvert.SerializeObject(multiplayerRoundData);

            _multiplayerGameHelper.SendNetworkEvents(NetworkCode.SendRoundScoreData, roundDataJson);
        }

        private bool CheckIFMidTileAdded(bool tileInMidBlock)
        {
            foreach (var validLetter in _gridController.GetValidLetterBlocks)
            {
                foreach (var letterBlock in validLetter.validWords)
                {
                    if (letterBlock.TileSet.CurrentTile.ColumnNo == 5 && letterBlock.TileSet.CurrentTile.RowNo == 5)
                    {
                        tileInMidBlock = true;
                        break;
                    }
                }

                if (tileInMidBlock)
                {
                    break;
                }
            }

            return tileInMidBlock;
        }

        #endregion

        #region BonusDuringGamePlay

        private bool CheckForExtraBonus()
        {
            List<ValidLetterBlocks> newValidLetters = _gridController.GetListNewWords();

            foreach (var validLetter in newValidLetters)
            {
                foreach (var letterBlock in validLetter.validWords)
                {
                    if ((BonusColumnNo.Contains(letterBlock.TileSet.CurrentTile.ColumnNo) &&
                         BonusRowNo.Contains(letterBlock.TileSet.CurrentTile.RowNo)) ||
                        (letterBlock.TileSet.CurrentTile.ColumnNo == 5 && letterBlock.TileSet.CurrentTile.RowNo == 5))
                    {
                        if (letterBlock.TileSet.IsAvailable)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void AddBonus()
        {
            int newLetterAdded = _gridController.GetAddedNewLetterCount;
            // LogSystem.LogEvent("NewLetterAdded {0}", newLetterAdded);
            if (newLetterAdded < 4)
            {
                ThreeXL.RemoveRoundBonusCount();
                TwoXW.RemoveRoundBonusCount();
                ThreeXW.RemoveRoundBonusCount();
                TwoXL.RemoveRoundBonusCount();
                return;
            }

            switch (_gridController.GetAddedNewLetterCount)
            {
                case 4:
                    ThreeXL.RemoveRoundBonusCount();
                    TwoXW.RemoveRoundBonusCount();
                    ThreeXW.RemoveRoundBonusCount();
                    TwoXL.AddRoundBonusCount(1);
                    break;
                case 5:
                    ThreeXL.AddRoundBonusCount(1);
                    TwoXW.RemoveRoundBonusCount();
                    ThreeXW.RemoveRoundBonusCount();
                    break;
                case 6:
                    TwoXW.AddRoundBonusCount(1);
                    ThreeXW.RemoveRoundBonusCount();
                    break;
                case 7:
                    ThreeXW.AddRoundBonusCount(1);
                    break;
            }
        }

        #endregion

        #region GameDataCreationAndSync

        public GameSyncData ConstructGameSyncData()
        {
            GameSyncData gameSyncData = new GameSyncData();

            List<LetterTileData> LetterTilesSyncData = new List<LetterTileData>();
            List<LetterBlock> trayLetterData = new List<LetterBlock>();

            LogSystem.LogEvent("LetterTiles {0}", _letterTiles.Count);

            foreach (var letterTile in _letterTiles)
            {
                if (letterTile.Key.gameObject.activeInHierarchy)
                {
                    var LetterData = letterTile.Key.GetGameSyncData();
                    LetterData.pos = letterTile.Value;
                    LetterTilesSyncData.Add(LetterData);
                }
            }

            LogSystem.LogEvent("LetterTileSyncData {0}", LetterTilesSyncData.Count);

            foreach (var trayData in _trayDatas)
            {
                trayLetterData.Add(trayData.GetSyncData);
            }

            trayLetterData.Reverse();
            gameSyncData.LetterTiles = LetterTilesSyncData;
            gameSyncData._trayDatas = trayLetterData;
            gameSyncData.tossList = tossedLetters;
            gameSyncData.MinusScore = _minusScore;
            gameSyncData.LastRoundScore = lastRoundScore;
            gameSyncData.ThreeXlCount = ThreeXL.BonusCount;
            gameSyncData.ThreeXwCount = ThreeXW.BonusCount;
            gameSyncData.TwoXlCount = TwoXL.BonusCount;
            gameSyncData.TwoXwCount = TwoXW.BonusCount;
            gameSyncData.TurnScore = _turnScore;
            gameSyncData.WordScore = wordScore;
            gameSyncData.BonusCount = _bonusController.InGameBonusCount;
//            Debug.Log(_bonusController.InGameBonusCount);
            // todo: reset to 61
            gameCompleted = _gridController.GetAddedLetterCount >= noOfTilesToWin || (_trayDatas.Count == 0 && !CheckIfLetterTileAvailable());
            gameCompleted = gameCompleted ? gameCompleted : forceGameEnd;
            gameSyncData.IsGameFinished = gameCompleted;
            gameSyncData.trayemptyIndex = tossedBinIndex;
            string serialized = JsonConvert.SerializeObject(gameSyncData);
            LogSystem.LogEvent("SerializedObj {0}", serialized);
            if (gameCompleted)
            {
                _controllerUI.gameOverPanel.SetActive(true);
            }


            return gameSyncData;
        }

        //todo: Remove after testing
        public void SetNoOfTilesToWin()
        {
            int inputNo = 61;

            try
            {
                inputNo = int.Parse(noOfTilesInput.text);
            }
            catch
            {
                UnityEngine.Debug.Log("Not a valid input");
            }

            noOfTilesToWin = inputNo;
            noOfTilesInput.text = "Set to: " + noOfTilesToWin.ToString();
        }

        public void ForceGameEnd()
        {
            forceGameEnd = true;
            HandleRoundDone();
        }

        private void SaveGameSyncData(GameSyncData gameSyncData)
        {
            int status = (gameCompleted || _trayDatas.Count == 0) ? 0 : 1;
            LogSystem.LogEvent("GameCompleted {0}", (status == 0) ? "True" : "False");
            //gameCompleted = status == 0;
            currentGameRoundData = JsonConvert.SerializeObject(gameSyncData);
            ApiManager.SaveGameData(GlobalData.GameId, GlobalData.UserId, status, gameSyncData, HandleSaveData);
        }

        public void SetCurrentGameRoundData(GameSyncData data)
        {
            currentGameRoundData = JsonConvert.SerializeObject(data);
        }

        private void HandleSaveData(bool asucess, UserSaveData callback)
        {
            if (asucess)
            {
                LogSystem.LogEvent("Success {0}", callback.message);
            }
            else
            {
                LogSystem.LogEvent("Failed {0}", callback.message);
            }
        }

        private void RecreateGameSyncData(GameSyncData gameSyncData)
        {
            _controllerUI.Initialize();
            forceGameEnd = false;
            var wordScore = this.wordScore;
            letterPos[7].gameObject.SetActive(false);
            ClearGamePlayData();
            // GameSyncData gameSyncData = JsonConvert.DeserializeObject<GameSyncData>(jsonData);
            currentGameRoundData = JsonConvert.SerializeObject(gameSyncData);
            GameData gameData = new GameData();
            gameData._trayData = gameSyncData._trayDatas;
            tossedLetters = gameSyncData.tossList;
            if (tossedLetters == null)
            {
                tossedLetters = new List<LetterBlock>();
            }


            _gameDataHeader = gameData;

            SetRemainingTile();
            SetEmptyTraySlots(gameSyncData);

            tossedBinIndex = gameSyncData.trayemptyIndex;

            //          Debug.Log("update bonus: " + updateBonusPoint);
//            Debug.Log(gameSyncData.WordScore + "," + gameSyncData.TurnScore);
            //  if ()

            if (GlobalData.GameType == GameType.SoloPlayer)
            {
                _bonusController.InGameBonusCount = gameSyncData.BonusCount;
            }

            if (updateBonusPoint && isBonusTile)
            {
                _bonusController.AddBonusCount(1, true);
            }

            updateBonusPoint = false;
            isBonusTile = false;

            _minusScore = gameSyncData.MinusScore;
            lastRoundScore = gameSyncData.LastRoundScore;

            if (GlobalData.GameType == GameType.SoloPlayer)
            {
                TwoXL.SetSyncData(gameSyncData.TwoXlCount);
                TwoXW.SetSyncData(gameSyncData.TwoXwCount);
                ThreeXL.SetSyncData(gameSyncData.ThreeXlCount);
                ThreeXW.SetSyncData(gameSyncData.ThreeXwCount);
            }

            int startPos = 0;
            for (int i = 0; i < gameSyncData.LetterTiles.Count; i++)
            {
                var currentLetterData = gameSyncData.LetterTiles[i];
                LetterTile letterTile = Instantiate(_letterBlock, parentLetterBlock);
                StartCoroutine(AddDataForLetterTile(letterTile, currentLetterData, startPos));
            }

            EventHandlerGame.EmitEvent(GameEventType.GridCountUpdate);
            _turnScore = gameSyncData.TurnScore;
            if (GlobalData.GameType == GameType.SoloPlayer)
            {
                this.wordScore = gameSyncData.WordScore;
            }
            else
            {
                this.wordScore = wordScore;
            }

            LogSystem.LogEvent("WordSCore {0}", wordScore);
            // wordScore = gameSyncData.WordScore;
            _controllerUI.SetWordScore();

            gameCompleted = gameSyncData.IsGameFinished;
            //StartCoroutine(WaitAndSendGameSyncData());
        }

        #endregion

        private void SetEmptyTraySlots(GameSyncData gameSyncData)
        {
            GridLayoutGroup gridLayoutGroup = parentRemainingBlock.GetComponent<GridLayoutGroup>();
            gridLayoutGroup.enabled = true;

            foreach (var emptyIndex in gameSyncData.trayemptyIndex)
            {
                RemainingTile tile = Instantiate(_remainingTile, parentRemainingBlock);
                tile.transform.SetSiblingIndex(emptyIndex);
                //tile.transform.GetChild(0).GetComponent<Text>().text = string.Empty;
                StartCoroutine(DeactivateTile(tile.gameObject));
                tile.transform.name = "emptySlot";
            }
        }

        private IEnumerator DeactivateTile(GameObject tile)
        {
            yield return new WaitForSeconds(0.1f);
            tile.SetActive(false);
        }

        #region ClearGameData

        public void ClearGame()
        {
            EventHandlerGame.EmitEvent(GameEventType.ClearRoom);
            ClearGamePlayData();
        }

        private void ClearGamePlayData()
        {
            availableLetters.Clear();
            tossedLetters.Clear();
            //   _controllerUI.wordList.text = string.Empty;
            _controllerUI.tossLetterList.text = string.Empty;
            _trayDatas.Clear();

            foreach (var tiles in _letterTiles)
            {
                Destroy(tiles.Key.transform.gameObject);
            }

            allMoveDatas.Clear();
            _letterTiles.Clear();
            tossedBinIndex.Clear();
            _gridController.ClearGame();
            _noTossPenalty = false;
            gameCompleted = false;
            lastRoundScore = 0;
            wordScore = 0;
            _minusScore = 0;
            _controllerUI.gameOverPanel.SetActive(false);
            EventHandlerGame.EmitEvent(GameEventType.UpdateScoreUI, wordScore);
        }

        #endregion

        #region LetterTileLogic

        private void InitializeLetterBlock()
        {
            foreach (var gLetterBlock in _gameDataHeader._letterBlocks)
            {
                availableLetters.Push(gLetterBlock);
            }

            LogSystem.LogColorEvent("yellow", "availableLetters {0}", availableLetters.Count);

            for (int i = 0; i < InitailLetterBlocks; i++)
            {
                LetterTile letterTile = Instantiate(_letterBlock, parentLetterBlock);
                //letterTile.transform.position = letterPos[i].transform.position;
                letterTile.SetUISlotsArray(letterPos);
                letterTile.SetUISlot(letterPos[i]);
                letterTile.SetLetterBlock(availableLetters.Pop());
                _letterTiles.Add(letterTile, i);
            }
        }

        public void AddExtraLetterBlock()
        {
            letterPos[7].gameObject.SetActive(true);
            LetterTile letterTile = Instantiate(_letterBlock, parentLetterBlock);
            //letterTile.transform.position = letterPos[7].transform.position;
            letterTile.SetUISlotsArray(letterPos);
            letterTile.SetUISlot(letterPos[7]);

            RemainingTile remainingTile = _trayDatas.Pop();
            remainingTile.GetLetterBlock.TossLetter();
            remainingTile.GetLetterBlock.score += 1;
            letterTile.SetLetterBlock(remainingTile.GetLetterBlock);

            letterTile.SetAsBonusLetterTile();

            remainingTile.gameObject.SetActive(false);
            tossedBinIndex.Add(remainingTile.transform.GetSiblingIndex());
            _letterTiles.Add(letterTile, 7);
            MoveData moveData = new MoveData();
            moveData.LetterTile = letterTile;
            moveData.RemainingTile = remainingTile;
            moveData.MoveType = MoveType.ExtraLetter;
            AddMoveSet(moveData);
        }

        private void SetRemainingTile()
        {
            foreach (Transform prefabs in parentRemainingBlock)
            {
                Destroy(prefabs.gameObject);
            }

            GridLayoutGroup gridLayoutGroup = parentRemainingBlock.GetComponent<GridLayoutGroup>();
            gridLayoutGroup.enabled = true;

            foreach (var letterBlock in _gameDataHeader._trayData)
            {
                RemainingTile tile = Instantiate(_remainingTile, parentRemainingBlock);
                tile.transform.SetSiblingIndex(0);
                tile.SetTile(letterBlock);
                _trayDatas.Push(tile);
            }
        }

        private IEnumerator AddDataForLetterTile(LetterTile letterTile, LetterTileData currentLetterData, int startPos)
        {
            SetAllUISlotsFree();
            yield return new WaitForEndOfFrame();

            letterTile.SetLetterBlock(currentLetterData.LetterBlock);
            LogSystem.LogEvent("startPos {0}", startPos);
            if (currentLetterData.collisionObj == null)
            {
                letterTile.transform.position = letterPos[currentLetterData.pos].transform.position;
                letterTile.SetInitialStartPos();
                letterTile.SetUISlotsArray(letterPos);
                letterTile.SetInitialPos(null);
                //startPos++;
                //_letterTiles.Add(letterTile, currentLetterData.pos);
            }
            else
            {
                var tileSet = _gridController.GetTile(currentLetterData.collisionObj.columnNumber,
                    currentLetterData.collisionObj.rowNumber);
                letterTile.SetTileSet(tileSet);
                letterTile.SetLetterDone();
                letterTile.SetBonusType(currentLetterData.PowerUpType);
                yield return new WaitForEndOfFrame();
                tileSet.transform.localScale = Vector3.one;
            }

            _letterTiles.Add(letterTile, currentLetterData.pos);

            parentRemainingBlock.GetComponent<GridLayoutGroup>().enabled = false;
        }

        private void SetAllUISlotsFree()
        {
            foreach (var uiSlot in letterPos)
            {
                uiSlot.ToggleFreeStatus(true);
            }
        }

        #endregion
    }
}