using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using APICalls;
using Facebook.Unity;
using GameEvents;
using Newtonsoft.Json;
using UnityEngine;
using HttpMethod = Facebook.Unity.HttpMethod;

public class FacebookHelper 
{
    public class FacebookData
    {
        public string id;
        public string name;
    }
    
    
    private Action<FacebookData> FbLoginCallBack;

    public List<string> permissions = new List<string>(new[] { "public_profile", "email"});

    public FacebookHelper()
    {
        LogSystem.LogColorEvent("green","FbInit {0}", FB.IsInitialized);
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    
    
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            
            #if  UNITY_IOS
            
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED)
            {
                bool trackingflag  =  FB.Mobile.SetAdvertiserTrackingEnabled(true);
                LogSystem.LogEvent("TrackingFlag {0}", trackingflag);
            }
            else
            {
                bool trackingflag  =  FB.Mobile.SetAdvertiserTrackingEnabled(false);
                LogSystem.LogEvent("NotTrackingFlag {0}", trackingflag);
            }
           
            
            LogSystem.LogEvent("TrackingFlag {0}", ATTrackingStatusBinding.GetAuthorizationTrackingStatus() );

#endif
            FB.ActivateApp();
     
        }
        else
        {
			Debug.Log ("Failed to Initialize the Facebook SDK");
        }
    }
    
    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            //Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            //Time.timeScale = 1;
        }
    }

    public void LoginFacebookOperation(Action<FacebookData> callback)
    {
        if (callback != null)
        {
            FbLoginCallBack = callback;
        }
        LogSystem.LogEvent("Facebook Login Called");
        FacebookLogin();
    }

    private void FacebookLogin()
    {
        if (FB.IsLoggedIn)
        {
            FB.LogOut();
        }

        foreach (var permission in permissions)
        {
            LogSystem.LogEvent("Permissions {0}", permission);
        }
        FB.LogInWithReadPermissions(permissions, LoginCallback);
    }

    void LoginCallback(ILoginResult result)
    {
        LogSystem.LogEvent("OnLoggedIn {0}",FB.IsLoggedIn);
        if (FB.IsLoggedIn)
        {
            OnLoggedIn();
        }
        else
        {
            // ITargetViewer hideviewr = Displayer.Instance.GetTargetViewer(Type.GetType("LoadingPage")) as LoadingPage;
            // Displayer.Instance.Hide(hideviewr);
            //
            // Displayer.Instance.Display (new ErrorPopup.Popup ("Error Message " + result.RawResult.ToString (), () => {
            // }), null);
            EventHandlerGame.EmitEvent(GameEventType.Loading,false);
            if (!result.RawResult.ToString().ToLower().Contains("cancel"))
            {
                
                //Displayer.Instance.Display(
                //   new ErrorPopup.Popup("Error Message " + result.RawResult.ToString(), () => { }), null);
            }
            else
            {
                // PlayerPrefs.SetInt("FbLogin", 0);
                // PlayerPrefs.SetInt("Remember", 0);
                // PlayerPrefs.SetString("userid", string.Empty);
                // PlayerPrefs.SetString("password", string.Empty); // todo reset playerPrefs
            }
        }
    }
    
    void OnLoggedIn()
    {
#if !UNITY_STANDALONE
       
        FB.API("/me?fields=id,name,email", HttpMethod.GET, GetUserData);
#endif
    }
    
    public void GetUserData(IGraphResult result)
    {
            LogSystem.LogEvent("Facebook Login Callback {0}",result.RawResult.ToString());

            FacebookData facebookData = JsonConvert.DeserializeObject<FacebookData>(result.RawResult);
            
            FbLoginCallBack?.Invoke(facebookData);
            
            //  Encoding unicode = new UnicodeEncoding();

            //LogSystem.LogColorEvent("green","Unicode Conversion {0}",  Encoding.UTF8.GetString(Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(result.RawResult.ToString()))));
            // JSONObject jsonas = new JSONObject(result.RawResult.ToString());
            // string id = jsonas.GetField("id").str; //id of a p[layer 
            // string playername = jsonas.GetField("name").str;
            //
            //  playername = Regex.Unescape(playername);
            // LogSystem.LogColorEvent("yellow","Playername {0}", playername);
            //
            //
            // string emailid = string.Empty;
            // if (jsonas.HasField("email"))
            // {
            //     emailid = result.ResultDictionary["email"].ToString();
            // }
            //
            // Debug.LogFormat("email Id {0}", emailid);
            // string name = playername;
            // string[] tmp = name.Split(' ');
            // string FirstName = tmp[0];
            // string LastName = tmp[1];
            // string firstname = FirstName;
            // string lastname = LastName;
            // playerfbid = id;
            // unique = id;
            //Mobile Login from facebook 

            // FbLoginCallBack?.Invoke(id, playername, emailid, false);
    }
    
    
    public static class UnicodeEncodingSet 
    {
        public static void CheckForUnicode(string playerName)
        {
            Encoding unicode = new UnicodeEncoding();
LogSystem.LogEvent("name {0}", playerName);
            // A Unicode string with two characters outside an 8-bit code range.
            string unicodeString =
                playerName;
            Console.WriteLine("Original string:");
            Console.WriteLine(unicodeString);
            Console.WriteLine();

            // Encode the string.
            Byte[] encodedBytes = unicode.GetBytes(unicodeString);
            Console.WriteLine("The encoded string has {0} bytes.\n",
                encodedBytes.Length);

            // Write the bytes to a file with a BOM.
            var fs = new FileStream(@".\UTF8Encoding.txt", FileMode.Create);
            Byte[] bom = unicode.GetPreamble();
            fs.Write(bom, 0, bom.Length);
            fs.Write(encodedBytes, 0, encodedBytes.Length);
            Console.WriteLine("Wrote {0} bytes to the file.\n", fs.Length);
            fs.Close();

            // Open the file using StreamReader.
            var sr = new StreamReader(@".\UTF8Encoding.txt");
            String newString = sr.ReadToEnd();
            sr.Close();
            Console.WriteLine("String read using StreamReader:");
            Console.WriteLine(newString);
            Console.WriteLine();

            // Open the file as a binary file and decode the bytes back to a string.
            fs = new FileStream(@".\UTF8Encoding.txt", FileMode.Open);
            Byte[] bytes = new Byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);
            fs.Close();

            String decodedString = unicode.GetString(bytes);
            Console.WriteLine("Decoded bytes:");
            LogSystem.LogColorEvent("yellow","PlayerName {0}", decodedString);
        }
    }
    

    }