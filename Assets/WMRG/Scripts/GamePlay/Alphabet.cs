using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using APICalls;
using System.Linq;

public class Alphabet : MonoBehaviour
{

    [System.Serializable]
    public class Letters
    {
        public string letter;
        public int score;
    }
   
    public List<LetterStack> LetterStackList = new List<LetterStack>();
    public List<Letters> LettersList;
    public List<string> LettersFeed;
    public List <string> TossLetter;
    public List <GameObject> Traydata;
    public  GameObject ReplaceLetter;
    public  GameObject LastReplaceLetter;
    public  GameObject TossReplaceLetter;
    [HideInInspector]public List<string>TrayGameObjectName =new List<string>();
    public int intTrayGameObjectName=-1;
    


    public static Alphabet data;
     public GameObject AlphabetPrefab;

    public GameObject AlphabetPanel;
    public GameObject StackPowerPanel;

    void Awake () {
        data = this;
    }






 private  int  rand =0;
 int toss ;

    void FillLettersFeed() {
        

        rand=LetterStackList.Count;
        
      
        toss= LetterStackList.Count-7;
        foreach(LetterStack letterItem in LetterStackList)
        {
           
                LettersFeed.Add(letterItem.letter);
          
        }

    }
     
    public string GetRandomLetter() {
        //int rand = UnityEngine.Random.Range(0,LettersFeed.Count);
        


          string result=" ";
       /* rand++;
        Debug.Log("rand : " + rand);
        result = TossLetter[rand];*/
        Debug.Log("TossLetter.Count........" + rand);
        if (rand > 0)
        {

            if (GameController.data.isReCreateTile == false)
             {
            rand--;
            Debug.Log("rand : " + rand);
            result = LettersFeed[rand];
            }





            SaveData.data.ResumeData.TrayLetterNumber = rand;
            // LettersFeed.RemoveAt(rand);

            if (GameController.data.isReCreateTile == true)
            {
                num1++;

                intTrayGameObjectName++;
                string name = "Name_" + num1.ToString();
                Debug.Log("intTrayGameObjectName........" + intTrayGameObjectName + "  name..." + name);
                if (num1 < 94)
                {
                    result = AlphabetPanel.transform.Find(name).GetChild(0).GetComponent<Text>().text;
                    AlphabetPanel.transform.Find(name).GetChild(0).GetComponent<Text>().text = " ";
                    TrayGameObjectName.Add(name);
                }



                SaveData.data.ResumeData.TrayLetterIndex = num1;
                //SaveData.data.ResumeData.intTrayGameObjectName = intTrayGameObjectName;

            }

        }
       

        Debug.Log("result....." + result + " ...rand..." + rand);

        return result ;
     
        // string str ="s";
        // return  str;
    }


//..............Get Letter,,,,,,,,,,,,,,,,,,,,,,,,//
 public string GetLetter() {
         string result =" ";   
            toss--;

        Debug.Log("toss : "+ toss);
        if (toss >= 0)
        {
            result = LettersFeed[toss];
        }


       // Debug.Log("LettersFeed........" + LettersFeed.Count + "toss......" + toss + " result..." + result);
        return result ;
        
    }

//..........Get Letter From API ......
public void GetLettertoApi()
{

        //     if (UI_Manager._instance.CheckInternetConnection()==false)
        // {
        // Debug.Log("not internet connection");
        // UI_Manager._instance.ShowErrorPopup("\n No internet connection");

        //}
        //else
        //{
        // GameObject.Find("Manager").GetComponent<WebServiceManager>().CallTodayStack(); 
         ApiManager.GetDailyLetter(HandleDailyLetterCall);
       // ApiManager.TodayStack(HandleTodayLetter);



    // }


    }

    private void HandleTodayLetter(bool aSucess, TodayLetterStackHeader stack)
    {
        if (aSucess)
        {
           
            LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", stack.result);
            for (int i = 0; i < stack.result.Length; i++)
            {
                // Alphabet.data.LetterStackList.Add(TodayLetter.result[i].alphabet); 

                // Alphabet.data.LetterStackList.Add(TodayLetter.result[i].alphabet);

                LetterStack newdata = new LetterStack(stack.result[i]. alphabet, stack.result[i].score);
                LetterStackList.Add(newdata);

            }


          
        }
        else
        {
            LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", stack.message);
        }
    }

    private void HandleDailyLetterCall(bool asucess, GetDailyLetterHeader callback)
        {
            if (asucess)
            {
                LogSystem.LogColorEvent("green", "HandleDailyLetterCall Count  ID : {0}", callback.result.Count);
                for(int i=0; i< callback.result.Count; i++)
                {
                   // Alphabet.data.LetterStackList.Add(TodayLetter.result[i].alphabet); 
                    
                   // Alphabet.data.LetterStackList.Add(TodayLetter.result[i].alphabet);

                   LetterStack newdata = new LetterStack(callback.result[i].name, callback.result[i].score); 
                   Alphabet.data.LetterStackList.Add(newdata);

                }
                 

            
            }
            else
            {
                LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
            }
        }
    public int GetLetterScore(string letter)
    {
        foreach(LetterStack letterItem in LetterStackList)
        {
            if (letterItem.letter == letter)
                return  int.Parse(letterItem.score);
        }
        return 0;
    }

    public void ResetFeed()
    {
        rand = 0;
        toss = 0;
        num1 = 0;
        LettersFeed.Clear();
        FillLettersFeed();
        
    }

 public void FillTrayResumeData(GameStateData _GetResumeData)
 {
        
        int startposx = -463 ;//-(Settings.gridSize/2)*130;
		int startposy = 40;    //(Settings.gridSize/2)*-150;
		int x = startposx;
		int y = startposy;
        foreach (Transform item in AlphabetPanel.transform)
        {
            Destroy(item.gameObject);
        }
        for (int i=0; i<96; )
     {
           // Debug.Log("  "+ _GetResumeData.TrayDatas[i].TileName);
                 GameObject intrestdata = Instantiate(AlphabetPrefab) as GameObject;
                 intrestdata.name = _GetResumeData.TrayDatas[i].TileName;
                  Traydata.Add(intrestdata);
                 intrestdata.transform.SetParent(AlphabetPanel.transform);
                 intrestdata.transform.localScale = Vector3.one;
                 intrestdata.transform.localPosition =  new Vector3(x,y);
                //intrestdata.transform.GetChild(0).GetComponent<Text>().text = _GetResumeData.TrayDatas[i].letter;
                if (intrestdata.transform.GetChild(0).GetComponent<Text>().text !="")
                {
                      // LettersFeed.Add(_GetResumeData.TrayDatas[i].letter);
                       rand =i;
                     
                }
                else{
                         //LettersFeed.Add(_GetResumeData.TrayDatas[i].letter);
                }
                
                intrestdata.SetActive(true);
                i++;
                 if(i%31!=0)
                {
                       x += 31;
                    
                }
                else
                {
                     x =startposx;
                     y -=40; 
                }
          

        }

        num1 = _GetResumeData.TrayLetterIndex;
       // intTrayGameObjectName = _GetResumeData.intTrayGameObjectName;                                 //(93-rand)-1;
        rand = _GetResumeData.TrayLetterNumber;         //rand+1;





 }


    //.........fillMultiplayerTrayData.........//
    public void FillMultiplayerTrayData(int _index , Dictionary<int, GameStateData> _MultiplayerWordData)
    {
        foreach (Transform item in AlphabetPanel.transform)
        {
            Destroy(item.gameObject);
        }
        Traydata.Clear();
        LettersFeed.Clear();
       

        Debug.Log("FillMultiplayerTrayData");

        int startposx = -463;
        int startposy = 40;    
        int x = startposx;
        int y = startposy;
        for (int i = 0; i < 93;)
        {
           // Debug.Log("count");
            GameObject intrestdata = Instantiate(AlphabetPrefab) as GameObject;
            intrestdata.name = _MultiplayerWordData[_index].TrayDatas[i].TileName;
            Traydata.Add(intrestdata);
            intrestdata.transform.SetParent(AlphabetPanel.transform);
            intrestdata.transform.localScale = Vector3.one;
            intrestdata.transform.localPosition = new Vector3(x, y);
           // intrestdata.transform.GetChild(0).GetComponent<Text>().text = _MultiplayerWordData[_index].TrayDatas[i].letter;
            if (intrestdata.transform.GetChild(0).GetComponent<Text>().text != "")
            {
                //LettersFeed.Add(_MultiplayerWordData[_index].TrayDatas[i].letter);
                rand = i;

            }
            else
            {
                //LettersFeed.Add(_MultiplayerWordData[_index].TrayDatas[i].letter);
            }

            intrestdata.SetActive(true);
            i++;
            if (i % 31 != 0)
            {
                x += 31;

            }
            else
            {
                x = startposx;
                y -= 40;
            }

        }

        rand = _MultiplayerWordData[_index].TrayLetterNumber;
        num1 = _MultiplayerWordData[_index].TrayLetterIndex;// (93 - rand) - 1;
       // rand = rand + 1;

        _MultiplayerWordData[_index].TrayDatas.Clear();
       

    }


    
 
 /// <summary>
 /// Letter from stack Power 
 /// </summary>

 public void LetterFromStackPower()
{

        foreach (Transform item  in AlphabetPanel.transform)
        {
            GameObject intrestdata = Instantiate(item.gameObject) as GameObject;
            intrestdata.transform.SetParent(StackPowerPanel.transform);
            intrestdata.transform.localScale = new Vector3(1f, 1f, 1f);
            intrestdata.SetActive(true);

        }
    }











    //................Generate 96 Alphabet ..........//

    public void DateWiseLetterScore()
    {
        int startposx = -300;
        int startposy = 35;
        int x = startposx;
        int y = startposy;

        int m = 0;
        int k = LetterStackList.Count - 7;
        int n = 0;

        foreach (Transform item in AlphabetPanel.transform)
        {
            Destroy(item.gameObject);
        }



        Debug.Log("LetterStackList.Count - 7 :  " + (LetterStackList.Count - 7));
        for (int i = 1; i <= (LetterStackList.Count - 7);)
        {


            string letter = GetLetter();


            // Debug.Log("i........"+i);
             TossLetter.Add(letter);
            GameObject intrestdata = Instantiate(AlphabetPrefab) as GameObject;
            intrestdata.name = "Name_" + i;
            intrestdata.transform.GetChild(0).GetComponent<Text>().text =  letter;
            Traydata.Add(intrestdata);
            intrestdata.transform.SetParent(AlphabetPanel.transform);
            intrestdata.transform.localScale = new Vector3(1f, 1f, 1f);
            intrestdata.transform.localPosition = new Vector3(x, y);
            intrestdata.SetActive(true);
            i++;


        }
    }












     //...........Generate 90 Alphabet............//
  public void GenerateTossAlbhabet()
  {
      	int startposx = -300;//-(Settings.gridSize/2)*130;
		int startposy = 35;    //(Settings.gridSize/2)*-150;
		int x = startposx;
		int y = startposy;
        
        int m=0;
        int k= LetterStackList.Count-7;
        int n=0;

         foreach(Transform item in AlphabetPanel.transform)
        {
            Destroy(item.gameObject);
        }
        // TossLetter = new List<string>(90);

        /* for (int i = LetterStackList.Count ; i > LetterStackList.Count-7; i--)
         {
             string letter = GetLetter();
             TossLetter.Add(letter);
         }*/

        for (int i = (LetterStackList.Count - 7); i > 0;)
        {
            // LettersFeed.Add(letterItem.letter);
            // Debug.Log("i........"+i);


            if (i >= 72 || i <= 24)
            {
                string letter = GetLetter();


                // Debug.Log("i........"+i);
                TossLetter.Add(letter);
                GameObject intrestdata = Instantiate(AlphabetPrefab) as GameObject;
                intrestdata.name = "Name_" + i;
                // intrestdata.transform.GetChild(0).GetComponent<Text>().text =  letter;
                Traydata.Add(intrestdata);
                intrestdata.transform.SetParent(AlphabetPanel.transform);
                intrestdata.transform.localScale = new Vector3(1f, 1f, 1f);
                intrestdata.transform.localPosition = new Vector3(x, y);
                intrestdata.SetActive(true);
                Debug.LogFormat(" name " + i + " letter ......" + letter);

                i--;
              


            }
            else
            {
                if (i == 71)
                {
                    n++;
                }

                // Debug.Log("i----------"+i);

                if (n != 1)
                {
                    // TossLetter.Add(GetLetter());
                    string letter = GetLetter();
                    TossLetter.Add(letter);
                    GameObject intrestdata = Instantiate(AlphabetPrefab) as GameObject;
                    intrestdata.name = "Name_" + i;
                    // intrestdata.transform.GetChild(0).GetComponent<Text>().text = letter;
                    Traydata.Add(intrestdata);

                    intrestdata.transform.SetParent(AlphabetPanel.transform);
                    intrestdata.transform.localScale = Vector3.one;
                    intrestdata.transform.localPosition = new Vector3(x, y);
                    intrestdata.SetActive(true);
                    Debug.LogFormat(" name " + i + " letter ......" + letter);
                    i++;
                    
                }
               



            }
        }
                
	

              




        int j = 0;
        Debug.Log("tosscount....." + TossLetter.Count);
        for(int i = TossLetter.Count-8; i >= 0; i--, j++) 
        {
            Traydata[j].transform.GetChild(0).GetComponent<Text>().text = TossLetter[i];
           Debug.Log(" Traydata[j].transform ........" + Traydata[j].transform.name + "toss letter ...."+ TossLetter[i]);
        }

        foreach(string item in TossLetter)
        {
            Debug.Log(item);

        }


    } 

//.............Select Letter to toss..........//
 public int num1 = 0;
public void SelectedLetter(GameObject  _letter)
{
   ReplaceLetter = _letter;
   //LastReplaceLetter = ReplaceLetter;

}
//............Show Highlighter....................//
public void ShowHighlighter()
{
    foreach (GameObject tile in  GameController.data.UITiles)
    {
       
             if (ReplaceLetter!=null)
             {
               if (tile.activeInHierarchy && tile.name==ReplaceLetter.name)
                 {
                  // Debug.Log("tile.name......."+ tile.name);
                  //Debug.Log("tile.name......."+ ReplaceLetter.name);
      
                tile.GetComponent<UITile>().Hilighter.SetActive(true);
                }
              else
                {
                tile.GetComponent<UITile>().Hilighter.SetActive(false);

                }
             }
            else
            {
                  tile.GetComponent<UITile>().Hilighter.SetActive(false);
            }
            
        
    }
}


//...........Toss Method.............//
 public void LetterToss()
 {
     GameController.data.isReCreateTile=true;
      // Debug.Log("dsjd...." + rand);
          //num1--;
         // Debug.Log("dsjd...." + rand);
  // ReplaceLetter.transform.Find("Letter").GetComponent<Text>().text =GetRandomLetter();
   // TossLetter[num1];
     Debug.Log("dsjd...." + ReplaceLetter);
   if (ReplaceLetter != null)
   {
       ReplaceLetter.GetComponent<UITile>().needSwap = true;
   Debug.Log("dsjd...." + rand);
   // AlphabetPanel.transform.Find("Name_"+num1).GetChild(0).GetComponent<Text>().text = " "
   TossReplaceLetter=ReplaceLetter;
               // ReplaceLetter.SetActive(true);
    GameController.data.ApplySwap();
   }
   else
   {
       GameController.data.SwapBlock.transform.Find("TextTip").GetComponent<Text>().text="You haven't select tile";
       
   }
    //ReplaceLetter=null;

      
 }
 //.............. undo tray Letter..............//
 public void UndotrayLetter(GameObject obj ,string str ,string traystr,string score)
 {
     Debug.Log("str......"+str);
     Debug.Log("traystr......"+traystr);
     Debug.Log("obj.........."+obj);

        foreach (string item in TrayGameObjectName)
        {
            Debug.Log(item);
        }
        string Name=TrayGameObjectName[intTrayGameObjectName];
 
        //obj.GetComponent<UITile>().TrayGameObjectName;
        TrayGameObjectName.RemoveAt(intTrayGameObjectName);
    AlphabetPanel.transform.Find(Name).GetChild(0).GetComponent<Text>().text =traystr;// obj.GetComponent<UITile>().TrayLetter;
    obj.GetComponent<UITile>().letterString.text= str;//obj.GetComponent<UITile>().BeforeTossLetter;
        obj.GetComponent<UITile>().letterScore.text = score;//obj.GetComponent<UITile>().PreviousScore.ToString();
    obj.GetComponent<UITile>().TrayLetter=string.Empty;

        obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().letter = str;
        obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().score = int.Parse(score);
        TextMesh[] txts = obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().GetComponentsInChildren<TextMesh>();
         txts[0].text = str;
         txts[1].text = score;

        rand++;
      num1--;
      intTrayGameObjectName--;

    
        // obj.GetComponent<UITile>().TrayLetter=" ";
    }


}
 [System.Serializable]
    public class LetterStack
    {
        public string letter;
        public string score;

        	public LetterStack (string _letter, string _score)
            {
                letter = _letter;
                score  = _score;
            }

    }
