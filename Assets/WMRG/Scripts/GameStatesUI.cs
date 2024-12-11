using APICalls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStatesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI yourScoreTxt;
    [SerializeField] private TextMeshProUGUI avgScoreTxt;
    [SerializeField] private TextMeshProUGUI percentileTxt;
    [SerializeField] private Button reloadStatsBtn;
    public static GameStatesUI Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void RefreshStats()
    {
        ApiManager.GetGameStats(GlobalData.UserId, GlobalData.GameId.ToString(), (bool success, PlayerStatsData statsData) =>
        {
            if (success)
            {
                nameTxt.text = "Name: " + statsData.ResponseData.name;
                yourScoreTxt.text = "Your Score: " + statsData.ResponseData.score;
                avgScoreTxt.text = "Average Score: " + statsData.ResponseData.average;
                percentileTxt.text = "Your Percentile: " + statsData.ResponseData.percentile_score + "%";
            }
            else
            {
                HandleEvents.PopoupErrorMsgOpen(statsData.ResponseMessage);
                reloadStatsBtn.gameObject.SetActive(true);
                nameTxt.text = "Name: ?";
                yourScoreTxt.text = "Your Score: ?";
                avgScoreTxt.text = "Average Score: ?";
                percentileTxt.text = "Your Percentile: ?";
            }
        });
    }
}