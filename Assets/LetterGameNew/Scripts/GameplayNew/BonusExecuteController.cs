using System.Collections.Generic;
using APICalls;
using GameEvents;
using UnityEngine;

namespace Gameplay
{
    public class BonusExecuteController : MonoBehaviour
    {
        [SerializeField] private GamePlayController gamePlayController;
        [SerializeField] private GamePlayControllerUI _controllerUI;
        [SerializeField] private BonusController _bonusController;

        [SerializeField] private SelectLetterTile _selectLetterTilePrefab;
        [SerializeField] private SelectTilePrefab _selectTilePrefab;


        private void Start()
        {
            EventHandlerGame.AnyLetterStack += AddAnyLetterStack;
            EventHandlerGame.AnyLetter += AddAnyLetter;
            AddBonusCallBacks();
        }

        private void OnDestroy()
        {
            EventHandlerGame.AnyLetterStack -= AddAnyLetterStack;
            EventHandlerGame.AnyLetter -= AddAnyLetter;
            RemoveBonusCallBacks();
        }

        private void AddBonusCallBacks()
        {
            _bonusController.AddExtraLetterCallBack += AddExtraLetterBlock;
            _bonusController.NoPenaltyCallBack += NoPenaltyBonus;
            _bonusController.AnyLetterCallBack += AnyLetter;
            _bonusController.LetterFromStackCallBack += AnyLetterFromStack;
        }

        private void RemoveBonusCallBacks()
        {
            _bonusController.AddExtraLetterCallBack -= AddExtraLetterBlock;
            _bonusController.NoPenaltyCallBack -= NoPenaltyBonus;
            _bonusController.AnyLetterCallBack -= AnyLetter;
            _bonusController.LetterFromStackCallBack -= AnyLetterFromStack;
        }

        private void AddAnyLetterStack(RemainingTile remainingTile)
        {
            LetterTile letterTile = null;
            DeleteSelectPrefabs();

            foreach (var tile in gamePlayController.LetterTiles)
            {
                if (!tile.Key.IsPlacedInTile)
                {
                    SelectLetterTile selectLetterTile =
                        Instantiate(_selectLetterTilePrefab, _controllerUI.selectLetterTileParent);
                    selectLetterTile.SetLetterTile(tile.Key, remainingTile, HandleAnyLetterStack);
                }
            }

            DeleteBonusPrefabs();

            _controllerUI.BonusTilePanel.SetActive(false);
            _controllerUI.selectTilePanel.SetActive(true);

            //HandleAnyLetterStack(remainingTile, letterTile);
        }

        private void HandleAnyLetterStack(RemainingTile remainingTile, LetterTile letterTile)
        {
            _controllerUI.selectTilePanel.SetActive(false);
            Stack<RemainingTile> _trayTemp = new Stack<RemainingTile>();
            int count = gamePlayController.TrayDatas.Count;
            for (int i = 0; i < count; i++)
            {
                RemainingTile tile = gamePlayController.TrayDatas.Pop();
                if (tile == remainingTile)
                {
                    int length = _trayTemp.Count;
                    LogSystem.LogEvent("Lengtth {0}", length);
                    for (int j = 0; j < length; j++)
                    {
                        var temp = _trayTemp.Pop();
                        gamePlayController.TrayDatas.Push(temp);
                    }

                    break;
                }
                else
                {
                    _trayTemp.Push(tile);
                }
            }

            MoveData moveData = new MoveData();
            moveData.MoveType = MoveType.AnyLetterStack;
            moveData.RemainingTile = remainingTile;
            moveData.LetterTile = letterTile;
            moveData.LastLetter = letterTile.GetBlockLetter;
            gamePlayController.TossedLetters.Add(moveData.LastLetter);
            gamePlayController.AddMoveSet(moveData);
            letterTile.SetLetterBlock(remainingTile.GetLetterBlock);
            remainingTile.gameObject.SetActive(false);
            gamePlayController.TossedBinIndex.Add(remainingTile.transform.GetSiblingIndex());
            _controllerUI.BonusTilePanel.SetActive(false);
        }

        private void DeleteBonusPrefabs()
        {
            foreach (Transform prefab in _controllerUI.bonusTileParent)
            {
                Destroy(prefab.gameObject);
            }
        }

        private void DeleteSelectPrefabs()
        {
            foreach (Transform prefab in _controllerUI.selectLetterTileParent)
            {
                Destroy(prefab.gameObject);
            }
        }

        private void AddAnyLetter(LetterBlock letterBlock)
        {
            _controllerUI.BonusTilePanel.SetActive(false);
            DeleteSelectPrefabs();
            foreach (var tile in gamePlayController.LetterTiles)
            {
                if (!tile.Key.IsPlacedInTile)
                {
                    SelectLetterTile selectLetterTile =
                        Instantiate(_selectLetterTilePrefab, _controllerUI.selectLetterTileParent);
                    selectLetterTile.SetAnyLetterTile(letterBlock, tile.Key, HandleAddAnyLetter);
                }
            }

            _controllerUI.selectTilePanel.SetActive(true);
        }

        private void HandleAddAnyLetter(LetterBlock letterBlock, LetterTile tile)
        {
            _controllerUI.selectTilePanel.SetActive(false);
            MoveData moveData = new MoveData();
            moveData.LastLetter = tile.GetBlockLetter;
            moveData.LetterTile = tile;
            tile.SetLetterBlock(letterBlock);
            // moveData.RemainingTile = remainingTile;
            moveData.MoveType = MoveType.AnyLetter;

            gamePlayController.AddMoveSet(moveData);
        }

        private void NoPenaltyBonus()
        {
            gamePlayController.SetNoTossPenalty(true);
            MoveData moveData = new MoveData();
            moveData.MoveType = MoveType.NoPenalty;
            gamePlayController.AddMoveSet(moveData);
        }

        private void AnyLetterFromStack()
        {
            DeleteBonusPrefabs();

            foreach (var remainingTile in gamePlayController.TrayDatas)
            {
                SelectTilePrefab selectTilePrefab = Instantiate(_selectTilePrefab, _controllerUI.bonusTileParent);
                selectTilePrefab.SetStackTile(remainingTile);
            }

            _controllerUI.BonusTilePanel.SetActive(true);
        }

        private void AnyLetter()
        {
            DeleteBonusPrefabs();
            char ch;

            for (ch = 'a'; ch <= 'z'; ch++)
            {
                LetterBlock letterBlock = new LetterBlock();
                letterBlock.letter = ch.ToString().ToUpper();
                letterBlock.score = 0;
                SelectTilePrefab selectTilePrefab = Instantiate(_selectTilePrefab, _controllerUI.bonusTileParent);
                selectTilePrefab.SetTile(letterBlock);
            }

            _controllerUI.BonusTilePanel.SetActive(true);
        }

        private void AddExtraLetterBlock()
        {
            gamePlayController.AddExtraLetterBlock();
        }
    }
}