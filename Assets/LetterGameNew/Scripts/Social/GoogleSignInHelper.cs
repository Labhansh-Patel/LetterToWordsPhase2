// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using APICalls;
// using Google;
// using UnityEngine;
//
// public class GoogleSignInHelper
// {
//     public class GoogleSignInData
//     {
//         public string UserId;
//         public string Email;
//         public string DisplayName;
//         public string ImageUrl;
//
//         public GoogleSignInData(string userId, string email, string displayName, string imageUrl)
//         {
//             UserId = userId;
//             Email = email;
//             DisplayName = displayName;
//             ImageUrl = imageUrl;
//         }
//
//         public override string ToString()
//         {
//             // return base.ToString();
//             return $"UserID {this.UserId}, Email{Email}, DisplayName{DisplayName}, ImageUrl {ImageUrl}";
//         }
//     }
//
//     private string webClientId = "226182724027-lkmnfnj8nqmmrd0p3mf98pke7raj5cfu.apps.googleusercontent.com";
//
//     private GoogleSignInConfiguration configuration;
//
//     private Action<GoogleSignInData> GoogleLoginCallBack;
//
//     public GoogleSignInHelper()
//     {
// // #if !UNITY_EDITOR
//                configuration = new GoogleSignInConfiguration
//         {
//             WebClientId = webClientId,
//             RequestIdToken = true,
//             RequestEmail = true,
//             RequestProfile = true
//         };
// // #endif
//     }
//
//     // // Defer the configuration creation until Awake so the web Client ID
//     // // Can be set via the property inspector in the Editor.
//     // void Awake()
//     // {
//     //
//     // }
//
//     public void OnSignIn(Action<GoogleSignInData> callback = null)
//     {
//         GoogleLoginCallBack = callback;
//
//
// // #if UNITY_EDITOR
// //
// //         GoogleSignInData googleSignInData = new GoogleSignInData("116366976222491292531", "joyaljoseag@gmail.com", "Joyal jose",
// //             "https://lh3.googleusercontent.com/a-/AOh14GiA8pebz1WqPZKkAH8KZmUkj_2FDKYslrmjkjIuQQ=s96-c");
// //         GoogleLoginCallBack?.Invoke(googleSignInData);
// //
// //          //GoogleLoginCallBack?.Invoke("112626502377494735787333","joyaltest@gmail.com","Joyal","https://lh3.googleusercontent.com/a-/AOh14GhGjn428-edxpwqBm1dVxqqFtRNSCEWIhAfdz3x=s96-c",false);
// //
// // #else
//         GoogleSignIn.Configuration = configuration;
//         GoogleSignIn.Configuration.UseGameSignIn = false;
//         GoogleSignIn.Configuration.RequestIdToken = true;
//         AddStatusText("Calling SignIn");
//
//      GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
//             OnAuthenticationFinished , TaskScheduler.FromCurrentSynchronizationContext());
// // #endif
//     }
//
//     public void OnSignOut()
//     {
//         AddStatusText("Calling SignOut");
//         GoogleSignIn.DefaultInstance.SignOut();
//     }
//
//     public void OnDisconnect()
//     {
//         AddStatusText("Calling Disconnect");
//         GoogleSignIn.DefaultInstance.Disconnect();
//     }
//
//     internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
//     {
//         LogSystem.LogEvent("Task {0}", task.IsCompleted);
//         if (task.IsFaulted)
//         {
//             using (IEnumerator<System.Exception> enumerator =
//                    task.Exception.InnerExceptions.GetEnumerator())
//             {
//                 if (enumerator.MoveNext())
//                 {
//                     GoogleSignIn.SignInException error =
//                         (GoogleSignIn.SignInException)enumerator.Current;
//                     LogSystem.LogEvent("Got Error: " + error.Status + " " + error.Message);
//                 }
//                 else
//                 {
//                     LogSystem.LogEvent("Got Unexpected Exception?!?" + task.Exception);
//                 }
//             }
//         }
//         else if (task.IsCanceled)
//         {
//             LogSystem.LogEvent("Canceled");
//         }
//         else
//         {
//             LogSystem.LogEvent("Welcome: " + task.Result.DisplayName + "!" + " Email: " + task.Result.Email);
//             task.Wait(200);
//
//             GoogleSignInData googleSignInData = new GoogleSignInData(task.Result.UserId, task.Result.Email,
//                 task.Result.DisplayName, task.Result.ImageUrl.ToString());
//
//             LogSystem.LogEvent("Welcome: " + googleSignInData.DisplayName + "!" + " Email: " + googleSignInData.Email);
//
//
//             GoogleLoginCallBack?.Invoke(googleSignInData);
//         }
//     }
//
//
//     public void OnSignInSilently()
//     {
//         GoogleSignIn.Configuration = configuration;
//         GoogleSignIn.Configuration.UseGameSignIn = false;
//         GoogleSignIn.Configuration.RequestIdToken = true;
//         AddStatusText("Calling SignIn Silently");
//
//         GoogleSignIn.DefaultInstance.SignInSilently()
//             .ContinueWith(OnAuthenticationFinished);
//     }
//
//
//     public void OnGamesSignIn()
//     {
//         GoogleSignIn.Configuration = configuration;
//         GoogleSignIn.Configuration.UseGameSignIn = true;
//         GoogleSignIn.Configuration.RequestIdToken = false;
//
//         AddStatusText("Calling Games SignIn");
//
//         GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
//             OnAuthenticationFinished);
//     }
//
//     private List<string> messages = new List<string>();
//
//     void AddStatusText(string text)
//     {
//         if (messages.Count == 5)
//         {
//             messages.RemoveAt(0);
//         }
//
//         messages.Add(text);
//         string txt = "";
//         foreach (string s in messages)
//         {
//             txt += "\n" + s;
//         }
//
//         Debug.Log(txt);
//     }
// }