// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class LoginCl
// {
//     public static LoginCl instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singletong
//             instance = this;
           
//         }
//     }
//    public string id  { get; set; }
//     public  string token { get; set; }


// }
// public class LoginHeader
// {
//     public string error { get; set; }
//     public string message { get; set; }
//     public LoginCl result { get; set; }
// }

// public class _SingUp
// {
//     public static _SingUp instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//     public  string id { get; set; }
//     public  string token { get; set; }
//     public   string SingUp_Otp{get; set;}
//     public string message{get; set;}
// }

// public class SingUpHeader
// {
//     public string error { get; set; }
//     public string message { get; set; }
//     public _SingUp result { get; set; }
// }



// public class ForgotPasswords
// {
//   public static ForgotPasswords instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }

//   public string Otp{get; set;} 
//    public string email{get; set;} 
//     public string id{get; set;} 

// }

// public class ForgotPasswordHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
//     public ForgotPasswords result {get;set;}

// }

// public class VerifyOTP
// {
//   public static VerifyOTP instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }

//   public string error {get; set;} 
//   public string result {get; set;} 
//   public string message {get; set;} 
//  public string userid {get; set;} 
  

// }


// public class verifyOTPHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
//     public VerifyOTP result {get;set;}

// }
// public class _ResendOtp
// {
//   public static _ResendOtp instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }

//   public string otp {get; set;} 
//   public string Email {get; set;} 
//  public string id {get; set;} 
// }

// public class ResendOtpHeader{
//      public bool error { get; set; }
//     public string message { get; set; } 
//     public _ResendOtp result {get;set;}

// }



// public class UpdateProfile
// {
//   public static UpdateProfile instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//     public string id{get; set;}
//     public string messege {get; set;}
//     public string name { get; set; }
//     public string email { get; set; }
//     public string user_image { get; set; }
    

// }

// public class UpdateProfileHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
//     public UpdateProfile result {get;set;}

// }

// public class _PrivacyPolicy
// {
//   public static _PrivacyPolicy instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//    public int id{get;set;}
//    public string name {get; set;} 
//   public string slug {get; set;}
//    public string short_description {get; set;}  


//   public string messege {get; set;} 

// }

// public class PrivacyPolicyHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
//     public _PrivacyPolicy result {get;set;}

// }

// public class TermsCondition
// {
//   public static TermsCondition instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//    public int id{get;set;}
//    public string name {get; set;} 
//   public string slug {get; set;}
//    public string short_description {get; set;}  
//    public string messege {get; set;} 

// }

// public class TermsConditionHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
//     public TermsCondition result {get;set;}

// }

// public class ChangePassword
// {
//   public static ChangePassword instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//    public string id{get;set;}
//    public string messege {get; set;} 
//    public string error { get; set; }


// }

// public class ChangePasswordHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
//     public ChangePassword result {get;set;}

// }


// public class Logout
// {
//   public static Logout instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//      public string error { get; set; }
//    public string messege {get; set;} 

// }

// public class LogoutHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
    
// }


// public class GetProfile
// {
//   public static GetProfile instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//     public string name { get; set; }
//     public string email { get; set; }
//     public string mobile { get; set; }
//     public string user_image { get; set; }
//     public string error { get; set; }
//     public string messege {get; set;} 
  


// }

// public class GetProfileHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
//     public GetProfile result {get; set; }
// }

// public class _GetUserRank
// {
//   public static  _GetUserRank instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//     public string id { get; set; }
//     public string name { get; set; }
//     public string user_image { get; set; }
//     public string rank { get; set; }
//      public string error { get; set; }
//    public string messege {get; set;} 

// }

// public class GetUserRankeHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
//     public  _GetUserRank []result {get; set; }
// }


// public class FbLogin
// {
//   public static  FbLogin instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//     public string id { get; set; }
//     public string name { get; set; }
//     public string email { get; set; }
//     public string user_image { get; set; }
//      public string fb_id { get; set; }
//    public string status {get; set;} 
//    public string device_type {get; set;} 
//    public string device_token {get; set;} 

// }

// public class FBHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
//     public  FbLogin result {get; set; }
// }




// public class AvtarImage
// {
//  public static  AvtarImage instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//     public string id { get; set; }
//     public string name { get; set; }
//     public string avatar_image { get; set; }
    
// }

// public class GetAvtarHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
//     public  AvtarImage []result {get; set; }
// }






// public class ContactAdmin
// {
//  public static  ContactAdmin instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//     public string id { get; set; }
    
// }

// public class ContactAdmintHeader{
//      public string error { get; set; }
//     public string message { get; set; } 
//     public  ContactAdmin result {get; set; }
// }


// public class TodayLetterStack
// {
//  public static  TodayLetterStack instance;
//    void  Awake()
//     {
//       if (instance == null)
//         {
//             //If I am the first instance, make me the Singleton
//             instance = this;
           
//         }
//     }
//     public string id { get; set; }
//      public string alphabet { get; set; }
//       public string score { get; set; }
    
// }

// public class TodayLetterStackHeader {
//      public string error { get; set; }
//     public string message { get; set; } 
//     public  TodayLetterStack [] result {get; set; }
// }

// public class GameStateDataHeader
// {
//     public string error { get; set; }
//     public string message { get; set; }
//     public GameStateData result { get; set; }
// }






// public class Decode : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }




// }
