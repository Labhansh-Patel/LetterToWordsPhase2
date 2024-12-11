using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class test : MonoBehaviour
{
  
     Dictionary<int,int> number  = new Dictionary<int, int>();


    // Start is called before the first frame update
    void Start()
    { 
      number.Add(3,100);
      number.Add(5,80);
      number.Add(10,120);
      number.Add(14,130);
      
        int num= number.ElementAt(0).Value;
        int keynum = number.ElementAt(0).Key;
     
       for (int i = 1; i <number.Count; i++)
       {
           int temp=number.ElementAt(i).Value;

          if (num <temp )
          {
             num=temp;
             keynum=number.ElementAt(i).Key;
             
          }
        

           
           
       }

   Debug.Log(keynum);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
