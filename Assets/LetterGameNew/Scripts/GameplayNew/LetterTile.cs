using System;
using System.Collections;
using APICalls;
using GameEvents;
using InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay
{
    [RequireComponent(typeof(SwipeControl))]
    public class LetterTile : MonoBehaviour
    {
        [Header("UIComponent")] [SerializeField]
        private Text letterUI;

        [SerializeField] private Text letterScore;
        [SerializeField] private Text wordScore;
        [SerializeField] private Text bonusText;
        [SerializeField] private GameObject wordScoreObj;
        [SerializeField] private Image blockImage;

        [SerializeField] private Sprite yellowSprite;

        //[SerializeField] private BonusColorsScriptable _bonusColorsScriptable;
        [SerializeField] private Image BorderImg;
        [SerializeField] private Button selectionPanel;

        [SerializeField] private Color initialColor;

        private bool isBonusLetter = false;

        private Vector3 lastPos;

        private LetterBlock _blockLetter;

        private UISlots[] _uiSlotsArray;
        private bool isOnGrid = false;

        public bool IsOnGrid => isOnGrid;

        public LetterBlock GetBlockLetter => _blockLetter;

        public string BlockLetterString => _blockLetter.letter;
        public int BlockScore => GetTileScore();

        public int WordBonus => GetWordBonus();

        private SwipeControl _swipeControl;

        private Vector3 startPos;

        private Transform collisionObj;

        private TileSet _tileSet = null;
        private TileSet previousTileSet = null;
        private UISlots _uiSlots = null;

        public bool IsPlacedInTile => _tileSet != null;

        public TileSet CurrentTile => _tileSet;

        private bool _isAvailable = true;

        public bool IsAvailable => _isAvailable;

        private PowerUpType _currentPowerUpType;

        //private float fingerOffSet = 75f;

        public PowerUpType CurrentPowerUp => _currentPowerUpType;

        private BonusTile _bonusTile;
        private bool isRoundBonus = false;
        public bool IsRoundBonusAdded => isRoundBonus;

        public BonusTile BonusTile => _bonusTile;

        public bool CanPlaceBonus => BonusTile == null && CurrentPowerUp == PowerUpType.None && collisionObj != null && _isAvailable;

        public bool CanSetGreenColorBorder => BorderImg.color != Color.red;


        private int GetWordBonus()
        {
            int WordBonus = 0;

            if (!_isAvailable)
            {
                return WordBonus;
            }

            //LogSystem.LogColorEvent("red","CurrentBonusType {0}",currentBonusType);
            switch (_currentPowerUpType)
            {
                case PowerUpType.None:
                    WordBonus = 0;
                    break;
                case PowerUpType.ThreeXW:
                    WordBonus = 3;
                    break;

                case PowerUpType.TwoXW:
                    WordBonus = 2;
                    break;
            }

            return WordBonus;
        }

        private int GetTileScore()
        {
            // LogSystem.LogEvent("gameobjectName {0}", gameObject.name);
            // LogSystem.LogEvent("Score {0}",_blockLetter.score);
            int score = Convert.ToInt32(_blockLetter.score);


            if (IsAvailable)
            {
                switch (_currentPowerUpType)
                {
                    case PowerUpType.None:
                        score *= 1;
                        break;
                    case PowerUpType.TwoXL:
                        score *= 2;
                        break;
                    case PowerUpType.ThreeXL:
                        score *= 3;
                        break;
                }
            }


            return score;
        }


        private void Start()
        {
            _swipeControl = GetComponent<SwipeControl>();
            _swipeControl.OnDragStart += OnDragStart;
            _swipeControl.OnDragging += OnDragging;
            _swipeControl.OnDragEnd += OnDragEnd;
            EventHandlerGame.WordDone += WordDone;
            EventHandlerGame.SelectTile += SelectTile;
            SetInitialStartPos();
            _isAvailable = true;
            blockImage.color = initialColor;
            LogSystem.LogEvent("InitialColor {0}", gameObject.name);
        }

        public void SetInitialStartPos()
        {
            startPos = transform.position;
            lastPos = startPos;
        }

        public void SetAsBonusLetterTile()
        {
            isBonusLetter = true;
        }


        public LetterTileData GetGameSyncData()
        {
            LetterTileData letterTileData = new LetterTileData();
            if (collisionObj != null && !collisionObj.CompareTag("UISlot"))
            {
                var tileSet = collisionObj.GetComponent<TileSet>();
                if (tileSet != null)
                {
                    letterTileData.collisionObj = tileSet.GetTileData();
                }
                else
                {
                    letterTileData.collisionObj = null;
                }
            }
            else
            {
                letterTileData.collisionObj = null;
            }

            letterTileData.PowerUpType = _currentPowerUpType;
            SetCurrentBonusText();
            letterTileData.IsAvailable = _isAvailable;
            letterTileData.LetterBlock = _blockLetter;
            return letterTileData;
        }

        private void SetCurrentBonusText()
        {
            switch (_currentPowerUpType)
            {
                case PowerUpType.ThreeXL:
                    bonusText.text = "3xL";
                    break;
                case PowerUpType.TwoXL:
                    bonusText.text = "2xL";
                    break;
                case PowerUpType.ThreeXW:
                    bonusText.text = "3xW";
                    break;
                case PowerUpType.TwoXW:
                    bonusText.text = "2xW";
                    break;
                default:
                    bonusText.text = string.Empty;
                    break;
            }
        }
        // public void SetGameSyncData(LetterTileData letterTileData)
        // {
        //     
        // }


        private void SelectTile()
        {
            selectionPanel.gameObject.SetActive(true);
            selectionPanel.transform.GetChild(0).GetComponent<Text>().text = GetBlockLetter.letter;
            selectionPanel.onClick.RemoveAllListeners();
            selectionPanel.onClick.AddListener(HandleTileSelect);
        }

        private void HandleTileSelect()
        {
        }

        private void OnDestroy()
        {
            _swipeControl.OnDragStart -= OnDragStart;
            _swipeControl.OnDragging -= OnDragging;
            _swipeControl.OnDragEnd -= OnDragEnd;
            EventHandlerGame.WordDone -= WordDone;
            EventHandlerGame.SelectTile -= SelectTile;
        }

        private void WordDone()
        {
            if (collisionObj != null && _isAvailable && _tileSet != null)
            {
                SetLetterDone();
                if (!isBonusLetter)
                {
                    EventHandlerGame.EmitEvent(GameEventType.LetterDone, this);
                }
            }
        }

        public void SetLetterDone()
        {
            //blockImage.sprite = yellowSprite;
            blockImage.color = Color.yellow;
            _isAvailable = false;
            LogSystem.LogEvent("LetterDONNNEE {0}", _blockLetter.letter);
        }


        private bool CheckForSlotTriggers(Collider2D other) => (other.CompareTag("BoardSlot") ||
                                                                other.CompareTag("TossBin") ||
                                                                other.CompareTag("UISlot"));

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CheckForSlotTriggers(other))
            {
                collisionObj = other.transform;
                if (other.CompareTag("UISlot"))
                {
                    if (other.GetComponent<UISlots>().IsFree)
                    {
                        other.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                    }
                    else
                    {
                        return;
                    }
                }

                other.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (CheckForSlotTriggers(other))
            {
                collisionObj = other.transform;
                //other.transform.localScale =  Vector3.one;
                // other.transform.localScale =  new Vector3(1.3f, 1.3f, 1.3f);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (CheckForSlotTriggers(other))
            {
                collisionObj = null;
                if (other.CompareTag("UISlot"))
                {
                    if (other.GetComponent<UISlots>().IsFree)
                    {
                        other.transform.localScale =Vector3.one;
                    }
                    else
                    {
                        return;
                    }
                }
                other.transform.localScale =Vector3.one;
            }
        }

        private void OnDragEnd()
        {
            if (!_isAvailable)
            {
                return;
            }

            if (collisionObj == null)
            {
                isOnGrid = false;
                //AddMoveData();
                RemoveUISlots();
                //RemoveTileSet();
                SetInitialPos(_uiSlots);
            }
            else
            {
                collisionObj.transform.localScale = Vector3.one;
                if (collisionObj.CompareTag("BoardSlot"))
                {
                    isOnGrid = true;
                    RemoveUISlots();
                    var wordBlock = collisionObj.GetComponent<TileSet>();
                    if (wordBlock.IsAvailable)
                    {
                        AddMoveData();
                        SetTileSet(wordBlock);
                        LetterGameAudioHolder.Instance.PlayTileSound();
                    }
                    else
                    {
                        AddMoveData();
                        SetInitialPos(_uiSlots);
                    }
                }

                if (collisionObj.CompareTag("TossBin"))
                {
                    isOnGrid = false;

                    LogSystem.LogColorEvent("yellow", "TossBin");
                    EventHandlerGame.EmitEvent(GameEventType.TossLetter, this);
                    previousTileSet = null;
                    LetterGameAudioHolder.Instance.PlayTossSound();
                    SetInitialPos(_uiSlots);
                    return;
                    // SetLastPos();
                }

                if (collisionObj.CompareTag("UISlot"))
                {
                    isOnGrid = false;
                    UISlots uiSlots = collisionObj.GetComponent<UISlots>();
                    // RemoveUISlots();
                    if (uiSlots.IsFree)
                    {
                        SetUISlot(uiSlots);
                        EventHandlerGame.EmitEvent(GameEventType.CalculateScore);
                    }
                    else
                    {
                        SetInitialPos(uiSlots);
                        EventHandlerGame.EmitEvent(GameEventType.CalculateScore);
                    }

                    LetterGameAudioHolder.Instance.PlayTileToTraySound();
                }
            }
        }

        private void RemoveUISlots()
        {
            if (_uiSlots == null)
            {
                return;
            }

            _uiSlots.ToggleFreeStatus(true);
            _uiSlots = null;
        }

        public void SetUISlot(UISlots uiSlots)
        {
            RemoveTileSet();
            previousTileSet = null;
            if (_uiSlots != null)
            {
                _uiSlots.ToggleFreeStatus(true);
            }

            _uiSlots = uiSlots;
            _uiSlots.ToggleFreeStatus(false);
            this.transform.position = _uiSlots.transform.position;
            SetBorderColor(Color.white);
            RemoveBonus();

            //if (_uiSlots.gameObject.activeSelf)
            //  StartCoroutine(RescaleSize(_uiSlots.transform));
        }

        private void AddMoveData()
        {
            MoveData moveData = new MoveData
            {
                MoveType = MoveType.AddedLetter, LetterTile = this, TileDropped = _tileSet
            };

            EventHandlerGame.EmitEvent(GameEventType.MoveAdded, moveData);
        }


        private UISlots GetUISlotsFreePos(UISlots uiSlots)
        {
            if (_uiSlots == null || uiSlots == null)
            {
                return GetFreeSlot();
            }

            if (_uiSlots.IsFree || uiSlots == _uiSlots)
            {
                return _uiSlots;
            }

            return GetFreeSlot();
        }

        private UISlots GetFreeSlot()
        {
            UISlots uiSlot = null;
            foreach (var slots in _uiSlotsArray)
            {
                if (slots.IsFree)
                {
                    uiSlot = slots;
                    break;
                }
            }

            if (uiSlot == null)
            {
                LogSystem.LogErrorEvent("NoFreeSlotsFound");
            }

            return uiSlot;
        }


        public void SetInitialPos(UISlots uiSlots)
        {
            if (_tileSet != null)
            {
                _tileSet.RemoveTile();
            }

            if (previousTileSet != null)
            {
                SetTileSet(previousTileSet);

                return;
            }


            UISlots newSlot = GetUISlotsFreePos(uiSlots);

            SetUISlot(newSlot);
        }

        public void SetTileSet(TileSet wordBlock)
        {
            if (_tileSet != null)
            {
                _tileSet.RemoveTile();
            }

            _tileSet = wordBlock;
            previousTileSet = _tileSet;
            if (previousTileSet == null)
            {
                LogSystem.LogEvent("PreviousTileSetNull");
            }

            wordBlock.SetTile(this);
            var wordPos = wordBlock.transform;
            transform.position = wordPos.position;
            StartCoroutine(RescaleSize(wordPos));
        }

        private IEnumerator RescaleSize(Transform wordPos)
        {
            yield return new WaitForSeconds(0.03f);
            SetScaleOne(wordPos);
        }

        private static void SetScaleOne(Transform wordPos)
        {
            wordPos.localScale = Vector3.one;
        }

        private void OnDragging(PointerEventData data)
        {
            if (!_isAvailable)
            {
                return;
            }

            if (GamePlayController.GameCompleted) return;
            RectTransform draggingPlane = transform as RectTransform;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, data.position, data.pressEventCamera, out var mousePos))
            {
                mousePos.y += GamePlayData.fingerTileOffset;
                transform.position = mousePos;
            }
        }

        private void OnDragStart(Vector3 obj)
        {
            if (GamePlayController.GameCompleted) return;

            transform.SetSiblingIndex(transform.parent.childCount - 1);

            if (collisionObj != null && _isAvailable)
            {
                // LetterGameAudioHolder.Instance.PlayTileSound();
                if (collisionObj.CompareTag("UISlot"))
                {
                    //RemoveTileSet();
                    if (_uiSlots != null)
                    {
                        _uiSlots.ToggleFreeStatus(true);
                    }
                }
                else
                {
                    collisionObj.GetComponent<TileSet>().RemoveTile();
                    EnableWordScore(false);
                }
            }
        }

        private void RemoveTileSet()
        {
            previousTileSet = _tileSet;

            if (previousTileSet == null)
            {
                // LogSystem.LogEvent("PreviousTileSetNull");
            }

            collisionObj = null;
            _tileSet = null;

            EnableWordScore(false);
        }


        public void SetLetterBlock(LetterBlock letterBlock)
        {
            _tileSet = null;
            _blockLetter = letterBlock;
            letterUI.text = letterBlock.letter;
            letterScore.text = letterBlock.score.ToString();
            gameObject.name = letterBlock.letter;
            SetBorderColor(Color.white);
            // AddMoveData();
        }

        public void SetWordScore(int score)
        {
            wordScore.text = score.ToString();
            EnableWordScore(true);
        }

        public void SetBorderColor(Color color)
        {
            BorderImg.color = color;
        }

        public void EnableWordScore(bool active)
        {
            wordScoreObj.SetActive(active);
        }

        public void RemoveBonus()
        {
            if (_bonusTile == null || !IsAvailable)
            {
                return;
            }

            LogSystem.LogEvent("RemoveBonus {0} {1}", BlockLetterString, _currentPowerUpType);
            _currentPowerUpType = PowerUpType.None;
            SetCurrentBonusText();
            _bonusTile.RemoveBonusAdded(1, isRoundBonus);
            _bonusTile = null;
            if (_isAvailable)
            {
                blockImage.color = initialColor;
            }

            EventHandlerGame.EmitEvent(GameEventType.CalculateScore);
        }


        public void SetBonus(BonusTile bonusTile, bool isRoundBonus)
        {
            _bonusTile = bonusTile;
            this.isRoundBonus = isRoundBonus;
            SetBonusType(bonusTile.PowerUpType);
            SetBonusTileMove(bonusTile);
            LogSystem.LogEvent("BonusLetter {0}", BlockLetterString);
            blockImage.color = bonusTile.GetBonusColor;
        }

        public void SetBonusType(PowerUpType powerUpType)
        {
            _currentPowerUpType = powerUpType;
            SetCurrentBonusText();
            EventHandlerGame.EmitEvent(GameEventType.CalculateScore);
        }

        private void SetBonusTileMove(BonusTile bonusTile)
        {
            MoveData moveData = new MoveData();
            moveData.MoveType = MoveType.AddedBonus;
            moveData.LetterTile = this;
            EventHandlerGame.EmitEvent(GameEventType.MoveAdded, moveData);
        }

        public void RemoveMove(TileSet tileSet)
        {
            if (tileSet == null)
            {
                previousTileSet = null;
                if (previousTileSet == null)
                {
                    LogSystem.LogEvent("PreviousTileSetNull");
                }

                SetInitialPos(_uiSlots);
            }
            else
            {
                SetInitialPos(_uiSlots);
                SetTileSet(tileSet);
            }
        }


        public void SetUISlotsArray(UISlots[] letterPos)
        {
            _uiSlotsArray = letterPos;
        }
    }
}