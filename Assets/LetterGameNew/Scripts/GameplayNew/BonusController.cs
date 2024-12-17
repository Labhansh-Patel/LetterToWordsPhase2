using System.Collections.Generic;
using GameEvents;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay
{
    public enum BonusType
    {
        ExtraLetter,
        NoPenalty,
        LetterFromStack,
        AnyLetter,
        BlockExtraTime,
        AddExtraTime,
    }

    public class BonusController : MonoBehaviour
    {
        public delegate void BonusEventCall();

        public event BonusEventCall AddExtraLetterCallBack;
        public event BonusEventCall NoPenaltyCallBack;
        public event BonusEventCall LetterFromStackCallBack;
        public event BonusEventCall AnyLetterCallBack;


        [Header("BonusCount")] private int extraLetterCount = 2;
        private int noPenaltyCount = 2;
        private int letterFromStackCount = 2;
        private int anyLetterCount = 2;


        [FormerlySerializedAs("bonusCount")] [SerializeField]
        private int inGameBonusCount = 0;

        [SerializeField] private int outGameBonusCount = 0;
        [SerializeField] private GameObject outgameBonusTxt;
        private int totalGameBonusCount;

        public int InGameBonusCount
        {
            get { return inGameBonusCount; }
            set
            {
                inGameBonusCount = value;
                UpdateBonusUI();
            }
        }

        public void SetBonusForHardCore()
        {
            outgameBonusTxt.SetActive(false);
            SetBonusMark(false);
            ExcludeOutGameBonus(true);
        }
        
        public void SetBonusForFun()
        {
            outgameBonusTxt.SetActive(true);
            
            if(outGameBonusCount > 0)
            {
                SetBonusMark(true);
            }
            else
            {
                SetBonusMark(false);
            }
            
            ExcludeOutGameBonus(false);
        }

        private void ExcludeOutGameBonus(bool exclude)
        {
            if (exclude)
            {
                totalGameBonusCount = inGameBonusCount;
            }
            else
            {
                totalGameBonusCount = inGameBonusCount + outGameBonusCount;
            }
        }

        public int OutGameBonusCount
        {
            get { return outGameBonusCount; }
            set
            {
                outGameBonusCount = value;
                UpdateBonusUI();
            }
        }

        public int OutGameBonusInNewRound
        {
            get { return outGameBonusInNewRound; }
            set { outGameBonusInNewRound = value; }
        }

        //   public void SetInGameBonusPoints(int points){}

        [Header("BonusBtn")] [SerializeField] private Button extraLetter;
        [SerializeField] private Button noPenalty;
        [SerializeField] private Button letterFromStack;
        [SerializeField] private Button anyLetter;


        [Header("BonusCount")] [SerializeField]
        private Text extraLetterCountText;

        [SerializeField] private Text noPenaltyCountText;
        [SerializeField] private Text letterFromStackCountText;

        [SerializeField] private Text anyLetterCountText;

        // [SerializeField] private Text bonusCountText;
        [SerializeField] private TextMeshProUGUI inGameBonusCountText;
        [SerializeField] private TextMeshProUGUI outGameBonusCountText;

        [Header("SelectBonus")] [SerializeField]
        private Button selectExtraLetter;

        [SerializeField] private Button selectNoPenalty;
        [SerializeField] private Button selectLetterFromStack;
        [SerializeField] private Button selectAnyLetter;
        [SerializeField] private Button[] addExtraTime;
        [SerializeField] private Button[] blockExtraTime;


        [SerializeField] private GameObject selectBonusPanel;
        [SerializeField] private GamePlayController _gamePlayController;

        private Stack<BonusType> BonusAdded = new Stack<BonusType>();
        [SerializeField] private GameObject bonusUpdate;

        private int outGameBonusInNewRound;

        private void Start()
        {
            extraLetter.onClick.RemoveAllListeners();
            extraLetter.onClick.AddListener(AddExtraLetterBlock);

            noPenalty.onClick.RemoveAllListeners();
            noPenalty.onClick.AddListener(NoPenaltyBonus);

            letterFromStack.onClick.RemoveAllListeners();
            letterFromStack.onClick.AddListener(AnyLetterFromStack);

            anyLetter.onClick.RemoveAllListeners();
            anyLetter.onClick.AddListener(AnyLetter);

            foreach (var addExtra in addExtraTime)
            {
                addExtra.onClick.RemoveAllListeners();
                addExtra.onClick.AddListener(AddExtraTimeCall);
            }


            foreach (var blockBtb in blockExtraTime)
            {
                blockBtb.onClick.RemoveAllListeners();
                blockBtb.onClick.AddListener(BlockExtraTime);
            }

            EventHandlerGame.WordDone += RoundDone;
            EventHandlerGame.ClearRoom += ClearRoom;

            UpdateBonusUI();
            // EventHandlerGame.AddBonus += AddBonusCount;
        }

        private void SetBonusMark(bool active)
        {
            bonusUpdate.SetActive(active);
        }

        private void ClearRoom()
        {
            Debug.Log("Bonus Cleared");
            inGameBonusCount = 0;
            CalculateTotalBonus();
            BonusAdded.Clear();
            UpdateBonusUI();
        }

        private void RoundDone()
        {
            BonusAdded.Clear();
        }

        private bool IsMultiplayerGame => GlobalData.GameType != GameType.SoloPlayer && GlobalData.MultiplayerType == MultiplayerType.WordToWord;

        private void BlockExtraTime()
        {
            if (IsMultiplayerGame)
            {
                BonusAdded.Push(BonusType.BlockExtraTime);
                EventHandlerGame.EmitEvent(GameEventType.BlockExtraTime);

                CheckSufficientMoney();

                UpdateBonusUI();
            }
            else
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.PowerUpOnlyUsableInMultiplayer);
            }
        }

        private void CalculateTotalBonus()
        {
            if (GlobalData.GameMode == "hardcore")
            {
                totalGameBonusCount = inGameBonusCount;
            }
            else
            {
                totalGameBonusCount = inGameBonusCount + outGameBonusCount;
            }
        }

        private void AddExtraTimeCall()
        {
            if (IsMultiplayerGame)
            {
                BonusAdded.Push(BonusType.AddExtraTime);
                EventHandlerGame.EmitEvent(GameEventType.AddExtraTime);
                CheckSufficientMoney();

                UpdateBonusUI();
            }
            else
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.PowerUpOnlyUsableInMultiplayer);
            }
        }

        private void CheckSufficientMoney()
        {
            if (GlobalData.GameMode != "hardcore")
            {
                if (inGameBonusCount == 0)
                {
                    outGameBonusCount--;
                }
                else
                {
                    inGameBonusCount--;
                }
            }
            else
            {
                if (inGameBonusCount > 0)
                {
                    inGameBonusCount--;
                }
            }

            CalculateTotalBonus();
        }

        private void OnEnable()
        {
            UpdateBonusUI();
        }

        private void UpdateBonusUI()
        {
            CalculateTotalBonus();
            // bonusCountText.text = "Bonus In Round : " + totalGameBonusCount.ToString();
            inGameBonusCountText.text = inGameBonusCount.ToString();
            outGameBonusCountText.text = outGameBonusCount.ToString();
            bonusUpdate.gameObject.SetActive(totalGameBonusCount > 0);
            // extraLetterCountText.text = extraLetterCount.ToString();
            // noPenaltyCountText.text = noPenaltyCount.ToString();
            // letterFromStackCountText.text = letterFromStackCount.ToString();
            // anyLetterCountText.text = anyLetterCount.ToString();
        }

        public void SetBonusCount(int bonusCount)
        {
            BonusAdded.Clear();
            this.inGameBonusCount = bonusCount;
            UpdateBonusUI();
        }

        // public void SaveOutGameBonusCountAtStart()
        // {
        //     //    if (!GamePlay.StoreOutBonusCountReset)
        //     {
        //         Debug.Log(outGameBonusCount);
        //         //     GamePlay.StoreOutBonusCountReset = true;
        //         //  outGameBonusInNewRound = outGameBonusCount;
        //     }
        // }

        public void AddBonusCount(int count, bool isEarnedInGame = false)
        {
            if (count > 0)
            {
                if (BonusAdded.Count > 0)
                {
                    BonusAdded.Pop();
                }
            }

            //   Debug.Log(outGameBonusCount + ", " + outGameBonusInNewRound);
            if (GlobalData.GameMode != "hardcore" && outGameBonusCount < outGameBonusInNewRound)
            {
                if (!isEarnedInGame)
                {
                    outGameBonusCount += count;
                }
                else
                {
                    inGameBonusCount += count;
                }
            }
            else
            {
                inGameBonusCount += count;
            }

            UpdateBonusUI();
        }

        public void ActivateBonusSelectionScreen()
        {
            selectAnyLetter.onClick.RemoveAllListeners();
            selectAnyLetter.onClick.AddListener(() => AddBonusCount(BonusType.AnyLetter));

            selectExtraLetter.onClick.RemoveAllListeners();
            selectExtraLetter.onClick.AddListener(() => AddBonusCount(BonusType.ExtraLetter));

            selectNoPenalty.onClick.RemoveAllListeners();
            selectNoPenalty.onClick.AddListener(() => AddBonusCount(BonusType.NoPenalty));

            selectLetterFromStack.onClick.RemoveAllListeners();
            selectLetterFromStack.onClick.AddListener(() => AddBonusCount(BonusType.LetterFromStack));

            selectBonusPanel.SetActive(true);
        }

        private void AddBonusCount(BonusType bonusType)
        {
            switch (bonusType)
            {
                case BonusType.AnyLetter:
                    anyLetterCount++;
                    UpdateBonusUI();
                    break;
                case BonusType.ExtraLetter:
                    extraLetterCount++;
                    UpdateBonusUI();
                    break;
                case BonusType.NoPenalty:
                    noPenaltyCount++;
                    UpdateBonusUI();
                    break;
                case BonusType.LetterFromStack:
                    letterFromStackCount++;
                    UpdateBonusUI();
                    break;
            }
        }

        private void AnyLetter()
        {
            if (BonusAdded.Contains(BonusType.AnyLetter))
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.CannotUseBonusTwice);
                return;
            }


            if (totalGameBonusCount <= 0)
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.InsufficentBonus);
                return;
            }

            var count = _gamePlayController.GetReplaceTileSetCount();

            if (count == 0)
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.NoAvailableTiles);

                return;
            }


            BonusAdded.Push(BonusType.AnyLetter);
            CheckSufficientMoney();


            AnyLetterCallBack?.Invoke();
            UpdateBonusUI();
        }

        private void AnyLetterFromStack()
        {
            if (BonusAdded.Contains(BonusType.LetterFromStack))
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.CannotUseBonusTwice);
                return;
            }

            if (totalGameBonusCount <= 0)
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.InsufficentBonus);
                return;
            }

            var count = _gamePlayController.GetReplaceTileSetCount();

            if (count == 0)
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.NoAvailableTiles);

                return;
            }

            BonusAdded.Push(BonusType.LetterFromStack);
            CheckSufficientMoney();
            LetterFromStackCallBack?.Invoke();
            UpdateBonusUI();
        }

        private void NoPenaltyBonus()
        {
            if (BonusAdded.Contains(BonusType.NoPenalty))
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.CannotUseBonusTwice);
                return;
            }


            if (totalGameBonusCount <= 0)
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.InsufficentBonus);
                return;
            }

            CheckSufficientMoney();

            BonusAdded.Push(BonusType.NoPenalty);
            NoPenaltyCallBack?.Invoke();
            UpdateBonusUI();
        }

        private void AddExtraLetterBlock()
        {
            if (BonusAdded.Contains(BonusType.ExtraLetter))
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.CannotUseBonusTwice);
                return;
            }

            Debug.Log("Total count " + totalGameBonusCount);
            if (totalGameBonusCount <= 0)
            {
                EventHandlerGame.EmitEvent(GameEventType.ShowPopupText, GameMessages.InsufficentBonus);
                return;
            }

            BonusAdded.Push(BonusType.ExtraLetter);
            CheckSufficientMoney();

            AddExtraLetterCallBack?.Invoke();
            UpdateBonusUI();
        }
    }
}