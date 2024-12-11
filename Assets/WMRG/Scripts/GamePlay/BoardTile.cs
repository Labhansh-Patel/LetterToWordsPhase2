using UnityEngine;
using System.Collections;

public class BoardTile : MonoBehaviour {


    public static BoardTile data;
    public bool completed;
    public GameObject UIclone;
    public BoardSlot currentslot;
    public string letter;
    public int score;
    public string BonusName = string.Empty;


    public TextMesh Bonous;
    public TextMesh ScoreText;
    public UITile BoardUITile ;

    //public bool IsBonousClick;

    void Awake()
    {
        if (data    == null)
        {
            data = this;
             DontDestroyOnLoad(this);
          
        }
        Bonous.text=string.Empty;
    }


    void OnMouseDown()
    {
           Debug.Log("this.gameObject.name........"+this.gameObject.name );

        //  if(GameController.data.isclickable)
        // {
        //    return; 
        // }
        //Debug.Log(currentslot.GetInstanceID());
        if (currentslot != null && !completed)
        {
           
            currentslot.free = true;
            Debug.Log("currentslot.free : ");

            currentslot = null;
        }
             //IsBonousClick=true;
      
    
    }
     void OnMouseUp()
    {
      // gameObject.transform.localScale = new Vector3(1f,1f,1f);
    }

    void OnMouseDrag()
    {
       Debug.Log("GameController.data.isclickable    " + GameController.data.isclickable);
         if(GameController.data.isclickable)
        {
           return; 
        }
        Debug.Log("completed     " + completed);
        if (completed)
            return;
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0;
        UIclone.SetActive(true);
        UIclone.GetComponent<UITile>().dragging = true;
        UIclone.transform.position = cursorPos;
        GameController.data.letterDragging = true;
        gameObject.SetActive(false);
        

        // GameController.data.SelectedTiel(this.gameObject);
    }


   

}
