using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonousTile : MonoBehaviour
{
    // Start is called before the first frame update
    public static BonousTile data;
     private float uiZaxis; 
    public bool dragging;
    public Vector2 lastPosition;

  [HideInInspector]public GameObject lastcollider;
    
   [HideInInspector]  public GameObject Usecollider;
    
    private Transformer transformer;

    void Awake()
    {

    }
    void Start()
    {
        transformer = GetComponent<Transformer>();
        uiZaxis = Camera.main.transform.position.z + FindObjectOfType<Canvas>().planeDistance;
        lastPosition=gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log("lastcollider........." + dragging);
         if (GameController.data.paused)
            return;
         if (dragging)
        {
            //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 9);
            // if (hit.collider != null)
            // {  
              
            //     //  if (  hit.collider.gameObject.tag=="BoardTile")
            //     //      {
                         
            //             //  if ( hit.collider.gameObject.GetComponent<BoardTile>().completed)
            //             //     {
            //                     lastcollider= hit.collider.gameObject;
            //                      Debug.Log("Gameobject name............."+hit.collider.gameObject.name);
             
            //                // }
            //         //  }
              
             
            // }
             // Debug.Log("1111111111111111111111");
            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            //cursorPos = new Vector3(0,-100,0);
           // Debug.Log("cursorPos........." + cursorPos);
        

                   cursorPos.z = uiZaxis;
            transform.position = new Vector3(cursorPos.x, cursorPos.y+0.5f, 0);
            // Debug.Log("transform.position......." + transform.position);
          
           


            // gameObject.transform.SetAsLastSibling();
            // Debug.Log("cursorPos......." +cursorPos + "TossCollider.transform.position "+TossCollider.transform.position);
           
                 // lastcollider=null;
                 // Usecollider=null;
            if (Input.GetMouseButtonUp(0))
            {
                   //Debug.Log("222222222222222");
                OnMouseUp();
                
            }
          

         }
        
    }


 void OnMouseDrag()
    {
        //   Debug.Log("GameController.data.isclickable......."+GameController.data.isclickable);
        if(GameController.data.isclickable)
        {
           return; 
        }
//               Debug.Log("GameController.data.paused......."+GameController.data.paused);
        if (GameController.data.paused)
            return;
        dragging = true;
       // Debug.Log("draging................................");
                
    }


 void OnMouseDown() {
     // Debug.Log(".......");
      // Debug.Log("GameController.data.isclickable......."+GameController.data.isclickable);
        if(GameController.data.isclickable)
        {
           return; 
        }
        /// gameObject.transform.position=FirstPosition;
        //   Debug.Log("transform .position,,,,,,,"+ transform.position);
        //   Debug.Log("targetSlot...................."+targetSlot);
        //  Alphabet.data.SelectedLetter(this.gameObject);
          // Debug.Log("GameController.data.paused......."+GameController.data.paused);
        if (GameController.data.paused)
            return;
        // if (CheckifOverUISlot())
        // {
        //     targetSlot.GetComponent<UISlot>().UITile = null;
        //    targetSlot = null;
        //   Debug.Log("......" +letterString.text);

        // }
  
     //FirstPosition=gameObject.transform.position;
        // gameObject.transform.position = FirstPosition; 
        // Debug.Log("......" +letterString.text);
        // Debug.Log("transform .position,,,,,,,"+ transform.position);
       
       // GameController.data.GetLastUISlot(UISlot,this.gameObject);
         //gameObject.transform.position=FirstPosition;
        // Debug.Log("boardtile..........."+boardTile.name); 
        


          
    }
   void OnTriggerEnter2D(Collider2D col)
    {
      // Debug.Log(col.gameObject.name );
        if ( col.gameObject.tag=="BoardTile")
        {
            if (!col.gameObject.GetComponent<BoardTile>().completed)
            {
                //resetcollider();
            }



        }
             
      
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "BoardTile")
        {
            if (!col.gameObject.GetComponent<BoardTile>().completed)
            {
               // Debug.Log("stay");
               // Debug.Log("enter");
                lastcollider = col.gameObject;
                GameController.data.canBePlant=true;
               // Debug.Log("Gameobject name............." + gameObject.name);
                col.transform.localScale = new Vector3(1.2f, 1.2f, 10f);
            }
        }
    }


        void OnTriggerExit2D(Collider2D col)
    {


        if (col.gameObject.tag == "BoardTile")
        {
            if (!col.gameObject.GetComponent<BoardTile>().completed)
            {
               // Debug.Log("exit");
                col.transform.localScale = new Vector3(1, 1, 1);

            }
        }
        resetcollider();
        //col.transform.localScale = new Vector3(1.2f, 1.2f, 10f);
        //Debug.Log("exit called");
        //Invoke("resetcollider",3);
    }
    
    void OnMouseUp()
    {

        // if(GameController.data.isclickable)
        //  {
        //     return; 
        // }
        
//         Alphabet.data.ShowHighlighter();

//         // Debug.Log("transform .position,,,,,,,"+ transform.position);
//         if (GameController.data.swapMode)
//         {       GameController.data.SwapBlock.transform.Find("TextTip").GetComponent<Text>().text=" ";

//              MoveToPos(lastPosition,targetSlot);
//             // Alphabet.data.ShowHighlighter();
//            //SetSwapState(!needSwap);
//             Debug.Log(".......");

//             return;
//         }
 if (GameController.data.paused)
        {
            return;
        }
       

         dragging = GameController.data.letterDragging = false;
//          if (CursorPosition.x>3.0f && CursorPosition.x< 4.0f )
//             {
//                     Debug.Log("transform.position......." + transform.position);
//                     GameController.data.EnableSwapMode();
//             }
      
//         if (letterScore.text == "0" && GameController.data.canBePlant)
//         {
//             GameController.data.OpenSelectJokerLetter(boardTile);
//             gameObject.SetActive(false);
//             Debug.Log(",,,,,,,,,,,");
//         }
    if (GameController.data.canBePlant)
        {
        //    Debug.Log(",,,,,,,,,,,"); 
        //     GameController.data.GetThreeTimeWord();
        Usecollider=lastcollider;
         GameController.data.SelectedTiel(Usecollider);
        Debug.Log("Usecollider............."+Usecollider);
        GameController.data.BonousApply(gameObject.name);
          
        }

        // Usecollider=lastcollider;
        // Debug.Log("Usecollider............."+Usecollider);
        MoveToPosSwap(lastPosition);

//         else {
//             if (CheckifOverUISlot())
//             {
//                 // Debug.Log("transform .position,,,,,,,"+ transform.position);
//                      if( targetSlot!=null)
//                   {
//                        MoveToPos(lastPosition,targetSlot);
//                      //gameObject.transform.position=lastPosition;
//                      Debug.Log("=======");
//                   }
//                // GoToSlot(targetSlot);
//                   Debug.Log("__________");
//             }
//             else {
//                 MoveToPos(lastPosition,targetSlot);
//                // GoToSlot(targetSlot);
//                   Debug.Log("--------");
//             }
//             GameController.data.PreApply();
//         }
       
    }

    public void resetcollider()
    {
         Usecollider=null;
        lastcollider=null;
    }


public  void MoveToPosSwap(Vector3 toPos)
    {
        //resetcollider();
        gameObject.transform.parent.SetAsLastSibling();
        transformer.MoveUI(toPos, 0.25f);
         Debug.Log("222222");
         gameObject.transform.position=toPos;
        Invoke("resetcollider",2);
        //  foreach(GameObject bTile in GameController.data.boardTilesMatters)
        // {
        //   bTile.transform.localScale = new Vector3(1f,1f,1f);
        // } 

        
      // gameObject.GetComponent<RectTransform>().anchoredPosition =toPos;
      


    }




}
