using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UITile : MonoBehaviour
{
    
    public GameObject UISlot;
    public GameObject boardTilePrefab;
    public GameObject boardTile;
    public GameObject targetSlot;

    public GameObject Hilighter;
    public Vector2 lastPosition;
    public GameObject TossCollider;
    
    public Vector3 FirstPosition;
    private Vector3  CursorPosition;
    public Text letterString;
    public Text letterScore;

    public int PreviousScore;
    public Text BonousText;
    public bool dragging;
    public bool needSwap;
    public bool finished;
    public BoardTile BoardUITile;

    public Transform testObj;
    public static UITile data;

    private Transformer transformer;
    private float uiZaxis;
    public string BonusName=string.Empty;
    public int LastScore=0;
    public string TrayGameObjectName=string.Empty;
    public string BeforeTossLetter=string.Empty;
    public string TrayLetter=string.Empty;

    private Vector3 LastCursorPosition = new Vector3(3f, 6.0f, -10f);
    
   // public List<GameObject> BoardUITile;
     public bool _isToss =false;

    private bool ismouseDown =false;
   // public bool BoardUITile;

    void Awake()
    {
       // letterString.text=string.Empty;
    }

    void Start()
    {
        
        transformer = GetComponent<Transformer>();
        FirstPosition=gameObject.transform.position;
        uiZaxis = Camera.main.transform.position.z + FindObjectOfType<Canvas>().planeDistance;

        //BonousText.text=string.Empty; 
        BonusName = string.Empty;
    
    }

    public void GetNewLetter() {
        if (Alphabet.data.LettersFeed.Count > 0)
        {


            string NewLetter = Alphabet.data.GetRandomLetter();
            letterString.text = NewLetter;
            Debug.LogFormat("NewLetter.....{0}",NewLetter);

            int score = Alphabet.data.GetLetterScore(letterString.text);
            letterScore.text = score.ToString();
            CreateNewBoardTile(letterString.text, score);
            gameObject.name = letterString.text;



        }
        else
        {
            finished = true;
            gameObject.SetActive(false);
        }
    }
    private void GenerateLetter()
    {
        string NewLetter = Alphabet.data.GetRandomLetter();
        letterString.text = NewLetter;
        Debug.Log("NewLetter.....");

        int score = Alphabet.data.GetLetterScore(letterString.text);
        letterScore.text = score.ToString();
        CreateNewBoardTile(letterString.text, score);
        gameObject.name = letterString.text;
    }

    void Update()
    {
        if (GameController.data.paused)
            return;

        
         if (dragging)
        {
            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CursorPosition = cursorPos;
            //cursorPos = new Vector3(0,-100,0);
           // Debug.Log("cursorPos........." + cursorPos);
             if( cursorPos.y < LastCursorPosition.y && cursorPos.x>-6.0f && cursorPos.x<6.0f )
             {

            cursorPos.z = uiZaxis;
            //transform.position = cursorPos;
            transform.position = new Vector3(cursorPos.x,cursorPos.y+0.5f,cursorPos.z);
          // Debug.Log("transform.position......." +transform.position);
            GameController.data.letterDragging = true;
               
                // gameObject.transform.position=  gameObject.transform.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 1);

                // gameObject.transform.SetAsLastSibling();
                // Debug.Log("cursorPos......." +cursorPos + "TossCollider.transform.position "+TossCollider.transform.position);


                if (Input.GetMouseButtonUp(0))
            {
                OnMouseUp();
            }
          }

         }
        
    }

    void OnMouseDrag()
    {
        if(GameController.data.isclickable)
        {
           return; 
        }
        if (GameController.data.paused)
            return;
        dragging = true;
        
       // Debug.Log("draging................................");
                
    }

    void OnMouseDown() {
        if(GameController.data.isclickable)
        {
           return; 
        }
        /// gameObject.transform.position=FirstPosition;
         // Debug.Log("transform .position,,,,,,,"+ transform.position);
        // Debug.Log("targetSlot...................."+targetSlot);
        GameController.data.SelectAnyLetterTile(this.gameObject);
        Alphabet.data.SelectedLetter(this.gameObject);
        // gameObject.transform.position= new Vector3(0f, gameObject.transform.position.y+1f,0f);
      

        //transform.position.y =;
        if (GameController.data.paused)
            return;
        Hilighter.SetActive(true);
        if (CheckifOverUISlot())
        {
            targetSlot.GetComponent<UISlot>().UITile = null;
           targetSlot = null;
          Debug.Log("......" +letterString.text);

        }
    ismouseDown =true;
     //FirstPosition=gameObject.transform.position;
        // gameObject.transform.position = FirstPosition; 
        // Debug.Log("......" +letterString.text);
        // Debug.Log("transform .position,,,,,,,"+ transform.position);
       
       // GameController.data.GetLastUISlot(UISlot,this.gameObject);
         //gameObject.transform.position=FirstPosition;
        // Debug.Log("boardtile..........."+boardTile.name); 
        


          
    }

    void OnMouseUp()
    {
       
        if(GameController.data.isclickable)
        {
           return; 
        }

        Hilighter.SetActive(false);
        //Alphabet.data.ShowHighlighter();
      
         //gameObject.transform.parent.SetAsLastSibling();
//transformer.MoveUI(gameObject.transform.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 60), 0.25f);

        // Debug.Log("transform .position,,,,,,,"+ transform.position);
        if (GameController.data.swapMode)
        {       GameController.data.SwapBlock.transform.Find("TextTip").GetComponent<Text>().text=" ";

             MoveToPos(lastPosition,targetSlot);
            // Alphabet.data.ShowHighlighter();
           //SetSwapState(!needSwap);
            Debug.Log(".......");


            return;
        }
 if (GameController.data.paused)
        {
            return;
        }
       

        dragging = GameController.data.letterDragging = false;



        if (letterString.text == "_" && GameController.data.canBePlant)
        {
            //Debug.Log("GameController.data.canBePlant......"+GameController.data.canBePlant);
            BeforeTossLetter=letterString.text;
            GameController.data.OpenSelectJokerLetter(boardTile);
            GameController.data.GetLastUISlot(UISlot,this.gameObject);
            gameObject.SetActive(false);
           // Debug.Log(",,,,,,,,,,,");
        }
        else if (GameController.data.canBePlant)
        {
            if (GameController.data.is_Move==true)
            {
              
                     GameController.data.PlantTile(boardTile);
                     GameController.data.GetLastUISlot(UISlot,this.gameObject);
                     gameObject.SetActive(false);
                    Debug.Log("......."+ letterScore.text);
                    GameController.data.is_Move=false;
            }
            else
            {
                 MoveToPos(lastPosition,targetSlot);
            }
          
        }
        else {
            if (CheckifOverUISlot())
            {
                MoveToPos(lastPosition, targetSlot);
                // Debug.Log("transform .position,,,,,,,"+ transform.position);
                if ( targetSlot!=null)
                  {
                     
                     //gameObject.transform.position=lastPosition;
                     Debug.Log("=======");
                    if (GameController.data.undo>-1)
                    {
                        GameController.data.LastUITiles.Remove(this.gameObject);
                        GameController.data.undo--;
                        Debug.Log(" GameController.data.undo......" + GameController.data.undo);

                        GameController.data.LastBoardTiles.Remove(this.gameObject);
                        GameController.data.undoBoardTile--;
                        if (BonusName !=string.Empty)
                        {
                            GameController.data.BonousReduce(BonusName, this.gameObject);
                            GameController.data.LastUITiles.Remove(this.gameObject);
                            GameController.data.undo--;
                            Debug.Log(" GameController.data.undo......" + GameController.data.undo);

                        }
                        GameController.data.PreApply();
                    }
                  

                    
                }

                // GoToSlot(targetSlot);
                Debug.Log("__________");
            }
            else {
                  Debug.Log("_isToss........."+_isToss);
                if (_isToss==true)
                  {
                      GameController.data.EnableSwapMode();
                      _isToss=false;
                  }
                else
                {
                    Debug.Log(" GameController.data.undo......" + GameController.data.undo);

                    if (GameController.data.undo>-1 && GameController.data.LastBoardTiles.Contains(this.gameObject))
                    {
                        GameController.data.LastUITiles.Remove(this.gameObject);
                        GameController.data.undo--;
                        Debug.Log(" GameController.data.undo......" + GameController.data.undo);

                        GameController.data.LastBoardTiles.Remove(this.gameObject);
                        GameController.data.undoBoardTile--;
                        if (BonusName !=string.Empty)
                        {
                            GameController.data.BonousReduce(BonusName, this.gameObject);
                            GameController.data.LastUITiles.Remove(this.gameObject);
                            GameController.data.undo--;
                            Debug.Log(" GameController.data.undo......"+ GameController.data.undo);
                        }
                        GameController.data.PreApply();
                    }
                    
                }
                MoveToPos(lastPosition,targetSlot);
                 
               // GoToSlot(targetSlot);
                  Debug.Log("--------");
            }
            if(BonusName ==string.Empty)
            {
                 GameController.data.PreApply();
            }
           
        }

       Alphabet.data.ReplaceLetter=null; 
       
    }

    public void ReCreateTile()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        
        GetNewLetter();
        Invoke("GoToFreeSlot",0.3f);
       // GoToFreeSlot();
    }

  public void CreateNewBoardTile(string letter, int score) {
        boardTile = (GameObject)Instantiate(boardTilePrefab, new Vector3(99, 0, 0), Quaternion.identity);
        boardTile.tag = "BoardTile";
        boardTile.name=letter;
        GameController.data.BoardTiles.Add(boardTile);
        boardTile.GetComponent<BoardTile>().UIclone = gameObject;
        boardTile.GetComponent<BoardTile>().letter = letter;
        boardTile.GetComponent<BoardTile>().score = score;
        TextMesh[] txts = boardTile.GetComponentsInChildren<TextMesh>();
        txts[0].text = letter;
        txts[1].text = score.ToString();
        boardTile.SetActive(false);
    }

    public void GoToSlot(GameObject slot)
    {
         Debug.Log("transform .position,,,,,,,"+ transform.position);
       MoveToPos(slot.GetComponent<RectTransform>().anchoredPosition,targetSlot);
        
        if (slot.GetComponent<UISlot>().UITile != null)
        {
            //slot.GetComponent<UISlot>().UITile.GetComponent<UITile>().GoToFreeSlot();
             Debug.Log("transform .position,,,,,,,"+ transform.position);
        }

        slot.GetComponent<UISlot>().UITile = gameObject;
        UISlot = slot;
        lastPosition = slot.GetComponent<RectTransform>().anchoredPosition;
         Debug.Log("transform .position,,,,,,,"+ transform.position);
    }

    public void GoToFreeSlot()
    {
        GameObject freeUISlot = GameController.data.GetFreeUISlot();
        if (GameController.data.GetFreeUISlot() != null)
        {
            MoveToPos(freeUISlot.GetComponent<RectTransform>().anchoredPosition,targetSlot);
            freeUISlot.GetComponent<UISlot>().UITile = gameObject;
            UISlot = freeUISlot;
            lastPosition = freeUISlot.GetComponent<RectTransform>().anchoredPosition;
        }
    }
//.........Get Last FreeSlot...........//
public void GoToLastFreeSlot()
{
       GameObject freeUISlot = GameController.data.GetLastFreeUISlot();

     
       if (GameController.data.GetLastFreeUISlot() != null)
        {
              Debug.Log("freeUISlot......."+freeUISlot.name);
            MoveToPos(freeUISlot.GetComponent<RectTransform>().anchoredPosition,targetSlot);
            freeUISlot.GetComponent<UISlot>().UITile = gameObject;
            UISlot = freeUISlot;
            lastPosition = freeUISlot.GetComponent<RectTransform>().anchoredPosition;
        }
}
    public void CancelTile() {

        Vector3 tempPos = boardTile.transform.position;
        tempPos.z = uiZaxis;
        transform.position = tempPos;
        Color color;
          
        if (UISlot.GetComponent<UISlot>().UITile == null)
        {
             MoveToPos(lastPosition,targetSlot);
             UISlot.GetComponent<UISlot>().UITile = gameObject;
            // boardTile.GetComponent<BoardTile>().currentslot=null;
            // gameObject.transform.parent = targetSlot.transform;
        }
        else
            GoToFreeSlot();


          
       

         ColorUtility.TryParseHtmlString("#63BC40", out color);
         boardTile.GetComponent<SpriteRenderer>().color = color; //new Color (1f, 1f, 0.5f); // Yellow
        
         var boardTileScript =  boardTile.GetComponent<BoardTile>();
        
        boardTileScript.Bonous.text=string.Empty;

        if (boardTileScript.currentslot != null)
        {
            boardTileScript.currentslot.free = true;
        }
        
         boardTile.SetActive(false);
       // boardTile.GetComponent<BoardTile>().currentslot=null;
       // GameController.data.PreApply();
       
    }

//.........Undo move ,.............//
public void UndoMove()
{
       Vector3 tempPos = boardTile.transform.position;
        tempPos.z = uiZaxis;
        transform.position = tempPos;
        Color color;
         if (UISlot.GetComponent<UISlot>().UITile == null)
        {
            MoveToPos(lastPosition,targetSlot);
            UISlot.GetComponent<UISlot>().UITile = gameObject;

            //GameController.data.BonousReduce(BonusName);
        }
        else
            GoToLastFreeSlot();
           //Debug.Log("gggggggggggggggggggg");
       // boardTile.GetComponent<SpriteRenderer>().color =FCFF0E;// new Color (1f, 1f, 0.5f); // Yellow
         ColorUtility.TryParseHtmlString("#63BC40", out color);
         boardTile.GetComponent<SpriteRenderer>().color = color;
         var boardTileScript =  boardTile.GetComponent<BoardTile>();
         if (boardTileScript.currentslot != null)
         {
             boardTileScript.currentslot.free = true;
         }
         
         boardTileScript.Bonous.text=string.Empty;
        boardTileScript.score= int.Parse(letterScore.text);
        
         boardTile.SetActive(false);
         
         

        
}



  public void MoveToPos(Vector3 toPos,GameObject slot)
    {
        gameObject.transform.parent.SetAsLastSibling();
        transformer.MoveUI(toPos, 0.25f);

        Debug.LogFormat("gameobjectId {0}", boardTile.GetComponent<BoardTile>().gameObject.GetInstanceID());

        foreach (var VARIABLE in GameController.data._BoardTileParent)
        {
           // Debug.LogFormat("ParentId {0}",VARIABLE.GetInstanceID());
        }
        
  
        
      // gameObject.GetComponent<RectTransform>().anchoredPosition =toPos;
       slot=UISlot;
      slot.GetComponent<UISlot>().UITile = gameObject;
     //lastPosition = slot.GetComponent<RectTransform>().anchoredPosition;
        //UISlot = slot;
         gameObject.SetActive(true);
              if (BonusName!=string.Empty && GameController.data.is_returnBonous!=false)
               {
                         
                  
                    BonusName=string.Empty;
               }
              
              BoardTile tileobj = boardTile.GetComponent<BoardTile>();

        if(GameController.data.targetBoardSlot !=null)
        GameController.data.targetBoardSlot.transform.localScale = new Vector3(1, 1, 1);


        if (tileobj.currentslot != null)
     {
            Debug.Log("currentslot.free : ");

           // tileobj.currentslot.free=true;
     }


     
     
     if (GameController.data._BoardTileParent.Contains(tileobj.gameObject))
     {
         
            Debug.LogFormat("Removedd Object ");
         
            GameController.data._BoardTileParent.Remove(tileobj.gameObject);
     }
    

         

    }
    
   public  void MoveToPosSwap(Vector3 toPos)
    {
        gameObject.transform.parent.SetAsLastSibling();
        transformer.MoveUI(toPos, 0.25f);
      // gameObject.GetComponent<RectTransform>().anchoredPosition =toPos;
      


    }





    public bool CheckifOverUISlot()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult res in raycastResults)
            {

                if (res.gameObject.name == "ErrorAlert")
                {
                    return false;
                }

                if (res.gameObject.tag == "UISlot")
                {
                    targetSlot = res.gameObject;
                    return true;
                }
            }
        }
        targetSlot = null;
        return false;
    }

    public void SetSwapState(bool swapState)
    {
         needSwap = swapState;

        if (needSwap)
             MoveToPosSwap(GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 60));
         else
            MoveToPosSwap(GetComponent<RectTransform>().anchoredPosition - new Vector2(0, 60));
    }


    public void GetTrayGameObject(string _str)
    {
        TrayGameObjectName=_str;
    }

}
