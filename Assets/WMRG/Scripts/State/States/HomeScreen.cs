using APICalls;
using Gameplay;
using UnityEngine;

public class HomeScreen : IState
{
    private GameUi gameUi;

    public HomeScreen(GameUi gameUi)
    {
        this.gameUi = gameUi;
    }

    public void Enter()
    {
        gameUi._canvasUi.Home.SetActive(true);
        gameUi._canvasUi.DisableWaitingPanel();
        RemoveListeners();
        AddAllListeners();
        gameUi._textUi.HomeNameTxt.text = GlobalData.UserName;
        
        if (GlobalData.userData.IsSocialLogin)
        {
            if (GlobalData.userData.socialAvatarImage == null)
            {
                LogSystem.LogEvent("ImageUrl {0}", GlobalData.userData.user_image);
                WebHelpers.instance.get<Texture2D>(GlobalData.userData.user_image, SetSocialImage);
            }
            else
            {
                gameUi._imageUi.HomeProfileImage.sprite = GlobalData.userData.socialAvatarImage;
            }
        }
        else
        {
            gameUi._imageUi.HomeProfileImage.sprite = gameUi._imageUi.Avtar[GlobalData.UserAvtarId - 1];
        }
    }

    private void SetSocialImage(string aurl, bool asuccess, object adata)
    {
        Texture2D tex = (Texture2D)adata;

        Sprite img = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        GlobalData.userData.socialAvatarImage = img;
        gameUi._imageUi.HomeProfileImage.sprite = img;
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.Home.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.SoloBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.MultiPlayerGameBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.SettingBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.ProfileBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.Purchase.onClick.RemoveAllListeners();
        gameUi._buttonUi.ExitBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.AchivementBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.SoloBtn.onClick.AddListener(SoloBtnClick);
        gameUi._buttonUi.SettingBtn.onClick.AddListener(SettingBtnClick);
        gameUi._buttonUi.ProfileBtn.onClick.AddListener(ProfileClick);
        gameUi._buttonUi.MultiPlayerGameBtn.onClick.AddListener(MultiPlayerGameBtnClick);
        gameUi._buttonUi.Purchase.onClick.AddListener(PurchaseBtnClick);
        gameUi._buttonUi.ExitBtn.onClick.AddListener(ExitBtnClick);

        gameUi._buttonUi.AchivementBtn.onClick.AddListener(AchivementBtnClick);
    }

    private void AchivementBtnClick()
    {
        HandleEvents.ChangeStates(States.achivement);
    }

    private void ExitBtnClick()
    {
        Application.Quit();
    }

    private void PurchaseBtnClick()
    {
        HandleEvents.ChangeStates(States.purchase);
    }

    private void MultiPlayerGameBtnClick()
    {
        GameController.gameType = GameController.GameType.Multiplayer;
        GlobalData.GameType = GameType.MultiPlayer;
        HandleEvents.ChangeStates(States.multiplayOption);
    }

    private void SoloBtnClick()
    {
        GameController.gameType = GameController.GameType.SinglePlayer;
        GlobalData.GameType = GameType.SoloPlayer;
        HandleEvents.ChangeStates(States.selectSolo);
    }

    private void ProfileClick()
    {
        HandleEvents.ChangeStates(States.profile);
    }

    private void SettingBtnClick()
    {
        HandleEvents.ChangeStates(States.setting);
    }
}