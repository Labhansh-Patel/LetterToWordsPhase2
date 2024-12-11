using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using APICalls;

public class LeaderBoardPrefab : MonoBehaviour
{
    private LeaderBoard board;
    [SerializeField] private Text Rank;
    [SerializeField] public Text Name;
    [SerializeField] private Image ProfieleImage;



    public void SetDataPrefab(LeaderBoard board)
    {
       this.board = board;
        SetUidata();
    }
    private void SetUidata()
    {
        Rank.text = board.rank.ToString();
        Name.text = board.user_data.name;

        if (board.user_data.avatar_id != null)
        {
            ProfieleImage.sprite= UI_Manager._instance.Avtar[int.Parse(board.user_data.avatar_id)];
         }


    }
}
