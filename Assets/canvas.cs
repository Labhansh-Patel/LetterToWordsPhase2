using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvas : MonoBehaviour
{
    // Start is called before the first frame update
           
    public static canvas _instance;
     void Awake()
    {
        if (_instance    == null)
        {
            _instance = this;
             DontDestroyOnLoad(this);
          
        }
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
