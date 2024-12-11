using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingTile : MonoBehaviour
{
    [Header("UI Component")] 
    [SerializeField] private Text letterText;

    private LetterBlock _trayData;

    public LetterBlock GetLetterBlock => _trayData;

    public LetterBlock GetSyncData => _trayData;
    

    public void SetTile(LetterBlock letter)
    {
        _trayData = letter;
        letterText.text = _trayData.letter;
    }
}
