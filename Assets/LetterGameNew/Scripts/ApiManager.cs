//#define  SERVER_LOCAL

using System.Collections.Generic;
using GameEvents;
using Gameplay;
using Newtonsoft.Json;
using UnityEngine;

namespace APICalls
{
    public static class ApiManager
    {
        public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ContractResolver = ShouldSerializeContractResolver.Instance,
        };

        #region DelegateCallback

        public delegate void BaseRequestCallBack(bool aSucess, BaseHeader aData);

        public delegate void LoginCallBack(bool aSucess, LoginHeader callBack);

        public delegate void SingUpCallBack(bool aSucess, SingUpHeader callBack);

        public delegate void ForgotPasswordsCallBack(bool aSucess, ForgotPasswordHeader callBack);

        public delegate void VerifyOTPCallBack(bool aSucess, verifyOTPHeader callBack);

        public delegate void ResendOtpCallBack(bool aSucess, ResendOtpHeader callBack);

        public delegate void UpdateProfileCallBack(bool aSucess, UpdateProfileHeader callBack);

        public delegate void PrivacyPolicyCallBack(bool aSucess, PrivacyPolicyHeader callBack);

        public delegate void TermsConditionCallBack(bool aSucess, TermsConditionHeader callBack);

        public delegate void ChangePasswordCallBack(bool aSucess, ChangePasswordHeader callBack);

        public delegate void LogoutCallBack(bool aSucess, LogoutHeader callBack);

        public delegate void GetUserRankCallBack(bool aSucess, GetUserRankeHeader rank);

        public delegate void AvtarImageCallBack(bool aSucess, GetAvtarHeader callBack);

        public delegate void ContactAdminCallBack(bool aSucess, ContactAdmintHeader callBack);

        public delegate void TodayLetterStackCallBack(bool aSucess, TodayLetterStackHeader stack);

        public delegate void GameStateDataCallBack(bool aSucess, GameStateDataHeader Gamedata);

        public delegate void SendFriendRequestCallBack(bool aSucess, SendFriendRequestHeader callBack);

        public delegate void AcceptFriendRequestCallBack(bool aSucess, AcceptFriendRequestHeader callBack);

        public delegate void RejectFriendRequestCallBack(bool aSucess, RejectFriendRequestHeader callBack);

        public delegate void MyFriendListCallBack(bool aSucess, MyFriendListHeader FriendList);

        public delegate void PendingFriendListCallBack(bool aSucess, PendingFriendListHeader Pending);

        public delegate void SearchFriendCallBack(bool aSucess, SearchFriendListHeader search);

        public delegate void GetProfileCallBack(bool aSucess, GetProfileHeader profileResponse);

        public delegate void UppdateNotificationCallBack(bool aSucess, UpdateNotificationHeader callback);

        public delegate void CreateGameCallBack(bool aSucess, CreateGameData callback);

        public delegate void GameListCallBack(bool aSucess, GameListHeader callback);

        public delegate void GameDataCallBack(bool aSucess, GameDataHeader callback);

        public delegate void SaveGameDataCallBack(bool aSucess, UserSaveData callback);

        public delegate void GameCompleteCallBack(bool aSucess, GameComplete callback);
        public delegate void OutGameBonusCallBack(bool aSucess, OutGameBonus callback);

        public delegate void CheckWordCallBack(bool aSucess, CheckWordHeader callback);

        public delegate void GetDailyLetterCallBack(bool aSucess, GetDailyLetterHeader DailyLetter);

        public delegate void LeaderBoardCallBack(bool aSucess, LeaderBoardHeader callback);

        public delegate void GetDatesCallBack(bool aSucess, GetDatesHeader callback);

        public delegate void DateWiseDataCallBack(bool aSucess, DateWiseDataHeader callback);

        public delegate void CalendarDataCallback(bool aScess, List<CalendarData> calendarData);

        public delegate void DateGameDataCallBack(bool aScess, DateGameData dateGameData);

        public delegate void DeleteGameCallBack(bool aScess, DeleteGameHeader callback);

        public delegate void GetBonousPointResponseCallBack(bool aScess, GetBonousPointResponse callback);

        public delegate void UpdateBonousPointResponseCallBack(bool aScess, UpdateBonousPointResponse callback);

        public delegate void SetPremiumResponseCallBack(bool aStatus, SetPremiumResponse callback);

        public delegate void GetPlayerStatsCallBack(bool aSucess, PlayerStatsData callback);

        #endregion


        private static void WebRequest(string serviceName, string jsonData, WebHelpers.CallbackGet callback)
        {
            string url = APIServices.BASE_URL + serviceName;

            if (string.IsNullOrEmpty(jsonData))
            {
                jsonData = "{}";
            }

            byte[] bytesContent = System.Text.Encoding.UTF8.GetBytes(jsonData);
            LogSystem.LogEvent("[][] requesting url {0}, requestParams {1}", url, jsonData);
            WebHelpers.instance.post<string>(url, bytesContent, "application/json", callback);
        }

        private static void WebRequest(string serviceName, WWWForm formData, WebHelpers.CallbackGet callback)
        {
            LogSystem.LogEvent("Called LogUserSocial9");
            string url = APIServices.BASE_URL + serviceName;

            LogSystem.LogEvent("[][] requesting url {0}, requestParams {1}", url, JsonConvert.SerializeObject(formData));
            WebHelpers.instance.post<string>(url, formData, "application/json", callback);
        }

        private static void WebRequestGet(string serviceName, WebHelpers.CallbackGet callback)
        {
            string url = APIServices.BASE_URL + serviceName;

            LogSystem.LogEvent("[][] requesting url {0}", url);
            WebHelpers.instance.get<string>(url, callback);
        }

        private static void OnCompleteBaseRequest(bool aSuccess, object aData, BaseRequestCallBack callBack)
        {
            if (aSuccess)
            {
                BaseHeader response = JsonConvert.DeserializeObject<BaseHeader>(aData.ToString());
                if (!response.error)
                {
                    callBack(aSuccess, response);
                }
                else
                {
                    callBack(false, response);
                }
            }
            else
            {
                LogSystem.LogErrorEvent("Failed Calling the Api {0}", aData);
            }
        }

        public static void LoginUser(string email, string pass, string Device_Type, string Device_Token,
            LoginCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("email", email);
            webForm.AddField("password", pass);
            webForm.AddField("device_type", Device_Type);
            webForm.AddField("device_token", Device_Token);
            WebRequest(APIServices.LOGIN_URL, webForm,
                (url, success, data) => OnCompleteLoginCall(success, data, callBack));
        }


        public static void LoginUserSocial(string appID, string name, string userImage, string email, int loginType, string Device_Type,
            string Device_Token, LoginCallBack callBack)
        {
            LogSystem.LogEvent("Called LogUserSocial");
            WWWForm webForm = new WWWForm();
            LogSystem.LogEvent("Called LogUserSocial1");
            webForm.AddField("name", name);
            webForm.AddField("user_image", userImage);
            LogSystem.LogEvent("Called LogUserSocial1");
            webForm.AddField("login_type", loginType);
            webForm.AddField("email", email);
            webForm.AddField("device_type", Device_Type);
            webForm.AddField("device_token", Device_Token);
            LogSystem.LogEvent("Called LogUserSocial2");
            webForm.AddField("app_id", appID);
            LogSystem.LogEvent("Called LogUserSocial3");
            WebRequest(APIServices.LOGIN_URLSocial, webForm,
                (url, success, data) => OnCompleteLoginCall(success, data, callBack));
        }

        private static void OnCompleteLoginCall(bool aSuccess, object aData, LoginCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            LoginHeader login = JsonConvert.DeserializeObject<LoginHeader>(aData.ToString());


            if (!login.error)
            {
                callBack(true, login);
            }
            else
            {
                callBack(false, login);
            }
        }


        public static void SingUp(string name, string email, string pass, string confPswd, string Device_Type, string Device_Token, SingUpCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("email", email);
            webForm.AddField("name", name);
            webForm.AddField("password", pass);
            webForm.AddField("cpassword", confPswd);
            webForm.AddField("device_type", Device_Type);
            webForm.AddField("device_token", Device_Token);
            WebRequest(APIServices.SIGNUP_URL, webForm,
                (url, success, data) => OnCompleteSinUpCall(success, data, callBack));
        }

        private static void OnCompleteSinUpCall(bool aSuccess, object aData, SingUpCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());
            SingUpHeader singup = JsonConvert.DeserializeObject<SingUpHeader>(aData.ToString());

            if (!singup.error)
            {
                callBack(true, singup);
            }
            else
            {
                callBack(false, singup);
            }
        }


        public static void ForgotPassword(string email, string Device_Type, string Device_Token, ForgotPasswordsCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("email", email);
            webForm.AddField("device_type", Device_Type);
            webForm.AddField("device_token", Device_Token);
            WebRequest(APIServices.FORGOTPASSWORD_URL, webForm,
                (url, success, data) => OnCompleteForgotCall(success, data, callBack));
        }

        private static void OnCompleteForgotCall(bool aSuccess, object aData, ForgotPasswordsCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            ForgotPasswordHeader forgot = JsonConvert.DeserializeObject<ForgotPasswordHeader>(aData.ToString());


            if (!forgot.error)
            {
                callBack(true, forgot);
            }
            else
            {
                callBack(false, forgot);
            }
        }


        public static void VerifyOtp(string _email, string _otp, VerifyOTPCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("email", _email);
            webForm.AddField("otp", _otp);
            WebRequest(APIServices.OTP_URL, webForm,
                (url, success, data) => OnCompleteVerifyOtpCall(success, data, callBack));
        }


        private static void OnCompleteVerifyOtpCall(bool aSuccess, object aData, VerifyOTPCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            verifyOTPHeader otp = JsonConvert.DeserializeObject<verifyOTPHeader>(aData.ToString());

            if (!otp.error)
            {
                callBack(true, otp);
            }
            else
            {
                callBack(false, otp);
            }
        }


        public static void ResendOtp(string _email, string Device_Type, string Device_Token, ResendOtpCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("email", _email);
            webForm.AddField("device_type", Device_Type);
            webForm.AddField("device_token", Device_Token);
            WebRequest(APIServices.Resend_Otp, webForm,
                (url, success, data) => OnCompleteResendOtpCall(success, data, callBack));
        }


        private static void OnCompleteResendOtpCall(bool aSuccess, object aData, ResendOtpCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            ResendOtpHeader resend = JsonConvert.DeserializeObject<ResendOtpHeader>(aData.ToString());


            if (!resend.error)
            {
                callBack(true, resend);
            }
            else
            {
                callBack(false, resend);
            }
        }


        public static void ChangePassword(string Pswd, string CnfPswd, string User_Id, string Device_Type, string Device_Token, ChangePasswordCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("userid", User_Id); //PlayerPrefs.GetString("PlayerId")
            webForm.AddField("newpass", Pswd);
            webForm.AddField("cpass", CnfPswd);
            webForm.AddField("device_type", Device_Type);
            webForm.AddField("device_token", Device_Token);
            WebRequest(APIServices.Change_Password, webForm,
                (url, success, data) => OnCompleteChangePasswordCall(success, data, callBack));
        }


        private static void OnCompleteChangePasswordCall(bool aSuccess, object aData, ChangePasswordCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            ChangePasswordHeader chngpswd = JsonConvert.DeserializeObject<ChangePasswordHeader>(aData.ToString());


            if (!chngpswd.error)
            {
                callBack(true, chngpswd);
            }
            else
            {
                callBack(false, chngpswd);
            }
        }


        public static void GetProfile(string userID, GetProfileCallBack callBack)
        {
            WebRequestGet(APIServices.Get_Profile + userID,
                (url, success, data) => OnCompleteGetProfile(success, data, callBack));
        }

        private static void OnCompleteGetProfile(bool aSuccess, object aData, GetProfileCallBack callBack)
        {
            LogSystem.LogEvent("Profile Response {0}", aData.ToString());

            GetProfileHeader profileResponse = JsonConvert.DeserializeObject<GetProfileHeader>(aData.ToString());

            //   LogSystem.LogEvent("PublicKey {0} PrivateKey {1}", profileResponse.result.PublicGameKey, profileResponse.result.PrivateGameKey);
            GamePlayController.Instance.BonusController.OutGameBonusCount = profileResponse.result.out_game_bonus;

            if (!profileResponse.error)
            {
                callBack(true, profileResponse);
            }
            else
            {
                callBack(false, profileResponse);
            }
        }


        public static void UpdateProfile(string name, string number, string AvatarId, string User_Id, string Device_Type, string Device_Token, UpdateProfileCallBack callBack)
        {
            WWWForm webForm = new WWWForm();

            webForm.AddField("userid", User_Id); //UI_Manager._instance.User_Id);user_image
            webForm.AddField("name", name);
            webForm.AddField("mobile", number);
            webForm.AddField("device_type", Device_Type);
            webForm.AddField("device_token", Device_Token);
            webForm.AddField("avatar_id", AvatarId);
            WebRequest(APIServices.Update_Profile, webForm,
                (url, success, data) => OnCompleteUpdateProfile(success, data, callBack));
        }

        private static void OnCompleteUpdateProfile(bool aSuccess, object aData, UpdateProfileCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            UpdateProfileHeader update = JsonConvert.DeserializeObject<UpdateProfileHeader>(aData.ToString());


            if (!update.error)
            {
                callBack(true, update);
            }
            else
            {
                callBack(false, update);
            }
        }


        public static void PrivacyPolicy(PrivacyPolicyCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("pageid", "25");
            WebRequest(APIServices.Privacy_Policy, webForm,
                (url, success, data) => OnCompletePrivacyPolicy(success, data, callBack));
        }

        private static void OnCompletePrivacyPolicy(bool aSuccess, object aData, PrivacyPolicyCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            PrivacyPolicyHeader policy = JsonConvert.DeserializeObject<PrivacyPolicyHeader>(aData.ToString());


            if (!policy.error)
            {
                callBack(true, policy);
            }
            else
            {
                callBack(false, policy);
            }
        }


        public static void TermsCondition(TermsConditionCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("pageid", "24");

            WebRequest(APIServices.Terms_Condition, webForm,
                (url, success, data) => OnCompleteTermsCondition(success, data, callBack));
        }

        private static void OnCompleteTermsCondition(bool aSuccess, object aData, TermsConditionCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            TermsConditionHeader term = JsonConvert.DeserializeObject<TermsConditionHeader>(aData.ToString());


            if (!term.error)
            {
                callBack(true, term);
            }
            else
            {
                callBack(false, term);
            }
        }

        public static void Logout(string userid, LogoutCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("userid", userid); //PlayerPrefs.GetString("PlayerId"));
            WebRequest(APIServices.Logout, webForm,
                (url, success, data) => OnCompleteLogout(success, data, callBack));
        }

        private static void OnCompleteLogout(bool aSuccess, object aData, LogoutCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            LogoutHeader log = JsonConvert.DeserializeObject<LogoutHeader>(aData.ToString());

            if (!log.error)
            {
                callBack(true, log);
            }
            else
            {
                callBack(false, log);
            }
        }

        public static void GetUserRank(GetUserRankCallBack callBack)
        {
            WebRequestGet(APIServices.User_Rank,
                (url, success, data) => OnCompleteGetUserRank(success, data, callBack));
        }

        private static void OnCompleteGetUserRank(bool aSuccess, object aData, GetUserRankCallBack callBack)
        {
            LogSystem.LogEvent("Profile Response {0}", aData.ToString());

            GetUserRankeHeader rank = JsonConvert.DeserializeObject<GetUserRankeHeader>(aData.ToString());


            if (!rank.error)
            {
                callBack(true, rank);
            }
            else
            {
                callBack(false, rank);
            }
        }

        public static void ContactAdmin(string subject, string messege, string User_Id, ContactAdminCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("userid", User_Id);
            webForm.AddField("subject", subject);
            webForm.AddField("message", messege);
            WebRequest(APIServices.Contact_Admin, webForm,
                (url, success, data) => OnCompleteContactAdmin(success, data, callBack));
        }

        private static void OnCompleteContactAdmin(bool aSuccess, object aData, ContactAdminCallBack callBack)
        {
            LogSystem.LogEvent("Profile Response {0}", aData.ToString());

            ContactAdmintHeader admin = JsonConvert.DeserializeObject<ContactAdmintHeader>(aData.ToString());


            if (!admin.error)
            {
                callBack(true, admin);
            }
            else
            {
                callBack(false, admin);
            }
        }

        public static void TodayStack(TodayLetterStackCallBack callBack)
        {
            WebRequestGet(APIServices.TodayStack_URL,
                (url, success, data) => OnCompleteTodayStack(success, data, callBack));
        }


        private static void OnCompleteTodayStack(bool aSuccess, object aData, TodayLetterStackCallBack callBack)
        {
            LogSystem.LogEvent("Today Letter {0}", aData.ToString());

            TodayLetterStackHeader stack = JsonConvert.DeserializeObject<TodayLetterStackHeader>(aData.ToString());

            if (!stack.error)
            {
                callBack(true, stack);
            }
            else
            {
                callBack(false, stack);
            }
        }

        public static void ResumeDataStack(int userID, GameStateDataCallBack callBack)
        {
            WebRequestGet(APIServices.ResumeData_URL + userID,
                (url, success, data) => OnCompleteResumeDataStack(success, data, callBack));
        }

        private static void OnCompleteResumeDataStack(bool aSuccess, object aData, GameStateDataCallBack callBack)
        {
            LogSystem.LogEvent("Profile Response {0}", aData.ToString());

            GameStateDataHeader Gamedata = JsonConvert.DeserializeObject<GameStateDataHeader>(aData.ToString());

            if (!Gamedata.error)
            {
                callBack(true, Gamedata);
            }
            else
            {
                callBack(false, Gamedata);
            }
        }


        public static void SendFriendRequest(string userId, string FriendId, string ActionUserId, SendFriendRequestCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", userId);
            webForm.AddField("friend_id", FriendId);
            webForm.AddField("action_user_id", ActionUserId);
            WebRequest(APIServices.SendFriendRequest_URL, webForm,
                (url, success, data) => OnCompleteSendFriendList(success, data, callBack));
        }

        private static void OnCompleteSendFriendList(bool aSuccess, object aData, SendFriendRequestCallBack callBack)
        {
            LogSystem.LogEvent("Profile Response {0}", aData.ToString());

            SendFriendRequestHeader send = JsonConvert.DeserializeObject<SendFriendRequestHeader>(aData.ToString());


            if (!send.error)
            {
                callBack(true, send);
            }
            else
            {
                callBack(false, send);
            }
        }


        public static void AcceptFriendRequest(string userId, string ActionUserId, string Accept_reject, AcceptFriendRequestCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("action_user_id", ActionUserId);
            webForm.AddField("user_id", userId);
            webForm.AddField("accept_reject", Accept_reject);

            WebRequest(APIServices.AceptFriendRequest_URL, webForm,
                (url, success, data) => OnCompleteAcceptFriendRequest(success, data, callBack));
        }

        private static void OnCompleteAcceptFriendRequest(bool aSuccess, object aData, AcceptFriendRequestCallBack callBack)
        {
            LogSystem.LogEvent("Profile Response {0}", aData.ToString());

            AcceptFriendRequestHeader Accept = JsonConvert.DeserializeObject<AcceptFriendRequestHeader>(aData.ToString());


            if (!Accept.error)
            {
                callBack(true, Accept);
            }
            else
            {
                callBack(false, Accept);
            }
        }


        public static void RejectFriendRequest(string userId, string ActionUserId, RejectFriendRequestCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", userId);
            webForm.AddField("action_user_id", ActionUserId);
            // webForm.AddField("accept_reject", Accept_reject);
            WebRequest(APIServices.AceptFriendRequest_URL, webForm,
                (url, success, data) => OnCompleteRejectFriendRequest(success, data, callBack));
        }

        private static void OnCompleteRejectFriendRequest(bool aSuccess, object aData, RejectFriendRequestCallBack callBack)
        {
            LogSystem.LogEvent("Profile Response {0}", aData.ToString());

            RejectFriendRequestHeader reject = JsonConvert.DeserializeObject<RejectFriendRequestHeader>(aData.ToString());

            if (!reject.error)
            {
                callBack(true, reject);
            }
            else
            {
                callBack(false, reject);
            }
        }

        public static void MyFriendList(string userId, MyFriendListCallBack callBack)
        {
            WebRequestGet(APIServices.GetMyFriendList_URL + "?user_id=" + userId,
                (url, success, data) => OnCompleteGetMyFriendList(success, data, callBack));
        }


        private static void OnCompleteGetMyFriendList(bool aSuccess, object aData, MyFriendListCallBack callBack)
        {
            LogSystem.LogEvent("Profile Response {0}", aData.ToString());

            MyFriendListHeader FriendList = JsonConvert.DeserializeObject<MyFriendListHeader>(aData.ToString());


            if (!FriendList.error)
            {
                callBack(true, FriendList);
            }
            else
            {
                callBack(false, FriendList);
            }
        }


        public static void PendingFriendList(string userId, PendingFriendListCallBack callBack)
        {
            WebRequestGet(APIServices.GetPendingFriendList_URL + "?user_id=" + userId,
                (url, success, data) => OnCompleteGetPendingFriendList(success, data, callBack));
        }


        private static void OnCompleteGetPendingFriendList(bool aSuccess, object aData, PendingFriendListCallBack callBack)
        {
            LogSystem.LogEvent("Profile Response {0}", aData.ToString());
            PendingFriendListHeader Pending = JsonConvert.DeserializeObject<PendingFriendListHeader>(aData.ToString());

            if (!Pending.error)
            {
                callBack(true, Pending);
            }
            else
            {
                callBack(false, Pending);
            }
        }

        public static void SearchFriend(string search, SearchFriendCallBack callBack)
        {
            WebRequestGet(APIServices.GetSearchFriend_URL + "?search=" + search,
                (url, success, data) => OnCompleteGetSearchFriend(success, data, callBack));
        }


        private static void OnCompleteGetSearchFriend(bool aSuccess, object aData, SearchFriendCallBack callBack)
        {
            LogSystem.LogEvent("Search Response {0}", aData.ToString());
            SearchFriendListHeader search = JsonConvert.DeserializeObject<SearchFriendListHeader>(aData.ToString());

            if (!search.error)
            {
                callBack(true, search);
            }
            else
            {
                callBack(false, search);
            }
        }

        public static void UpdateNotification(string userid, string notificationid, UppdateNotificationCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", userid);
            webForm.AddField("notification_id", notificationid);

            WebRequest(APIServices.UpdateNotificationId, webForm,
                (url, success, data) => OnCompleteUpdatenotification(success, data, callBack));
        }

        private static void OnCompleteUpdatenotification(bool aSuccess, object aData, UppdateNotificationCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            UpdateNotificationHeader term = JsonConvert.DeserializeObject<UpdateNotificationHeader>(aData.ToString());


            if (!term.error)
            {
                callBack(true, term);
            }
            else
            {
                callBack(false, term);
            }
        }

        public static void CreateGame(string userid, string gametype, string time, string date, string game_mode, int multiplayerType, CreateGameCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", userid);
            webForm.AddField("game_type", gametype);
            webForm.AddField("time", time);
            webForm.AddField("date", date);
            webForm.AddField("game_mode", game_mode);
            webForm.AddField("multiplayer_type", multiplayerType);

            Debug.Log($"{userid}, {gametype}, {time}, {date}, {game_mode}, {multiplayerType}");
            WebRequest(APIServices.CreateGame_URL, webForm,
                (url, success, data) => OnCompleteCreateGame(success, data, callBack));
        }

        private static void OnCompleteCreateGame(bool aSuccess, object aData, CreateGameCallBack callBack)
        {
            LogSystem.LogColorEvent("yellow", "RecievedData {0}", aData.ToString());

            CreateGameData gameData = JsonConvert.DeserializeObject<CreateGameData>(aData.ToString());

            if (aSuccess)
            {
                callBack(true, gameData);
            }
            else
            {
                callBack(false, gameData);
            }
        }

        public static void GameList(string userid, GameListCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", userid);

            WebRequest(APIServices.GameList_URL, webForm,
                (url, success, data) => OnCompleteGameList(success, data, callBack));
        }

        private static void OnCompleteGameList(bool aSuccess, object aData, GameListCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            GameListHeader game = JsonConvert.DeserializeObject<GameListHeader>(aData.ToString());

            if (!game.error)
            {
                callBack(true, game);
            }
            else
            {
                callBack(false, game);
            }
        }

        public static void GameData(string gameid, GameDataCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("game_id", gameid);


            WebRequest(APIServices.GameData_URL, webForm,
                (url, success, data) => OnCompleteGameData(success, data, callBack));
        }

        private static void OnCompleteGameData(bool aSuccess, object aData, GameDataCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            GameDataHeader game = JsonConvert.DeserializeObject<GameDataHeader>(aData.ToString());
            DataSaver.saveData(game, "GameData");

            if (!game.error)
            {
                callBack(true, game);
            }
            else
            {
                callBack(false, game);
            }
        }

        public static void GameComplete(string gameid, string userid, string win, int score, GameCompleteCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("game_id", gameid);
            webForm.AddField("user_id", userid);
            webForm.AddField("is_win", win);
            webForm.AddField("score", score);


            Debug.Log("Check game complete ........." + "game_id.." + gameid + " user_id...." + userid + "is_win.." + win + "score..." + score);

            WebRequest(APIServices.GameComPlete_URL, webForm,
                (url, success, data) => OnCompleteGameComplete(success, data, callBack));
        }

        private static void OnCompleteGameComplete(bool aSuccess, object aData, GameCompleteCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            GameComplete game = JsonConvert.DeserializeObject<GameComplete>(aData.ToString());


            if (!game.error)
            {
                Debug.Log("Success!");
                callBack(true, game);
            }
            else
            {
                Debug.Log("Faliure!");
                callBack(false, game);
            }
        }
        public static void OutGameBonus(string userid, string outgamebonus, OutGameBonusCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", userid);
            webForm.AddField("out_game_bonus", outgamebonus);


            Debug.Log("Check out game bonus ........." + " user_id...." + userid  + " outGameBonus...." + outgamebonus );

            WebRequest(APIServices.OutGameBonus_URL, webForm,
                (url, success, data) => OnOutGameBonusComplete(success, data, callBack));
        }

        private static void OnOutGameBonusComplete(bool aSuccess, object aData, OutGameBonusCallBack callBack)
        {
            Debug.Log("RecievedData: "+aData.ToString());

            OutGameBonus outGameBonus = JsonConvert.DeserializeObject<OutGameBonus>(aData.ToString());


            if (!outGameBonus.error)
            {
                Debug.Log("Success!");
                callBack(true, outGameBonus);
            }
            else
            {
                Debug.Log("Faliure!");
                callBack(false, outGameBonus);
            }
        }

        private static int tempStatus = -1;

        public static void SaveGameData(int game_id, string user_id, int status, GameSyncData game_data, SaveGameDataCallBack callBack)
        {
            tempStatus = status;
            UserSaveGameData savedata = new UserSaveGameData(game_id, user_id, status, game_data);
            string json = JsonConvert.SerializeObject(savedata);
            Debug.Log(json);
            WebRequest(APIServices.SaveGameData_URL, json,
                (url, success, data) => OnCompleteSaveGameData(success, data, callBack));
        }

        private static void OnCompleteSaveGameData(bool aSuccess, object aData, SaveGameDataCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());
            Debug.Log("RecievedData " + aData.ToString());
            UserSaveData game = JsonConvert.DeserializeObject<UserSaveData>(aData.ToString());

            if (!game.error)
            {
                if (tempStatus == 0)
                {
                    tempStatus = -1;
                    GameStatesUI.Instance.RefreshStats();
                }

                callBack(true, game);
            }
            else
            {
                callBack(false, game);
            }
        }

        public static void CheckWord(string word, CheckWordCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("word", word);


            WebRequest(APIServices.WordCheck_URL, webForm,
                (url, success, data) => OnCompleteCheckWord(success, data, callBack));
        }

        private static void OnCompleteCheckWord(bool aSuccess, object aData, CheckWordCallBack callBack)
        {
            LogSystem.LogEvent("RecievedData {0}", aData.ToString());

            CheckWordHeader word = JsonConvert.DeserializeObject<CheckWordHeader>(aData.ToString());


            if (!word.error)
            {
                callBack(true, word);
            }
            else
            {
                callBack(false, word);
            }
        }


        public static void GetDailyLetter(GetDailyLetterCallBack callBack)
        {
            WebRequestGet(APIServices.GetDailyLetter_URL,
                (url, success, data) => OnCompleteGetDailyLetter(success, data, callBack));
        }

        private static void OnCompleteGetDailyLetter(bool aSuccess, object aData, GetDailyLetterCallBack callBack)
        {
            LogSystem.LogEvent("Daily Letter Response {0}", aData.ToString());


            GetDailyLetterHeader DailyLetter = JsonConvert.DeserializeObject<GetDailyLetterHeader>(aData.ToString());


            if (!DailyLetter.error)
            {
                callBack(true, DailyLetter);
            }
            else
            {
                callBack(false, DailyLetter);
            }
        }

        public static void LeaderBoard(string userId, LeaderBoardCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", userId);

            WebRequest(APIServices.LeaderBoard_URL, webForm,
                (url, success, data) => OnCompleteLeaderboard(success, data, callBack));
        }

        private static void OnCompleteLeaderboard(bool aSuccess, object aData, LeaderBoardCallBack callBack)
        {
            LogSystem.LogEvent("Leaderboard  Response {0}", aData.ToString());


            LeaderBoardHeader Board = JsonConvert.DeserializeObject<LeaderBoardHeader>(aData.ToString());


            if (!Board.error)
            {
                callBack(true, Board);
            }
            else
            {
                callBack(false, Board);
            }
        }


        public static void GetDates(GetDatesCallBack callBack)
        {
            WebRequest(APIServices.GetDates_URL, string.Empty,
                (url, success, data) => OnCompleteDates(success, data, callBack));
        }

        private static void OnCompleteDates(bool aSuccess, object aData, GetDatesCallBack callBack)
        {
            LogSystem.LogEvent("Response {0}", aData.ToString());


            GetDatesHeader data = JsonConvert.DeserializeObject<GetDatesHeader>(aData.ToString());


            if (!data.error)
            {
                callBack(true, data);
            }
            else
            {
                callBack(false, data);
            }
        }


        public static void DateWiseData(string date, DateWiseDataCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("date", date);

            WebRequest(APIServices.GetDateWise_URL, webForm,
                (url, success, data) => OnCompleteDateWiseData(success, data, callBack));
        }

        private static void OnCompleteDateWiseData(bool aSuccess, object aData, DateWiseDataCallBack callBack)
        {
            LogSystem.LogEvent("  Response {0}", aData.ToString());


            DateWiseDataHeader data = JsonConvert.DeserializeObject<DateWiseDataHeader>(aData.ToString());


            if (!data.error)
            {
                callBack(true, data);
            }
            else
            {
                callBack(false, data);
            }
        }


        public static void GetCalendarData(int month, int year, int gameType, string userid, CalendarDataCallback callback)
        {
            WWWForm form = new WWWForm();
            form.AddField("month", month);
            form.AddField("year", year);
            form.AddField("user_id", userid);
            form.AddField("game_type", gameType);
            WebRequest(APIServices.GetCalendar, form, (url, success, data) => OnCompletedCalendarData(success, data, callback));
        }

        private static void OnCompletedCalendarData(bool aSuccess, object aData, CalendarDataCallback callback)
        {
            LogSystem.LogEvent("CalendarData {0}", aData.ToString());

            if (string.IsNullOrEmpty(aData.ToString()))
            {
                callback?.Invoke(false, null);
                return;
            }

            if (aSuccess)
            {
                List<CalendarData> calendarData = JsonConvert.DeserializeObject<List<CalendarData>>(aData.ToString());
                callback?.Invoke(true, calendarData);
            }
            else
            {
                callback?.Invoke(false, null);
            }
        }

        public static void GetGameDataDate(string userId, int month, string datestring, int gameType, int gameMode, DateGameDataCallBack callBack)
        {
            WWWForm form = new WWWForm();
            form.AddField("user_id", userId);
            form.AddField("month", month);
            form.AddField("date", datestring);
            form.AddField("game_type", gameType);
            form.AddField("game_mode", gameMode);
            WebRequest(APIServices.DateData, form, (url, success, data) => OnCompletedGameDateData(success, data, callBack));
        }

        private static void OnCompletedGameDateData(bool aSuccess, object aData, DateGameDataCallBack callBack)
        {
            LogSystem.LogEvent("DateGameData {0}", aData.ToString());
            if (aSuccess)
            {
                DateGameData dateGameData = JsonConvert.DeserializeObject<DateGameData>(aData.ToString(), jsonSettings);
                callBack?.Invoke(true, dateGameData);
            }
            else
            {
                callBack?.Invoke(false, null);
            }
        }


        public static void DeleteGame(string Gameid, DeleteGameCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", GlobalData.UserId);
            webForm.AddField("game_id", Gameid);

            WebRequest(APIServices.GameDelete, webForm,
                (url, success, data) => OnCompleteDeleteGame(success, data, callBack));
        }

        private static void OnCompleteDeleteGame(bool aSuccess, object aData, DeleteGameCallBack callBack)
        {
            LogSystem.LogEvent("  Response {0}", aData.ToString());


            DeleteGameHeader data = JsonConvert.DeserializeObject<DeleteGameHeader>(aData.ToString());


            if (!data.error)
            {
                callBack(true, data);
            }
            else
            {
                callBack(false, data);
            }
        }


        public static void GetUserPower(GetBonousPointResponseCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", GlobalData.UserId);

            WebRequest(APIServices.GetGamePower, webForm,
                (url, success, data) => OnCompleteGetUserPower(success, data, callBack));
        }

        private static void OnCompleteGetUserPower(bool aSuccess, object aData, GetBonousPointResponseCallBack callBack)
        {
            LogSystem.LogEvent("  Response {0}", aData.ToString());
            GetBonousPointResponse data = JsonConvert.DeserializeObject<GetBonousPointResponse>(aData.ToString());


            if (!data.error)
            {
                callBack(true, data);
            }
            else
            {
                callBack(false, data);
            }
        }

        public static void UpdateUserPower(string power, string point, string sign, UpdateBonousPointResponseCallBack callBack)
        {
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", GlobalData.UserId);
            webForm.AddField("power", power);
            webForm.AddField("point", point);
            webForm.AddField("sign", sign);

            WebRequest(APIServices.UpdateGamePower, webForm,
                (url, success, data) => OnCompleteUpdateUserPower(success, data, callBack));
        }

        private static void OnCompleteUpdateUserPower(bool aSuccess, object aData, UpdateBonousPointResponseCallBack callBack)
        {
            LogSystem.LogEvent("  Response {0}", aData.ToString());
            UpdateBonousPointResponse data = JsonConvert.DeserializeObject<UpdateBonousPointResponse>(aData.ToString());


            if (!data.error)
            {
                callBack(true, data);
            }
            else
            {
                callBack(false, data);
            }
        }

        public static void SetPremiumStatus(bool status, SetPremiumResponseCallBack callBack)
        {
            int i = status ? 1 : 2;
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", GlobalData.UserId);
            webForm.AddField("accept_reject", i);
            EventHandlerGame.EmitEvent(GameEventType.Loading, true);

            WebRequest(APIServices.SetPremiumStatus, webForm,
                (url, success, data) => OnCompleteSetPremium(success, data, callBack));
        }

        private static void OnCompleteSetPremium(bool aSuccess, object aData, SetPremiumResponseCallBack callBack)
        {
            EventHandlerGame.EmitEvent(GameEventType.Loading, false);

            LogSystem.LogEvent("  Response {0}", aData.ToString());
            SetPremiumResponse data = JsonConvert.DeserializeObject<SetPremiumResponse>(aData.ToString());

            if (!data.error)
            {
                callBack(true, data);
            }
            else
            {
                callBack(false, data);
            }
        }

        public static void GetGameStats(string user_id, string game_id, GetPlayerStatsCallBack callBack)
        {
            Debug.Log(user_id + ", " + game_id);
            WWWForm webForm = new WWWForm();
            webForm.AddField("user_id", user_id);
            webForm.AddField("game_id", game_id);
            WebRequest(APIServices.GetPlayerStats, webForm,
                (url, success, data) => OnCompleteGetGameStats(success, data, callBack));
        }

        private static void OnCompleteGetGameStats(bool aSuccess, object aData, GetPlayerStatsCallBack callBack)
        {
            PlayerStatsData game = JsonConvert.DeserializeObject<PlayerStatsData>(aData.ToString());
            Debug.Log(aData.ToString());

            if (game.succeeded)
            {
                callBack(true, game);
            }
            else
            {
                callBack(false, game);
            }
        }
    }
}