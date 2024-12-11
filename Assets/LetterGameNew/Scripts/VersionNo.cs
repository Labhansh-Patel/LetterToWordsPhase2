using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(TextMeshProUGUI))]
public class VersionNo : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI versionText;

    private void OnValidate()
    {
        versionText = GetComponent<TextMeshProUGUI>();
        versionText.text = "Version "+Application.version;
    }

    private void Awake()
    {
        versionText.text = "Version "+Application.version;
    }
}
