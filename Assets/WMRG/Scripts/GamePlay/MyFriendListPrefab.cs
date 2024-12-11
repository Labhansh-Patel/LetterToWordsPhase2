using APICalls;
using UnityEngine;
using UnityEngine.UI;
public class MyFriendListPrefab : MonoBehaviour
{
    
    private MyFriendList friendData;
    [SerializeField] private Text FriendName;
    [SerializeField] public Toggle AddFriend;
     [SerializeField] private Image ProfileImage;



 public void SetDataPrefab(MyFriendList friend)
    {
        friendData = friend;
        SetUidata();
    }
    private void  SetUidata()
    {
        FriendName.text = friendData.friend.name;
        if (friendData.friend.avatar_id !=null)
        ProfileImage.sprite= UI_Manager._instance.Avtar[int.Parse(friendData.friend.avatar_id)];
        AddFriend.onValueChanged.RemoveAllListeners();
        AddFriend.onValueChanged.AddListener(CallAddFriendApi);
        

    }

   /* private void CallAddfriendApi()
    {
        UI_Manager._instance.AddNotificationID( friendData.friend.notification_id);
     
    }*/
    private void CallAddFriendApi(bool arg0)
    {
        if (arg0)
        {
            UI_Manager._instance.AddNotificationID(friendData.friend.notification_id);
        }
        else
        {

            UI_Manager._instance.RemoveNotificationId(friendData.friend.notification_id);
        }

    }


}
