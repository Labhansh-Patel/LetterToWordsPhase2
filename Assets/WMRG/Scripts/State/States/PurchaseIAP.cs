using APICalls;
using Gameplay;

public class PurchaseIAP : IState
{
    private GameUi gameUi;

    public PurchaseIAP(GameUi gameUi)
    {
        this.gameUi = gameUi;
    }

    public void Enter()
    {
        gameUi._canvasUi.Purchase.SetActive(true);
        gameUi._buttonUi.PremiumMemberPurchaseBtn.gameObject.SetActive(!GlobalData.userData.IsPremiumUser);
        gameUi._buttonUi.PremiumMemberDetailsBtn.gameObject.SetActive(GlobalData.userData.IsPremiumUser);
        RemoveListeners();
        AddAllListeners();
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        gameUi._canvasUi.Purchase.SetActive(false);
    }

    public void RemoveListeners()
    {
        gameUi._buttonUi.PurchaseBackBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.BuyBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.PremiumMemberPurchaseBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.PremiumMemberDetailsBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.CloseCoinPopUpBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.ClosePremiumPurchasePopUpBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.ClosePremiumDetailsPopUpBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.CanclePremiumBtn.onClick.RemoveAllListeners();
        gameUi._buttonUi.UnlockPremiumBtn.onClick.RemoveAllListeners();
    }

    public void AddAllListeners()
    {
        gameUi._buttonUi.PurchaseBackBtn.onClick.AddListener(PurchaseBackBtnClick);
        gameUi._buttonUi.BuyBtn.onClick.AddListener(BuyBtnClick);
        gameUi._buttonUi.PremiumMemberPurchaseBtn.onClick.AddListener(PremiumMemberPurchaseBtnClick);
        gameUi._buttonUi.PremiumMemberDetailsBtn.onClick.AddListener(PremiumMemberDetailsBtnClick);
        gameUi._buttonUi.CloseCoinPopUpBtn.onClick.AddListener(CloseCoinPopUpBtnClick);
        gameUi._buttonUi.ClosePremiumPurchasePopUpBtn.onClick.AddListener(ClosePremiumPurchasePopUpBtnClick);
        gameUi._buttonUi.ClosePremiumDetailsPopUpBtn.onClick.AddListener(ClosePremiumDetailsPopUpBtnClick);
        gameUi._buttonUi.UnlockPremiumBtn.onClick.AddListener(UnlockPremiumMembershipBtnClick);
        gameUi._buttonUi.CanclePremiumBtn.onClick.AddListener(CanclePremiumMembershipBtnClick);
    }

    private void PremiumMemberPurchaseBtnClick()
    {
        gameUi._canvasUi.PremiumMemberPurchasePopUp.SetActive(true);
    }

    private void PremiumMemberDetailsBtnClick()
    {
        gameUi._canvasUi.PremiumMemberDetailsPopUp.SetActive(true);
    }

    private void BuyBtnClick()
    {
        gameUi._canvasUi.CoinByPopUp.SetActive(true);
    }

    private void CloseCoinPopUpBtnClick()
    {
        gameUi._canvasUi.CoinByPopUp.SetActive(false);
    }

    private void ClosePremiumPurchasePopUpBtnClick()
    {
        gameUi._canvasUi.PremiumMemberPurchasePopUp.SetActive(false);
    }

    private void ClosePremiumDetailsPopUpBtnClick()
    {
        gameUi._canvasUi.PremiumMemberDetailsPopUp.SetActive(false);
    }

    private void UnlockPremiumMembershipBtnClick()
    {
        ApiManager.SetPremiumStatus(true, (aSuccess, data) =>
        {
            if (aSuccess)
            {
                HandleEvents.PopoupErrorMsgOpen(data.message);
                GlobalData.userData.premium_user = 1;
                gameUi._buttonUi.PremiumMemberPurchaseBtn.gameObject.SetActive(false);
                gameUi._buttonUi.PremiumMemberDetailsBtn.gameObject.SetActive(true);
            }
            else
            {
                HandleEvents.PopoupErrorMsgOpen(GameMessages.JoinMembershipRequestFailed);
            }

            ClosePremiumPurchasePopUpBtnClick();
        });
    }

    private void CanclePremiumMembershipBtnClick()
    {
        ApiManager.SetPremiumStatus(false, (aSuccess, data) =>
        {
            if (aSuccess)
            {
                HandleEvents.PopoupErrorMsgOpen(data.message);
                GlobalData.userData.premium_user = 0;
                gameUi._buttonUi.PremiumMemberDetailsBtn.gameObject.SetActive(false);
                gameUi._buttonUi.PremiumMemberPurchaseBtn.gameObject.SetActive(true);
            }
            else
            {
                HandleEvents.PopoupErrorMsgOpen(GameMessages.CancleMembershipRequestFailed);
            }

            ClosePremiumDetailsPopUpBtnClick();
        });
    }

    private void PurchaseBackBtnClick()
    {
        HandleEvents.BackToPreviousState();
    }
}