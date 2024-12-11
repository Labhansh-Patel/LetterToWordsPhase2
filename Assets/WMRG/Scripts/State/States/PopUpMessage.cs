public class PopUpMessage
{
    private GameUi gameUi;


    public PopUpMessage(GameUi gameUi)
    {
        this.gameUi = gameUi;
        PopUpListnerCalled();
    }

    private void PopUpListnerCalled()
    {
        HandleEvents.PopoupErrorMsg += CallPopErrorMsg;

        RemoveListeners();
        AddAllListeners();
    }

    private void RemoveListeners()
    {
        gameUi._buttonUi.ErrorOKBtn.onClick.RemoveAllListeners();
    }

    private void AddAllListeners()
    {
        gameUi._buttonUi.ErrorOKBtn.onClick.AddListener(PopupOkbtnClick);
    }

    private void PopupOkbtnClick()
    {
        gameUi._canvasUi.ErrorPopUp.SetActive(false);
    }

    private void CallPopErrorMsg(string Msg)
    {
        gameUi._textUi.PopoupMsgTxt.text = Msg;
        gameUi._canvasUi.ErrorPopUp.SetActive(true);
    }
}