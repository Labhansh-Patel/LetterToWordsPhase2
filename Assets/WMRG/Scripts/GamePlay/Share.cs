using UnityEngine;
using UnityEngine.UI;

public class Share : MonoBehaviour
{
    [SerializeField] private Button shareBtn;
    [SerializeField] private Button privateCreateBtn;


    // Start is called before the first frame update
    void Start()
    {
        shareBtn.onClick.AddListener(ShareApp);
        privateCreateBtn.onClick.RemoveAllListeners();
        privateCreateBtn.onClick.AddListener(ShareApp);
    }

    private void ShareApp()
    {
        /*// UI_Manager._instance.PrivateRoomCreate();

         NativeShare nativeShare = new NativeShare();
         nativeShare.SetSubject("TestShare");
         nativeShare.SetTitle("TestTitle");
         nativeShare.SetText(
             UI_Manager._instance.User_Name +" invited you Room Name is : " + UI_Manager._instance.PrivateRoomName);
            // "Dowload test link : https://play.google.com/store/apps/details?id=com.supercell.clashroyale");
         nativeShare.Share();*/
    }
}