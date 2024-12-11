using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake Called");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Called");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable Called");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
