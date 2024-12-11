using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackAnyLetterPower : MonoBehaviour
{
  
    public Button _stackLetterBtn;
    public Text _letter;
    void Start()
    {
        _stackLetterBtn.onClick.RemoveAllListeners();
        _stackLetterBtn.onClick.AddListener(StackLetterBtnClick);
    }

    private void StackLetterBtnClick()
    {
        if (GlobalData.is_StackPowerOn)
        {
            GlobalData.is_StackPowerOn = false;
            GameController.data._TempLastUITiles.GetComponent<UITile>().letterString.text = _letter.text;
            GameUi.instance._canvasUi.StackAnyLetterPanel.SetActive(false);
        }
    }
}
