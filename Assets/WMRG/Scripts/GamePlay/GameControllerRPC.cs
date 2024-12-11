using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Photon.Pun;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Networking;

public class GameControllerRPC : MonoBehaviour
{

	public static GameControllerRPC data;


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

  void Start()
  {
        	if (photonView == null)
		{
			photonView = GetComponent<PhotonView>();
		}
  }

public void SendData(GameStateData _ResumeData )
{
  // _SaveData = _SaveData.FillData();
 //ResumeData._MultiplayerWordData.Add(PhotonNetwork.LocalPlayer.ActorNumber , ResumeData );
 

       
 string json = JsonConvert.SerializeObject( _ResumeData, jsonSettings);
	 
 Debug.LogFormat("Json {0}", json);
  byte[] bytes = Encoding.ASCII.GetBytes(json);

	photonView.RPC("RecivePlayerGameData", RpcTarget.All, bytes,PhotonNetwork.LocalPlayer.ActorNumber);



 // Debug.LogFormat("Json {0}", bytes);
 // File.WriteAllText(Application.dataPath +"/Savedata.jason",json);
// File.WriteAllText(Application.streamingAssetsPath +"/Savedata.jason",json);
 

}


[PunRPC]
	public void RecivePlayerGameData(byte[] bytesData,int ActorNumber)
	{
		if (GameController.data._MultiplayerWordData.Count<1)
        {
			UI_Manager._instance._IsTimmer = false;
			UI_Manager._instance._IsTurnTimmer = false;

			UI_Manager._instance._time = 30;
			UI_Manager._instance.RoundStartTimmer(true);
			UI_Manager._instance.Timmer();
		}
	

		string json = Encoding.ASCII.GetString(bytesData);
		Debug.LogFormat("JsonRecieved {0}", json);
		GameStateData _Newdata = JsonConvert.DeserializeObject<GameStateData>(json);
         Debug.Log("ActorNumber............ "+ActorNumber);
	
        GameController.data.RecivePlayerWordData(_Newdata,ActorNumber);

	}


	public void SendWordScore(int score)
    {
		string json = JsonConvert.SerializeObject(score, jsonSettings);
		Debug.LogFormat("Json {0}", json);
		byte[] bytes = Encoding.ASCII.GetBytes(json);
		photonView.RPC("ReciveWordScore", RpcTarget.All, bytes, PhotonNetwork.LocalPlayer.ActorNumber);

	}


	[PunRPC]
	public  void ReciveWordScore(byte[] bytesData, int ActorNumber)
    {
		string json = Encoding.ASCII.GetString(bytesData);
		Debug.LogFormat("JsonRecieved {0}", json);
		int _Newdata = JsonConvert.DeserializeObject<int>(json);
  
		GameController.data.RecivePlayerScore(ActorNumber, _Newdata);
	}

	public void SendTurnWord(string word)
	{
		string json = JsonConvert.SerializeObject(word, jsonSettings);
		Debug.LogFormat("Json {0}", json);
		byte[] bytes = Encoding.ASCII.GetBytes(json);
		photonView.RPC("ReciveTurnWord", RpcTarget.All, bytes, PhotonNetwork.LocalPlayer.ActorNumber);

	}


	[PunRPC]
	public void ReciveTurnWord(byte[] bytesData, int ActorNumber)
	{
		string json = Encoding.ASCII.GetString(bytesData);
		Debug.LogFormat("JsonRecieved {0}", json);
		string _Newdata = JsonConvert.DeserializeObject<string>(json);

		GameController.data.RecivePlayerWord(ActorNumber, _Newdata);
	}


	public void SendPreviousData(byte[] bytesData, Player player)
    {
		//string json = JsonConvert.SerializeObject(_previousData, jsonSettings);
		//Debug.LogFormat("Json {0}", json);
		//byte[] bytes = Encoding.ASCII.GetBytes(json);
		photonView.RPC("RecivePreviousData",player, bytesData);
	}

	[PunRPC]
	public void RecivePreviousData(byte[] bytesData)
	{
		string json = Encoding.ASCII.GetString(bytesData);
		Debug.LogFormat("JsonRecieved {0}", json);
		GameStateData _Newdata = JsonConvert.DeserializeObject<GameStateData>(json);

		GameController.data.GetResumeData( _Newdata);
	}
}
