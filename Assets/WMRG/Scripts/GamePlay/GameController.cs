using System.Collections.Generic;
using System.Linq;
using System.Text;
using APICalls;
using Newtonsoft.Json;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController data;
    public GameObject mainMenu;
    public GameObject GameUI;
    public GameObject GameOverUI;
    public GameUi _gameUi;
    public GameObject ExitPanel;
    public GameObject UILettersPanel_p1, UILettersPanel_p2, UILettersPanel_p3, UILettersPanel_p4, ButtonsPanel;
    public List<GameObject> BoardSlots;
    public GameObject menuPanel;
    public GameObject SelectJokerLetter;
    public GameObject SwapBlock;
    public GameObject ErrorAlert;
    public GameObject confirmationDialog;
    public GameObject TossPanel;
    public GameObject newPlayerTitle;
    public GameObject newScoreBlock;
    public Text letterLeftTxt;
    public Text currentPlayerTxt;
    public Text confirmationDialogTxt;
    public Text TwoWord_Text;
    public Text ThreeWord_Text;
    public Text TwoLetter_Text;
    public Text ThreeLetter_Text;

    public Text WordScore_Text;
    public Text TurnScore_Text;
    public Text[] WordText;
    public Text[] Scoretext;

    public Text gameOverText;
    public InputField inputPlayersCount;
    public Toggle soundToggle;
    public bool dictChecking;
    public List<PlayerData> players;
    public GameObject[] bonousChip;
    private GameObject LastUISlot;
    [HideInInspector] public List<GameObject> LastUITiles = new List<GameObject>(7);
    [HideInInspector] public List<GameObject> LastBoardTiles = new List<GameObject>(7);
    public GameObject WordPrefabParent;
    public GameObject wordPrefab;

    public GameObject Extra_Tray_Space_Bonous_Tile;
    public GameObject Extra_Tray_Space_Bonous_Slot;
    [HideInInspector] public bool uiTouched, letterDragging, canBePlant, paused, swapMode, pointerOverPanel, isclickable;

    [HideInInspector] public List<GameObject> BoardTiles;

    private GameControllerRPC controllerRPC;


    [HideInInspector] public GameObject _TempLastUITiles;
    private List<GameObject> UITileSlots;
    public List<GameObject> UITiles;
    public List<GameObject> boardTilesMatters;
    public List<GameObject> _BoardTileParent = new List<GameObject>();
    private List<GameObject> TotalboardTiles = new List<GameObject>();
    [HideInInspector] public GameObject targetBoardSlot;
    private GameObject tempJokerTile;
    [HideInInspector] public GameObject SelectedBoarTile;
    private List<string> newWords;
    private List<WordCombos> _wordComboses = new List<WordCombos>();

    private List<int> newScore;
    private List<string> addedWords;
    private List<int> newLetterIds;
    private List<string> UndoLetter = new List<string>();
    private List<string> UndoScore = new List<string>();


    public List<string> TurnLetter;
    private string newWordsList;

    private List<string> TrayLetter = new List<string>();

    public Dictionary<int, GameStateData> _MultiplayerWordData = new Dictionary<int, GameStateData>();
    public Dictionary<int, int> _MultiplayerWordScoreData = new Dictionary<int, int>();
    public Dictionary<int, string> _MultiplayerTurnWordData = new Dictionary<int, string>();
    public List<GameObject> WordPanelPrefab = new List<GameObject>();

    private int IntUndoLetter = -1;
    private int IntTrayLetter = -1;
    private string preApplyInfo;
    private string confirmationID;
    public int playersCount = 2;
    private int currentPlayer;
    private int currentScore;
    private int errorCode;
    private int skipCount;
    private float canvasWidth;

    private int TwoTimeLetter = 0;
    private int ThreeTimeLetter = 0;
    private int TwoTimeWord = 0;
    private int ThreeTimeWord = 0;
    public int WordScore = 0;
    private int PreviousScore = 0;
    private int undoAllScore = 0;
    [HideInInspector] public int undo = -1;
    [HideInInspector] public int undoBoardTile = -1;
    private int BonousTileId;

    private int TwoTileId = -1;
    private int ThreeTileId = -1;

    public bool isReCreateTile = false;
    private bool is_2L = false;
    private bool is_3L = false;
    private bool is_2W = false;
    private bool is_3W = false;
    public bool is_Move = false;

    private int Two_lscore;

    private int Three_lscore;

    private int TurnScore;
    private int TempScore;

    private bool is_temp = true;
    private bool istwoBonous = false;
    private bool isthreeBonous = false;

    private bool istwoLetterBonous = false;
    private bool isthreeLetterBonous = false;

    private int HorizontalScore = 0;
    private int VerticalScore = 0;

    private bool isCheck = false;
    private bool is_horizontal;
    private bool is_vertical;
    private bool is_BonousRecalculate;
    private bool is_recalculateCheck;
    private bool is_VerticalTwoBonous = false;
    private bool is_VerticalThreeBonous = false;
    public bool Is_WordCheck;
    private bool is_ExtraBonous = false;
    private bool is_GarbageBonous = false;
    public GameObject boardTilePrefab;
    public static GameType gameType;
    public static MultiplayerGameType _multiplayergameType;
    private PhotonView photonView;
    public byte[] Previousdata;

    private int previousCurrentScore = 0;
    private int WordResponseCounter = 0;
    private bool isTileOnBoard = false;
    private bool isValid = false;
    private bool isGameStatus = false;

    public enum GameType
    {
        SinglePlayer,
        Multiplayer
    };


    public class WordCombos
    {
        public string word;
        public List<int> wordIDs = new List<int>();

        public WordCombos(string word, List<int> wordIDs)
        {
            this.word = word;
            this.wordIDs = wordIDs;
        }
    }

    public enum MultiplayerGameType
    {
        PublicMultiplayer,
        PrivateMultiplayer,
        JoinRoomMultiplayer,
        CreateMultiplayer,
        nulls
    };

    // [System.Serializable]
    //     public class 
    //     {

    //     }
    public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    [System.Serializable]
    public class PlayerData
    {
        public string name;
        public bool active;
        public bool complete;
        public int score;
        public Text scoreTxt;
        public GameObject UILettersPanel;
        public List<GameObject> UITileSlots;
        public List<GameObject> UITiles;
    }

    void Awake()
    {
        if (data == null)
        {
            data = this;
            DontDestroyOnLoad(this);
        }
    }

    public void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("here");
    }

    void Start()
    {
        data = this;
        // soundToggle.isOn = PlayerPrefs.GetInt("sound", 1) == 1 ? true : false;
        //Alphabet.data.GetLettertoApi();
        WordScore_Text.text = "Word Score     " + 0.ToString();
        if (photonView == null)
        {
            photonView = GetComponent<PhotonView>();
        }

        controllerRPC = GetComponent<GameControllerRPC>();

        //StartGame();
    }


//.............Start Game ..................//
    public void StartGame()
    {
        Debug.Log("start game");


        playersCount = int.Parse(inputPlayersCount.text);
        ResetData();
        ResetAllValue();
        FitUIElements();
        FillUITiles();
        for (int i = 0; i <= playersCount - 1; i++)
        {
            players[i].active = true;
            players[i].scoreTxt.text = "0";
        }

        SwitchPlayer();
        UpdateTxts();
        mainMenu.SetActive(false);
        GameUI.SetActive(true);
        GameOverUI.SetActive(false);
        isclickable = false;
        TurnScore_Text.text = "Turn Score     " + 0.ToString();
        WordScore_Text.text = "Word Score     " + 0.ToString();
        _gameUi._textUi.AquireTileTxt.text = 0.ToString() + "/ 121";
        _MultiplayerWordData.Clear();
        _MultiplayerWordScoreData.Clear();
        _MultiplayerTurnWordData.Clear();
        if (gameType == GameType.Multiplayer)
        {
            // UI_Manager._instance.ShowPlayerNameButton.SetActive(true);
        }
        else
        {
            // UI_Manager._instance.ShowPlayerNameButton.SetActive(false);
        }

        if (targetBoardSlot != null)
            targetBoardSlot.transform.localScale = new Vector3(1f, 1f, 1f);

        if (gameType == GameType.SinglePlayer)
        {
            SendSaveData();
        }


        _gameUi._canvasUi.CheckBtnPanel.SetActive(false);
        UILettersPanel_p1.SetActive(true);
        CommonApi.CallGetUserPower();

        RemoveListeners();
        AddAllListeners();

        HandleEvents.ChangeStates(States.GamePlay);
    }

    public void RemoveListeners()
    {
        _gameUi._buttonUi.GameExitYesBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.GameExitNoBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.StarBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.CloseGameBonousBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.CloseSelectBonousBtn.onClick.RemoveAllListeners();

        _gameUi._buttonUi.ExtraTraySpaceBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.AnyTileBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.AnyLetterBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.FreeGarbageBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.ExtendedTimeBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.BlockExtendedBtn.onClick.RemoveAllListeners();

        _gameUi._buttonUi.GameExtraTraySpaceBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.GameAnyTileBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.GameAnyLetterBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.GameFreeGarbageBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.GameExtendedTimeBtn.onClick.RemoveAllListeners();
        _gameUi._buttonUi.GameBlockExtendedBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        _gameUi._buttonUi.GameExitYesBtn.onClick.AddListener(GameExitYesBtnClick);
        _gameUi._buttonUi.GameExitNoBtn.onClick.AddListener(GameExitNoBtnClick);
        _gameUi._buttonUi.StarBtn.onClick.AddListener(StarBtnClick);
        _gameUi._buttonUi.CloseGameBonousBtn.onClick.AddListener(CloseGameBonousBtnClick);
        _gameUi._buttonUi.CloseSelectBonousBtn.onClick.AddListener(CloseSelectBonousBtnClick);

        _gameUi._buttonUi.ExtraTraySpaceBtn.onClick.AddListener(ExtraTraySpaceBtnClick);
        _gameUi._buttonUi.AnyTileBtn.onClick.AddListener(AnyTileBtnClick);
        _gameUi._buttonUi.AnyLetterBtn.onClick.AddListener(AnyLetterBtnClick);
        _gameUi._buttonUi.FreeGarbageBtn.onClick.AddListener(FreeGarbageBtnClick);
        _gameUi._buttonUi.ExtendedTimeBtn.onClick.AddListener(ExtendedTimeBtnClick);
        _gameUi._buttonUi.BlockExtendedBtn.onClick.AddListener(BlockExtendedBtnClick);

        _gameUi._buttonUi.GameExtraTraySpaceBtn.onClick.AddListener(GameExtraTraySpaceBtnClick);
        _gameUi._buttonUi.GameAnyTileBtn.onClick.AddListener(GameAnyTileBtnClick);
        _gameUi._buttonUi.GameAnyLetterBtn.onClick.AddListener(GameAnyLetterBtnClick);
        _gameUi._buttonUi.GameFreeGarbageBtn.onClick.AddListener(GameFreeGarbageBtnClick);
        _gameUi._buttonUi.GameExtendedTimeBtn.onClick.AddListener(GameExtendedTimeBtnClick);
        _gameUi._buttonUi.GameBlockExtendedBtn.onClick.AddListener(GameBlockExtendedBtnClick);
    }

    private void GameExtraTraySpaceBtnClick()
    {
        is_ExtraBonous = false;
        _gameUi._canvasUi.GameBonous.SetActive(false);
        CommonApi.CallUpdateUserPower("extra_tray_space", "1", "+");
    }

    private void GameAnyTileBtnClick()
    {
        is_ExtraBonous = false;
        _gameUi._canvasUi.GameBonous.SetActive(false);
        CommonApi.CallUpdateUserPower("any_tile_from_stack", "1", "+");
    }

    private void GameAnyLetterBtnClick()
    {
        is_ExtraBonous = false;
        _gameUi._canvasUi.GameBonous.SetActive(false);
        CommonApi.CallUpdateUserPower("any_letter", "1", "+");
    }

    private void GameFreeGarbageBtnClick()
    {
        is_ExtraBonous = false;
        _gameUi._canvasUi.GameBonous.SetActive(false);
        CommonApi.CallUpdateUserPower("free_garbage", "1", "+");
    }

    private void GameExtendedTimeBtnClick()
    {
        is_ExtraBonous = false;
        _gameUi._canvasUi.GameBonous.SetActive(false);
        HandleEvents.PopoupErrorMsgOpen("Work in progress");
    }

    private void GameBlockExtendedBtnClick()
    {
        is_ExtraBonous = false;
        _gameUi._canvasUi.GameBonous.SetActive(false);
        HandleEvents.PopoupErrorMsgOpen("Work in progress");
    }

    private void BlockExtendedBtnClick()
    {
        HandleEvents.PopoupErrorMsgOpen("Work in progress");
        _gameUi._canvasUi.SelectBonous.SetActive(false);
    }

    private void ExtendedTimeBtnClick()
    {
        HandleEvents.PopoupErrorMsgOpen("Work in progress");
        _gameUi._canvasUi.SelectBonous.SetActive(false);
    }

    private void FreeGarbageBtnClick()
    {
        if (GlobalData.FreeGarbage > 0)
        {
            is_GarbageBonous = true;
            _gameUi._canvasUi.SelectBonous.SetActive(false);
            // GlobalData.FreeGarbage--;
            CommonApi.CallUpdateUserPower("free_garbage", "1", "-");
        }
    }

    private void AnyLetterBtnClick()
    {
        if (GlobalData.AnyLetter > 0 && _TempLastUITiles != null)
        {
            OpenSelectJokerLetter(_TempLastUITiles);
            _gameUi._canvasUi.SelectBonous.SetActive(false);
            // GlobalData.AnyLetter--;
            CommonApi.CallUpdateUserPower("any_letter", "1", "-");
        }
    }

    private void AnyTileBtnClick()
    {
        if (GlobalData.StackTile > 0 && _TempLastUITiles != null)
        {
            GlobalData.is_StackPowerOn = true;
            // HandleEvents.PopoupErrorMsgOpen("Work in progress");
            _gameUi._canvasUi.SelectBonous.SetActive(false);
            Alphabet.data.LetterFromStackPower();
            _gameUi._canvasUi.StackAnyLetterPanel.SetActive(true);
            CommonApi.CallUpdateUserPower("any_tile_from_stack", "1", "-");
        }
    }

    private void ExtraTraySpaceBtnClick()
    {
        if (GlobalData.ExtraTraySpace > 0)
        {
            Extra_Tray_Space_Bonous_Tile.SetActive(true);
            Extra_Tray_Space_Bonous_Slot.SetActive(true);
            Extra_Tray_Space_Bonous_Tile.GetComponent<UITile>().GetNewLetter();
            _gameUi._canvasUi.SelectBonous.SetActive(false);
            // GlobalData.ExtraTraySpace--;
            CommonApi.CallUpdateUserPower("extra_tray_space", "1", "-");
        }
    }

    private void CloseSelectBonousBtnClick()
    {
        _gameUi._canvasUi.GameBonous.SetActive(false);
    }

    private void CloseGameBonousBtnClick()
    {
        _gameUi._canvasUi.SelectBonous.SetActive(false);
    }

    private void StarBtnClick()
    {
        Debug.Log("Powers Loaded");
        UpdatePowerUi();
        _gameUi._canvasUi.SelectBonous.SetActive(true);
    }

    private void GameExitNoBtnClick()
    {
        _gameUi._canvasUi.ExitPanel.SetActive(false);
    }

    private void GameExitYesBtnClick()
    {
        ResetData();
        ResetAllValue();
        ExitPanel.SetActive(false);
        // UI_Manager._instance.OpenHomeScreen();
        //UI_Manager._instance.CloseAllPanel(false);

        foreach (Transform item in Alphabet.data.AlphabetPanel.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (GameObject bTile in _BoardTileParent)
        {
            Destroy(bTile);
        }

        /* foreach (GameObject tile in UITiles)
         {
             Destroy(tile);
         }*/

        SaveData.data.ResumeData.BoardData.Clear();
        SaveData.data.ResumeData.PlayerHandData.Clear();
        SaveData.data.ResumeData.TrayDatas.Clear();
        SaveData.data.ResumeData.score = 0;
        SaveData.data.ResumeData.ThreeLetter = 0;
        SaveData.data.ResumeData.ThreeWord = 0;
        SaveData.data.ResumeData.TrayLetterIndex = 0;
        SaveData.data.ResumeData.TrayLetterNumber = 0;
        SaveData.data.ResumeData.TwoLetter = 0;
        SaveData.data.ResumeData.TwoWord = 0;

        GlobalData.isCheckBtnOn = false;
        _gameUi._canvasUi.CheckBtnPanel.SetActive(false);
        UILettersPanel_p1.SetActive(true);


        if (gameType == GameType.Multiplayer)
        {
            PhotonNetwork.Disconnect();
            _multiplayergameType = MultiplayerGameType.nulls;
            Debug.Log("GameController._multiplayergameType..." + _multiplayergameType);
            _MultiplayerWordData.Clear();
        }

        HandleEvents.ChangeStates(States.home);
    }

    //........GameBonousUi Update................//
    public void UpdatePowerUi()
    {
        _gameUi._textUi.GameAnyTileText.text = GlobalData.AnyLetter.ToString();
        _gameUi._textUi.GameExtraTraySpaceText.text = GlobalData.ExtraTraySpace.ToString();
        _gameUi._textUi.GameStackTileText.text = GlobalData.StackTile.ToString();
        _gameUi._textUi.GameFreeGarbageText.text = GlobalData.FreeGarbage.ToString();

        _gameUi._textUi.SelectAnyLetterText.text = GlobalData.AnyLetter.ToString();
        _gameUi._textUi.SelectExtraTraySpaceText.text = GlobalData.ExtraTraySpace.ToString();
        _gameUi._textUi.SelectStackTileText.text = GlobalData.StackTile.ToString();
        _gameUi._textUi.SelectFreeGarbageText.text = GlobalData.FreeGarbage.ToString();
    }

    //.....................After Resume Game Start..............//
    public void GetResumeData(GameStateData _GetResumeData)
    {
        ResetData();
        GetPlayerHandTileData(_GetResumeData);
        FillTrayData(_GetResumeData);
        FillBoarData(_GetResumeData);
        //UI_Manager._instance.LoadGameScreen();
        HandleEvents.ChangeStates(States.GamePlay);
        FitUIElements();

        if (GlobalData.isCheckBtnOn)
        {
            _gameUi._canvasUi.CheckBtnPanel.SetActive(true);
            UILettersPanel_p1.SetActive(false);
        }

        RemoveListeners();
        AddAllListeners();
        // FillUITiles();
        // SwitchPlayer();
    }

//..........Fill .GetPlayerHand Data After ResumeData.............//
    private void GetPlayerHandTileData(GameStateData _GetResumeData)
    {
        currentPlayer = currentPlayer + 1 <= 4 ? currentPlayer + 1 : 1;
        int j = 0;
        for (int i = 0; i < 1; i++)
        {
            foreach (GameObject slot in players[i].UITileSlots)
            {
                players[i].UITiles.Add(slot.GetComponent<UISlot>().UITile);
            }

            foreach (GameObject tile in players[i].UITiles)
            {
                // tile.GetComponent<UITile>().GetNewLetter();
                tile.name = _GetResumeData.PlayerHandData[j].letter;

                tile.GetComponent<UITile>().letterString.text = _GetResumeData.PlayerHandData[j].letter;
                tile.GetComponent<UITile>().letterScore.text = _GetResumeData.PlayerHandData[j].score.ToString();
                // Debug.Log("GetPlayerHandTileData........" + j);
                tile.GetComponent<UITile>()
                    .CreateNewBoardTile(_GetResumeData.PlayerHandData[j].letter,
                        int.Parse(_GetResumeData.PlayerHandData[j].score.ToString()));
                j++;
                tile.SetActive(true);
            }
        }

        UITileSlots = players[currentPlayer - 1].UITileSlots;
        UITiles = players[currentPlayer - 1].UITiles;

        if (gameType == GameType.SinglePlayer)
        {
            ResetDataUI(_GetResumeData);
        }
    }


    private void ResetDataUI(GameStateData _GetResumeData)
    {
        WordScore_Text.text = "WordScore  " + _GetResumeData.score;
        TwoLetter_Text.text = _GetResumeData.TwoLetter.ToString();
        ThreeLetter_Text.text = _GetResumeData.ThreeLetter.ToString();
        TwoWord_Text.text = _GetResumeData.TwoWord.ToString();
        ThreeWord_Text.text = _GetResumeData.ThreeWord.ToString();
        WordScore = _GetResumeData.score;
        TwoTimeLetter = _GetResumeData.TwoLetter;
        ThreeTimeLetter = _GetResumeData.ThreeLetter;
        TwoTimeWord = _GetResumeData.TwoWord;
        ThreeTimeWord = _GetResumeData.ThreeWord;


        _gameUi._textUi.GameTypeTxt.text = _GetResumeData.GameType;
        _gameUi._textUi.GameModeTxt.text = _GetResumeData.GameMode;
        _gameUi._textUi.TimeDateTxt.text = _GetResumeData.GameDate;
        _gameUi._textUi.GameIdTxt.text = "Game ID : " + _GetResumeData.GameId;

        if (_GetResumeData.TwoLetter > 0)
        {
            bonousChip[0].SetActive(true);
        }

        if (_GetResumeData.ThreeLetter > 0)
        {
            bonousChip[1].SetActive(true);
        }

        if (_GetResumeData.TwoWord > 0)
        {
            bonousChip[2].SetActive(true);
        }

        if (_GetResumeData.ThreeWord > 0)
        {
            bonousChip[3].SetActive(true);
        }
    }


//...............Fill Tray Data After ResumeData..............//
    private void FillTrayData(GameStateData _GetResumeData)
    {
        Alphabet.data.FillTrayResumeData(_GetResumeData);
    }

//...........Fill BoardData After ResumeData......................//
    private void FillBoarData(GameStateData _GetResumeData)
    {
        int i = 0;
        Color color;
        int counter = 0;
        for (int j = 0; j < _GetResumeData.BoardData.Count; j++)
        {
            foreach (GameObject tile in BoardSlots)
            {
                if (_GetResumeData.BoardData.Count > 0)
                    // Debug.Log("_GetResumeData.result.BoardData[i].tilename...."+_GetResumeData.result.BoardData[i].tilename  +"tile.name........."+tile.name );
                    if (tile.name == _GetResumeData.BoardData[j].tilename)
                    {
                        // Debug.Log("_GetResumeData.BoardData[i].tilename....." + _GetResumeData.BoardData[j].tilename);
                        GameObject instandata = (GameObject)Instantiate(boardTilePrefab, new Vector3(99, 0, 0),
                            Quaternion.identity);
                        instandata.tag = "BoardTile";
                        instandata.transform.parent = tile.transform;
                        instandata.transform.localPosition = new Vector3(0, 0, -0.1f);
                        instandata.name = _GetResumeData.BoardData[j].letter;
                        GameController.data.BoardTiles.Add(instandata);
                        ColorUtility.TryParseHtmlString("#FAFD00", out color);
                        instandata.GetComponent<SpriteRenderer>().color = color;
                        instandata.GetComponent<BoardTile>().completed = true;
                        instandata.GetComponent<BoardTile>().currentslot =
                            instandata.transform.parent.GetComponent<BoardSlot>();
                        instandata.transform.parent.GetComponent<BoardSlot>().free = false;
                        tile.transform.GetComponent<BoardSlot>().completed = false;
                        //PreApply(true);
                        PreApply(true);
                        instandata.GetComponent<BoardTile>().UIclone = gameObject;
                        instandata.GetComponent<BoardTile>().letter = _GetResumeData.BoardData[j].letter;
                        instandata.GetComponent<BoardTile>().score = int.Parse(_GetResumeData.BoardData[j].score.ToString());
                        TextMesh[] txts = instandata.GetComponentsInChildren<TextMesh>();
                        txts[0].text = _GetResumeData.BoardData[j].letter;
                        txts[1].text = _GetResumeData.BoardData[j].score.ToString();
                        txts[2].text = _GetResumeData.BoardData[j].bonousname;
                        instandata.transform.parent.GetComponent<BoardSlot>().completed = true;
                        instandata.transform.parent.GetComponent<BoardSlot>().free = false;
                        instandata.SetActive(true);
                        i++;
                        counter++;
                        break;
                    }
            }
        }

        _gameUi._textUi.AquireTileTxt.text = counter.ToString() + "/ 121";
    }

    void FitUIElements()
    {
        canvasWidth = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>().rect.width;
        float canvasHeight = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>().rect.height * 1020;
        float slotSize = canvasWidth / 11.0f;

        float ratio = (float)Screen.width / Screen.height;
        // ButtonsPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1.0f - (ratio+0.01f));
        //menuPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasWidth, canvasHeight);
        //menuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-canvasWidth, 0);
        //menuPanel.SetActive(true);

        for (int i = 0; i < 1; i++)
        {
            float dx = -3.9f * slotSize;
            foreach (GameObject slot in players[i].UITileSlots)
            {
                // Debug.Log(players[i].name);
                RectTransform slotRT = slot.GetComponent<RectTransform>();
                //slotRT.anchoredPosition = new Vector2(dx, -115);
                dx += (slotSize + 32);
                slotRT.sizeDelta = new Vector2(slotSize, slotSize);
                slot.GetComponent<UISlot>().UITile.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(slotSize - 2, slotSize - 2);
                slot.GetComponent<UISlot>().UITile.GetComponent<BoxCollider2D>().size =
                    new Vector2(slotSize - 2, slotSize - 2);
                slot.GetComponent<UISlot>().UITile.GetComponent<RectTransform>().anchoredPosition =
                    slot.GetComponent<RectTransform>().anchoredPosition;
                slot.GetComponent<UISlot>().UITile.GetComponent<UITile>().lastPosition =
                    slot.GetComponent<RectTransform>().anchoredPosition;
            }

            players[i].UILettersPanel.SetActive(true);
            // players[i].UILettersPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1.0f - ratio);
        }
    }

    void FillUITiles()
    {
        for (int i = 0; i < 1; i++)
        {
            foreach (GameObject slot in players[0].UITileSlots)
            {
                players[0].UITiles.Add(slot.GetComponent<UISlot>().UITile);
            }

            foreach (GameObject tile in players[0].UITiles)
            {
                tile.SetActive(true);
                tile.GetComponent<UITile>().GetNewLetter();
                // tile.name= GetComponent<UITile>().letterString.text;
            }
        }

        Alphabet.data.DateWiseLetterScore();
    }

    void Update()
    {
        if (Input.GetKey("escape")) Application.Quit();
        //if (Input.GetKey(KeyCode.R))
        //  SceneManager.LoadScene(0);
        if (paused) return;
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) uiTouched = true;
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) uiTouched = true;
        }
#endif
        if (Input.GetMouseButtonUp(0)) uiTouched = false;

        if (letterDragging)
        {
            if (newScoreBlock.activeInHierarchy) newScoreBlock.SetActive(false);
            CheckifPointerOverPanel();
            if (pointerOverPanel)
            {
                canBePlant = false;
                return;
            }

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
                Mathf.Infinity, 1 << 9);
            if (hit.collider != null)
            {
                is_Move = true;
                targetBoardSlot = hit.collider.gameObject;
                // Debug.Log("targetBoardSlot.................."+targetBoardSlot);
                ScaleTile();
                if (targetBoardSlot.GetComponent<BoardSlot>().free)
                {
                    canBePlant = true;
                }

                else
                {
                    canBePlant = false;
                }
            }
            else
            {
                canBePlant = false;
            }
        }
    }

    private void ScaleTile()
    {
        foreach (GameObject go in BoardSlots)
        {
            if (go == targetBoardSlot && targetBoardSlot.GetComponent<BoardSlot>().free != false)
            {
                // Debug.Log(go.transform.name);
                go.transform.localScale = new Vector3(1.1f, 1.1f, 1.2f);
            }
            else
            {
                go.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }


    public void CheckifPointerOverPanel()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);
        pointerOverPanel = false;

        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult res in raycastResults)
            {
                if (res.gameObject.tag == "UIPanel") pointerOverPanel = true;
            }
        }
    }

    public void PlantTileBoard(GameObject tile)
    {
        Debug.Log("PlantTile.............................." + tile.transform.parent);
        Debug.Log("targetBoardSlot.................." + targetBoardSlot);
        //tile.transform.parent = targetBoardSlot.transform;

        Debug.Log(" Afteer  PlantTile.............................." + tile.transform.parent);
        tile.transform.localPosition = new Vector3(0, 0, -0.1f);
        tile.SetActive(true);
        tile.GetComponent<BoardTile>().currentslot = targetBoardSlot.GetComponent<BoardSlot>();
        targetBoardSlot.GetComponent<BoardSlot>().free = false;
        targetBoardSlot.GetComponent<BoardSlot>().vacant = false;

        // Camera.main.BroadcastMessage("ZoomIn", targetBoardSlot.transform.position);
        PreApply();
        SoundController.data.playTap();
    }

    public void PlantTile(GameObject tile)
    {
        Debug.Log("undo.........." + undo);
        foreach (GameObject item in _BoardTileParent)
        {
            // Debug.Log("............." + item);
        }

        // Debug.Log("targetBoardSlot.................."+targetBoardSlot);

        // Debug.Log("PlantTile.............................."+ tile.transform.parent.name);
        tile.transform.parent = targetBoardSlot.transform;
        targetBoardSlot.transform.localScale = new Vector3(1f, 1f, 1f);
        //  Debug.Log(" Afteer  PlantTile.............................."+ tile.transform.parent);
        tile.transform.localPosition = new Vector3(0, 0, -0.1f);
        tile.transform.localScale = new Vector3(1, 1, 1);
        tile.SetActive(true);
        tile.GetComponent<BoardTile>().currentslot = targetBoardSlot.GetComponent<BoardSlot>();
        targetBoardSlot.GetComponent<BoardSlot>().free = false;
        // Camera.main.BroadcastMessage("ZoomIn", targetBoardSlot.transform.position);
        canBePlant = false;
        TotalboardTiles.Add(tile);
        Debug.Log("GetInstanceID.........." + tile.GetInstanceID());


        if (tile.GetComponent<BoardTile>().Bonous.text == " ")
        {
            PreApply();
        }

        if (Alphabet.data.TossReplaceLetter != null)
            if (Alphabet.data.TossReplaceLetter.name == tile.name)
            {
                Debug.Log("hhhhhhhhhhhhhhhhhhh");
                Alphabet.data.TossReplaceLetter = null;
            }

        SoundController.data.playTap();
    }

    public GameObject GetFreeUISlot()
    {
        foreach (GameObject slot in UITileSlots)
        {
            if (slot.GetComponent<UISlot>().UITile == null) return slot;
        }

        return null;
    }

//....GetLast Tile..............//
    public GameObject GetLastFreeUISlot()
    {
        if (LastUISlot.GetComponent<UISlot>().UITile == null)
        {
            // Debug.Log("LastUISlot......"+LastUISlot.name);
            return LastUISlot;
        }

        return null;
    }

//........LastUISlot.........//


    public void SelectAnyLetterTile(GameObject _LastUITiles)
    {
        _TempLastUITiles = _LastUITiles;
    }

    public void GetLastUISlot(GameObject _LastUISlot, GameObject _LastUITiles)
    {
        int flag = 0;


        foreach (GameObject tile in LastBoardTiles)
        {
            if (tile.GetInstanceID() == _LastUITiles.GetInstanceID())
            {
                if (undo != 0)
                {
                    flag = 1;
                    Debug.Log("--------" + undo);
                }
            }

            // Debug.Log(",,,,,,,,,,,,,,,"+undo);
        }

        if (flag == 0)
        {
            undo++;
            undoBoardTile++;
            LastUISlot = _LastUISlot;
            LastUITiles.Add(_LastUITiles);
            LastBoardTiles.Add(_LastUITiles);

            Debug.Log(".........................undo..." + undo);
            Debug.Log(".........................undoBoardTile..." + undoBoardTile);
        }

        PreApply();
    }

//............Undo All Method..................//
    public void CancelLetters()
    {
        for (int i = undo; i >= 0; i--)
        {
            if (undo > -1)
            {
                Debug.Log("LastUITiles[undo].........." + LastUITiles[undo].name);
                if (LastUITiles[undo].activeInHierarchy)
                {
                    // if (LastUITiles[undo] !=null  )
                    // Debug.Log(" LastUITiles[undo].activeInHierarchy...."+LastUITiles[undo].activeInHierarchy);

                    if (IntUndoLetter > -1 && IntTrayLetter > -1)
                    {
                        Alphabet.data.UndotrayLetter(LastUITiles[undo], UndoLetter[IntUndoLetter],
                            TrayLetter[IntTrayLetter], UndoScore[IntUndoLetter]);
                        // WordScore = WordScore + int.Parse(LastUITiles[undo].GetComponent<UITile>().letterScore.text);


                        UndoLetter.RemoveAt(IntUndoLetter);
                        UndoScore.RemoveAt(IntUndoLetter);
                        TrayLetter.RemoveAt(IntTrayLetter);
                        IntUndoLetter--;
                        IntTrayLetter--;
                        // Debug.Log("WordScore......................."+WordScore + " IntUndoLetter"+IntUndoLetter);
                    }

                    // }  
                }

                /// Debug.Log("WordScore......................."+WordScore + " IntUndoLetter"+IntUndoLetter);

                if (!LastUITiles[undo].activeInHierarchy && !LastUITiles[undo].GetComponent<UITile>().finished)
                {
                    //Debug.Log("WordScore......................."+WordScore + " IntUndoLetter"+IntUndoLetter);

                    if (LastUITiles[undo].GetComponent<UITile>().BonusName != string.Empty)
                    {
                        // Debug.Log("WordScore......................."+WordScore + " IntUndoLetter"+IntUndoLetter);

                        BonousReduce(LastUITiles[undo].GetComponent<UITile>().BonusName, LastUITiles[undo]);
                        // LastUITiles[undo].GetComponent<UITile>().BonusName = string.Empty;
                    }
                    else
                    {
                        // Debug.Log("WordScore......................."+WordScore + " IntUndoLetter"+IntUndoLetter);

                        LastUITiles[undo].SetActive(true);
                        LastUITiles[undo].GetComponent<UITile>().CancelTile();

                        LastUITiles[undo].GetComponent<UITile>().Hilighter.SetActive(false);

                        TurnScore = TurnScore - int.Parse(LastUITiles[undo].GetComponent<UITile>().letterScore.text);
                        TurnScore_Text.text = "Turn Score   " + TurnScore.ToString();
                        Debug.Log("undoBoardTile.........." + undoBoardTile);
                        if (undoBoardTile > -1)
                        {
                            LastBoardTiles.RemoveAt(undoBoardTile);

                            undoBoardTile--;
                        }
                    }
                }
            }

            LastUITiles.RemoveAt(undo);
            undo--;
            Debug.Log("undo.........." + undo);
        }

        TurnScore_Text.text = "Turn Score     " + 0.ToString();
        GameController.data.PreApply();
        foreach (GameObject tile in UITiles)
        {
            tile.GetComponent<UITile>().BonusName = string.Empty;
            tile.GetComponent<UITile>().Hilighter.SetActive(false);
        }

        SelectedBoarTile = null;
        undo = -1;
        LastUITiles.Clear();
        isthreeBonous = false;
        istwoBonous = false;
        isthreeLetterBonous = false;
        istwoLetterBonous = false;
        is_temp = true;
        IntUndoLetter = -1;
        IntTrayLetter = -1;
        isCheck = false;
        LastBoardTiles.Clear();
        undoBoardTile = -1;
        previousCurrentScore = 0;
        isTileOnBoard = false;

        for (int i = 0; i < WordText.Length; i++)
        {
            WordText[i].text = " ";
            Scoretext[i].text = " ";
            WordText[i].gameObject.SetActive(false);
            Scoretext[i].gameObject.SetActive(false);
        }
    }

//.............Undo  Method................//
    public void Undo()
    {
        //foreach (GameObject tile in UITiles)
        //   if (Alphabet.data.TossReplaceLetter !=null)
        //   {
        //     Alphabet.data.UndotrayLetter(Alphabet.data.TossReplaceLetter);
        //     Alphabet.data.TossReplaceLetter=null;
        //   }
        //   else{

        if (undo > -1)
        {
            if (LastUITiles[undo].activeInHierarchy)
            {
                if (LastUITiles[undo] != null)
                    //  if(LastUITiles[undo].GetComponent<UITile>().TrayLetter !=" ")
                    //    {

                    // Debug.Log(""+LastUITiles[undo]); 
                    //  Debug.Log(""+IntUndoLetter); 
                    //   Debug.Log(""+TrayLetter[IntUndoLetter]); 
                    if (IntUndoLetter > -1 && IntTrayLetter > -1)
                    {
                        Alphabet.data.UndotrayLetter(LastUITiles[undo], UndoLetter[IntUndoLetter],
                            TrayLetter[IntTrayLetter], UndoScore[IntUndoLetter]);
                        // WordScore = WordScore + int.Parse(LastUITiles[undo].GetComponent<UITile>().letterScore.text);
                        previousCurrentScore = previousCurrentScore + int.Parse(LastUITiles[undo].GetComponent<UITile>().letterScore.text);
                        Debug.Log("PREVIOUSCURRENT SCORE ... " + previousCurrentScore);
                        TurnScore_Text.text = "TurnScore     " + previousCurrentScore.ToString();
                        PreApply();
                        UndoLetter.RemoveAt(IntUndoLetter);
                        UndoScore.RemoveAt(IntUndoLetter);
                        TrayLetter.RemoveAt(IntTrayLetter);
                        IntUndoLetter--;
                        IntTrayLetter--;
                        // Debug.Log("WordScore......................."+WordScore + " IntUndoLetter"+IntUndoLetter);
                    }
            }


            if (!LastUITiles[undo].activeInHierarchy && !LastUITiles[undo].GetComponent<UITile>().finished)
            {
                if (LastUITiles[undo].GetComponent<UITile>().BonusName != string.Empty)
                {
                    BonousReduce(LastUITiles[undo].GetComponent<UITile>().BonusName, LastUITiles[undo]);
                    // LastUITiles[undo].GetComponent<UITile>().BonusName = " ";
                }
                else
                {
                    LastUITiles[undo].SetActive(true);
                    LastUITiles[undo].GetComponent<UITile>().UndoMove();


                    TurnScore = TurnScore - int.Parse(LastUITiles[undo].GetComponent<UITile>().letterScore.text);
                    TurnScore_Text.text = "Turn Score    " + TurnScore.ToString();
                    if (undoBoardTile > -1)
                    {
                        LastBoardTiles.RemoveAt(undoBoardTile);
                        Debug.Log("undoBoardTile.........." + undoBoardTile);
                        undoBoardTile--;
                    }
                }
            }

            LastUITiles.RemoveAt(undo);
            undo--;
            if (undo == 0)
            {
                is_temp = false;
                TurnScore = 0;
                previousCurrentScore = 0;
                TurnScore_Text.text = "Turn Score " + 0.ToString();
                isthreeBonous = false;
                istwoBonous = false;
                isthreeLetterBonous = false;
                istwoLetterBonous = false;
                is_temp = true;
                for (int i = 0; i < WordText.Length; i++)
                {
                    WordText[i].text = string.Empty;
                    Scoretext[i].text = string.Empty;
                    WordText[i].gameObject.SetActive(false);
                    Scoretext[i].gameObject.SetActive(false);
                }

                //Debug.Log("isthreeBonous...."+isthreeBonous+ "istwoBonous ......."+istwoBonous);
            }
        }

        // Debug.Log("undo..."+undo);
        if (undo == -1)
        {
            undo = -1;
            IntUndoLetter = -1;
            IntTrayLetter = -1;
            WordScore = undoAllScore;
            WordScore_Text.text = "Word Score     " + WordScore;
            TurnScore = 0;
            TurnScore_Text.text = "Turn Score     " + 0.ToString();
            previousCurrentScore = 0;
            for (int i = 0; i < WordText.Length; i++)
            {
                WordText[i].text = string.Empty;
                Scoretext[i].text = string.Empty;
                WordText[i].gameObject.SetActive(false);
                Scoretext[i].gameObject.SetActive(false);
            }
        }

        Debug.Log(" undo()...." + undo);

        PreApply();
        //
        SelectedBoarTile = null;

        // }
    }

    public void ShuffleUITiles()

    {
        foreach (GameObject slot in UITileSlots)
        {
            slot.GetComponent<UISlot>().UITile = null;
        }

        for (int i = 0; i < UITiles.Count; i++)
        {
            if (UITiles[i] != null)
            {
                GameObject tempObj = UITiles[i];
                int randomIndex = UnityEngine.Random.Range(i, UITiles.Count);
                UITiles[i] = UITiles[randomIndex];
                UITiles[randomIndex] = tempObj;
            }
        }

        for (int i = 0; i < UITiles.Count; i++)
        {
            UITiles[i].GetComponent<UITile>().GoToSlot(UITileSlots[i]);
        }
    }

    public void OpenSelectJokerLetter(GameObject jt)
    {
        paused = true;
        tempJokerTile = jt;
        //  undo++;
        //  LastUITiles.Add(tempJokerTile);
        _gameUi._textUi.AnyLetterSelectedText.text = tempJokerTile.GetComponent<UITile>().letterString.text;


        SelectJokerLetter.SetActive(true);
    }

    public void ApplyJokerTile(string letter)
    {
        Debug.Log(" tempJokerTile..........." + tempJokerTile);
        tempJokerTile.GetComponent<UITile>().letterString.text = letter;
        tempJokerTile.GetComponentInChildren<Text>().text = letter;
        // PlantTile(tempJokerTile);
        SelectJokerLetter.SetActive(false);
        //   undo++;
        // LastUITiles.Add(tempJokerTile);
        paused = false;
    }

    //......SelectedBoardTile...................//
    public void SelectedTiel(GameObject _tile)
    {
        SelectedBoarTile = _tile;
        for (int i = 0; i < BoardSlots.Count; i++)
        {
            if (SelectedBoarTile != null)
                if (BoardSlots[i].name == SelectedBoarTile.transform.parent.name)
                {
                    // Debug.Log(" i......."+i);
                    BonousTileId = i;
                }
        }

        // SelectedBoarTile.GetComponent<SpriteRenderer>().color = new Color (1f, 0.5f, 0.5f); 
    }
//.......Afterturn..............two w three score cal calculate..................//

//......Get Two Time Letter............//
    public void GetTwoTimeLetter()
    {
        Color color;
        int score = 0;
        Debug.Log("SelectedBoarTile...." + SelectedBoarTile);
        if (TwoTimeLetter > 0 && SelectedBoarTile != null)
        {
            isCheck = true;
            is_recalculateCheck = true;

            istwoLetterBonous = true;
            isthreeLetterBonous = false;
            int i = 0;
            //Alphabet.data.ReplaceLetter.transform.Find("Image").GetChild(0).GetComponent<Text>().text ="2xL";

            //Alphabet.data.ReplaceLetter.GetComponent<UITile>().BonusName="2xL";
            //SelectedBoarTile.GetComponent<BoardTile>().BonusName="2xL";
            foreach (GameObject tile in UITiles)
            {
                //Debug.Log("SelectedBoarTile.name..." + SelectedBoarTile.name + "tile.name... " + tile.name +
                // "tile.GetComponent<UITile>().BonusName....." + tile.GetComponent<UITile>().BonusName);
                // Debug.Log("SelectedBoarTile...." + SelectedBoarTile.GetInstanceID() +"  " + tile.GetInstanceID());
                if (SelectedBoarTile.GetComponent<BoardTile>().UIclone == tile && SelectedBoarTile.GetComponent<BoardTile>().Bonous.text == string.Empty && tile.GetComponent<UITile>().BonusName == string.Empty)
                {
                    if (i == 0)
                    {
                        Debug.Log("Enter");
                        ColorUtility.TryParseHtmlString("#DA8B03", out color);
                        SelectedBoarTile.GetComponent<SpriteRenderer>().color = color; //Red color DA8B03
                        SelectedBoarTile.GetComponent<BoardTile>().Bonous.text = "2xL";
                        SelectedBoarTile.SetActive(true);
                        TwoTimeLetter--;
                        PreviousScore = TurnScore;
                        TwoLetter_Text.text = TwoTimeLetter.ToString();
                        tile.GetComponent<UITile>().LastScore = WordScore;
                        Debug.Log(" Turn TwoTimeLetter ......" + TwoTimeLetter);

                        TurnScore = TurnScore - SelectedBoarTile.GetComponent<BoardTile>().score;
                        //Debug.Log(" Turn Score ......"+TurnScore);
                        //int.Parse( Alphabet.data.ReplaceLetter.GetComponent<UITile>().letterScore.text);
                        Two_lscore = 2 * SelectedBoarTile.GetComponent<BoardTile>().score;
                        //Debug.Log(" Turn Score ......"+TurnScore);

                        TurnScore = Two_lscore + TurnScore;
                        //Debug.Log(" Turn Score ......"+TurnScore);

                        if (isthreeBonous == false && istwoBonous == false)
                        {
                            TempScore = TurnScore;
                        }

                        // Debug.Log(" Turn Score ......"+TurnScore);
                        TurnScore_Text.text = "Turn Score  " + TurnScore.ToString();
                        SelectedBoarTile.GetComponent<BoardTile>().score = Two_lscore;
                        //TextMesh[] txts = SelectedBoarTile.GetComponentsInChildren<TextMesh>();

                        // Debug.Log("tile.GetComponent<UITile>().BonusName................"+tile.name);
                        tile.GetComponent<UITile>().BonusName = "2xL";
                        tile.GetComponent<UITile>().LastScore = Two_lscore;
                        undo++;
                        Debug.Log(".........................undo..." + undo);

                        LastUITiles.Add(tile);
                        SelectedBoarTile.transform.localScale = new Vector3(1f, 1f, 1f);
                        i++;
                    }
                }
            }
        }

        if (TwoTimeLetter <= 0)
        {
            bonousChip[0].SetActive(false);
        }

        //PreApply();
    }

//.....Get Three Time Letter...........//
    public void GetThreeTimeLetter()
    {
        int score = 0;
        Color color;
        Debug.Log(" ThreeTimeLetter......" + ThreeTimeLetter + " " + SelectedBoarTile);
        int i = 0;
        if (ThreeTimeLetter > 0 && SelectedBoarTile != null)
        {
            isCheck = true;
            is_recalculateCheck = true;
            isthreeLetterBonous = true;
            istwoLetterBonous = false;

            // Alphabet.data.ReplaceLetter.transform.Find("Image").GetChild(0).GetComponent<Text>().text ="3xL";

            // Alphabet.data.ReplaceLetter.GetComponent<UITile>().BonusName="3xL";
            foreach (GameObject tile in UITiles)
            {
                ///  Debug.Log("SelectedBoarTile.name..." + SelectedBoarTile.name + "tile.name... " + tile.name +
                //  "tile.GetComponent<UITile>().BonusName....." + tile.GetComponent<UITile>().BonusName);

                if (SelectedBoarTile.GetComponent<BoardTile>().UIclone == tile && SelectedBoarTile.GetComponent<BoardTile>().Bonous.text == string.Empty && tile.GetComponent<UITile>().BonusName == string.Empty)
                {
                    if (i == 0)
                    {
                        ThreeTimeLetter--;

                        ColorUtility.TryParseHtmlString("#EA1616", out color);
                        SelectedBoarTile.GetComponent<SpriteRenderer>().color = color; //new Color (0.5f, 0.5f, 1f); // Blue   EA1616
                        SelectedBoarTile.GetComponent<BoardTile>().Bonous.text = "3xL";
                        SelectedBoarTile.SetActive(true);
                        PreviousScore = WordScore;
                        ThreeLetter_Text.text = ThreeTimeLetter.ToString();

                        tile.GetComponent<UITile>().LastScore = WordScore;
                        TurnScore = TurnScore - SelectedBoarTile.GetComponent<BoardTile>().score;
                        Three_lscore = 3 * SelectedBoarTile.GetComponent<BoardTile>().score;
                        TurnScore = TurnScore + Three_lscore;
                        if (isthreeBonous == false && istwoBonous == false)
                        {
                            TempScore = TurnScore;
                        }

                        TurnScore_Text.text = "Turn Score  " + TurnScore.ToString();
                        tile.GetComponent<UITile>().BonusName = "3xL";
                        tile.GetComponent<UITile>().LastScore = Three_lscore;
                        SelectedBoarTile.GetComponent<BoardTile>().score = Three_lscore;

                        Debug.Log(" tile.GetComponent<UITile>().BonusName ......" + tile.GetComponent<UITile>().BonusName);
                        // TextMesh[] txts = SelectedBoarTile.GetComponentsInChildren<TextMesh>();
                        // txts[1].text = Three_lscore.ToString();
                        undo++;
                        SelectedBoarTile.transform.localScale = new Vector3(1f, 1f, 1f);
                        Debug.Log(".........................undo..." + undo);
                        LastUITiles.Add(tile);
                        i++;
                    }
                }
            }
        }

        // PreApply();
        if (ThreeTimeLetter <= 0)
        {
            bonousChip[1].SetActive(false);
        }
    }

//......Get Two Time Word.............//
    public void GetTwoTimeWord()
    {
        Color color;
        int score = 0;
        int letterscore = 0;
        int j = 0;

        if (TwoTimeWord > 0 && SelectedBoarTile != null)
        {
            isCheck = true;
            istwoBonous = true;
            is_BonousRecalculate = false;
            is_recalculateCheck = true;
            isthreeLetterBonous = false;
            istwoLetterBonous = false;
            is_VerticalTwoBonous = true;
            // Alphabet.data.ReplaceLetter.GetComponent<UITile>().BonusName="2xW";
            //SelectedBoarTile.GetComponent<BoardTile>().BonusName = "2xW";
            foreach (GameObject tile in UITiles)
            {
                Debug.LogFormat("tile {0}bonoustext {1} bonousname {2} name {3}", SelectedBoarTile.GetComponent<BoardTile>().UIclone == tile, SelectedBoarTile.GetComponent<BoardTile>().Bonous.text == string.Empty, tile.GetComponent<UITile>().BonusName == string.Empty, tile.GetComponent<UITile>().BonusName);
                if (SelectedBoarTile.GetComponent<BoardTile>().UIclone == tile && SelectedBoarTile.GetComponent<BoardTile>().Bonous.text == string.Empty && tile.GetComponent<UITile>().BonusName == string.Empty)
                {
                    if (j == 0)
                    {
                        // Alphabet.data.ReplaceLetter.transform.Find("Image").GetChild(0).GetComponent<Text>().text ="2xW";
                        ColorUtility.TryParseHtmlString("#7C43C9", out color);

                        SelectedBoarTile.GetComponent<SpriteRenderer>().color =
                            color; //new Color (0.5f, 1f, 0.5f); // Green;  7C43C9
                        SelectedBoarTile.GetComponent<BoardTile>().Bonous.text = "2xW";
                        SelectedBoarTile.GetComponent<BoardTile>().BonusName = "2xW";
                        SelectedBoarTile.SetActive(true);
                        TwoTimeWord--;
                        Debug.Log(" Turn TwoTimeWord ......" + TwoTimeWord);
                        Debug.Log("GetInstanceID.........." + tile.GetInstanceID());
                        PreviousScore = WordScore;
                        TwoWord_Text.text = TwoTimeWord.ToString();

                        // Debug.Log("TempScore.........."+TempScore);

                        // TurnScore_Text.text = "Turn Score  " + TurnScore.ToString();
                        tile.GetComponent<UITile>().BonusName = "2xW";
                        tile.GetComponent<UITile>().LastScore = score;
                        //SelectedBoarTile.GetComponent<BoardTile>().score=score;

                        undo++;
                        Debug.Log(".........................undo..." + undo + "Tile...." + tile.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().GetInstanceID());
                        LastUITiles.Add(tile);
                        SelectedBoarTile.transform.localScale = new Vector3(1f, 1f, 1f);
                        j++;
                    }
                }
            }

            for (int i = 0; i < BoardSlots.Count; i++)
            {
                if (BoardSlots[i].name == SelectedBoarTile.transform.parent.name)
                {
                    // Debug.Log(" i......."+i);
                    TwoTileId = i;
                }
            }

            if (TwoTimeWord <= 0)
            {
                bonousChip[2].SetActive(false);
            }
        }
    }

//......Get Three Time Word...........//
    public void GetThreeTimeWord()
    {
        Color color;
        int score = 0;
        int letterscore = 0;
        int j = 0;

        if (ThreeTimeWord > 0 && SelectedBoarTile != null)
        {
            isCheck = true;
            isthreeBonous = true;
            is_BonousRecalculate = false;
            is_recalculateCheck = true;
            is_VerticalThreeBonous = true;
            //Alphabet.data.ReplaceLetter.GetComponent<UITile>().BonusName="3xW";
            foreach (GameObject tile in UITiles)
            {
                //Debug.Log("tile.GetComponent<UITile>().BonusName....." +tile.GetComponent<UITile>().BonusName + "SelectedBoarTile.name.."+SelectedBoarTile.name +",,,,,"+tile.name);

                if (SelectedBoarTile.GetComponent<BoardTile>().UIclone == tile && SelectedBoarTile.GetComponent<BoardTile>().Bonous.text == string.Empty && tile.GetComponent<UITile>().BonusName == string.Empty)
                {
                    if (j == 0)
                    {
                        ColorUtility.TryParseHtmlString("#2D5FCF", out color);

                        // Alphabet.data.ReplaceLetter.transform.Find("Image").GetChild(0).GetComponent<Text>().text ="3xW";
                        SelectedBoarTile.GetComponent<SpriteRenderer>().color =
                            color; //new Color(0.1f,0.5f,0.2f);//Light Gray 2D5FCF
                        SelectedBoarTile.GetComponent<BoardTile>().Bonous.text = "3xW";
                        SelectedBoarTile.GetComponent<BoardTile>().BonusName = "3xW";
                        SelectedBoarTile.SetActive(true);
                        ThreeTimeWord--;
                        Debug.Log(" Turn ThreeTimeWord ......" + ThreeTimeWord);

                        PreviousScore = WordScore;
                        ThreeWord_Text.text = ThreeTimeWord.ToString();

                        // Debug.Log("Word sceore..."+TempScore);

                        //  }

                        // Debug.Log("Word sceore..."+TurnScore);
                        TurnScore_Text.text = "Turn Score  " + TurnScore.ToString();
                        // Debug.Log("tile.GetComponent<UITile>().BonusName....." +tile.GetComponent<UITile>().BonusName);
                        tile.GetComponent<UITile>().BonusName = "3xW";

                        is_temp = false;

                        undo++;
                        SelectedBoarTile.transform.localScale = new Vector3(1f, 1f, 1f);

                        LastUITiles.Add(tile);
                        Debug.Log(".........................undo..." + undo);
                        j++;
                    }
                }
            }

            for (int i = 0; i < BoardSlots.Count; i++)
            {
                if (BoardSlots[i].name == SelectedBoarTile.transform.parent.name)
                {
                    // Debug.Log(" i......."+i);
                    ThreeTileId = i;
                }
            }

            if (ThreeTimeWord <= 0)
            {
                bonousChip[3].SetActive(false);
            }

//PreApply();
        }
    }

//......Apply Bonous.....................//
    public void BonousApply(string str)
    {
        Debug.Log("STR..........." + str);
        switch (str)
        {
            case "3xW":

                GetThreeTimeWord();
                PreApply();
                break;

            case "2xW":
                GetTwoTimeWord();
                PreApply();
                break;

            case "3xL":
                GetThreeTimeLetter();
                PreApply();
                break;

            case "2xL":
                GetTwoTimeLetter();
                PreApply();
                break;

            default:
                Debug.Log("Bonous name is empty");
                break;
        }
    }

//........Bonous Reduce.......//
    public void BonousReduce(string str, GameObject obj)
    {
        // Debug.Log("str..............."+str+ " obj.name.........."+obj.name);
        switch (str)
        {
            case "3xW":
                Debug.Log("ThreeTimeWord..............." + ThreeTimeWord);
                if (ThreeTimeWord >= 0)
                {
                    ThreeTileId = -1;
                    isthreeLetterBonous = true;
                    istwoLetterBonous = true;
                    Color color;

                    ThreeTimeWord++;
                    ThreeWord_Text.text = ThreeTimeWord.ToString();
                    Debug.Log("Word sceore..." + WordScore);
                    //  Debug.Log("int.Parse(obj.GetComponent<UITile>().letterScore.text)..."+int.Parse(obj.GetComponent<UITile>().letterScore.text));
                    //  Debug.Log("obj.GetComponent<UITile>().LastScore..."+obj.GetComponent<UITile>().LastScore);

                    //TurnScore_Text.text ="Turn Score  "+ (TurnScore-obj.GetComponent<UITile>().LastScore).ToString();
                    //TurnScore=TurnScore-obj.GetComponent<UITile>().LastScore;
                    TurnScore = TurnScore - 3 * TempScore;
                    TurnScore_Text.text = "Turn Score  " + TurnScore;
                    bonousChip[3].SetActive(true);

                    obj.GetComponent<UITile>().BonusName = string.Empty;
                    obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().Bonous.text = string.Empty;
                    obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().BonusName = string.Empty;
                    ColorUtility.TryParseHtmlString("#63BC00", out color);
                    obj.GetComponent<UITile>().boardTile.GetComponent<SpriteRenderer>().color = color;

                    obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().score = int.Parse(obj.GetComponent<UITile>().letterScore.text);
                    // foreach(GameObject bTile in GameController.data.boardTilesMatters)
                    //       {
                    //            // bTile.transform.localScale = new Vector3(1f,1f,1f);
                    //       }  
                    //de
                }

                break;

            case "2xW":
                Debug.Log("TwoTimeWord..............." + TwoTimeWord);
                if (TwoTimeWord >= 0)
                {
                    TwoTileId = -1;
                    isthreeLetterBonous = true;
                    istwoLetterBonous = true;
                    Color color;

                    TwoTimeWord++;
                    TwoWord_Text.text = TwoTimeWord.ToString();
                    //WordScore_Text.text ="Score  "+( WordScore - obj.GetComponent<UITile>().LastScore)+undoAllScore.ToString();
                    // Debug.Log("int.Parse(obj.GetComponent<UITile>().letterScore.text)..."+int.Parse(obj.GetComponent<UITile>().letterScore.text));
                    //Debug.Log("obj.GetComponent<UITile>().LastScore..."+obj.GetComponent<UITile>().LastScore);

                    // TurnScore_Text.text ="Turn Score  "+ (TurnScore-obj.GetComponent<UITile>().LastScore).ToString(); 
                    //TurnScore=TurnScore-obj.GetComponent<UITile>().LastScore;
                    /*  TurnScore = TurnScore - 2 * TempScore;
                       TurnScore_Text.text = "Turn Score  " + TurnScore;*/
                    bonousChip[2].SetActive(true);

                    obj.GetComponent<UITile>().BonusName = string.Empty;

                    obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().Bonous.text = string.Empty;
                    obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().BonusName = string.Empty;


                    ColorUtility.TryParseHtmlString("#63BC40", out color);
                    obj.GetComponent<UITile>().boardTile.GetComponent<SpriteRenderer>().color = color;
                    obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().score =
                        int.Parse(obj.GetComponent<UITile>().letterScore.text);
                    //  foreach(GameObject bTile in GameController.data.boardTilesMatters)
                    //   {
                    //         bTile.transform.localScale = new Vector3(1f,1f,1f);
                    //   }
                    Debug.Log("Bonous check .........." + obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().GetInstanceID() + "name ..." + obj.name);

                    Debug.Log("Bonous check .........." + obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().BonusName);
                }

                break;

            case "3xL":
                Debug.Log("ThreeTimeLetter..............." + ThreeTimeLetter);

                if (ThreeTimeLetter >= 0)
                {
                    Debug.Log("TurnScore.........." + TurnScore);
                    Color color;
                    ThreeTimeLetter++;
                    isthreeLetterBonous = true;
                    istwoLetterBonous = true;
                    ThreeLetter_Text.text = ThreeTimeLetter.ToString();
                    //WordScore_Text.text ="Score  "+( WordScore - obj.GetComponent<UITile>().LastScore)+undoAllScore.ToString(
                    /*   TurnScore_Text.text = "Turn Score  " +
                                             (TurnScore - (obj.GetComponent<UITile>().LastScore) +
                                              int.Parse(obj.GetComponent<UITile>().letterScore.text)).ToString();
                       TurnScore = (TurnScore - (obj.GetComponent<UITile>().LastScore) +
                                    int.Parse(obj.GetComponent<UITile>().letterScore.text));*/
                    bonousChip[1].SetActive(true);
                    obj.GetComponent<UITile>().BonusName = string.Empty;
                    ColorUtility.TryParseHtmlString("#63BC40", out color);
                    obj.GetComponent<UITile>().boardTile.GetComponent<SpriteRenderer>().color = color;
                    obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().Bonous.text = string.Empty;
                    obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().score =
                        int.Parse(obj.GetComponent<UITile>().letterScore.text);
                    //  foreach(GameObject bTile in GameController.data.boardTilesMatters)
                    //   {
                    //         bTile.transform.localScale = new Vector3(1f,1f,1f);
                    //   }
                }

                //Debug.Log("TurnScore.........." +TurnScore);
                break;

            case "2xL":
                Debug.Log("TwoTimeLetter..............." + TwoTimeLetter);

                if (TwoTimeLetter >= 0)
                {
                    Color color;
                    TwoTimeLetter++;
                    isthreeLetterBonous = true;
                    istwoLetterBonous = true;

                    TwoLetter_Text.text = TwoTimeLetter.ToString();
                    //WordScore_Text.text ="Score  "+( WordScore - obj.GetComponent<UITile>().LastScore)+undoAllScore.ToString();
                    TurnScore_Text.text = "Turn Score  " +
                                          (TurnScore - (obj.GetComponent<UITile>().LastScore) +
                                           int.Parse(obj.GetComponent<UITile>().letterScore.text)).ToString();
                    TurnScore = (TurnScore - (obj.GetComponent<UITile>().LastScore) +
                                 int.Parse(obj.GetComponent<UITile>().letterScore.text));
                    bonousChip[0].SetActive(true);
                    obj.GetComponent<UITile>().BonusName = string.Empty;
                    ColorUtility.TryParseHtmlString("#63BC40", out color);
                    obj.GetComponent<UITile>().boardTile.GetComponent<SpriteRenderer>().color = color;
                    obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().Bonous.text = string.Empty;
                    obj.GetComponent<UITile>().boardTile.GetComponent<BoardTile>().score =
                        int.Parse(obj.GetComponent<UITile>().letterScore.text);
                    //  foreach(GameObject bTile in GameController.data.boardTilesMatters)
                    //   {
                    //         bTile.transform.localScale = new Vector3(1f,1f,1f);
                    //   }               
                }

                // Debug.Log("TurnScore.........." +TurnScore);
                break;
            case "":

                isthreeLetterBonous = true;
                istwoLetterBonous = true;
                //WordScore_Text.text ="Score  "+( WordScore - obj.GetComponent<UITile>().LastScore)+undoAllScore.ToString();
                TurnScore_Text.text =
                    "Turn Score  " + (TurnScore - int.Parse(obj.GetComponent<UITile>().letterScore.text));
                TurnScore = TurnScore - int.Parse(obj.GetComponent<UITile>().letterScore.text);
                // foreach(GameObject bTile in GameController.data.boardTilesMatters)
                //   {
                //         bTile.transform.localScale = new Vector3(1f,1f,1f);
                //   }                 

                break;

            default:
                Debug.Log("Bonous name is empty");
                break;
        }
    }

//.........Get VerticalBonous.......................//


//........Get horizontalscore ...........................//


    public void PreApply(bool _isCheckMultiplayer = false)
    {
        Debug.Log("PreAply Called");
        errorCode = 0;
        preApplyInfo = string.Empty;
        newScoreBlock.SetActive(false);
        newLetterIds = new List<int>();
        newWords = new List<string>();
        _wordComboses = new List<WordCombos>();
        newScore = new List<int>();
        boardTilesMatters = new List<GameObject>();
        List<int> intanceId = new List<int>();
        bool firstWord = true;
        string _VerticalWord = "";
        //Checking if tiles are not alone
        for (int i = 0; i < BoardSlots.Count; i++)
        {
            if (!BoardSlots[i].GetComponent<BoardSlot>().free && !BoardSlots[i].GetComponent<BoardSlot>().completed)
            {
                if (!CheckIfNewTileConnected(i))
                {
                    TurnScore = 0;
                    ClearErrorData();
                    Debug.Log(" Npot ADD newLetterIds " + i);
                    errorCode = 1;

                    return;
                }

                newLetterIds.Add(i);


                if (i == 192 || i == 182 || i == 102 || i == 92)
                {
                    //_gameUi._canvasUi.GameBonous.SetActive(true);
                    is_ExtraBonous = true;
                    // Debug.Log(" newLetterIds  true" + i);
                }
            }

            if (BoardSlots[i].GetComponent<BoardSlot>().completed && firstWord)
            {
                firstWord = false;
                // Debug.Log(" eeeeeeeeeeeeeee ");
            }
        }

        if (newLetterIds.Count == 0) return;

        //Check if first word intersects center tile

        if (firstWord)
        {
            bool correct = false;
            foreach (int id in newLetterIds)
            {
                if (id == 112) correct = true;
            }

            if (!correct)
            {
                errorCode = 2;
                ClearErrorData();
                Debug.Log(" errorCode = 2;");
                return;
            }
        }

        //Checking if new tiles are at one line
        int prevX = 0;
        int prevY = 0;
        bool horizontal = false;
        bool vertical = false;
        is_vertical = false;
        is_horizontal = false;

        foreach (int id in newLetterIds)
        {
            int x = id / 15 + 1;
            int y = (id + 1) % 15 == 0 ? 15 : (id + 1) % 15;

            if (prevX == 0 && prevY == 0)
            {
                prevX = x;
                prevY = y;
            }
            else
            {
                /* Debug.Log("x....." + x);
                 Debug.Log("y....." + y);
                 Debug.Log("prevX....." + prevX);
                 Debug.Log("prevY....." + prevY);
                 Debug.Log("vertical....." + vertical);
                 Debug.Log("horizontal....." + horizontal);*/

                if (x == prevX && !vertical)
                {
                    // Debug.Log(" ///////////// ");
                    horizontal = true;
                    is_horizontal = true;
                }
                else if (y == prevY && !horizontal)
                {
                    /// Debug.Log(" \\\\\\\\\\/ ");
                    vertical = true;
                    is_vertical = true;
                }
                else
                {
                    Debug.Log("errorCode = 3; ");
                    ClearErrorData();
                    errorCode = 3;
                    return;
                }
            }
        }

        //Checking if a free space between letters
        int firstNewId = newLetterIds[0];
        if (horizontal)
        {
            for (int i = firstNewId; i < newLetterIds[newLetterIds.Count - 1]; i++)
            {
                if (BoardSlots[i].GetComponent<BoardSlot>().free)
                {
                    Debug.Log("  errorCode = 4; ");
                    errorCode = 4;
                    ClearErrorData();

                    return;
                }
            }
        }


        if (vertical)
        {
            for (int i = firstNewId; i < newLetterIds[newLetterIds.Count - 1]; i = i + 15)
            {
                if (BoardSlots[i].GetComponent<BoardSlot>().free)
                {
                    errorCode = 4;
                    ClearErrorData();

                    return;
                }
            }
        }


        //Check if new tile contact old tile
        bool haveConnect = false;
        //Debug.Log("New Letter count....." + newLetterIds.Count);
        foreach (int id in newLetterIds)
        {
            // Debug.Log(" 2222 ");
            if (CheckIfNewTileConnectsOld(id))
            {
                haveConnect = true;
                Debug.Log(" 111111 ");
            }
        }

        if (!haveConnect && !firstWord)
        {
            errorCode = 5;
            return;
        }

        if (is_recalculateCheck != true)
        {
            is_BonousRecalculate = true;
        }

        //Buildig words and scores

        currentScore = 0;
        newWords = new List<string>();
        _wordComboses = new List<WordCombos>();
        newScore = new List<int>();

        //  Debug.Log("newLetterIds count" + newLetterIds.Count);
        foreach (int id in newLetterIds)
        {
            int i;
            for (i = id; i > 0; i -= 15)
            {
                GameObject topSlot = GetАdjacentSlot(i, "top");
                if (!topSlot || topSlot.GetComponent<BoardSlot>().free) break;
            }


            _VerticalWord = VerticalWord(i, out List<int> wordIDs);

            if (_VerticalWord.Length > 1 && !CheckWordAlreadyExist(_VerticalWord, wordIDs))
            {
                int scores = 0;

                newWords.Add(_VerticalWord);
                _wordComboses.Add(new WordCombos(_VerticalWord, wordIDs));
                scores = GetVerticalScore(i);
                currentScore += scores;
                newScore.Add(scores);

                Debug.Log("Vertical currentScore.............." + currentScore);


                //  Debug.Log("vertical.........."+vertical +"horizontal..... "+horizontal +"GetVerticalScore............."+currentScore);

                // Debug.Log("GetVerticalScore.........."+VerticalScore);
                if (vertical == true)
                {
                    // Debug.Log("GetVerticalScore.........."+VerticalScore);

                    //VerticalScore=currentScore;

                    if (isCheck != true)
                    {
                        TurnScore = VerticalScore;
                        TurnScore_Text.text = "Turn Score " + TurnScore.ToString();
                    }
//Debug.Log("isthreeBonous.........."+isthreeBonous +"istwoBonous..... "+istwoBonous +"GetVerticalScore............."+scores);

                    if (isthreeBonous == true || istwoBonous == true)
                    {
                        //    int score=0;
                        //  score= VerticalBonousScoreCalCulate(i);
                        // TempScore=VerticalScore;
                        // Debug.Log("GetVerticalScore------------"+currentScore);
                        // //BonousReCalculate();
                        // newScore.RemoveAt(newScore.Count-1);
                        // currentScore =score;
                        //  newScore.Add(score);
                    }
                }
            }

            int y;
            for (y = id; y > 0; y--)
            {
                GameObject leftSlot = GetАdjacentSlot(y, "left");
                if (!leftSlot || leftSlot.GetComponent<BoardSlot>().free) break;
            }

            string _HorizontalWord = HorizontalWord(y, out List<int> HwordIds);

            if (_HorizontalWord.Length > 1 && !CheckWordAlreadyExist(_HorizontalWord, HwordIds))
            {
                int score = 0;
                newWords.Add(_HorizontalWord);
                _wordComboses.Add(new WordCombos(_HorizontalWord, HwordIds));
                // newScore.Add(GetHorizontalScore(y));
                score = GetHorizontalScore(y);
                currentScore += score;
                newScore.Add(score);
                foreach (string item in newWords)
                {
                    // Debug.Log("Horizontal word.............."+item);
                }

                if (horizontal == true)
                    // {
                    //Debug.Log("HorizontalScore......... "+HorizontalScore);

                    // Debug.Log("GetHorizontalScore.........."+HorizontalScore);
                    // HorizontalScore=currentScore;
                    if (isCheck != true)
                    {
                        TurnScore = HorizontalScore;
                        TurnScore_Text.text = "Turn Score " + TurnScore.ToString(); //
                    }

                if (isthreeBonous == true || istwoBonous == true)
                {
                    //      int scores=0;
                    //     scores= HorizontalBonousScoreCalculate(y);

                    //   //Debug.Log("GetHorizontalScore.........."+currentScore);
                    //    //TempScore=HorizontalScore;
                    //     //BonousReCalculate();
                    //    newScore.RemoveAt(newScore.Count-1);
                    //    currentScore = scores;
                    //    newScore.Add(scores);
                    //    Debug.Log("HorizontalBonousScoreCalculate............ "+currentScore);
                }

                // }
            }
        }

        newWordsList = string.Empty;
        int w = 0;
        int s = 0;
        for (int i = 0; i < WordText.Length; i++)
        {
            WordText[i].text = string.Empty;
            Scoretext[i].text = string.Empty;
        }

        if (!_isCheckMultiplayer)
        {
            foreach (string word in newWords)
            {
                newWordsList += word + ", ";
                WordText[w].gameObject.SetActive(true);
                WordText[w].text = word;
                // Debug.Log("word........"+word);
                w++;
            }

            foreach (int word in newScore)
            {
                Scoretext[s].gameObject.SetActive(true);
                // newWordsList += word + ", ";
                Scoretext[s].text = word.ToString();
                // Debug.Log(" currentScore..........................."+word);
                s++;
            }


            if (isTileOnBoard == true)
            {
                currentScore = currentScore + previousCurrentScore;
            }

            intanceId.Clear();

            preApplyInfo = "APPLY '" + newWordsList + "' for " + currentScore + " scores?";

            newScoreBlock.SetActive(true);
            newScoreBlock.transform.position = boardTilesMatters[boardTilesMatters.Count - 1].transform.position +
                                               new Vector3(0.5f, -0.5f, -0.4f);
            newScoreBlock.GetComponentInChildren<TextMesh>().text = currentScore.ToString();
            newScoreBlock.GetComponent<Transformer>().ScaleImpulse(new Vector3(0.6f, 0.6f, 1), 0.15f, 1);
            is_recalculateCheck = false;

            TurnScore_Text.text = "Turn Score     " + currentScore.ToString();
        }
    }

    private bool CheckWordAlreadyExist(string word, List<int> wordIds)
    {
        bool isWordPresent = false;


        foreach (var wordCombose in _wordComboses)
        {
            if (wordCombose.word.Equals(word))
            {
                LogSystem.LogColorEvent("green", "wordalreadyExist {0}", word);

                bool isEqual = wordIds.SequenceEqual(wordCombose.wordIDs);

                if (isEqual)
                {
                    isWordPresent = true;
                    break;
                }
            }
        }

        return isWordPresent;
    }

    public void ClearErrorData()
    {
        for (int m = 0; m < WordText.Length; m++)

        {
            WordText[m].text = string.Empty;
            Scoretext[m].text = string.Empty;
            WordText[m].gameObject.SetActive(false);
            Scoretext[m].gameObject.SetActive(false);
        }

        TurnScore_Text.text = "Turn Score     " + 0.ToString();
    }


    public bool is_returnBonous = false;

    public void ApplyTurn()
    {
        Debug.Log("_BoardTileParent : " + _BoardTileParent.Count);

        string info = string.Empty;
        int wordSize = 0;
        Debug.Log("......." + newWords.Count + "  errorCode   " + errorCode);
        if (newWords.Count == 0 && errorCode == 0)
            errorCode = 7;

        newWords = newWords.Distinct().ToList();

        //CHECKING NEW WORD WITH DICTIONARY
        if (dictChecking)
        {
            info += "\r\n";
            foreach (string word in newWords)
            {
                if (wordDictionary.data != null && !wordDictionary.data.hasWord(word))
                {
                    info += word + " ";
                    errorCode = 8;
                }

                if (addedWords.Contains(word))
                {
                    info = word;
                    errorCode = 9;
                }
            }
        }

        if (errorCode != 0)
        {
            ShowErrorAlert(errorCode, info);
            return;
        }
        else
        {
            foreach (string word in newWords) addedWords.Add(word);
        }

        newScoreBlock.SetActive(false);
        // Checking word is correct or not //
        foreach (GameObject item in WordPanelPrefab)
        {
            Destroy(item);
        }

        Debug.Log("_BoardTileParent : " + _BoardTileParent.Count);

        if (Is_WordCheck)
        {
            WordResponseCounter = 0;
            isValid = true;
            foreach (string item in newWords)
            {
                // UI_Manager._instance.LoadingBarStatus(false);
                ApiManager.CheckWord(item, HandleWordCheck);
            }
        }
        else
        {
            ApplyingWord();
        }
    }

    public void ApplyingWord()
    {
        Debug.Log("_BoardTileParent : " + _BoardTileParent.Count);

        int wordSize = 0;
        //APPLYING WORD
        foreach (int id in newLetterIds)
        {
            BoardSlots[id].GetComponent<BoardSlot>().completed = true;
            BoardSlots[id].GetComponentInChildren<BoardTile>().completed = true;
        }

        Color color;
        foreach (GameObject bTile in boardTilesMatters)
        {
            //Debug.Log("  "+bTile.name);
            ColorUtility.TryParseHtmlString("#FAFD00", out color);
            bTile.GetComponent<Transformer>().ScaleImpulse(new Vector3(1.2f, 1.2f, 1), 0.125f, 1);
            bTile.transform.localPosition = new Vector3(0, 0, 44f);
            bTile.GetComponent<SpriteRenderer>().color = color;
            bTile.GetComponent<BoardTile>().score = int.Parse(bTile.GetComponent<BoardTile>().ScoreText.text);
        }

        if (WordScore == 0)
        {
            foreach (GameObject tile in UITiles)
            {
                if (!tile.activeInHierarchy)
                {
                    isReCreateTile = true;
                    wordSize++;
                    WordScore = WordScore + int.Parse(tile.GetComponent<UITile>().letterScore.text);
                    tile.SetActive(true);
                    tile.GetComponent<UITile>().ReCreateTile();
                    tile.GetComponent<UITile>().Hilighter.SetActive(false);
                    // Debug.Log("ReplaceLetter........."+Alphabet.data.ReplaceLetter);
                }
            }

            WordScore = currentScore;
            WordScore_Text.text = "Word Score     " + WordScore.ToString();
            TurnScore_Text.text = "Turn Score     " + 0.ToString();
            undoAllScore = WordScore;
        }
        else
        {
            //Debug.Log("......." +TurnScore);
            foreach (GameObject tile in UITiles)
            {
                is_returnBonous = false;
                if (!tile.activeInHierarchy)
                {
                    isReCreateTile = true;
                    wordSize++;
                    //WordScore=WordScore + int.Parse(tile.GetComponent<UITile>().letterScore.text);
                    tile.SetActive(true);
                    tile.GetComponent<UITile>().ReCreateTile();
                    tile.GetComponent<UITile>().Hilighter.SetActive(false);
                    //Debug.Log("ReplaceLetter........."+Alphabet.data.ReplaceLetter);
                }
            }

            //Debug.Log("TurnScore ........"+TurnScore);
            // TotalScore();
            // Debug.Log("TurnScore ........" + TurnScore);
            WordScore = WordScore + currentScore;
            WordScore_Text.text = "Word Score     " + WordScore.ToString();
            undoAllScore = WordScore;
            TurnScore_Text.text = "Turn Score     " + 0.ToString();
        }

        undo = -1;
        LastUITiles.Clear();
        istwoBonous = false;
        isthreeBonous = false;
        isthreeLetterBonous = false;
        istwoLetterBonous = false;
        is_temp = true;
        is_returnBonous = true;
        LastBoardTiles.Clear();
        undoBoardTile = -1;
        previousCurrentScore = 0;
        isTileOnBoard = false;

        UndoLetter.Clear();
        UndoScore.Clear();
        TrayLetter.Clear();

        IntUndoLetter = -1;
        IntTrayLetter = -1;

        SaveData.data.ResumeData.score = WordScore;


        // Debug.Log("horizontalscore............."+HorizontalScore);
        // Debug.Log("verticalscore.........."+VerticalScore);
        // RewardedAdsScript._instance.ShowRewardedVideo();
        for (int i = 0; i < WordText.Length; i++)
        {
            WordText[i].text = " ";
            Scoretext[i].text = " ";
            WordText[i].gameObject.SetActive(false);
            Scoretext[i].gameObject.SetActive(false);
        }

        /*  SaveData.data.ResumeData.BoardData.Clear();
          SaveData.data.ResumeData.PlayerHandData.Clear();
          SaveData.data.ResumeData.TrayDatas.Clear();*/

        //Debug.Log("boardtile......" + _BoardTileParent.Count);
        /* foreach (GameObject item in _BoardTileParent)
         {
             Debug.Log("item......." + item.GetInstanceID() + " name..." + item.name);
         }*/

        //Debug.Log("Alphabet.data.Traydata......" + Alphabet.data.Traydata.Count);
        int counter = 0;
        foreach (GameObject bTile in _BoardTileParent)
        {
            counter++;
        }

        _gameUi._textUi.AquireTileTxt.text = counter.ToString() + " / 121";


        /* foreach (GameObject tile in UITiles)
         {
             LetterBlock sendata = new LetterBlock();
             sendata.LetterBlocks(tile.GetComponent<UITile>().letterString.text,
                 tile.GetComponent<UITile>().letterScore.text);
             SaveData.data.ResumeData.PlayerHandData.Add(sendata);
         }

         foreach (GameObject tile in Alphabet.data.Traydata)
         {
             //Debug.Log("llllllllllllllll"+ Alphabet.data.Traydata.Count);
             TrayData sendatas = new TrayData(tile.transform.GetChild(0).GetComponent<Text>().text, tile.name);
             SaveData.data.ResumeData.TrayDatas.Add(sendatas);
         }*/

        foreach (GameObject tile in UITiles)
        {
            tile.GetComponent<UITile>().BonusName = string.Empty;
        }


        //Debug.Log("_MultiplayerWordData....................... " + _MultiplayerWordData.Count);

        //........Bonus point................//

        if (is_ExtraBonous == true && wordSize < 4)
        {
            wordSize = 4;
        }
        else if (is_ExtraBonous == true && wordSize >= 4)
        {
            wordSize = wordSize + 1;
        }

        if (wordSize >= 4)
        {
            TwoTimeLetter++;
            Debug.Log("two l");
            bonousChip[0].SetActive(true);
        }

        if (wordSize >= 5)
        {
            ThreeTimeLetter++;
            Debug.Log("three l");
            bonousChip[1].SetActive(true);
        }

        if (wordSize >= 6)
        {
            TwoTimeWord++;
            Debug.Log("two w");
            bonousChip[2].SetActive(true);
        }

        if (wordSize >= 7)
        {
            ThreeTimeWord++;
            Debug.Log("three w");
            bonousChip[3].SetActive(true);
        }


        if (is_ExtraBonous)
        {
            UpdatePowerUi();
            _gameUi._canvasUi.GameBonous.SetActive(true);
        }

        SaveData.data.ResumeData.TwoLetter = TwoTimeLetter;
        SaveData.data.ResumeData.ThreeLetter = ThreeTimeLetter;
        SaveData.data.ResumeData.TwoWord = TwoTimeWord;
        SaveData.data.ResumeData.ThreeWord = ThreeTimeWord;

        Alphabet.data.intTrayGameObjectName = -1;
        Alphabet.data.TrayGameObjectName.Clear();

        int tilesLeft = 0;
        foreach (GameObject uiTile in UITiles)
        {
            if (uiTile.activeInHierarchy) tilesLeft++;
        }

/*        if (tilesLeft == 0) players[currentPlayer - 1].complete = true;

        players[currentPlayer - 1].score += currentScore;*/
        skipCount = 0;
        UpdateTxts();
        // foreach( var data in ResumePlayerData)
        // Debug.Log(data);

        if (gameType == GameType.SinglePlayer)
        {
            if (!isGameStatus)
            {
                Debug.Log("_BoardTileParent : " + _BoardTileParent.Count);

                SendSaveData();
            }

            SinglePlayerWinner();
            // SaveData.data.SinglePlayerResumeGameData();
        }

        if (gameType == GameType.Multiplayer)
        {
            controllerRPC.SendData(SaveData.data.ResumeData);
            controllerRPC.SendWordScore(currentScore);
            controllerRPC.SendTurnWord(newWordsList);

            MultiplayerWinner();
        }

        Debug.Log("TotalboardTiles..." + TotalboardTiles.Count);
        TotalboardTiles.Clear();
        //Invoke("SwitchPlayer", 0.35f);
        SoundController.data.playApply();
    }


    private void HandleWordCheck(bool aSucess, CheckWordHeader callback)
    {
        if (aSucess)
        {
            LogSystem.LogColorEvent("green", "Check word  : {0}", callback.message);
            WordPrefabGenerate(callback);
        }
        else
        {
            LogSystem.LogColorEvent("red", "Check word : {0}", callback.message);
            isValid = false;
            WordPrefabGenerate(callback);
        }
    }

    private void SendSaveData()
    {
        SaveData.data.ResumeData.BoardData.Clear();
        SaveData.data.ResumeData.PlayerHandData.Clear();
        SaveData.data.ResumeData.TrayDatas.Clear();


        Debug.Log("_BoardTileParent : " + _BoardTileParent.Count);
        foreach (GameObject bTile in _BoardTileParent)
        {
            BoardData sendata = new BoardData(bTile.transform.parent.name, bTile.GetComponent<BoardTile>().letter,
                bTile.GetComponent<BoardTile>().score.ToString(), bTile.GetComponent<BoardTile>().Bonous.text);
            SaveData.data.ResumeData.BoardData.Add(sendata);
        }


        foreach (GameObject tile in UITiles)
        {
            LetterBlock sendata = new LetterBlock();
            sendata.LetterBlocks(tile.GetComponent<UITile>().letterString.text,
                tile.GetComponent<UITile>().letterScore.text);
            SaveData.data.ResumeData.PlayerHandData.Add(sendata);
        }

        foreach (GameObject tile in Alphabet.data.Traydata)
        {
            //Debug.Log("llllllllllllllll"+ Alphabet.data.Traydata.Count);
            //TrayData sendatas = new TrayData(tile.transform.GetChild(0).GetComponent<Text>().text, tile.name);
            //SaveData.data.ResumeData.TrayDatas.Add(sendatas);
        }

        SaveData.data.ResumeData.TwoLetter = TwoTimeLetter;
        SaveData.data.ResumeData.ThreeLetter = ThreeTimeLetter;
        SaveData.data.ResumeData.TwoWord = TwoTimeWord;
        SaveData.data.ResumeData.ThreeWord = ThreeTimeWord;
        SaveData.data.ResumeData.score = WordScore;
        SaveData.data.ResumeData.GameId = GlobalData.GameId;
        // SaveData.data.ResumeData.GameType = GlobalData.GameType; ;
        SaveData.data.ResumeData.GameMode = GlobalData.GameMode;
        ;
        SaveData.data.ResumeData.GameDate = GlobalData.GameDate;

        SaveData.data.SinglePlayerResumeGameData();
    }


    private void WordPrefabGenerate(CheckWordHeader callback)
    {
        GameObject intrestdata = Instantiate(wordPrefab) as GameObject;
        intrestdata.transform.SetParent(WordPrefabParent.transform);
        intrestdata.transform.localScale = new Vector3(1, 1, 1);
        intrestdata.GetComponent<wordPrefab>().SetData(callback.word, callback.message);
        intrestdata.SetActive(true);
        WordPanelPrefab.Add(intrestdata);
        UI_Manager._instance.WorldResult.SetActive(true);
        UI_Manager._instance.LoadingBarStatus(false);
        WordResponseCounter++;

        if (isValid == true && newWords.Count == WordResponseCounter)
        {
            ApplyingWord();
        }
    }


    private void SinglePlayerWinner()
    {
        int i = 0;
        int flag = 0;
        foreach (GameObject bTile in _BoardTileParent)
        {
            i++;
        }

        foreach (GameObject bTile in UITiles)
        {
            if (bTile.GetComponent<UITile>().letterString.text != " ")
            {
                flag = 1;
            }
        }

        if (i > 61)
        {
            isGameStatus = true;
            // UI_Manager._instance.singlePlayersesult("You Won this round");
            CommonApi.CallResultApi(GlobalData.GameId.ToString(), GlobalData.UserId, "1", WordScore);
            //RewardedAdsScript._instance.ShowRewardedVideo();
        }

        else if (i < 61 && flag == 0)
        {
            isGameStatus = true;
            // UI_Manager._instance.singlePlayersesult("You Lose this round");

            CommonApi.CallResultApi(GlobalData.GameId.ToString(), GlobalData.UserId, "0", WordScore);
        }
    }

    private void MultiplayerWinner()
    {
        int i = 0;
        foreach (GameObject bTile in _BoardTileParent)
        {
            i++;
        }

        if (i > 60)
        {
            UI_Manager._instance.ShowMultiplayerResultPanel();
            UI_Manager._instance.CallResultApi(UI_Manager._instance.User_GameId.ToString(), UI_Manager._instance.User_Id, "1", WordScore);
            //RewardedAdsScript._instance.ShowRewardedVideo();
        }
    }

    public void RecivePlayerWordData(GameStateData _data, int _ActorNumber)
    {
        _MultiplayerWordData.Add(_ActorNumber, _data);


        PlayerWordComplete(_ActorNumber);
    }

    public void RecivePlayerScore(int _ActorNumber, int _Score)
    {
        _MultiplayerWordScoreData.Add(_ActorNumber, _Score);
    }

    public void RecivePlayerWord(int _ActorNumber, string _word)
    {
        _MultiplayerTurnWordData.Add(_ActorNumber, _word);
    }

    public void ShowApplyConfirmDialog()
    {
        Debug.Log("_BoardTileParent : " + _BoardTileParent.Count);

        isclickable = true;
        if (TotalboardTiles.Count <= 0)
        {
            errorCode = 7;
        }

        if (errorCode == 0 && preApplyInfo.Length > 0)
        {
            Debug.Log("......." + TurnScore);
            // PreApply();
            ShowConfirmationDialog("ApplyTurn");
        }

        else
        {
            Debug.Log("......." + TurnScore);
            ApplyTurn();
        }
    }

    bool CheckIfNewTileConnected(int tileId)
    {
        GameObject topTile = GetАdjacentSlot(tileId, "top");

        if (topTile != null && !topTile.GetComponent<BoardSlot>().free) return true;
        GameObject rightTile = GetАdjacentSlot(tileId, "right");
        if (rightTile != null && !rightTile.GetComponent<BoardSlot>().free) return true;
        GameObject bottomTile = GetАdjacentSlot(tileId, "bottom");
        if (bottomTile != null && !bottomTile.GetComponent<BoardSlot>().free) return true;
        GameObject leftTile = GetАdjacentSlot(tileId, "left");
        if (leftTile != null && !leftTile.GetComponent<BoardSlot>().free) return true;

        return false;
    }

    bool CheckIfNewTileConnectsOld(int tileId)
    {
        GameObject topTile = GetАdjacentSlot(tileId, "top");
        if (topTile != null && topTile.GetComponent<BoardSlot>().completed)
        {
            return true;
        }

        GameObject rightTile = GetАdjacentSlot(tileId, "right");
        if (rightTile != null && rightTile.GetComponent<BoardSlot>().completed) return true;
        GameObject bottomTile = GetАdjacentSlot(tileId, "bottom");
        if (bottomTile != null && bottomTile.GetComponent<BoardSlot>().completed)
        {
            return true;
        }

        GameObject leftTile = GetАdjacentSlot(tileId, "left");
        if (leftTile != null && leftTile.GetComponent<BoardSlot>().completed) return true;

        return false;
    }

    public GameObject GetАdjacentSlot(int tileId, string pos)
    {
        switch (pos)
        {
            case "top":
                int topTileID = tileId - 15; //tileId - 15;

                if (topTileID + 1 > 0)
                {
                    return BoardSlots[topTileID];
                }

                break;
            case "right":
                int rightTileId = tileId + 1;
                if ((rightTileId) % 15 != 0)
                {
                    return BoardSlots[rightTileId];
                }

                break;
            case "bottom":
                int bottomTileId = tileId + 15; //tileId - 15;


                if (bottomTileId + 1 < 226)
                {
                    return BoardSlots[bottomTileId];
                }

                break;
            case "left":
                int leftTileId = tileId - 1;
                if ((leftTileId + 1) % 15 != 0)
                {
                    return BoardSlots[leftTileId];
                }

                break;
        }

        return null;
    }

    public string VerticalWord(int firstId, out List<int> wordIDs)
    {
        string word = "";
        wordIDs = new List<int>();
        // Debug.Log("BoardTile Master....." + boardTilesMatters.Count + "Boardtileparent....." + _BoardTileParent.Count);

        for (int i = firstId; i < 225; i += 15)
        {
            if (BoardSlots[i] && !BoardSlots[i].GetComponent<BoardSlot>().free)
            {
                LogSystem.LogEvent("BoardSlotName {0}", BoardSlots[i].name);
                word += BoardSlots[i].GetComponentInChildren<BoardTile>().letter;
                wordIDs.Add(i);
                // Debug.Log(" boardsloat of i .........." + word);
                if (!boardTilesMatters.Contains(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject))
                {
                    boardTilesMatters.Add(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject);
                    if (!_BoardTileParent.Contains(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject))
                    {
                        // Debug.LogFormat("wordddd {0}", word);
                        _BoardTileParent.Add(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject);
                    }
                    //

                    // Debug.Log(" letter....." + BoardSlots[i].GetComponentInChildren<BoardTile>().letter + "id ..." +
                    //   BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject.GetInstanceID());
                }

                else
                {
                    // Debug.Log(" letter....." + BoardSlots[i].GetComponentInChildren<BoardTile>().letter + "id ..." +
                    //           BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject.GetInstanceID());
                    // boardTilesMatters.Remove(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject);
                    // boardTilesMatters.Add(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject);
                    // _BoardTileParent.Remove(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject);
                    // _BoardTileParent.Add(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject);
                }

                if (i + 15 > 224) return word;
            }
            else
            {
                return word;
            }
        }

        return "";
    }

    public string HorizontalWord(int firstId, out List<int> wordIDs)
    {
        string word = "";
        wordIDs = new List<int>();
        for (int i = firstId; i < 225; i++)
        {
            if (BoardSlots[i] && !BoardSlots[i].GetComponent<BoardSlot>().free)
            {
                word += BoardSlots[i].GetComponentInChildren<BoardTile>().letter;
                wordIDs.Add(i);
                if (!boardTilesMatters.Contains(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject))
                {
                    if (!boardTilesMatters.Contains(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject))
                    {
                        boardTilesMatters.Add(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject);
                    }

                    if (!_BoardTileParent.Contains(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject))
                    {
                        _BoardTileParent.Add(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject);
                    }
                }

                // else
                //     boardTilesMatters.Remove(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject);
                // boardTilesMatters.Add(BoardSlots[i].GetComponentInChildren<BoardTile>().gameObject);
                if ((i + 1) % 15 == 0) return word;
            }
            else
            {
                return word;
            }
        }

        return word;
    }

    int tempVerticalScore = 0;

    public int GetVerticalScore(int firstId)
    {
        int score = 0;
        int wordFactor = 1;
        isthreeBonous = false;
        istwoBonous = false;
        bool isBonus = false;
        int BonousFactor = 0;

        //  Debug.Log("firstId.........." + firstId);

        for (int i = firstId; i < 225; i += 15)
        {
            BoardSlot boardSlot = BoardSlots[i].GetComponent<BoardSlot>();

            if (BoardSlots[i] && !boardSlot.free)
            {
                if (!BoardSlots[i].GetComponent<BoardSlot>().completed)
                {
                    if (BoardSlots[i].GetComponentInChildren<BoardTile>().BonusName == "2xW" && !boardSlot.completed)
                    {
                        isBonus = true;
                        BonousFactor += 2;
                    }

                    if (BoardSlots[i].GetComponentInChildren<BoardTile>().BonusName == "3xW" && !boardSlot.completed)
                    {
                        isBonus = true;
                        BonousFactor += 3;
                    }
                }

                if (!boardSlot.completed)
                {
                    // LogSystem.LogColorEvent("red","verticalscore{0}",i);
                    //score += Alphabet.data.GetLetterScore(BoardSlots[i].GetComponentInChildren<BoardTile>().letter) * (int)boardSlot.letterFactor;
                    score += BoardSlots[i].GetComponentInChildren<BoardTile>().score * (int)boardSlot.letterFactor;
                    VerticalScore = score;
                    // score= GetVerticalBonous(BoardSlots[i].GetComponentInChildren<BoardTile>().Bonous.text,tempVerticalScore,BoardSlots[i].GetComponentInChildren<BoardTile>().score);
                    // if ((int)boardSlot.wordFactor > 1)
                    //     wordFactor *= (int)boardSlot.wordFactor;
                }
                else
                {
                    //score += Alphabet.data.GetLetterScore(BoardSlots[i].GetComponentInChildren<BoardTile>().letter);
                    // Debug.Log("score..........."+score);
                    // LogSystem.LogColorEvent("red","verticalscore{0}",i);

                    score += BoardSlots[i].GetComponentInChildren<BoardTile>().score;
                    VerticalScore = score;
                }

                if (i + 15 > 224) break;
            }
            else
            {
                break;
            }
        }
        //     for (int i = firstId; i < 225; i+=15)
        // {
        //     BoardSlot boardSlot = BoardSlots[i].GetComponent<BoardSlot>();

        //     if (!BoardSlots[i].GetComponent<BoardSlot>().free)
        //     {

        //         if (!boardSlot.completed)
        //         {    
        // if (SelectedBoarTile != null)
        // if(BoardSlots[i].name == SelectedBoarTile.transform.parent.name)
        if (isBonus)
        {
            // if (BoardSlots[i].GetComponentInChildren<BoardTile>().Bonous.text !=" " && (is_VerticalThreeBonous!=true || is_VerticalTwoBonous!=true))
            // {
            // Debug.Log("Score......."+score);
            score = score * BonousFactor;
            // score = GetVerticalBonous(BoardSlots[i].GetComponentInChildren<BoardTile>().Bonous.text,score,BoardSlots[i].GetComponentInChildren<BoardTile>().score);
            /////// Debug.Log("Score......."+score);

            //}
        }

        // }
        // }
        // }
        tempVerticalScore = score;
        return score * wordFactor;
    }

    int tempHorizontalScore = 0;

    public int GetHorizontalScore(int firstId)
    {
        int score = 0;
        int wordFactor = 1;
        is_VerticalTwoBonous = false;
        is_VerticalThreeBonous = false;
        bool isBonus = false;
        int BonousFactor = 0;
        // Debug.Log("firstId''''''''''''''''''''''"+firstId);

        for (int i = firstId; i < 225; i++)
        {
            BoardSlot boardSlot = BoardSlots[i].GetComponent<BoardSlot>();

            if (!BoardSlots[i].GetComponent<BoardSlot>().free)
            {
                if (!BoardSlots[i].GetComponent<BoardSlot>().completed)
                {
                    if (BoardSlots[i].GetComponentInChildren<BoardTile>().BonusName == "2xW" && !boardSlot.completed)
                    {
                        isBonus = true;
                        BonousFactor += 2;
                    }

                    if (BoardSlots[i].GetComponentInChildren<BoardTile>().BonusName == "3xW" && !boardSlot.completed)
                    {
                        isBonus = true;
                        BonousFactor += 3;
                    }
                }

                if (!boardSlot.completed)
                {
                    //score += Alphabet.data.GetLetterScore(BoardSlots[i].GetComponentInChildren<BoardTile>().letter) * (int)boardSlot.letterFactor;
                    score += BoardSlots[i].GetComponentInChildren<BoardTile>().score * (int)boardSlot.letterFactor;
                    // HorizontalScore=score;
                    // Debug.Log("Score''''''''''''''''''''''"+ BoardSlots[i].name + " score.."+ BoardSlots[i].GetComponentInChildren<BoardTile>().score * (int)boardSlot.letterFactor);
                    // score = GetHorizontalBonous(BoardSlots[i].GetComponentInChildren<BoardTile>().Bonous.text,tempHorizontalScore,BoardSlots[i].GetComponentInChildren<BoardTile>().score);
                }
                else
                {
                    score += BoardSlots[i].GetComponentInChildren<BoardTile>().score;
                    // HorizontalScore=score;
                    // Debug.Log("Score'''''''''''+"+BoardSlots[i].name+"'''''''''''" + score);
                    // score = GetHorizontalBonous(BoardSlots[i].GetComponentInChildren<BoardTile>().Bonous.text,HorizontalScore,BoardSlots[i].GetComponentInChildren<BoardTile>().score);
                }

                if ((i + 1) % 15 == 0) break;
            }
            else
            {
                break;
            }
        }

        Debug.Log("isBonus......." + isBonus);
        if (isBonus)
        {
            // if (BoardSlots[i].GetComponentInChildren<BoardTile>().Bonous.text !=" " && (is_VerticalThreeBonous!=true || is_VerticalTwoBonous!=true))
            // {
            Debug.Log("BonousFactor......." + score);
            score = score * BonousFactor;
            // score = GetVerticalBonous(BoardSlots[i].GetComponentInChildren<BoardTile>().Bonous.text,score,BoardSlots[i].GetComponentInChildren<BoardTile>().score);
            // Debug.Log("Score......."+score);

            //}
        }

        //tempHorizontalScore=score;
        Debug.Log("Score......." + score);
        return score * wordFactor;
    }

    public void ShowErrorAlert(int code, string info)
    {
        paused = true;
        string errorText = string.Empty;
        switch (code)
        {
            case 1:
                errorText = "TILES SHOULD BE CONNECTED!";
                ClearErrorData();
                break;
            case 2:
                errorText = "FIRST WORD SHOULD INTERSECT CENTER TILE!";
                ClearErrorData();
                break;
            case 3:
                errorText = "TILES SHOULD BE IN 1 LINE!";
                ClearErrorData();
                break;
            case 4:
                errorText = "TILES SHOULD NOT HAVE SPACES!";
                ClearErrorData();
                break;
            case 5:
                errorText = "NO CONNECTION WITH OLD TILES!";
                ClearErrorData();
                break;
            case 6:
                errorText = "NOT ENOUGH FREE TILES";
                break;
            case 7:
                errorText = "YOU NEED TO PLACE TILES FIRST!";
                break;
            case 8:
                errorText = "INCORRECT WORDS: " + info;
                break;
            case 9:
                errorText = "ALREADY USED WORD: " + info;
                break;
        }

        ErrorAlert.SetActive(true);
        ErrorAlert.GetComponentInChildren<Text>().text = errorText;
    }

    public void CloseErrorAlert()
    {
        ErrorAlert.SetActive(false);
        paused = false;
        isclickable = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void EnableSwapMode()
    {
        // paused = swapMode =  true;
        // CancelLetters();
        // Alphabet.data.ReplaceLetter=null;
        foreach (GameObject tile in UITiles)
        {
            tile.GetComponent<UITile>().Hilighter.SetActive(false);
        }

        foreach (GameObject bTile in TotalboardTiles)
        {
            // bTile.transform.localPosition = new Vector3(0,0,44f);
        }

        newScoreBlock.SetActive(false);
        Debug.Log("............................");
        // SwapBlock.SetActive(true);
        Alphabet.data.LetterToss();
    }

    public void ApplySwap()
    {
        List<GameObject> tilesToSwap = new List<GameObject>();
        List<string> oldLetters = new List<string>();
        Debug.Log(UITiles.Count);

        foreach (GameObject uiTile in UITiles)
        {
            // Debug.Log(" uiTile.GetComponent<UITile>().needSwap..."+ uiTile.GetComponent<UITile>().needSwap);
            if (uiTile.GetComponent<UITile>().needSwap)
            {
                tilesToSwap.Add(uiTile);
                oldLetters.Add(uiTile.GetComponent<UITile>().letterString.text);

                //oldLetters.Add(uiTile.GetComponent<UITile>().lett);
                Debug.Log("swap letter......" + uiTile.GetComponent<UITile>().letterString.text);

                uiTile.GetComponent<UITile>().needSwap = false;
                undo++;
                LastUITiles.Add(uiTile);

                Debug.Log(".........................undo..." + undo);
            }
        }

        if (tilesToSwap.Count == 0)
        {
            DisableSwapMode();
            Debug.Log("kkkkkkkkkk");
            return;
        }

        if (tilesToSwap.Count >= Alphabet.data.LettersFeed.Count)
        {
            DisableSwapMode();
            ShowErrorAlert(6, "");
            Debug.Log("kmmmmmmmmm");
            return;
        }

        foreach (GameObject uiTile in tilesToSwap)
        {
            IntUndoLetter++;
            IntTrayLetter++;

            // uiTile.GetComponent<UITile>().BeforeTossLetter = uiTile.GetComponent<UITile>().letterString.text;
            UndoLetter.Add(uiTile.GetComponent<UITile>().letterString.text);
            UndoScore.Add(uiTile.GetComponent<UITile>().letterScore.text);

            // uiTile.GetComponent<UITile>().PreviousScore = int.Parse(uiTile.GetComponent<UITile>().letterScore.text);

            //currentScore = currentScore - int.Parse(uiTile.GetComponent<UITile>().letterScore.text);
            // TurnScore_Text.text = "Turn Score     " + currentScore.ToString();

            isTileOnBoard = true;
            if (!is_GarbageBonous)
            {
                previousCurrentScore = previousCurrentScore - int.Parse(uiTile.GetComponent<UITile>().letterScore.text);
                TurnScore_Text.text = "TurnScore     " + previousCurrentScore.ToString();
            }

            is_GarbageBonous = false;


            PreApply();
            Debug.Log("IntUndoLetter............" + IntUndoLetter + " uiTile....." + uiTile.name);

            uiTile.GetComponent<UITile>().GetNewLetter();
            // uiTile.GetComponent<UITile>().TrayLetter = uiTile.GetComponent<UITile>().letterString.text;
            TrayLetter.Add(uiTile.GetComponent<UITile>().letterString.text);
            uiTile.GetComponent<UITile>().TrayLetter = uiTile.GetComponent<UITile>().letterString.text;

            // uiTile.SetActive(true);

            // Debug.Log("swap letter......"+uiTile.GetComponent<UITile>().letterString.text);
        }

        foreach (string letter in oldLetters)
        {
            // Debug.Log("kkjhjhmmkmkmk");
            Alphabet.data.LettersFeed.Add(letter);
        }

        Invoke("DisableSwapMode", 0.15f);
        //Invoke("SwitchPlayer", 0.5f);
        skipCount = 0;
    }

    public void DisableSwapMode()
    {
        paused = swapMode = false;
        // isclickable=false;
        SwapBlock.SetActive(false);

        foreach (GameObject uiTile in UITiles)
        {
            if (uiTile.GetComponent<UITile>().needSwap)
            {
                // uiTile.GetComponent<UITile>().SetSwapState(false);
            }
        }

        Alphabet.data.ReplaceLetter = Alphabet.data.LastReplaceLetter;
    }

    void UpdateTxts()
    {
        // players[currentPlayer - 1].scoreTxt.text = players[currentPlayer - 1].score.ToString();
        letterLeftTxt.text = Alphabet.data.LettersFeed.Count.ToString() + " LETTERS LEFT";
        TwoLetter_Text.text = TwoTimeLetter.ToString();
        ThreeLetter_Text.text = ThreeTimeLetter.ToString();
        TwoWord_Text.text = TwoTimeWord.ToString();
        ThreeWord_Text.text = ThreeTimeWord.ToString();
    }

    public void SwitchPlayer()
    {
        currentPlayer = currentPlayer + 1 <= 4 ? currentPlayer + 1 : 1;
        if (!players[currentPlayer - 1].active || players[currentPlayer - 1].complete)
        {
            SwitchPlayer();
            return;
        }

        if (CheckForGameOver())
        {
            GameOver();
            return;
        }

        for (int i = 1; i <= 1; i++)
        {
            if (i == currentPlayer)
            {
                players[i - 1].UILettersPanel.SetActive(true);
                players[i - 1].scoreTxt.text += "←";
            }
            else
            {
                players[i - 1].UILettersPanel.SetActive(false);
                players[i - 1].scoreTxt.text.Trim((new System.Char[] { '←' }));
            }
        }

        UITileSlots = players[currentPlayer - 1].UITileSlots;
        UITiles = players[currentPlayer - 1].UITiles;

        currentPlayerTxt.text = "Player " + currentPlayer;
        PreApply();
        //Showing Title
        //  newPlayerTitle.GetComponentInChildren<Text>().text = "PLAYER'S " + currentPlayer + " MOVE";
        // newPlayerTitle.GetComponent<Transformer>().MoveUIImpulse(Vector2.zero, 1, 1);

        UpdateTxts();
    }

    public void SkipTurn()
    {
        CancelLetters();
        Invoke("SwitchPlayer", 0.35f);
    }

    public void GiveUp()
    {
        Debug.Log("Player " + currentPlayer + " gived up!");

        foreach (GameObject uiTile in UITiles)
            Alphabet.data.LettersFeed.Add(uiTile.GetComponent<UITile>().letterString.text);

        SkipTurn();
        playersCount -= 1;
        players[currentPlayer - 1].active = false;
    }

    public void ShowConfirmationDialog(string confirmationID)
    {
        this.confirmationID = confirmationID;
        switch (confirmationID)
        {
            case "ApplyTurn":
                confirmationDialogTxt.text = preApplyInfo;
                break;
            case "SkipTurn":
                confirmationDialogTxt.text = "Are you sure to skip turn?";
                break;
            case "GiveUp":
                confirmationDialogTxt.text = "Give up?";
                break;
        }

        confirmationDialog.SetActive(true);
        switchMenu(false);
    }

    public void ConfirmDialog()
    {
        isclickable = false;
        switch (confirmationID)
        {
            case "ApplyTurn":
                ApplyTurn();
                break;
            case "SkipTurn":
                skipCount++;
                SkipTurn();
                break;
            case "GiveUp":
                Invoke("GiveUp", 0.35f);
                break;
        }

        confirmationDialog.SetActive(false);
    }

    public void CloseConfirmDialog()
    {
        isclickable = false;

        confirmationDialog.SetActive(false);
    }

    public void switchMenu(bool state)
    {
        /*
            if (state)
               // menuPanel.GetComponent<Transformer>().MoveUI(new Vector2(0, 0), 0.5f);
            else
               // menuPanel.GetComponent<Transformer>().MoveUI(new Vector2(-canvasWidth, 0), 0.5f);*/
    }

    public void switchSound()
    {
        PlayerPrefs.SetInt("sound", PlayerPrefs.GetInt("sound", 1) == 1 ? 0 : 1);
        soundToggle.isOn = PlayerPrefs.GetInt("sound", 1) == 1 ? true : false;
        AudioListener.volume = PlayerPrefs.GetInt("sound", 1);
        //Debug.Log("volume set to "+ AudioListener.volume);
    }

    public void ChangePlayerCount(int delta)
    {
        if (playersCount + delta > 4)
            playersCount = 2;
        else if (playersCount + delta < 2)
            playersCount = 4;
        else
            playersCount += delta;
        inputPlayersCount.text = playersCount.ToString();
    }

    public void ResetData()
    {
        foreach (GameObject go in BoardTiles) Destroy(go);
        BoardTiles = new List<GameObject>();

        foreach (GameObject go in BoardSlots)
        {
            Debug.Log("currentslot.free : ");

            go.GetComponentInChildren<BoardSlot>().free = true;
            go.GetComponentInChildren<BoardSlot>().completed = false;
        }

        for (int i = 0; i < 1; i++)
        {
            players[i].score = 0;
            players[i].active = false;
            players[i].complete = false;
            players[i].scoreTxt.text = "-";
            players[i].UITiles = new List<GameObject>();
        }

        currentPlayer = 0;
        addedWords = new List<string>();
        Alphabet.data.ResetFeed();
        _BoardTileParent.Clear();
        Alphabet.data.Traydata.Clear();
        // Debug.Log(" Letter feed count.... " + Alphabet.data.LettersFeed.Count);
    }

    public void GoToMainMenu()
    {
        mainMenu.SetActive(true);
        GameOverUI.SetActive(false);
    }

    public void GameOver()
    {
        SoundController.data.playFinish();
        if (playersCount == 1)
        {
            GameOverUI.SetActive(true);
            gameOverText.text = "PLAYER " + currentPlayer + " WINS!";
            return;
        }
        else
        {
            int winnerPlayer = 0;
            int maxScore = 0;
            for (int i = 0; i <= 3; i++)
            {
                if (players[i].active && players[i].score > maxScore)
                {
                    maxScore = players[i].score;
                    winnerPlayer = i + 1;
                }
            }

            GameOverUI.SetActive(true);
            gameOverText.text = "PLAYER " + winnerPlayer + " WINS!";
        }
    }

    public bool CheckForGameOver()
    {
        foreach (PlayerData pd in players)
        {
            if (pd.complete) return true;
        }

        if (playersCount == 1 || skipCount == playersCount * 2)
            return true;
        else
            return false;
    }

//.......Reset all Value.......................//
    public void ResetAllValue()
    {
        // CancelLetters();
        // FitUIElements();
        // DisableSwapMode();
        Alphabet.data.ShowHighlighter();
        Alphabet.data.num1 = 0;
        Alphabet.data.TossLetter.Clear();
        LastUITiles.Clear();
        TotalboardTiles.Clear();
        UndoLetter.Clear();
        TrayLetter.Clear();

        //UI_Manager._instance.Timmer_Text.text =" ";

        foreach (GameObject child in Alphabet.data.Traydata)
        {
            GameObject.Destroy(child.gameObject);
        }

        TwoTimeLetter = 0;
        ThreeTimeLetter = 0;
        TwoTimeWord = 0;
        ThreeTimeWord = 0;
        WordScore = 0;
        PreviousScore = 0;
        undoAllScore = 0;
        undo = -1;
        isReCreateTile = false;
        // WordScore_Text.text = " ";
        IntUndoLetter = -1;
        IntTrayLetter = -1;
        previousCurrentScore = 0;
        isTileOnBoard = false;
        Alphabet.data.intTrayGameObjectName = -1;
        Alphabet.data.TrayGameObjectName.Clear();

        for (int i = 0; i < bonousChip.Length; i++)
        {
            bonousChip[i].SetActive(false);
        }

        /*  for (int i = 0; i < WordText.Length; i++)
          {
              WordText[i].text = string.Empty;
              Scoretext[i].text = string.Empty;
              WordText[i].gameObject.SetActive(false);
              Scoretext[i].gameObject.SetActive(false);
          }*/

        //Alphabet.data.LetterStackList.Clear();
        if (gameType == GameType.SinglePlayer)
        {
            // _gameUi._textUi.GameTypeTxt.text = GlobalData.GameType;
            _gameUi._textUi.GameModeTxt.text = GlobalData.GameMode;
            _gameUi._textUi.TimeDateTxt.text = GlobalData.GameDate;
            _gameUi._textUi.GameIdTxt.text = "Game ID : " + GlobalData.GameId;
        }
    }

    public void OpenExitPanel()
    {
        ExitPanel.SetActive(true);
    }

    public void Load()
    {
        UI_Manager._instance.OpenHomeScreen();
    }

    public void ExitYesButton()
    {
        Debug.Log("exit");
        ResetData();
        ResetAllValue();
        ExitPanel.SetActive(false);
        UI_Manager._instance.OpenHomeScreen();
        UI_Manager._instance.CloseAllPanel(false);

        foreach (Transform item in Alphabet.data.AlphabetPanel.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (GameObject bTile in _BoardTileParent)
        {
            Destroy(bTile);
        }

        /* foreach (GameObject tile in UITiles)
         {
             Destroy(tile);
         }*/

        SaveData.data.ResumeData.BoardData.Clear();
        SaveData.data.ResumeData.PlayerHandData.Clear();
        SaveData.data.ResumeData.TrayDatas.Clear();
        SaveData.data.ResumeData.score = 0;
        SaveData.data.ResumeData.ThreeLetter = 0;
        SaveData.data.ResumeData.ThreeWord = 0;
        SaveData.data.ResumeData.TrayLetterIndex = 0;
        SaveData.data.ResumeData.TrayLetterNumber = 0;
        SaveData.data.ResumeData.TwoLetter = 0;
        SaveData.data.ResumeData.TwoWord = 0;


        if (gameType == GameType.Multiplayer)
        {
            PhotonNetwork.Disconnect();
            _multiplayergameType = MultiplayerGameType.nulls;
            Debug.Log("GameController._multiplayergameType..." + _multiplayergameType);
            _MultiplayerWordData.Clear();
        }
    }

    public void ExitNoButton()
    {
        ExitPanel.SetActive(false);
    }

    public void SearchHeighScore()
    {
        if (_MultiplayerWordScoreData.Count > 0)
        {
            int number = _MultiplayerWordScoreData.ElementAt(0).Value;
            int Keynum = _MultiplayerWordScoreData.ElementAt(0).Key;

            for (int i = 1; i < _MultiplayerWordScoreData.Count; i++)
            {
                int temp = _MultiplayerWordScoreData.ElementAt(i).Value;

                if (number < temp)
                {
                    number = temp;
                    Keynum = _MultiplayerWordScoreData.ElementAt(i).Key;
                }
            }

            if (PhotonNetwork.IsMasterClient)
            {
                // Previousdata = _MultiplayerWordData[Keynum];

                photonView.RPC("FillMultiplayerBoarData", RpcTarget.All, Keynum);
            }
        }
    }

    private void MultiplayerWinnerDeclare()
    {
        if (_MultiplayerWordScoreData.Count > 0)
        {
            int number = _MultiplayerWordScoreData.ElementAt(0).Value;
            int Keynum = _MultiplayerWordScoreData.ElementAt(0).Key;

            for (int i = 1; i < _MultiplayerWordScoreData.Count; i++)
            {
                int temp = _MultiplayerWordScoreData.ElementAt(i).Value;

                if (number < temp)
                {
                    number = temp;
                    Keynum = _MultiplayerWordData.ElementAt(i).Key;
                }
            }
        }
    }

    private void MultiPreApply()
    {
        errorCode = 0;
        preApplyInfo = "";
        newScoreBlock.SetActive(false);
        newLetterIds = new List<int>();
        newWords = new List<string>();
        _wordComboses = new List<WordCombos>();
        newScore = new List<int>();
        boardTilesMatters = new List<GameObject>();
        bool firstWord = true;
        //Checking if tiles are not alone

        for (int i = 0; i < BoardSlots.Count; i++)
        {
            // Debug.LogFormat("free {0}---->",  BoardSlots[i].GetComponent<BoardSlot>().free);
            // Debug.LogFormat("completed {0}---->",  BoardSlots[i].GetComponent<BoardSlot>().completed);

            if (!BoardSlots[i].GetComponent<BoardSlot>().free && !BoardSlots[i].GetComponent<BoardSlot>().completed)
            {
                // Debug.LogFormat("CheckIfNewTileConnected {0}---->{1}",i,CheckIfNewTileConnected(i));
                if (!CheckIfNewTileConnected(i))
                {
                    TurnScore = 0;
                    ClearErrorData();

                    Debug.Log(" ///////////// ");
                    errorCode = 1;
                    return;
                }

                newLetterIds.Add(i);
                Debug.Log(" \\\\\\\\\\\\ ");
            }

            if (BoardSlots[i].GetComponent<BoardSlot>().completed && firstWord)
            {
                firstWord = false;
                // Debug.Log(" eeeeeeeeeeeeeee ");
            }
        }

        if (newLetterIds.Count == 0) return;

        //Check if first word intersects center tile

        if (firstWord)
        {
            bool correct = false;
            foreach (int id in newLetterIds)
            {
                if (id == 112) correct = true;
            }

            if (!correct)
            {
                errorCode = 2;
                return;
            }
        }

        //Checking if new tiles are at one line
        int prevX = 0;
        int prevY = 0;
        bool horizontal = false;
        bool vertical = false;
        is_vertical = false;
        is_horizontal = false;

        foreach (int id in newLetterIds)
        {
            int x = id / 15 + 1;
            int y = (id + 1) % 15 == 0 ? 15 : (id + 1) % 15;

            if (prevX == 0 && prevY == 0)
            {
                prevX = x;
                prevY = y;
            }
            else
            {
                if (x == prevX && !vertical)
                {
                    // Debug.Log(" ///////////// ");
                    horizontal = true;
                    is_horizontal = true;
                }
                else if (y == prevY && !horizontal)
                {
                    // Debug.Log(" \\\\\\\\\\/ ");
                    vertical = true;
                    is_vertical = true;
                }
                else
                {
                    errorCode = 3;
                    return;
                }
            }
        }

        //Checking if a free space between letters
        int firstNewId = newLetterIds[0];
        if (horizontal)
        {
            for (int i = firstNewId; i < newLetterIds[newLetterIds.Count - 1]; i++)
            {
                if (BoardSlots[i].GetComponent<BoardSlot>().free)
                {
                    errorCode = 4;
                    return;
                }
            }
        }

        //Check if new tile contact old tile
        bool haveConnect = false;

        foreach (int id in newLetterIds)
        {
            if (CheckIfNewTileConnectsOld(id))
            {
                haveConnect = true;
                Debug.Log(" 111111 ");
            }
        }

        if (!haveConnect && !firstWord)
        {
            errorCode = 5;
            return;
        }

        if (is_recalculateCheck != true)
        {
            is_BonousRecalculate = true;
        }

        //Buildig words and scores

        currentScore = 0;
        newWords = new List<string>();
        _wordComboses = new List<WordCombos>();
        newScore = new List<int>();
        foreach (int id in newLetterIds)
        {
            Debug.Log("Vertical word..............");
            int i;
            for (i = id; i > 0; i -= 15)
            {
                GameObject topSlot = GetАdjacentSlot(i, "top");
                if (!topSlot || topSlot.GetComponent<BoardSlot>().free) break;
            }

            string verticalWord = VerticalWord(i, out List<int> newIds);

            if (verticalWord.Length > 1 && !CheckWordAlreadyExist(verticalWord, newIds))
            {
                int scores = 0;

                newWords.Add(verticalWord);
                _wordComboses.Add(new WordCombos(verticalWord, newIds));
                scores = GetVerticalScore(i);
                currentScore += scores;
                newScore.Add(scores);
                foreach (GameObject item in _BoardTileParent)
                {
                    Debug.Log("Vertical word.............." + item);
                }

                //  Debug.Log("vertical.........."+vertical +"horizontal..... "+horizontal +"GetVerticalScore............."+currentScore);

                // Debug.Log("GetVerticalScore.........."+VerticalScore);
                if (vertical == true)
                {
                    // Debug.Log("GetVerticalScore.........."+VerticalScore);

                    //VerticalScore=currentScore;

                    if (isCheck != true)
                    {
                        TurnScore = VerticalScore;
                        TurnScore_Text.text = "Turn Score " + TurnScore.ToString();
                    }

                    //Debug.Log("isthreeBonous.........."+isthreeBonous +"istwoBonous..... "+istwoBonous +"GetVerticalScore............."+scores);
                }

                int y;
                for (y = id; y > 0; y--)
                {
                    GameObject leftSlot = GetАdjacentSlot(y, "left");
                    if (!leftSlot || leftSlot.GetComponent<BoardSlot>().free) break;
                }

                string _horizontalword = HorizontalWord(y, out List<int> hWordIDs);

                if (_horizontalword.Length > 1 && !CheckWordAlreadyExist(_horizontalword, hWordIDs))
                {
                    int score = 0;
                    newWords.Add(_horizontalword);
                    _wordComboses.Add(new WordCombos(_horizontalword, hWordIDs));
                    // newScore.Add(GetHorizontalScore(y));
                    score = GetHorizontalScore(y);
                    currentScore += score;
                    newScore.Add(score);
                    foreach (string item in newWords)
                    {
                        // Debug.Log("Horizontal word.............."+item);
                    }

                    if (horizontal == true)
                        // {
                        //Debug.Log("HorizontalScore......... "+HorizontalScore);

                        // Debug.Log("GetHorizontalScore.........."+HorizontalScore);
                        // HorizontalScore=currentScore;
                        if (isCheck != true)
                        {
                            TurnScore = HorizontalScore;
                            TurnScore_Text.text = "Turn Score " + TurnScore.ToString(); //
                        }
                }
            }

            string newWordsList = "";
            int w = 0;
            int s = 0;
            for (int j = 0; j < WordText.Length; j++)
            {
                WordText[j].text = string.Empty;
                Scoretext[j].text = string.Empty;
            }

/*
            foreach (string word in newWords)
            {
                newWordsList += word + ", ";
                WordText[w].gameObject.SetActive(true);
                WordText[w].text = word;
                Debug.Log("word........" + word);
                w++;
            }*/
            // foreach (int word in newScore)
            // {
            //     Scoretext[s].gameObject.SetActive(true);
            //     // newWordsList += word + ", ";
            //     Scoretext[s].text = word.ToString();
            //     // Debug.Log(" currentScore..........................."+word);
            //     s++;
            // }
            // if (newWordsList !=null)
            //newWordsList = newWordsList.Remove(newWordsList.Length - 2);

            //preApplyInfo = "APPLY '" + newWordsList + "' for " + currentScore + " scores?";

            // newScoreBlock.SetActive(true);
            //newScoreBlock.transform.position = boardTilesMatters[boardTilesMatters.Count - 1].transform.position + new Vector3(0.5f, -0.5f, -0.4f);
            //newScoreBlock.GetComponentInChildren<TextMesh>().text = currentScore.ToString();
            //newScoreBlock.GetComponent<Transformer>().ScaleImpulse(new Vector3(0.6f, 0.6f, 1), 0.15f, 1);
            is_recalculateCheck = false;
        }
    }

    public void SetmultiplayerBoardData(int _index)
    {
        /*  Previousdata = _MultiplayerWordData[_index];
          Debug.Log("Previousdata..."   + Previousdata);*/

        Color color;
        int counter = 0;


        for (int j = 0; j < _MultiplayerWordData[_index].BoardData.Count; j++)
        {
            foreach (GameObject tile in BoardSlots)
            {
                if (_MultiplayerWordData[_index].BoardData.Count > 0)
                    // Debug.Log("_GetResumeData.result.BoardData[i].tilename...."+_GetResumeData.result.BoardData[i].tilename  +"tile.name........."+tile.name );
                    if (tile.name == _MultiplayerWordData[_index].BoardData[j].tilename)
                    {
                        Debug.Log("_GetResumeData.BoardData[i].tilename....." + _MultiplayerWordData[_index].BoardData[j].tilename);
                        GameObject instandata = (GameObject)Instantiate(boardTilePrefab, new Vector3(99, 0, 0),
                            Quaternion.identity);
                        instandata.tag = "BoardTile";
                        instandata.transform.parent = tile.transform;
                        instandata.transform.localPosition = new Vector3(0, 0, -0.1f);
                        instandata.name = _MultiplayerWordData[_index].BoardData[j].letter;
                        GameController.data.BoardTiles.Add(instandata);
                        ColorUtility.TryParseHtmlString("#FAFD00", out color);
                        instandata.GetComponent<SpriteRenderer>().color = color;
                        instandata.GetComponent<BoardTile>().completed = true;
                        instandata.GetComponent<BoardTile>().currentslot =
                            instandata.transform.parent.GetComponent<BoardSlot>();
                        instandata.transform.parent.GetComponent<BoardSlot>().free = false;
                        tile.transform.GetComponent<BoardSlot>().completed = false;
                        //PreApply(true);
                        PreApply(true);
                        instandata.GetComponent<BoardTile>().UIclone = gameObject;
                        instandata.GetComponent<BoardTile>().letter = _MultiplayerWordData[_index].BoardData[j].letter;
                        instandata.GetComponent<BoardTile>().score = int.Parse(_MultiplayerWordData[_index].BoardData[j].score.ToString());
                        TextMesh[] txts = instandata.GetComponentsInChildren<TextMesh>();
                        txts[0].text = _MultiplayerWordData[_index].BoardData[j].letter;
                        txts[1].text = _MultiplayerWordData[_index].BoardData[j].score.ToString();
                        txts[2].text = _MultiplayerWordData[_index].BoardData[j].bonousname;
                        instandata.transform.parent.GetComponent<BoardSlot>().completed = true;
                        instandata.transform.parent.GetComponent<BoardSlot>().free = false;
                        instandata.SetActive(true);
                        // i++;
                        counter++;
                        break;
                    }
            }


            // foreach (GameObject tile in BoardSlots)
            // {
            //     if (_MultiplayerWordData[_index].BoardData.Count > 0)
            //     {
            //         // Debug.Log("i_MultiplayerWordData[_index].BoardData[i].tilename....."+_MultiplayerWordData[_index].BoardData[i].tilename+"_BoardTileParent.Count..."+tile.name);
            //
            //         if (tile.name == _MultiplayerWordData[_index].BoardData[j].tilename)
            //         {
            //             // Debug.Log("i....."+i+"_BoardTileParent.Count..."+tile.name);
            //             GameObject instandata =
            //                 (GameObject) Instantiate(boardTilePrefab, new Vector3(99, 0, 0), Quaternion.identity);
            //             instandata.tag = "BoardTile";
            //             instandata.transform.parent = tile.transform;
            //             instandata.transform.localPosition = new Vector3(0, 0, -0.1f);
            //             instandata.name = _MultiplayerWordData[_index].BoardData[j].letter;
            //             GameController.data.BoardTiles.Add(instandata);
            //             ColorUtility.TryParseHtmlString("#FAFD00", out color);
            //             instandata.GetComponent<SpriteRenderer>().color = color;
            //             instandata.GetComponent<BoardTile>().completed = true;
            //             instandata.transform.parent.GetComponent<BoardSlot>().free = false;
            //             tile.transform.GetComponent<BoardSlot>().completed = false;
            //
            //             instandata.GetComponent<BoardTile>().UIclone = gameObject;
            //             instandata.GetComponent<BoardTile>().letter = _MultiplayerWordData[_index].BoardData[j].letter;
            //             instandata.GetComponent<BoardTile>().score =
            //                 int.Parse(_MultiplayerWordData[_index].BoardData[j].score);
            //             PreApply(true);
            //             TextMesh[] txts = instandata.GetComponentsInChildren<TextMesh>();
            //             txts[0].text = _MultiplayerWordData[_index].BoardData[j].letter;
            //             TurnLetter.Add(_MultiplayerWordData[_index].BoardData[j].letter);
            //             txts[1].text = _MultiplayerWordData[_index].BoardData[j].score;
            //             txts[2].text = _MultiplayerWordData[_index].BoardData[j].bonousname;
            //
            //             instandata.transform.parent.GetComponent<BoardSlot>().free = false;
            //
            //             instandata.SetActive(true);
            //           
            //         }
            //     }
        }

        // Debug.Log(" NewlETTER count...." + newLetterIds.Count);
        // foreach (int id in newLetterIds)
        // {
        //     BoardSlots[id].GetComponent<BoardSlot>().completed = true;
        //     BoardSlots[id].GetComponentInChildren<BoardTile>().completed = true;
        // }
        UI_Manager._instance.acquireTile.text = "Tile : " + counter.ToString();

        _MultiplayerWordData.Clear();
        _MultiplayerWordScoreData.Clear();
        _MultiplayerTurnWordData.Clear();
    }

    //...........Fill BoardData After ResumeData......................//
    [PunRPC]
    private void FillMultiplayerBoarData(int _index)
    {
        newScoreBlock.SetActive(false);
        Debug.Log("kkkkkkkkkkkk");
        Debug.Log("Keynum..." + _index + "  " + _index);
        string Json = JsonConvert.SerializeObject(_MultiplayerWordData[_index], jsonSettings);
        Debug.LogFormat("Json {0}", Json);
        Previousdata = Encoding.ASCII.GetBytes(Json);

        TurnLetter = new List<string>();

        foreach (var item in _MultiplayerWordData[_index].BoardData)
        {
            //  Debug.Log("value ..." + item.letter + "tile name..." + item.tilename);
            TurnLetter.Add(item.letter);
        }

        foreach (GameObject item in BoardTiles)
        {
            Destroy(item);
        }

        BoardTiles.Clear();
        /*  foreach (GameObject item in _BoardTileParent)
          {
              Destroy(item);
          }*/

        _BoardTileParent.Clear();

        foreach (GameObject item in BoardSlots)
        {
            Debug.Log("currentslot.free : ");

            item.GetComponent<BoardSlot>().free = true;
            item.GetComponent<BoardSlot>().completed = false;
        }

        isclickable = false;
        UI_Manager._instance.AfterCompleteWord(false);
        UI_Manager._instance.MultiplayerTurnResult(_index);
        UI_Manager._instance.GetMultiplayerTurnPanel(true);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetMultiplayerhandData", RpcTarget.All, _index);
        }

        //_MultiplayerWordData.Clear();
    }

    [PunRPC]
    public void SetMultiplayerhandData(int _index)
    {
        Debug.Log("index.." + _index);
        int i = 0;
        foreach (GameObject tile in UITiles)
        {
            tile.GetComponent<UITile>().letterString.text = _MultiplayerWordData[_index].PlayerHandData[i].letter;
            tile.GetComponent<UITile>().letterScore.text = _MultiplayerWordData[_index].PlayerHandData[i].score.ToString();
            tile.GetComponent<UITile>()
                .CreateNewBoardTile(_MultiplayerWordData[_index].PlayerHandData[i].letter,
                    int.Parse(_MultiplayerWordData[_index].PlayerHandData[i].score.ToString()));

            i++;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetMultiplayerTrayData", RpcTarget.All, _index);
        }
    }

    [PunRPC]
    public void SetMultiplayerTrayData(int _index)
    {
        Alphabet.data.FillMultiplayerTrayData(_index, _MultiplayerWordData);
    }

//...............Player Word Complete...............//
    private void PlayerWordComplete(int _ActorNumber)
    {
        Dictionary<int, GameStateData>.KeyCollection keys = _MultiplayerWordData.Keys;

        foreach (int item in keys)
        {
            if (item == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                Debug.Log(_ActorNumber);
                if (PhotonNetwork.CurrentRoom.PlayerCount != 1)
                {
                    isclickable = true;
                    UI_Manager._instance.AfterCompleteWord(true);
                    // UI_Manager._instance.RoundStartTimmer(false);
                }
                // UI_Manager._instance.RoundStartTimmer(false);
            }
        }

        /* if(PhotonNetwork.LocalPlayer.ActorNumber ==_ActorNumber)
         {

             if(PhotonNetwork.IsMasterClient)
            {
           Debug.Log(" not called");
           Invoke("SearchHeighScore",30);
          }
         }
         else{
               UI_Manager._instance.Timmer();

         }*/
    }
} //End of GameController class