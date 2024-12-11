namespace APICalls
{
    public static class APIServices
    {
        public const string BASE_URL = "https://webmobril.org/dev/letterofwords/api/v1/";


        public const string LOGIN_URL = "api_login";
        public const string LOGIN_URLSocial = "socialLogin";
        public const string SIGNUP_URL = "api_signup";
        public const string FORGOTPASSWORD_URL = "api_forgetpassword";
        public const string OTP_URL = "api_verifyotp";
        public const string Resend_Otp = "api_resendotp";
        public const string Change_Password = "api_resetpassword";
        public const string Logout = "api_logout";
        public const string Get_Profile = "api_getprofile?userid=";
        public const string Update_Profile = "api_updateprofile";
        public const string Contact_Admin = "api_contactus";
        public const string Privacy_Policy = "api_privacypolicy?pageid=25";
        public const string Terms_Condition = "api_tnc?pageid=24";
        public const string User_Rank = "api_getuserrank";
        public const string FbLogin_URL = "api_fblogin";

        public const string TodayStack_URL = "api_todaystack";

        public const string ResumeData_URL = "api_letterboard";
        public const string SendFriendRequest_URL = "api_sendfriendrequest";
        public const string AceptFriendRequest_URL = "api_acceptfriendrequest";
        public const string RejectFriendRequest_URL = "api_declinefriendrequest";
        public const string GetMyFriendList_URL = "api_friendlist";
        public const string GetPendingFriendList_URL = "api_pendingfriendrequest";
        public const string GetSearchFriend_URL = "api_searchfriends";
        public const string UpdateNotificationId = "api_update_notificationid";
        public const string CreateGame_URL = "api_create_game";
        public const string GameList_URL = "api_game_list";
        public const string GameData_URL = "api_game_data";

        public const string SaveGameData_URL = "api_save_game_data";
        public const string GameComPlete_URL = "api_game_complete";
        public const string OutGameBonus_URL = "out_game_bonus";

        public const string CheckWord_URL = "api_check_word";
        public const string GetDailyLetter_URL = "api_getdailyletter";
        public const string LeaderBoard_URL = "api_leaderboard";
        public const string WordCheck_URL = "api_check_word";

        public const string GetDates_URL = "getdata_for_dates";
        public const string GetDateWise_URL = "getdata_datewise";

        public static string GetCalendar = "game_list_calender";
        public static string DateData = "game_list_datewise";
        public static string GameDelete = "game_delete";

        public static string GetGamePower = "get_bonous_point";

        //  public static string GetGamePower = "get_bonous_point"; 
        public static string UpdateGamePower = "update_game_power";
        public const string GetPlayerStats = "percentile_score";
        public const string SetPremiumStatus = "api_premimum_user";
    }
}