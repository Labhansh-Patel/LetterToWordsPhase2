using APICalls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendListPrefab : MonoBehaviour
{

    private SearchFriendList friendData;
    [SerializeField] private Text FriendName;
    [SerializeField] private Button AddFriend;
     [SerializeField] private Image ProfileImage;


    public void SetDataPrefab(SearchFriendList friend)
    {
        friendData = friend;
        SetUidata();
    }
    private void  SetUidata()
    {
        FriendName.text = friendData.Name;
        if (friendData.avatar_id!=null)
        ProfileImage.sprite= UI_Manager._instance.Avtar[int.Parse(friendData.avatar_id)];

        AddFriend.onClick.RemoveAllListeners();
        AddFriend.onClick.AddListener(CallAddfriendApi) ;
    }

    private void CallAddfriendApi()
    {
        UI_Manager._instance.SendFriendRequest( friendData.id);
    }
}
