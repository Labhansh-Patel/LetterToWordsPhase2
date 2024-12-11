using GameEvents;
using Newtonsoft.Json;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPrefab : MonoBehaviour
{
    private RoomInfo RoomData;

    [SerializeField] private Button JoinFriend;
    [SerializeField] public Text RoomName;
    [SerializeField] public Text PlayerNumber;
    [SerializeField] public Text gameMode;
    [SerializeField] public Text gameType;


    private CreateMultiplayerData roomMultiplayerData;

    public void SetDataPrefab(RoomInfo RoomData)
    {
        this.RoomData = RoomData;
        string roomDataJson = (string)RoomData.CustomProperties["gameData"];
        roomMultiplayerData = JsonConvert.DeserializeObject<CreateMultiplayerData>(roomDataJson);
        SetUidata();
    }


    private void SetUidata()
    {
        RoomName.text = RoomData.Name + "   Host:" + roomMultiplayerData.hostName;
        PlayerNumber.text = "Players : " + RoomData.PlayerCount;
        gameMode.text = (roomMultiplayerData.gamemode == "1") ? "FunGame" : "HardCore";
        gameType.text = roomMultiplayerData.multiplayerType.ToString();
        JoinFriend.onClick.RemoveAllListeners();
        JoinFriend.onClick.AddListener(() =>
        {
            if (GlobalData.userData.IsPremiumUser)
            {
                CallJoinFriend();
            }
            else
            {
                AdsManager.Instance.onAdFinised -= CallJoinFriend;
                AdsManager.Instance.onAdFinised += CallJoinFriend;
                AdsManager.Instance.PlayAdInterstitial();
            }
        });
    }

    public void CallJoinFriend()
    {
        string roomDataJson = (string)RoomData.CustomProperties["gameData"];
        CreateMultiplayerData currentGameData = JsonConvert.DeserializeObject<CreateMultiplayerData>(roomDataJson);
        EventHandlerGame.EmitEvent(GameEventType.JoinGame, currentGameData);
        EventHandlerGame.EmitEvent(GameEventType.Loading, true);
    }
}