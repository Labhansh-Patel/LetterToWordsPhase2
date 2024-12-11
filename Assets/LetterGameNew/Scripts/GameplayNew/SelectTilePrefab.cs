using System.Collections;
using System.Collections.Generic;
using APICalls;
using GameEvents;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;


public class SelectTilePrefab : MonoBehaviour
{

    [SerializeField] private Text tileName;
    [SerializeField] private Button _button;

    private RemainingTile _remainingTile;
    private LetterBlock _letterBlock;

    public void SetStackTile(RemainingTile remainingTile)
    {
        _remainingTile = remainingTile;
        tileName.text = _remainingTile.GetLetterBlock.letter;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(SendAnyLetterStack);
    }

    private void SendAnyLetterStack()
    {
        LogSystem.LogEvent("SendAnyLetterStack");
        EventHandlerGame.EmitEvent(GameEventType.AnyLetterStack,_remainingTile);
    }

    public void SetTile(LetterBlock letterTile)
    {
        _letterBlock = letterTile;
        tileName.text = _letterBlock.letter;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(SendAnyLetter);
    }

    private void SendAnyLetter()
    {
        LogSystem.LogEvent("SendAnyLetter");
        EventHandlerGame.EmitEvent(GameEventType.AnyLetter,_letterBlock);
    }
}
