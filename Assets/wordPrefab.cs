using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wordPrefab : MonoBehaviour
{
    [SerializeField] private Text Word;
    [SerializeField] private Text Messege;

    public void SetData(string word,string messege)
    {
       Word.text = word;
        Messege.text = messege;

    }
}
