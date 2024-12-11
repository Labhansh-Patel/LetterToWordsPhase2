using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TossCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    
      void OnTriggerEnter2D(Collider2D col)
    {
        //col.gameObject.SetActive(false);

       Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);

      col.gameObject.transform.GetComponent<UITile>()._isToss= true;
      Alphabet.data.ReplaceLetter = col.gameObject; 
        //GameController.data.EnableSwapMode();
        // col.gameObject.SetActive(true);
      
    }





}
