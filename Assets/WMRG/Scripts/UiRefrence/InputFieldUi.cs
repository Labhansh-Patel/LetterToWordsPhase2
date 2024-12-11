using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class InputFieldUi : MonoBehaviour
{
    [Header("Log In")]
    public InputField SignEmailInput;
    public InputField SignPswdInput;

    [Header("Sign Up")]
    public InputField SinUpNameInput;
    public InputField SinUpEmailInput;
    public InputField SinUpPswdInput;
    public InputField SinUpCnfPswdInput;


    [Header("Otp")]
    public InputField OtpTextInput;

    [Header("Forgot Password")]
    public InputField ForgotEmailInput;

    [Header("Profile")]
    public InputField ProfileNameInput;
    public InputField ProfileEmailInput;
    public InputField ProfileContactInput;


    [Header("Reset pswd")]
    public InputField NewPswdInput;
    public InputField ConfPswdInput;

    [Header("JoinRoom")] public InputField roomCode;


}

    
