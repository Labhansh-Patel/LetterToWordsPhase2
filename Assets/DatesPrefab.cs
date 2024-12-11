using APICalls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DatesPrefab : MonoBehaviour
{
    private GetDatesHeader GetDatesHeader;
    [SerializeField] private TextMeshProUGUI Date;

    [SerializeField] private Button Play;
    private string date;


    public void SetDataPrefab(string date)
    {
        this.date = date;
        SetUidata();
    }

    private void SetUidata()
    {
        Date.text = date;

        Play.onClick.RemoveAllListeners();
        Play.onClick.AddListener(PlayButtonClick);
    }

    private void PlayButtonClick()
    {
        UI_Manager._instance.CreateGame(1, date);
        CommonApi.CallDateWiseData(date);
    }
}