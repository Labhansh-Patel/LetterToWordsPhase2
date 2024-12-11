using System.Collections.Generic;
using APICalls;
using GameEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    [RequireComponent(typeof(GamePlayController))]
    public class GamePlayControllerUI : MonoBehaviour
    {
        [SerializeField] private bool completeGame;

        public Button wordScoreBtn;
        public Text Wordscore;

        public Text turnScore;

        public Text tossLetterList;
        [SerializeField] private Text errorText;

        public Button undoBtn;

        public Button undoAllBtn;

        public Button homeBtn;

        public Button tossBinBtn;
        public Button wordsListBtn;

        public TextMeshProUGUI gameDetails;
        public TextMeshProUGUI gameTime;
        [SerializeField] private TextMeshProUGUI gridCount;

        //public Text connectedPlayerText;


        public GameObject BonusTilePanel;
        public GameObject selectTilePanel;
        public GameObject gameOverPanel;
        public GameObject tossLetterListPanel;
        public GameObject wordsListPanel;
        [SerializeField] private GameObject GameErrorPanel;


        public Transform bonusTileParent;
        public Transform selectLetterTileParent;
        [SerializeField] private GameObject wordScorePrefab;
        [SerializeField] private Transform wordScoreParent;

        [SerializeField] private GamePlayController _gamePlayController;


        [SerializeField] private GameObject roundEndPanel;
        [SerializeField] private TextMeshProUGUI roundEndScoreCount;
        [SerializeField] private GameObject scoreUpdateParticle;
        [SerializeField] private Text roundWordsText;
        [SerializeField] private Text tossedLetterText;
        [SerializeField] private Text scoreText;
        [SerializeField] private GameObject tossPanel;
        [SerializeField] private GameObject forceGameEndPanel;
        [SerializeField] private GameObject gameExitPanel;


        private int trashIndex = 0;
        private int letterBlockIndex = 0;
        private int letterIndex = 0;
        private HighlightUI highlightUITossBin;
        private int currentScore;
        private List<ValidLetterBlocks> gridControllerGetValidLetterBlocks;

        private bool gamecompleted;

        private void Start()
        {
            EventHandlerGame.ShowPopup += SetErrorText;
            EventHandlerGame.GridCountUpdate += UpdateGridUI;
            EventHandlerGame.UpdateScore += UpdateScore;
            EventHandlerGame.UpdateWordList += UpdateWordList;
            EventHandlerGame.CreateGame += SetGameDetailsUI;
            AddListener();
        }

        private void AddListener()
        {
            tossBinBtn.onClick.RemoveAllListeners();
            tossBinBtn.onClick.AddListener(OpenTossBinPanel);
            wordsListBtn.onClick.RemoveAllListeners();

            wordsListBtn.onClick.AddListener(OpenWordsListPanel);

            wordScoreBtn.onClick.RemoveAllListeners();
            wordScoreBtn.onClick.AddListener(RoundValidCheck);
        }

        public void ToggleCompleteGame(bool enabled)
        {
            completeGame = enabled;
            Debug.Log(completeGame);
        }

        private void RoundValidCheck()
        {
            CommonApi.CallSetOutGameBonusApi(GlobalData.UserId, GamePlayController.Instance.BonusController.OutGameBonusCount.ToString());
            if (completeGame)
            {
                // isGameStatus = true;
                // UI_Manager._instance.singlePlayersesult("You Won this round");
                CommonApi.CallResultApi(GlobalData.GameId.ToString(), GlobalData.UserId, "1", 0);
                return;
            }

            _gamePlayController.RoundDone();
        }

        private void OnDisable()
        {
            EventHandlerGame.ShowPopup -= SetErrorText;
            EventHandlerGame.GridCountUpdate -= UpdateGridUI;
            EventHandlerGame.UpdateScore -= UpdateScore;
            EventHandlerGame.UpdateWordList -= UpdateWordList;
            EventHandlerGame.CreateGame -= SetGameDetailsUI;
        }

        public void SetErrorText(string error)
        {
            errorText.text = error;
            GameErrorPanel.SetActive(true);
        }

        private void UpdateGridUI()
        {
            gridCount.text = _gamePlayController.GridController.GetAddedLetterCount + "/61";
        }

        private void UpdateScore(int turnScore)
        {
            turnScore -= _gamePlayController.MinusScore;
            this.turnScore.text = "TurnScore \t" + turnScore.ToString();
        }

        private void UpdateWordScore(CallBack callBack = null)
        {
            int turnScore;
            int tempwordScore = _gamePlayController.WordScore + _gamePlayController.TurnScore;
            Wordscore.text = "WordScore \t" + tempwordScore.ToString();
            callBack();
            roundEndPanel.gameObject.SetActive(false);
        }

        private void OpenTossBinPanel()
        {
            string tosstext = string.Empty;

            foreach (var tossedLetter in _gamePlayController.TossedLettersCurrentRound)
            {
                tosstext += tossedLetter.letter + " " + tossedLetter.score + ", ";
            }

            tosstext = tosstext.Substring(0, tosstext.Length - 2);

            tossLetterList.text = tosstext;
            tossLetterListPanel.SetActive(!tossLetterListPanel.activeInHierarchy);
        }

        private void OpenWordsListPanel()
        {
            wordsListPanel.SetActive(!wordsListPanel.activeInHierarchy);
            tossBinBtn.gameObject.SetActive(!tossBinBtn.gameObject.activeInHierarchy);
        }

        public void ClearScoreWordsList()
        {
            foreach (Transform c in wordScoreParent)
            {
                Destroy(c.gameObject);
            }
        }

        private void UpdateWordList(List<ValidLetterBlocks> newLetterBlocksList)
        {
            //wordList.text = string.Empty;
            if (newLetterBlocksList == null)
            {
                return;
            }

            foreach (Transform c in wordScoreParent)
            {
                Destroy(c.gameObject);
            }

            foreach (var validLetterBlock in newLetterBlocksList)
            {
                GameObject go = Instantiate(wordScorePrefab, transform.position, Quaternion.identity);
                go.transform.SetParent(wordScoreParent);
                go.transform.GetChild(0).GetComponent<Text>().text = validLetterBlock.ToString();
                go.transform.GetChild(1).GetComponent<Text>().text = validLetterBlock.GetWordScore().ToString();
                //  wordList.text += validLetterBlock + "\t\t" + validLetterBlock.GetWordScore() + "\n";
            }
        }

        private void SetGameDetailsUI(bool asucess, string date, CreateGameData stack)
        {
            gameDetails.text = GlobalData.GameId + " / " + GlobalData.GameType + " / " + GlobalData.GameMode;
            gameTime.text = date;
        }


        private CallBack RoundEndUiCallback;
        private List<LetterBlock> currentRoundTossedTile;


        public void ShowRoundEndScore(List<ValidLetterBlocks> gridControllerGetValidLetterBlocks, List<LetterBlock> currentRoundTossedTile)
        {
            roundWordsText.text = GetWordsText(gridControllerGetValidLetterBlocks);
            scoreText.text = GetWordScoreText(gridControllerGetValidLetterBlocks);
            tossedLetterText.text = GetTossedText(currentRoundTossedTile);
            roundEndPanel.gameObject.SetActive(true);
        }

        public void SetWordScore()
        {
            Wordscore.text = "Word Score \n" + _gamePlayController.WordScore.ToString();
        }

        private string GetTossedText(List<LetterBlock> letterBlocks)
        {
            string tossedText = string.Empty;

            foreach (var letterBlock in letterBlocks)
            {
                tossedText += letterBlock.letter + "\t";
            }

            return tossedText;
        }

        private string GetWordsText(List<ValidLetterBlocks> validLetterBlocksList)
        {
            string wordslist = string.Empty;
            foreach (var validLetterBlock in validLetterBlocksList)
            {
                wordslist += validLetterBlock.ToString() + "\n";
            }

            return wordslist;
        }

        private string GetWordScoreText(List<ValidLetterBlocks> validLetterBlocksList)
        {
            string wordScoreText = string.Empty;
            foreach (var validLetterBlock in validLetterBlocksList)
            {
                wordScoreText += validLetterBlock.GetWordScore() + "\n";
            }

            return wordScoreText;
        }

        public void ShowRoundEnd(List<ValidLetterBlocks> gridControllerGetValidLetterBlocks, List<LetterBlock> currentRoundTossedTile, CallBack callback)
        {
            this.RoundEndUiCallback = callback;
            this.currentRoundTossedTile = currentRoundTossedTile;
            currentScore = 0;
            roundEndScoreCount.text = currentScore.ToString();
            roundEndPanel.gameObject.SetActive(true);

            ShowHighlights(gridControllerGetValidLetterBlocks, currentRoundTossedTile, currentScore);
        }

        // [SerializeField] private ParticleSystem roundScoreBurst;

        private void ShowHighlights(List<ValidLetterBlocks> gridControllerGetValidLetterBlocks, List<LetterBlock> currentRoundTossedTile, int currentScore)
        {
            this.gridControllerGetValidLetterBlocks = gridControllerGetValidLetterBlocks;
            HighlightTrash(currentRoundTossedTile, currentScore);
        }

        private void HighlightLetters(List<ValidLetterBlocks> gridControllerGetValidLetterBlocks)
        {
            if (gridControllerGetValidLetterBlocks.Count > 0)
            {
                letterBlockIndex = 0;
                letterIndex = 0;
                MoveAlongLetter(gridControllerGetValidLetterBlocks);
            }
        }

        private void HighlightTrash(List<LetterBlock> currentRoundTossedTile, int currentScore)
        {
            highlightUITossBin = tossBinBtn.GetComponent<HighlightUI>();
            if (currentRoundTossedTile.Count > 0)
            {
                trashIndex = 0;
                ShowTrashTransitionsTrash(currentRoundTossedTile[trashIndex], currentScore, highlightUITossBin);
            }
            else
            {
                HighlightLetters(gridControllerGetValidLetterBlocks);
            }
        }

        private void MoveAlongLetter(List<ValidLetterBlocks> gridControllerGetValidLetterBlocks)
        {
            scoreUpdateParticle.transform.position = gridControllerGetValidLetterBlocks[letterBlockIndex]
                .validWords[letterIndex].TileSet.transform.position;
            scoreUpdateParticle.gameObject.SetActive(true);
            LeanTween.move(scoreUpdateParticle, gridControllerGetValidLetterBlocks[letterBlockIndex]
                    .validWords[letterIndex + 1].TileSet.transform.position, 0.2f)
                .setOnComplete(() => DoneLetterTransition(gridControllerGetValidLetterBlocks));
        }

        private void DoneLetterTransition(List<ValidLetterBlocks> gridControllerGetValidLetterBlocks)
        {
            //scoreUpdateParticle.gameObject.SetActive(false);
            //LetterGameAudioHolder.Instance.PlayTileSound(letterIndex);
            letterIndex++;

            if (letterIndex < gridControllerGetValidLetterBlocks[letterBlockIndex].validWords.Count - 1)
            {
                MoveAlongLetter(gridControllerGetValidLetterBlocks);
                return;
            }

            LeanTween.move(scoreUpdateParticle, roundEndScoreCount.transform, 0.7f)
                .setOnComplete(() => DoneLettersInBlock(gridControllerGetValidLetterBlocks));
        }

        private void DoneLettersInBlock(List<ValidLetterBlocks> gridControllerGetValidLetterBlocks)
        {
            scoreUpdateParticle.gameObject.SetActive(false);
            // roundScoreBurst.Play();


            UpdateRoundScore(gridControllerGetValidLetterBlocks[letterBlockIndex].GetWordScore());
            letterBlockIndex++;
            if (letterBlockIndex < gridControllerGetValidLetterBlocks.Count)
            {
                letterIndex = 0;
                MoveAlongLetter(gridControllerGetValidLetterBlocks);
                return;
            }

            HighlightFinalScoreUpdate();
        }

        private void HighlightFinalScoreUpdate()
        {
            scoreUpdateParticle.transform.position = roundEndScoreCount.transform.position;
            scoreUpdateParticle.gameObject.SetActive(true);
            LeanTween.move(scoreUpdateParticle, Wordscore.transform.position, 0.7f)
                .setOnComplete(() => UpdateWordScore(RoundEndUiCallback));
        }

        private void ShowTrashTransitionsTrash(LetterBlock currentRoundTossedTile, int currentScore,
            HighlightUI highlightUITossBin)
        {
            var tossedTile = currentRoundTossedTile;

            highlightUITossBin.ActivateHighlight(null);
            scoreUpdateParticle.transform.position = this.highlightUITossBin.transform.position;
            scoreUpdateParticle.gameObject.SetActive(true);
            LeanTween.move(scoreUpdateParticle, roundEndScoreCount.transform, 0.7f)
                .setOnComplete(() => DoneTrashTileMovement(tossedTile.score));
        }

        private void DoneTrashTileMovement(int tossedTileScore)
        {
            scoreUpdateParticle.gameObject.SetActive(false);
            //roundScoreBurst.Play();
            UpdateRoundScore(tossedTileScore);

            trashIndex++;
            if (trashIndex < currentRoundTossedTile.Count)
            {
                ShowTrashTransitionsTrash(currentRoundTossedTile[trashIndex], currentScore, highlightUITossBin);
                return;
            }

            HighlightLetters(gridControllerGetValidLetterBlocks);
        }

        private void UpdateRoundScore(int tossedTileScore)
        {
            currentScore -= tossedTileScore;
            roundEndScoreCount.text = currentScore.ToString();
        }

        public void ForceGameEnd()
        {
            forceGameEndPanel.gameObject.SetActive(true);
        }

        public void Initialize()
        {
            forceGameEndPanel.gameObject.SetActive(false);
            gameOverPanel.gameObject.SetActive(false);
            roundEndPanel.gameObject.SetActive(false);
            GameErrorPanel.gameObject.SetActive(false);
            gameExitPanel.gameObject.SetActive(false);
        }
    }
}