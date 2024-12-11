using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace APICalls
{
    public class BaseHeader
    {
        public bool error;
        public string message;
    }

    public class BaseHeaderNew
    {
        public string ResponseCode;
        public bool succeeded;
        public string ResponseMessage;
    }

    public class Login
    {
        public string id;
        public string token;
    }

    public class LoginHeader : BaseHeader
    {
        public Login result;
    }

    public class SingUp
    {
        public string id;
        public string token;
        public string SingUp_Otp;
        public string message;
    }

    public class SingUpHeader : BaseHeader
    {
        public SingUp result;
    }


    public class ForgotPasswords
    {
        public string Otp;
        public string email;
        public string id;
    }

    public class ForgotPasswordHeader : BaseHeader
    {
        public ForgotPasswords result;
    }

    public class VerifyOTP
    {
        public string error;
        public string result;
        public string message;
        public string userid;
        public string access_token;
    }


    public class verifyOTPHeader : BaseHeader
    {
        public VerifyOTP result;
    }

    public class ResendOtp
    {
        public string otp;
        public string Email;
        public string id;
    }

    public class ResendOtpHeader : BaseHeader
    {
        public ResendOtp result;
    }


    public class UpdateProfile
    {
        public string id;
        public string messege;
        public string name;
        public string email;
        public string mobile;
        public string user_image;
        public string avatar_id;
    }

    public class UpdateProfileHeader : BaseHeader
    {
        public UpdateProfile result;
    }

    public class PrivacyPolicy
    {
        public int id;
        public string name;
        public string slug;
        public string short_description;


        public string messege;
    }

    public class PrivacyPolicyHeader : BaseHeader
    {
        public PrivacyPolicy result;
    }

    public class TermsCondition
    {
        public int id;
        public string name;
        public string slug;
        public string short_description;
        public string messege;
    }

    public class TermsConditionHeader : BaseHeader
    {
        public TermsCondition result;
    }

    public class ChangePassword
    {
        public string id;
        public string messege;
        public string error;
    }

    public class ChangePasswordHeader : BaseHeader
    {
        public ChangePassword result;
    }


    public class Logout
    {
        public string error;
        public string messege;
    }

    public class LogoutHeader : BaseHeader
    {
        public Logout result;
    }


    public class GetProfile
    {
        public string name;
        public string email;
        public string mobile;
        public string user_image;
        public string error;
        public string messege;
        public string avatar_id;
        public string PublicGameKey;
        public string PrivateGameKey;
        public int login_type;
        public Sprite socialAvatarImage;
        public int premium_user;
        public int out_game_bonus;

        public bool IsPremiumUser => premium_user == 1; // 1 == true, 0 == false

        public bool IsSocialLogin => login_type == 2 || login_type == 3;
    }

    public class GetProfileHeader : BaseHeader
    {
        public GetProfile result;
    }

    public class GetUserRank
    {
        public string id;
        public string name;
        public string user_image;
        public string rank;
        public string error;
        public string messege;
        public string avatar_id;
    }

    public class GetUserRankeHeader : BaseHeader
    {
        public GetUserRank[] result;
    }


    public class FbLogin
    {
        public string id;
        public string name;
        public string email;
        public string user_image;
        public string fb_id;
        public string status;
        public string device_type;
        public string device_token;
    }

    public class FBHeader : BaseHeader
    {
        public FbLogin result;
    }


    public class AvtarImage
    {
        public string id;
        public string name;
        public string avatar_image;
    }

    public class GetAvtarHeader : BaseHeader
    {
        public AvtarImage[] result;
    }


    public class ContactAdmin
    {
        public string id;
    }

    public class ContactAdmintHeader : BaseHeader
    {
        public ContactAdmin result;
    }


    public class TodayLetterStack
    {
        public string id;
        public string alphabet;
        public string score;
    }

    public class TodayLetterStackHeader : BaseHeader
    {
        public TodayLetterStack[] result;
    }


    public class GameStateDataHeader : BaseHeader
    {
        public GameStateData result;
    }


    public class SendFriendRequest
    {
        public string id;
        public string FriendID;
        public string ActionUserID;
        public string avatar_id;
    }

    public class SendFriendRequestHeader : BaseHeader
    {
        public SendFriendRequest result;
    }


    public class AcceptFriendRequest
    {
        public string id;
        public string ActionUserID;
        public string avatar_id;
    }

    public class AcceptFriendRequestHeader : BaseHeader
    {
        public AcceptFriendRequest result;
    }

    public class RejectFriendRequest
    {
        public string id;
        public string ActionUserID;
    }

    public class RejectFriendRequestHeader : BaseHeader
    {
        public RejectFriendRequest result;
    }


    public class MyFriendList
    {
        public string id;
        public string userid;
        public string FriendID;
        public string ActionUserID;
        public string status;
        public Friend friend;
    }

    public class MyFriendListHeader : BaseHeader
    {
        public List<MyFriendList> result;
    }

    public class Friend
    {
        public string id;
        public string name;
        public string first_name;
        public string last_name;
        public string email;
        public string txtpassword;
        public string mobile;
        public string user_language;
        public string country_id;
        public string state_id;
        public string city_id;
        public string agree_terms;
        public string user_image;
        public string roles_id;
        public string otp_verified;
        public string otp;
        public string otp_time;
        public string userlevel;
        public string gamelevel;
        public string user_rank;
        public string totalcoin;
        public string device_type;
        public string device_token;
        public string status;
        public string auth_token;
        public string user_lat;
        public string user_long;
        public string last_login;
        public string stripe_customer_id;
        public string record_created_by;
        public string record_updated_by;
        public string notification_id;
        public string avatar_id;
    }

    public class PendingFriendList
    {
        public string id;
        public string name;
        public string first_name;
        public string last_name;
        public string email;
        public string txtpassword;
        public string mobile;
        public string user_language;
        public string country_id;
        public string state_id;
        public string city_id;
        public string agree_terms;
        public string user_image;
        public string roles_id;
        public string otp_verified;
        public string otp;
        public string otp_time;
        public string userlevel;
        public string gamelevel;
        public string user_rank;
        public string totalcoin;
        public string device_type;
        public string device_token;
        public string status;
        public string auth_token;
        public string user_lat;
        public string user_long;
        public string last_login;
        public string stripe_customer_id;
        public string record_created_by;
        public string record_updated_by;
        public string avatar_id;
    }

    public class MyPendingFriendList
    {
        public string id;
        public string FriendID;
        public string ActionUserID;
        public string avatar_id;

        public string status;
        public PendingFriendList user_data;
    }

    public class PendingFriendListHeader : BaseHeader
    {
        public List<MyPendingFriendList> result;
    }


    public class SearchFriendList
    {
        public string id;
        public string Name;
        public string UserImage;
        public string avatar_id;
    }

    public class SearchFriendListHeader : BaseHeader
    {
        public List<SearchFriendList> result;
    }

    public class UpdateNotification
    {
        public string id;
        public string Name;
    }

    public class UpdateNotificationHeader : BaseHeader
    {
        public UpdateNotification result;
    }

    [Serializable]
    public class Letter
    {
        public int id;
        public string name;
        public string letterdate;
        public int status;
        public string created_at;
        public string updated_at;
        public string score;
        public string count;
        public int repeat;
    }

    public class CreateGameData : BaseHeader
    {
        public string game_id;
        public List<Letter> letters;
    }

    public class GameList
    {
        public string id;
        public string game_type;
        public string user_id;
        public string status;
        public string score;
        public string is_win;
        public string created_at;
        public string updated_at;

        public string game_data;
        public string time;
        public string date;
    }

    public class GameListHeader : BaseHeader
    {
        public IList<GameList> game_list;
    }

    [Serializable]
    public class GameData
    {
        public int id;
        public int game_type;
        public int user_id;

        public int status;

        //blic string score ;
        //ublic int is_win ;
        public string created_at;
        public string updated_at;
        public GameSyncData game_data;
    }

    [Serializable]
    public class GameDataHeader : BaseHeader
    {
        public GameData game_data;
    }


    public class UserSaveData : BaseHeader
    {
        // public string message ;
    }


    public class GameComplete : BaseHeader
    {
        // public string message;
    }
    
    public class OutGameBonus : BaseHeader
    {
        public OutGameBonusData data;
    }
    
    public class OutGameBonusData
    {
        public int user_id;
        public string name;
        public int out_game_bonus;
    }
    
    public class UserSaveGameData
    {
        public int game_id;
        public string user_id;
        public int status;
        public GameSyncData game_data;

        public UserSaveGameData(int gameid, string userid, int status, GameSyncData gamedata)
        {
            game_id = gameid;
            user_id = userid;
            this.status = status;
            game_data = gamedata;
        }
    }


    public class CheckWordHeader : BaseHeader
    {
        public string word;
    }

    public class GetDailyLetter
    {
        public int id;
        public string name;
        public string score;
        public string letterdate;
    }

    public class GetDailyLetterHeader : BaseHeader
    {
        public IList<GetDailyLetter> result;
    }


    public class UserData
    {
        public string name;
        public string avatar_id;
    }

    public class LeaderBoard
    {
        public int userid;
        public object ratio;
        public UserData user_data;
        public int rank;
    }

    public class LeaderBoardHeader : BaseHeader
    {
        public List<LeaderBoard> data;
        public int user_rank;
    }


    public class GetDatesHeader : BaseHeader
    {
        public List<string> result;
    }

    public class DateWiseDataHeader : BaseHeader
    {
        public List<GetDailyLetter> result;
    }

    public class CalendarData
    {
        public DateTime date;
        public int game_status;
    }

    public class GameListDate
    {
        public int id { get; set; }
        public string game_type { get; set; }
        public string game_mode { get; set; }
        public object multiplayer_type { get; set; }
        public int user_id { get; set; }
        public int status { get; set; }
        public string score { get; set; }
        public int? is_win { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object game_data { get; set; }
        public string time { get; set; }
        public string date { get; set; }
        public string game_status { get; set; }
    }

    public class DateGameData
    {
        public string message { get; set; }
        public List<GameListDate> game_list { get; set; }
    }


    public class DeleteGameHeader : BaseHeader
    {
    }


    public class BonousPoint
    {
        public int id;
        public int user_id;
        public int extended_time;
        public int block_extended_time;
        public int extra_tray_space;
        public int any_tile_from_stack;
        public int any_letter;
        public int free_garbage;
        public string created_at;
        public string updated_at;
    }

    public class GetBonousPointResponse : BaseHeader
    {
        public List<BonousPoint> data;
    }


    public class SendGetBonousPoint
    {
        public string user_id;

        public SendGetBonousPoint(string user_id)
        {
            this.user_id = user_id;
        }
    }

    public class UpdateBonousPointResponse : BaseHeader
    {
        public BonousPoint data;
    }

    public class SendUpdateBonousPoint
    {
        public string user_id;
        public string power;
        public string point;
        public string sign;

        public SendUpdateBonousPoint(string user_id, string power, string point, string sign)
        {
            this.user_id = user_id;
            this.power = power;
            this.point = point;
            this.sign = sign;
        }
    }

    public class SetPremiumResponse : BaseHeader
    {
        public SetPremium result;
    }

    public class SetPremium
    {
        public int user_id;
        public string name;
        public int premium_user;
        public string updated_at;

        public SetPremium(int _id, string _name, int _premium, string _updatedAt)
        {
            user_id = _id;
            name = _name;
            premium_user = _premium;
            updated_at = _updatedAt;
        }
    }


    public class PlayerStatsData : BaseHeaderNew
    {
        public PlayerStatsDataObj ResponseData;
    }

    public class PlayerStatsDataObj
    {
        public string score;
        public string name;
        public string average;
        public string percentile_score;

        public PlayerStatsDataObj(string _score, string _name, string _average, string _percentile_score)
        {
            score = _score;
            name = _name;
            average = _average;
            percentile_score = _percentile_score;
        }
    }
}