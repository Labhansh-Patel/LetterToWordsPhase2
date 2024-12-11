using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

namespace Gameplay
{
    public class TileSet : MonoBehaviour
    {
      [SerializeField]  private int columnNumber;
       [SerializeField] private int rowNumber;

       public int ColumnNo => columnNumber;
       public int RowNo => rowNumber;
       
        
        private bool _isTaken = false;
         private LetterTile _block = null;

        public bool IsAvailable => !_isTaken;
        
        private LetterBlockData _letterBlockData = new LetterBlockData();


        public LetterTile CurrentLetterTile => _block;
        
        public bool CheckTileEquals(int columnNo, int rowNo) =>
            (columnNo == this.columnNumber && rowNo == this.rowNumber);

        private void Start()
        {
            
            Initialize();
        }


        public TileData GetTileData()
        {
            TileData tileData = new TileData();
            tileData.columnNumber = columnNumber;
            tileData.rowNumber = rowNumber;
            return tileData;
        }
        
        public void Initialize()
        {
            _isTaken = false;
            
        }

        public void SetTileSetNumber(int columnNumber,int rowNumber)
        {
            this.columnNumber = columnNumber;
            this.rowNumber = rowNumber;
            _letterBlockData.columnNumber = columnNumber;
            _letterBlockData.rowNumber = rowNumber;
            
        }
        
        public void SetTile(LetterTile block)
        {
            _isTaken = true;
            _block = block ;
            _letterBlockData.TileSet = _block;
            EventHandlerGame.EmitEvent(GameEventType.AddedLetter,_letterBlockData);
        }

        public void RemoveTile()
        {
            _isTaken = false;
            _block = null;
            _letterBlockData.TileSet = null;
            EventHandlerGame.EmitEvent(GameEventType.RemovedLetter,_letterBlockData);
        }
        
    }
}