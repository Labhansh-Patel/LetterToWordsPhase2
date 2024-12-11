// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using APICalls;
// using System;


// namespace DefaultNamespace
// {
// public class ApiServiceManager : MonoBehaviour
// {
    

//        // [SerializeField] private GameObject menuScreen;

//          void Start()
//         {
//            // ApiManager.LoginUser("test01@gmail.com", "1234", HandleLoginCall);
//         }




//  public void CallSignIn(string email, string pass)
//     {
//        // ApiManager.LoginUser(email,pass);
//     }


//     private  void HandleLoginCall(bool asucess, LoginHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result.id);

              
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }



//  public void CallSingUp(string name, string email, string pass, string confPswd)
//     {
//        // ApiManager.SingUp (name, email, pass, confPswd , HandleSinUpCall);
//     }
//             private void HandleSinUpCall(bool asucess, SingUpHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result.id);

              
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }


//  public void Call_ForgotPassword(string email)
//     {
//       // ApiManager.ForgotPassword(email,HandleForgotPswdCall);
        
//     }

//      private void HandleForgotPswdCall(bool asucess, ForgotPasswordHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result.id);

              
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }
        
//         private void HandleVerifyOtpCall(bool asucess, verifyOTPHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }
//            private void HandleResendOtpCall(bool asucess, ResendOtpHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }
//             private void HandleUpdateProfileCall(bool asucess, UpdateProfileHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }
        
//             private void HandlePrivacyCall(bool asucess, PrivacyPolicyHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }      
//          private void HandleTermCall(bool asucess, TermsConditionHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }
        
//          private void HandleChangePswdCall(bool asucess, ChangePasswordHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }
        
//          private void HandleLogoutCall(bool asucess, LogoutHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}");
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }

//         private void HandleProfileCall(bool asucess, GetProfileHeader profileResponse)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", profileResponse.result.name);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", profileResponse.message);
//             }
//         }


        
//          private void HandleUseRanktCall(bool asucess, GetUserRankeHeader rank)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}");
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", rank.message);
//             }
//         }

          
//          private void HandleContactAdminCall(bool asucess, ContactAdmintHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }
        
//          private void HandleContactAdminCall(bool asucess, TodayLetterStackHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }

//           private void HandleGameStateCall(bool asucess, GameStateDataHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }


//           private void HandleSendFriendRequestCall(bool asucess, SendFriendRequestHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }
        
//           private void HandleAcceptFriendRequestCall(bool asucess, AcceptFriendRequestHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }
//      private void HandleRejectFriendRequestCall(bool asucess, RejectFriendRequestHeader callback)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}", callback.result);
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", callback.message);
//             }
//         }
        
//      private void HandleFriendListCall(bool asucess, MyFriendListHeader FriendList)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}");
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", FriendList.message);
//             }
//         }

        
//      private void HandlePendingListCall(bool asucess, PendingFriendListHeader Pending)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}");
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", Pending.message);
//             }
//         }



        
//      private void HandleSearchCall(bool asucess, SearchFriendListHeader search)
//         {
//             if (asucess)
//             {
//                 LogSystem.LogColorEvent("green", "LoggedInSucessfully  ID : {0}");
//             }
//             else
//             {
//                 LogSystem.LogColorEvent("red", "LoginFailed  ID : {0}", search.message);
//             }
//         }


// }



// }
