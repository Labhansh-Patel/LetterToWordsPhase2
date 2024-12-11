using Gameplay;
using UnityEngine;

public class AddFreeCoinsTesting : MonoBehaviour
{
    [SerializeField] private GamePlayController gamePlayControllerRef;
    [SerializeField] private int coinAmount;

    public void FreeCoinsBtn()
    {
        gamePlayControllerRef.BonusController.OutGameBonusCount += coinAmount;
        CommonApi.CallSetOutGameBonusApi(GlobalData.UserId, gamePlayControllerRef.BonusController.OutGameBonusCount.ToString());
        HandleEvents.PopoupErrorMsgOpen("Bonus points added");
    }
}