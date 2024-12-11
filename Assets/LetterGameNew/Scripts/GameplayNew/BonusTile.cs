using System;
using System.Collections.Generic;
using APICalls;
using GameEvents;
using InputSystem;
using LetterGameNew.ScriptableObj;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay
{

    public class BonusData
    {
        public PowerUpType PowerUpType;
        public bool IsThisRound = true;
        
    }
    
    [RequireComponent(typeof(SwipeControl))]
    public class BonusTile : MonoBehaviour
    {
        private SwipeControl _swipeControl;
        private Transform collisionObj;
        [FormerlySerializedAs("_bonusType")] [SerializeField] private PowerUpType powerUpType;

        [SerializeField] private Text bonusCount;
        [SerializeField] private BonusColorsScriptable _bonusColorsScriptable;
        [SerializeField] private Canvas canvas;
        
        private int count =0, roundCount;

        private LetterTile _letterTile;

        private bool AddedForRound = false;
        
        public int BonusCount => count;
        
        
        private int bonusFullCount => roundCount + count;

        public PowerUpType PowerUpType => powerUpType;

        public Color GetBonusColor => _bonusColorsScriptable.GetBonusColor(powerUpType);
        
        private Vector3 startPos;
        private bool roundBonusAdded = false;
        
        private void Start()
        {
            startPos = transform.position;
            _swipeControl = GetComponent<SwipeControl>();
            _swipeControl.OnDragStart += OnDragStart;
            _swipeControl.OnDragging += OnDragging;
            _swipeControl.OnDragEnd += OnDragEnd;
            EventHandlerGame.ClearRoom += ClearRoom;
            collisionObj = null;
            EventHandlerGame.WordDone += RoundDone;
            //AddBonusCount(1);

        }

        private void RoundDone()
        {
            count += roundCount;
            roundCount = 0;
            roundBonusAdded = false;
            SetBonusCount();
        }

        public void ClearRoom()
        {
            count = 0;
            roundCount = 0;
            SetBonusCount();
        }

        public void SetSyncData(int count)
        {
            LogSystem.LogEvent("SetSyncCount {0}",count);
            this.count = count;
            SetBonusCount();
        }

        public void AddBonusCount(int count)
        {
            this.count += count;
            SetBonusCount();
        }

        public void AddRoundBonusCount(int count, bool isRoundBonus = true)
        {
            
            // LogSystem.LogEvent("Type {0} AddRoundBonusCount {1}", PowerUpType, count);
            if ( (count > 0 && roundBonusAdded) || (roundCount != 0 && count > 0))
            {  
                return;
            }

            
            if (isRoundBonus)
            {
                roundCount+=count;
                roundBonusAdded = true;
            }
            else
            {
                this.count += count;
                
            }

     
            // LogSystem.LogEvent("RoundCOunt {0} Count {1}", roundCount, this.count);

            SetBonusCount();
        }

        public void RemoveBonusAdded(int count , bool isRoundBonus)
        {
            
            if (isRoundBonus)
            {
                roundBonusAdded = false;
                AddRoundBonusCount(count , isRoundBonus);
            }
            else
            {
                this.count += count;
                // LogSystem.LogEvent("Type {0} RemoveBonusAdded {1} this.count {2}", PowerUpType, count , this.count);
                SetBonusCount();
            }
            
        }
        

        public void RemoveRoundBonusCount()
        {

            // LogSystem.LogEvent("Type {0} RemoveRoundBonusCount {1}", PowerUpType, roundCount);
            if ( (roundCount > 0 || roundBonusAdded))
            {
                if (_letterTile != null) 
                {
                    if (_letterTile.IsRoundBonusAdded)
                    {
                        _letterTile.RemoveBonus();
                        RemoveBonusLetterTile(_letterTile);
                    }
                }
        
                roundBonusAdded = false;
                roundCount = 0;
                LogSystem.LogEvent("Type {0} RemoveRoundBonusCount11 {1}", PowerUpType, roundCount);
            }
       

            SetBonusCount();
        }

        public void RemoveBonusLetterTile(LetterTile letterTile)
        {
            if (_letterTile == letterTile)
            {
                _letterTile = null;
            }
        }

        private void SetBonusCount()
        {
            
            bonusCount.text = bonusFullCount.ToString();
        }

        private void OnDestroy()
        {
            _swipeControl.OnDragStart -= OnDragStart;
            _swipeControl.OnDragging -= OnDragging;
            _swipeControl.OnDragEnd -= OnDragEnd;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
           
            if (other.CompareTag("LetterTile"))
            {
                
                collisionObj = other.transform;

               
            }
            
        
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
               
            if (other.CompareTag("LetterTile"))
            {
                
                collisionObj = other.transform;

               
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("LetterTile") )
            {
                collisionObj = null;
               
             
            }
        
        }

        private void OnDragEnd()
        {
            
            if (bonusFullCount <=0)
            {
                return;
            }

            canvas.sortingOrder = 0;
            
            if (collisionObj == null)
            {
                transform.position = startPos;
            }
            else if (collisionObj.CompareTag("LetterTile"))
            {
                var letterTile = collisionObj.GetComponent<LetterTile>();

                if (letterTile.CanPlaceBonus)
                {
                    bool isThisRoundBonus = (roundCount != 0);
                    letterTile.SetBonus(this ,isThisRoundBonus);
                    if (isThisRoundBonus)
                    {
                        _letterTile = letterTile;
                    }
                    AddRoundBonusCount(-1 , isThisRoundBonus);
                    transform.position = startPos;
                    LetterGameAudioHolder.Instance.PlayBonusSound(powerUpType);
                    
                   
                }
                else
                {
                    transform.position = startPos;
                }
            }
        }
        
        

        private void OnDragging(PointerEventData data)
        {
            
            if (bonusFullCount <=0)
            {
                return;
            }
            
            RectTransform draggingPlane = transform as RectTransform;
        
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, data.position, data.pressEventCamera, out var mousePos))
            {
                mousePos.y += GamePlayData.fingerTileOffset;
                transform.position = mousePos;
                
            }
        }

        private void OnDragStart(Vector3 obj)
        {
            if (bonusFullCount <=0) 
            {
                return;
            }
            
            canvas.sortingOrder = 2;
            transform.SetSiblingIndex(transform.parent.childCount-1);
            
            
            if (collisionObj != null) 
            {
                LetterTile letterTile =  collisionObj.GetComponent<LetterTile>();
                if (letterTile.CurrentPowerUp != PowerUpType.None ) 
                {
                    if (letterTile.BonusTile == null)
                    {
                        //etterTile.RemoveBonus();
                    }
                    else if (letterTile.BonusTile == this)
                    {
                       // letterTile.RemoveBonus();
                    }
               
                }
                
            }
        }
    }
}