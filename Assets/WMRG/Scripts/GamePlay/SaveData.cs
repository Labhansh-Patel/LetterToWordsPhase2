using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Photon.Pun;
using APICalls;

public class SaveData : MonoBehaviour
{
   public static SaveData data;

   public GameStateData ResumeData= new GameStateData();
   private PhotonView photonView;
  
   
  
   
	public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
	{
		NullValueHandling = NullValueHandling.Ignore,
		MissingMemberHandling = MissingMemberHandling.Ignore,
		ReferenceLoopHandling = ReferenceLoopHandling.Ignore
	};


   void Awake()
   {
        data = this;
   }






    // Start is called before the first frame update
    void Start()
    { 
         	if (photonView == null)
		{
			photonView = GetComponent<PhotonView>();
		}
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 //.........Show data on Board.................//
 public void ShowDataOnBoard()
 {





   UI_Manager._instance.LoadGameScreen();
  // GameController.data. FitUIElements();



 }

    public void SinglePlayerResumeGameData()
    {

        string json = JsonConvert.SerializeObject(ResumeData, jsonSettings);
        Debug.LogFormat("Json {0}", json);
        byte[] bytes = Encoding.ASCII.GetBytes(json);

        //ApiManager.SaveGameData(GlobalData.GameId, GlobalData.UserId, 1, ResumeData, HandleSaveGameData);







        // Debug.LogFormat("Json {0}", bytes);
        // File.WriteAllText(Application.dataPath +"/Savedata.jason",json);
        // File.WriteAllText(Application.streamingAssetsPath +"/Savedata.jason",json);



    }

    private void HandleSaveGameData(bool asucess, UserSaveData callback)
    {
        if (asucess)
        {
            LogSystem.LogColorEvent("green", "UpdateNotification  ID : {0}", callback.message);
            // ShowErrorPopup(callback.message);
        }
        else
        {
            LogSystem.LogColorEvent("red", "UpdateNotification  ID : {0}", callback.message);
            //(callback.message);
        }
    }

    //.............Resume Game Data...................//
    public void ResumeGameData()
{
 
 

       
 string json = JsonConvert.SerializeObject( ResumeData, jsonSettings);
 Debug.LogFormat("Json {0}", json);
  byte[] bytes = Encoding.ASCII.GetBytes(json);
       
       
	photonView.RPC("RecivePlayerGameData", RpcTarget.All, bytes,PhotonNetwork.LocalPlayer.ActorNumber);




 // Debug.LogFormat("Json {0}", bytes);
 // File.WriteAllText(Application.dataPath +"/Savedata.jason",json);
// File.WriteAllText(Application.streamingAssetsPath +"/Savedata.jason",json);
 


}



public void LoadDataFromJson()

{

   // string json = Encoding.ASCII.GetString(bytes);
   //Debug.LogFormat("JsonRecieved {0}", json);
	
   //GameStateData GetResumeData = JsonConvert.DeserializeObject<GameStateData>(json);
    		string FilePath = Application.streamingAssetsPath + "/Savedata.jason";
		   string jsonString = "";


		#if UNITY_ANDROID
		   WWW reader = new WWW(FilePath);
		   while(!reader.isDone){}
		   jsonString = reader.text;

		#else  
		    jsonString = File.ReadAllText(FilePath);
	     #endif 
		   GameStateData GetResumeData = JsonConvert.DeserializeObject<GameStateData>(jsonString);




           Debug.Log("jsonString............"+jsonString);
         // GameController.data. GetResumeData(GetResumeData);
}




}

      


[System.Serializable]
    public class BoardData :LetterBlock
    {
        public string tilename;
     
        public  string bonousname;
    
         
         public BoardData(string _tilename, string _letter, string _score, string _bonousname) 
         {

           tilename =_tilename;
           letter =_letter;
           score = Convert.ToInt32( _score);
           bonousname=_bonousname;   
         }

    }

[Serializable]
 public class GameStateData
 {
   
      public List<BoardData> BoardData = new  List<BoardData>();
      public List<LetterBlock> PlayerHandData= new List<LetterBlock>();
      public List<TrayData> TrayDatas=new List<TrayData>();

      public int TwoLetter;
      public int ThreeLetter;
      public int TwoWord;
      public int ThreeWord;
      public int score;
      public int TrayLetterNumber;
      public int TrayLetterIndex;
      public int intTrayGameObjectName;
      public int GameId;
      public string GameType;
      public string GameMode;
      public string GameDate;



    //Your App ID: ace01936-b173-4f6e-aea3-449f6c8328b3


}

 


[Serializable]
 public class LetterBlock
 {
  

    public string letter;
    public int score;
    private bool isTossedMinus = false;

    public  void LetterBlocks ( string _letter, string _score)
    {
          letter =_letter;
          try
          {
	          score =  Convert.ToInt32(_score);
          }
          catch (Exception e)
          {
	          Console.WriteLine(e);
	          score = 1;
	         
          }

          isTossedMinus = false;
    }

    public void TossLetter()
    {
	    score -= 1;
	    isTossedMinus = true;
    }

    public void UndoLetter()
    {
	    if (isTossedMinus)
	    {
		    LogSystem.LogEvent("UNDOLETTER SCORE ADDED");
		    score += 1;
		    isTossedMinus = false;
	    }
	   
    }

 }

[Serializable]
 public class TrayData 
 {
     public string TileName;
     public string score;

     // public TrayData(string _letter,string _tilename)
     // {
     //     letter =_letter;
     //     TileName=_tilename;
     // }


 }



 










