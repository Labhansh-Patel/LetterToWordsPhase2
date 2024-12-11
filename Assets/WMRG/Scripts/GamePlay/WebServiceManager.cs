// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.UI;
// using System.Text.RegularExpressions;
// using System.Text;
// using System.IO;
// using Newtonsoft;
// using Newtonsoft.Json;
// using System .Net;
// using UnityEngine.Networking;




// public class WebServiceManager : MonoBehaviour
// {

//     private static WebServiceManager _instance;


//     private const string BASE_URL = "https://webmobril.org/dev/letterofwords/api/v1/";


//     private const string LOGIN_URL = BASE_URL + "api_login";
//     private const string SIGNUP_URL = BASE_URL + "api_signup";
//     private const string FORGOTPASSWORD_URL = BASE_URL + "api_forgetpassword";
//     private const string OTP_URL = BASE_URL + "api_verifyotp";
//     private const string Resend_Otp = BASE_URL + "api_resendotp";
//     private const string Change_Password = BASE_URL + "api_resetpassword";
//     private const string Logout = BASE_URL + "api_logout";
//     private const string Get_Profile = BASE_URL + "api_getprofile?userid=";
//     private const string Update_Profile = BASE_URL + "api_updateprofile";
//     private const string Contact_Admin = BASE_URL + "api_contactus";
//     private const string Privacy_Policy = BASE_URL + "api_privacypolicy";
//     private const string Terms_Condition = BASE_URL + "api_tnc";
//     private const string User_Rank = BASE_URL + "api_getuserrank";
//     private const string FbLogin_URL = BASE_URL + "api_fblogin";
    
//     private const string TodayStack_URL = BASE_URL + "api_todaystack";

//     private const string  ResumeData_URL = BASE_URL +"api_letterboard";


    











//     void Awake()
//     {
//         if (_instance == null)
//         {
//             _instance = this;
//             DontDestroyOnLoad(this);
//         }
//         Application.runInBackground = true;
       
//     }




//     public void CallSignIn(string email, string pass)
//     {
//         StartCoroutine(UserLogin(email, pass));
//     }

//     IEnumerator UserLogin(string _email, string _pass)
//     {
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);
//         WWWForm webForm = new WWWForm();
//         webForm.AddField("email", _email);
//         webForm.AddField("password", _pass);
//         webForm.AddField("device_type", UI_Manager._instance.Device_Type);
//         webForm.AddField("device_token",UI_Manager._instance.Device_Token);
//          Debug.Log(LOGIN_URL + _email + _pass + UI_Manager._instance.Device_Type +UI_Manager._instance.Device_Token);
//         WWW www = new WWW(LOGIN_URL, webForm);
//         yield return www;
//         Debug.Log("service hit " + www.text);
//         if (www.error == null)
//         {
//             Debug.Log("service hit properly login-sdsdsd----" + www.text);
//             LoginHeader login = JsonConvert.DeserializeObject<LoginHeader>(www.text);
//             Debug.Log("id" + login.message);
//             if ((login.error == "true") || (login.error == "True"))
//             {
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(login.message);
//                 if (login.message == "Please verify your account first to login")   
//                 {
//                     Debug.Log("in this");
//                     UI_Manager._instance.OpenOtpScreen();
//                     UI_Manager._instance.User_Id = login.result.id;
//                     UI_Manager._instance.User_Token = login.result.token;
//                     UI_Manager._instance.SignEmailInput.text = _email;
//                     CallResendOtp(UI_Manager._instance.SignEmailInput.text);
//                     //PlayerPrefs.SetInt("Loginstatus", 1);
//                     //PlayerPrefs.SetString("PlayerId", login.result.id);
//                     //PlayerPrefs.SetString("PlayerName", );
//                 }
//                  GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);

//             }

//             else
//             {
//                 UI_Manager._instance.User_Id = login.result.id;
//                 UI_Manager._instance.User_Token = login.result.token;
//                // UI_Manager._instance.OpenMapScreen();
//                 PlayerPrefs.SetInt("Loginstatus", 1);
//                 PlayerPrefs.SetString("PlayerId", login.result.id);
//                 UI_Manager._instance.ProfileButtonPressed();
//                // UI_Manager._instance.HomeProfileName_text.text = login.result.name;
//                 UI_Manager._instance.OpenHomeScreen();
//             }

//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {
//             Debug.Log("error Generated login" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//     }


//  private void  OpenMap()
//     {
//         //UI_Manager._instance.OpenMapScreen();
//          GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//     }

//     public void CallSingUp(string name, string email, string pass, string confPswd)
//     {
//         StartCoroutine(SingUp(name, email, pass, confPswd));
//     }

//     IEnumerator SingUp(string name, string email, string pass, string confPswd)
//     {
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);

//         WWWForm webForm = new WWWForm();
//         webForm.AddField("email", email);
//         webForm.AddField("name", name);
//         webForm.AddField("password", pass);
//         webForm.AddField("cpassword", confPswd);
//         webForm.AddField("device_type", UI_Manager._instance.Device_Type);
//         webForm.AddField("device_token", UI_Manager._instance.Device_Token);
//         Debug.Log(LOGIN_URL + email + pass + name + "android" + "677564344rr566667677");
//         WWW www = new WWW(SIGNUP_URL, webForm);
//         yield return www;
//         if (www.error == null)
//         {
//             Debug.Log("service hit properly SingUp-sdsdsd----" + www.text);
//             SingUpHeader singup = JsonConvert.DeserializeObject<SingUpHeader>(www.text);

//             if ((singup.error == "true") || (singup.error == "True"))
//             {
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(singup.message);

//             }
//             else
//             {
//                // UI_Manager._instance.OpenOtpScreen();
//                 UI_Manager._instance.User_Signup_otp = singup.result.SingUp_Otp;
//                 UI_Manager._instance.User_Id = singup.result.id;
//                 UI_Manager._instance.User_Token = singup.result.token;
//                 UI_Manager._instance.User_Email = email;
//                  PlayerPrefs.SetInt("Loginstatus", 1);
//                 PlayerPrefs.SetString("PlayerId", singup.result.id);
//                 UI_Manager._instance.OpenOtpScreen();
                                                                      
//             }
//            GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {
//             Debug.Log("error Generated singup" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//     }




//     public void Call_ForgotPassword(string email)
//     {
//         StartCoroutine(ForgotPassword(email));
        
//     }

//     IEnumerator ForgotPassword(string email)
//     {
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);

//         WWWForm webForm = new WWWForm();
//         webForm.AddField("email", email);
//         webForm.AddField("device_type", UI_Manager._instance.Device_Type);
//         webForm.AddField("device_token", UI_Manager._instance.Device_Token);
//         Debug.Log("LOGIN_URL- " + email + "android" + "677564344rr566667677");
//         WWW www = new WWW(FORGOTPASSWORD_URL, webForm);
//         yield return www;
//         if (www.error == null)
//         {
//             Debug.Log("service hit properly ForgotPassword-sdsdsd----" + www.text);
//             ForgotPasswordHeader forgot = JsonConvert.DeserializeObject<ForgotPasswordHeader>(www.text);
//             if ((forgot.error == "true") || (forgot.error == "True"))
//             {
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(forgot.message);
//             }
//             else
//             {
//                 UI_Manager._instance.OpenLoginScreen();
//                 //UI_Manager._instance.User_Email = forgot.result.email;
//                 //UI_Manager._instance.User_Id = forgot.result.id;
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(forgot.message);
//                 //UI_Manager._instance.User_Signup_otp = forgot.result.Otp;
//                 PlayerPrefs.SetString("PlayerId",UI_Manager._instance.User_Id);
//             }

//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {
//             Debug.Log("error Generated ForgotPassword" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//     }


//     public void CallVerifyOtp(string email, string otp)
//     {
//         StartCoroutine(VerifyOtp(email, otp));
//     }


//     IEnumerator VerifyOtp(string _email, string _otp)
//     {
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);
//         WWWForm webForm = new WWWForm();
//         webForm.AddField("email", _email);
//         webForm.AddField("otp", _otp);
//         WWW www = new WWW(OTP_URL, webForm);
//         yield return www;
//         if (www.error == null)
//         {
//             Debug.Log("service hit properly Otp-sdsdsd----" + www.text);
//             verifyOTPHeader forgot = JsonConvert.DeserializeObject<verifyOTPHeader>(www.text);
//             if ((forgot.error == "true") || (forgot.error == "True"))
//             {
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(forgot.message);
//                 UI_Manager._instance.OtpTextInput.text=" ";
//             }
//             else
//             {
//                 Debug.Log("ererer " + forgot.result.userid);
//                UI_Manager._instance.User_Id=forgot.result.userid;
//                  PlayerPrefs.SetString("PlayerId", forgot.result.userid);
//                 Debug.Log("UI_Manager._instance.LastScreenNo"+UI_Manager._instance.LastScreenNo);

//                 if(UI_Manager._instance.LastScreenNo==2)
//                 {
//                      UI_Manager._instance.OpenResetPasswordScreen();
                    
//                 }
//                else{
//                     UI_Manager._instance.ProfileButtonPressed();
//                     UI_Manager._instance.OpenHomeScreen();
//                    } 
            
//             }
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {
//             Debug.Log("error Generated Otp" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//     }

//     public void CallVerifyOtpafterforgot(string email, string otp)
//     {
//         StartCoroutine(VerifyOtpafterforgot(email, otp));
//     }
//     IEnumerator VerifyOtpafterforgot(string _email, string _otp)
//     {
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);

//         WWWForm webForm = new WWWForm();
//         webForm.AddField("email", _email);
//         webForm.AddField("otp", _otp);
//         WWW www = new WWW(OTP_URL, webForm);
//         yield return www;
//         if (www.error == null)
//         {
//             Debug.Log("service hit properly Otp-sdsdsd----" + www.text);
//             verifyOTPHeader verify = JsonConvert.DeserializeObject<verifyOTPHeader>(www.text);

//             if ((verify.error == "true") || (verify.error == "True"))
//             {
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(verify.message);
//             }
//             else
//             {
//                 UI_Manager._instance.OpenResetPasswordScreen();
//                 UI_Manager._instance.User_Id = verify.result.userid;
//             }
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {
//             Debug.Log("error Generated Otp" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//     }


//     public void CallResendOtp(string email)
//     {
//         StartCoroutine(ResendOtp(email));
//     }
//     IEnumerator ResendOtp(string _email)
//     {
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);

//         WWWForm webForm = new WWWForm();
//         webForm.AddField("email", _email);
//         webForm.AddField("device_type", UI_Manager._instance.Device_Type);
//         webForm.AddField("device_token", UI_Manager._instance.Device_Token);
//         WWW www = new WWW(Resend_Otp, webForm);
//         yield return www;
//         if (www.error == null)
//         {
//             Debug.Log("service hit properly ResendOtp-sdsdsd----" + www.text);
//             ResendOtpHeader resendOtp = JsonConvert.DeserializeObject<ResendOtpHeader>(www.text);
//             UI_Manager._instance.User_Email = resendOtp.result.Email;
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
         
//         }
//         else
//         {
//             Debug.Log("error Generated ResendOtp" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//     }


//     public void CallChangePassword(string Pswd, string CnfPswd)
//     {
//         StartCoroutine(ChangePassword(Pswd, CnfPswd));
//     }
//     IEnumerator ChangePassword(string Pswd, string CnfPswd)
//     {
//         UI_Manager._instance.LoadingBarStatus(true);

//         WWWForm webForm = new WWWForm();
//         webForm.AddField("userid",UI_Manager._instance.User_Id);//PlayerPrefs.GetString("PlayerId")
//         webForm.AddField("newpass", Pswd);
//         webForm.AddField("cpass", CnfPswd);
//         webForm.AddField("device_type", "1");
//         webForm.AddField("device_token", UI_Manager._instance.Device_Token);
//         WWW www = new WWW(Change_Password, webForm);
//         yield return www;
//         if (www.error == null)
//         {
//             Debug.Log("service hit properly ChangePassowd-sdsdsd----" + www.text);
//             ChangePasswordHeader change = JsonConvert.DeserializeObject<ChangePasswordHeader>(www.text);
//             if ((change.error == "true") || (change.error == "True"))
//             {
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(change.message);
//             }
//             else
//             {
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(change.message);
//                 PlayerPrefs.SetInt("Loginstatus", 1);
//                 PlayerPrefs.SetString("PlayerId", change.result.id);
//             }

//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {
//             Debug.Log("error Generated ChangePassword" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//     }

// public void CallGetProfile()
// {
//     StartCoroutine(GetProfile());
// }

//     IEnumerator GetProfile()
//     {
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);
//         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Get_Profile + PlayerPrefs.GetString("PlayerId"));
//         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//         StreamReader reader = new StreamReader(response.GetResponseStream());
//         string jsonResponse = reader.ReadToEnd();
//         Debug.Log(jsonResponse);

//         GetProfileHeader profile = JsonConvert.DeserializeObject<GetProfileHeader>(jsonResponse);
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         if ((profile.error == "true") || (profile.error == "True"))
//         {
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(profile.message);
//         }
//         else
//         {

//            // UI_Manager._instance.HomeProfileName_text.text = profile.result.name;
//             UI_Manager._instance.User_Name = profile.result.name;
//             UI_Manager._instance.User_Email = profile.result.email;
           



          

//            // SceneNevegation._instance.UnlockLevels();
                                        
//             //UI_Manager.GetuserimageUrl=profile.result.user_image;
//            // DownloadProfileImage(profile.result.user_image);
//             yield return new WaitForSeconds(1f);

         
//             UI_Manager._instance.ShowUserDetailOnHOme();
//             PlayerPrefs.SetString("PlayerName", profile.result.name);
           

//         }

//     }


//   IEnumerator GetProfileImage(string _url)
//     {
//         Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
//         WWW Link = new WWW(_url);
//         yield return Link;
//         Link.LoadImageIntoTexture(tex);
//        // yield return new WaitForSeconds(7f);
//            Debug.Log("sdsad"+tex);
//        // UI_Manager._instance.userprofilesprite.Add(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0)));
//         UI_Manager._instance.GetUserSprite=(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0)));
//             //UI_Manager._instance.User_Image = UI_Manager._instance.GetUserSprite;
//             // UI_Manager._instance.ShowUserDetailOnHOme();
//              Debug.Log("sdsad");
//            // StartCoroutine("UserImage");
       

//     }

//     //Button Event
//     public void DownloadProfileImage(string _url)
//     {
//         StartCoroutine(GetProfileImage(_url));
//     }


//     public void CallUpdateProfile(string name, string url)
//     {
//        // Debug.Log("service hit properly UpdateProile-sdsdsd----");
//         StartCoroutine(UpdateProfile(name,url));

//     }

//     IEnumerator UpdateProfile(string name, string AvatarUrl)
//     {
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);
        
//  Debug.Log("AvatarUrl..........."+AvatarUrl+name);
//         WWWForm webForm = new WWWForm();
        
//         webForm.AddField("userid",UI_Manager._instance.User_Id); //UI_Manager._instance.User_Id);user_image
//         webForm.AddField("name", name);
//         webForm.AddField("device_type", UI_Manager._instance.Device_Type);
//         webForm.AddField("device_token", UI_Manager._instance.Device_Token);
//          webForm.AddField("user_image", AvatarUrl);
        
        
//         // Debug.Log(Update_Profile + UI_Manager._instance.User_Id + name + UI_Manager._instance.Device_Type) + "android" + "677564344rr566667677");
//         WWW www = new WWW(Update_Profile, webForm);
        
//         yield return www;
//           Debug.Log("service hit properly UpdateProile-sdsdsd----" + www.text);
//         if (www.error == null)
//         {
//             Debug.Log("service hit properly UpdateProile-sdsdsd----" + www.text);
//             UpdateProfileHeader Update = JsonConvert.DeserializeObject<UpdateProfileHeader>(www.text);

//             if ((Update.error == "true") || (Update.error == "True"))
//             {
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(Update.message);
//             }
//             else
//             {
//                 UI_Manager._instance.User_Id = Update.result.id;
//                 UI_Manager._instance.HomeProfileName_text.text = Update.result.name;
//                 UI_Manager._instance.User_Name = Update.result.name;
//                 UI_Manager._instance.User_Email = Update.result.email;
//                // UI_Manager.AvatarUrl = Update.result.user_image;
//                // UI_Manager._instance.EnabledProfiletexts(false);

//                  DownloadUserImage(Update.result.user_image);
//                  yield return new WaitForSeconds(4);
//                 //  UI_Manager._instance.userimage_in_profile.sprite=UI_Manager._instance.userprofilesprite;
//                 //  UI_Manager._instance.userimage_in_home.sprite=UI_Manager._instance.userprofilesprite;
//                 //  UI_Manager._instance.User_Image=UI_Manager._instance.userprofilesprite;

//                  GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(Update.message);



//             }

//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {
//             Debug.Log("error Generated Update Profile" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }

//     }

//         IEnumerator LoadUserProfileImage(string _url)
//     {
//         Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
//         WWW Link = new WWW(_url);
//         yield return Link;
//         Link.LoadImageIntoTexture(tex);
//        // UI_Manager._instance.userprofilesprite.Add(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0)));
//         UI_Manager._instance.userprofilesprite=(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0)));
//              Debug.Log("sdsad");
//            // StartCoroutine("UserImage");
       

//     }

//     //Button Event
//     public void DownloadUserImage(string _url)
//     {
//         StartCoroutine(LoadUserProfileImage(_url));
//     }

//     public void CallPrivacyPolicy()
//     {
//         StartCoroutine(PrivacyPolicy());
//     }

//     IEnumerator PrivacyPolicy()
//     {
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);

//         WWWForm webForm = new WWWForm();
//         webForm.AddField("pageid", "24");
//         WWW www = new WWW(Privacy_Policy, webForm);
//         yield return www;
//         if (www.error == null)
//         {
//             Debug.Log("service hit properly PrivacyPolicy-sdsdsd----" + www.text);
//             PrivacyPolicyHeader policy = JsonConvert.DeserializeObject<PrivacyPolicyHeader>(www.text);

//             if ((policy.error == "true") || (policy.error == "True"))
//             {
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(policy.message);
//             }
//             else
//             {

//                 UI_Manager._instance.OpenPrivacyScreen();
//                 UI_Manager._instance.PrivacyPolicyDescription_text.text = policy.result.short_description;

//             }    
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {
//             Debug.Log("error Generated PrivacyPolicy" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }

//     }
//     public void CallTermsCondition()
//     {
//         StartCoroutine(_TermsCondition());
//     }

//     IEnumerator _TermsCondition()
//     {
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);
//         WWWForm webForm = new WWWForm();
//         webForm.AddField("pageid", "24");
//         WWW www = new WWW(Terms_Condition, webForm);
//         yield return www;
//         if (www.error == null)
//         {
//             Debug.Log("service hit properly Terms & condition-sdsdsd----" + www.text);
//             TermsConditionHeader term = JsonConvert.DeserializeObject<TermsConditionHeader>(www.text);
//             if ((term.error == "true") || (term.error == "True"))
//             {
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(term.message);
//             }
//             else
//             {
//                 UI_Manager._instance.OpenTermScreen();
//                 UI_Manager._instance.TermsAndConditionDescription_text.text = term.result.short_description;

//             }
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {
//             Debug.Log("error Generated terms Condition" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }

//     }

   // public void CallLogout()
//{
       //  StartCoroutine(_Logout());
     //}

     //IEnumerator _Logout()
    // {
        // GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);

          // WWWForm webForm = new WWWForm();
          //Debug.Log("UI_Manager._instance.User_Id  " + UI_Manager._instance.User_Id);
    //webForm.AddField("userid", PlayerPrefs.GetString("PlayerId"));
      // WWW www = new WWW(Logout, webForm);

     //  yield return www;
        // if (www.error == null)
    //{
        //             Debug.Log("service hit properly Logout-sdsdsd----" + www.text);
        //  LogoutHeader log = JsonConvert.DeserializeObject<LogoutHeader>(www.text);

         //  if ((log.error == "true") || (log.error == "True"))
       // {
              //GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(log.message);
        //   }
       // else
          //  {
               //  GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(log.message);
                // UI_Manager._instance.OpenLoginScreen();
                // PlayerPrefs.DeleteAll();
                
       //  }

//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }

//         else
//         {
//             Debug.Log("error Generated Logout" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }

//     }





//     public void CallGetUserRank()
//     {
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);
//         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(User_Rank);
//         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//         StreamReader reader = new StreamReader(response.GetResponseStream());
//         string jsonResponse = reader.ReadToEnd();
//         Debug.Log(jsonResponse);
//         GetUserRankeHeader rank = JsonConvert.DeserializeObject<GetUserRankeHeader>(jsonResponse);

//         if ((rank.error == "true") || (rank.error == "True"))
//         {
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(rank.message);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {


//            // UI_Manager._instance.Rankprofilename_text.text=UI_Manager._instance.User_Name;
//            // UI_Manager._instance.Rankprofile_Image.sprite=UI_Manager._instance.User_Image;
//             //UI_Manager.LengthCount = rank.result.Length;
//             UI_Manager.LeaderbaordProfileImage = new string[rank.result.Length];
//             UI_Manager. LeaderbaordProfileName = new string[rank.result.Length];
//             UI_Manager. LeaderbaordProfileRank = new string[rank.result.Length];
//              UI_Manager._instance.leadebaordProfileImageSprite = new List<Sprite>();
//             Debug.Log(rank.result.Length);
//             for (int i = 0; i < rank.result.Length; i++)
//             {
//                 UI_Manager.LeaderbaordProfileName[i] = rank.result[i].name;
//                 UI_Manager.LeaderbaordProfileRank[i] =  rank.result[i].rank;//i.ToString(); //rank.result[i].user_rank;//  
//                 UI_Manager.LeaderbaordProfileImage[i] =  rank.result[i].user_image;    //"https://i.ibb.co/K6RzQLX/award-rostrum-vector-19145084.jpg";
//                  Debug.Log("LeaderbaordProfileRank"+ rank.result[i].rank);
//             }
//             for (int i = 0; i < rank.result.Length; i++)
//             {
//                // Debug.Log("name" + UI_Manager.LeaderbaordProfileName[i]);
//                 DownloadImage(UI_Manager.LeaderbaordProfileImage[i]);
//                 // Debug.Log("rank.result.Length--"+ UI_Manager.LeaderbaordProfileImage[i]);

//             }
//         }
//     }



//     IEnumerator LoadLeaderboardProfileImage(string _url)
//     {
//         Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
//         WWW Link = new WWW(_url);
//         yield return Link;
//         Link.LoadImageIntoTexture(tex);
//         UI_Manager._instance.leadebaordProfileImageSprite.Add(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0)));
//       //  Debug.Log("UI_Manager._instance.leadebaordProfileImageSprite.Count"+UI_Manager._instance.leadebaordProfileImageSprite.Count+"UI_Manager.LeaderbaordProfileImage.Length"+UI_Manager.LeaderbaordProfileImage.Length);
//         if (UI_Manager._instance.leadebaordProfileImageSprite.Count == UI_Manager.LeaderbaordProfileImage.Length)
//         {
//             UI_Manager._instance.OpenLeaderbaordScreen();
//             StartCoroutine("test");
//         }

//     }

//     //Button Event
//     public void DownloadImage(string _url)
//     {
//         StartCoroutine(LoadLeaderboardProfileImage(_url));
//     }

//     IEnumerator test()
//     {
//         Debug.Log("sdsad");
//         yield return new WaitForSeconds(1);
//        // UI_Manager._instance.SetLeaderbaord();        
//         GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//     }



// public void CallFbLogin(string FBname,string FBid, string FBemail)
// {
//     StartCoroutine(FacebookLogin(FBname,FBid,FBemail));
// }



// IEnumerator FacebookLogin(string FBname,string FBid, string FBemail)
// {
//     GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(true);

//         WWWForm webForm = new WWWForm();
//         webForm.AddField("email", FBemail);
//         webForm.AddField("fb_id", FBid);
//         webForm.AddField("first_name", FBname);
//         webForm.AddField("device_token", UI_Manager._instance.Device_Token);
//         Debug.Log(FbLogin_URL + FBemail + FBid + FBname + "android" + "677564344rr566667677");
//         WWW www = new WWW(FbLogin_URL, webForm);

          
//         yield return www;
//          if (www.error == null)
//         {
//             Debug.Log("service hit properly FbLogin-sdsdsd----" + www.text);
//             FBHeader fb = JsonConvert.DeserializeObject<FBHeader>(www.text);

//             if ((fb.error == "true") || (fb.error == "True"))
//             {
//                 GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(fb.message);
                
//                 if (fb.message == "Your account has been created successfully")   
//                 {
//                     Debug.Log("in this");
//                    // UI_Manager._instance.OpenOtpScreen();
//                     UI_Manager._instance.User_Id = fb.result.id;
//                     UI_Manager._instance.User_Token = fb.result.device_token;
//                     UI_Manager._instance.User_Email = fb.result.email;
//                     UI_Manager._instance.User_Name=fb.result.name;


//                     CallResendOtp(fb.result.email);
                   
//                     //PlayerPrefs.SetString("PlayerName", );
//                      PlayerPrefs.SetInt("Loginstatus", 1);
//                     PlayerPrefs.SetString("PlayerId", fb.result.id);
//                 }

//             }
//             else
//             {
//                    // UI_Manager._instance.OpenMapScreen();
//                     UI_Manager._instance.User_Id = fb.result.id;
//                     UI_Manager._instance.User_Token = fb.result.device_token;
//                     UI_Manager._instance.User_Email = fb.result.email;
//                     UI_Manager._instance.User_Name=fb.result.name;

//                     PlayerPrefs.SetInt("Loginstatus", 1);
//                     PlayerPrefs.SetString("PlayerId", fb.result.id);

//             }
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {
//             Debug.Log("error Generated fbLogin" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }

// }






//       public void CallContactAdmin(string subject,string messege)
//     {
//         StartCoroutine(ContactAdmin(subject,messege));
//     }

//     IEnumerator ContactAdmin(string subject,string messege)
//     {
//         UI_Manager._instance.LoadingBarStatus(true);
//             WWWForm webForm = new WWWForm();
//             webForm.AddField("userid",UI_Manager._instance.User_Id);
//             webForm.AddField("subject", subject);
//             webForm.AddField("message", messege);
           
//              WWW www = new WWW(Contact_Admin, webForm);
//              Debug.Log(Contact_Admin + UI_Manager._instance.User_Id + subject+messege );
//              yield return www;
//               Debug.Log("service hit Contact_Admin ----" + www.text);
      
//         if (www.error == null)
//         {
           
//             ContactAdmintHeader admin = JsonConvert.DeserializeObject<ContactAdmintHeader>(www.text);
//             if ((admin.error == "true") || (admin.error == "True"))
//             {
//                 UI_Manager._instance .ShowErrorPopup(admin.message);
//             }
//             else
//             {
//                 UI_Manager._instance.ShowErrorPopup(admin.message);
//                  UI_Manager._instance.SubjectInputField.text=" "; 
//                  UI_Manager._instance.MessegeInputField.text=" ";
//                 UI_Manager._instance.OpenSettingScreen();
//                // UI_Manager._instance.SubjectInputFieldPlaceholder.text = "Subject";
//                // UI_Manager._instance.MessegeInputFieldPlaceholder.text = "Message";
//                // UI_Manager._instance.SubjectInputFieldPlaceholder.text
//                // UI_Manager._instance.MessegeInputField.placeholder.GetComponent

//                 //UI_Manager._instance.OpenContactUsScreen();
               

//             }
//            GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }
//         else
//         {
//             Debug.Log("error Generated Contact_Admin" + www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().ShowErrorPopup(www.error);
//             GameObject.Find("Manager").GetComponent<UI_Manager>().LoadingBarStatus(false);
//         }

//     }



// //..........Letter Stack................//
//   public void CallTodayStack()
//     {
//         StartCoroutine(TodayStack());
//     }

//     IEnumerator TodayStack()
//     {
        
//         UI_Manager._instance.LoadingBarStatus(true);
//         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TodayStack_URL);
//         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//         StreamReader reader = new StreamReader(response.GetResponseStream());
//         string jsonResponse = reader.ReadToEnd();
        
//         Debug.Log(jsonResponse);

//        // WWWForm webForm = new WWWForm();
//        // WWW www = new WWW(TodayStack_URL, webForm);
//         yield return new WaitForSeconds(0);
    
    
//             Debug.Log("service hit properly LetterStack-sdsdsd----" +jsonResponse );
//             TodayLetterStackHeader TodayLetter = JsonConvert.DeserializeObject<TodayLetterStackHeader>(jsonResponse);
            
//            UI_Manager._instance.LoadingBarStatus(false);

//             if ((TodayLetter.error == "true") || (TodayLetter.error == "True"))
//             {
//                 UI_Manager._instance.ShowErrorPopup(TodayLetter.message);
//             }
//             else
//             { 
//                 for(int i=0; i< TodayLetter.result.Length; i++)
//                 {
//                    // Alphabet.data.LetterStackList.Add(TodayLetter.result[i].alphabet); 
                    
//                    // Alphabet.data.LetterStackList.Add(TodayLetter.result[i].alphabet);

//                    LetterStack newdata = new LetterStack(TodayLetter.result[i].alphabet,TodayLetter.result[i].score); 
//                    Alphabet.data.LetterStackList.Add(newdata);

//                 }
                 

//             foreach(LetterStack letteritem in Alphabet.data.LetterStackList)
//             {
//                 //Debug.Log("alphabet...."+letteritem.letter + "score........."+letteritem.score);
//                 //Debug.Log(" Alphabet.data.LetterStackList........."+ Alphabet.data.LetterStackList.Count);
//             }
              
//         }    
        
      

//     }


// //.............Get Resume Data...................//

//   public void CallResumeDataStack()
//     {
//         StartCoroutine(ResumeDataStack());
//     }

//     IEnumerator ResumeDataStack()
//     {
        
//         UI_Manager._instance.LoadingBarStatus(true);
//         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ResumeData_URL+"?userid=34");
//         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//         StreamReader reader = new StreamReader(response.GetResponseStream());
//         string jsonResponse = reader.ReadToEnd();
//           Debug.Log(jsonResponse);

//         yield return new WaitForSeconds(0);
    
    
//             Debug.Log("service hit properly CallResumeDataStack-sdsdsd----" +jsonResponse );
//              GameStateDataHeader GetResumeData = JsonConvert.DeserializeObject< GameStateDataHeader>(jsonResponse);
            
//            UI_Manager._instance.LoadingBarStatus(false);

//             if ((GetResumeData.error == "true") || (GetResumeData.error == "True"))
//             {
//                 UI_Manager._instance.ShowErrorPopup(GetResumeData.message);
//             }
//             else
//             { 
                
//                  GameController.data. GetResumeData(GetResumeData);
              
//             }    
        
      

//     }



// //.........Letter Board....................//
// //  public void CallPlayerTurnData(string userid, string gameid,)
// //     {
// //         StartCoroutine(SingUp());
// //     }



// }//End Of Webservice class....

















