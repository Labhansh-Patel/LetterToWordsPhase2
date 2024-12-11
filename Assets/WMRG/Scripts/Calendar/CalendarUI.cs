using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class CalendarUI : MonoBehaviour
{
    public Sprite None;
     public Sprite GameAvailable;
     public Sprite gameInProgress;
    public Sprite gameCompleted;
    
    
    public Image[] calendarTiles;
    public Text month;
    public Button plusArrow;
    public Button minusArrow;
    public Button CreateGame;

    public GameObject GameDataListPrefab;
    public Transform GameListContent;
    public GameObject DateGamePanel;
    public Text GameListDate;

    public Transform[] calendarPositions;


}
