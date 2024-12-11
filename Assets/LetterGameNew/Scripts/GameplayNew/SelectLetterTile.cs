using System;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class SelectLetterTile : MonoBehaviour
{
    [SerializeField] private Text tileName;
    [SerializeField] private Button _button;
    


    public void SetLetterTile(LetterTile letterTile, RemainingTile remainingTile,  Action<RemainingTile,LetterTile> callback)
    {
        tileName.text = letterTile.BlockLetterString;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener((() => HandleSelection(letterTile,remainingTile, callback)));
        
    }
    
    public void SetAnyLetterTile(LetterBlock letterBlock, LetterTile letterTile,  Action<LetterBlock,LetterTile> callback)
    {
        tileName.text = letterTile.BlockLetterString;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener((() => HandleSelectionAny(letterBlock,letterTile, callback)));
        
    }

    private void HandleSelectionAny(LetterBlock letterBlock, LetterTile letterTile, Action<LetterBlock, LetterTile> callback)
    {
        callback?.Invoke(letterBlock,letterTile);
    }


    private void HandleSelection( LetterTile letterTile, RemainingTile remainingTile ,  Action<RemainingTile,LetterTile> callback)
    {
        callback?.Invoke(remainingTile,letterTile);
    }
}