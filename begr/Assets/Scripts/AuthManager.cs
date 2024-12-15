using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

// Writen with the assistance of ChatGPT
// I just asked it to write base code and then i edit from there lol

public class AuthManager : MonoBehaviour
{
    public FirebaseAuth auth;

    // UI References for Login
    public GameObject SignInCanvas;
    public TMP_InputField loginEmailField;
    public TMP_InputField loginPasswordField;
    public TextMeshProUGUI loginError;

    // UI References for Sign Up
    public GameObject SignUpCanvas;
    public TMP_InputField signupEmailField;
    public TMP_InputField signupPasswordField;
    public TMP_InputField signupUsernameField;
    public TextMeshProUGUI signupError;

    public Camera CameraMain;

    public Canvas LoginUI;
    public Canvas GameUI;

    private GameManager gameManager;
    private DataManager dataManager;

    private bool SignIn = true;

    public string userId;

    void Start()
    {
        LoginUI.enabled = true;
        loginError.text = string.Empty;
        signupError.text = string.Empty;
        gameManager = FindObjectOfType<GameManager>();
        dataManager = FindObjectOfType<DataManager>();
        auth = FirebaseAuth.DefaultInstance;
        //StartGame();
    }

    // Sign-Up Function
    public void SignUp()
    {
        string email = signupEmailField.text;
        string password = signupPasswordField.text;
        string username = signupUsernameField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Sign-Up failed: " + task.Exception);
                signupError.text = "either an error or u need a better password";
                return;
            }
            if (task.IsCompleted)
            {
                AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
                userId = result.User.UserId;
                dataManager.CreatePlayer(username);
                Debug.Log("l");
                StartGame();
            }
        });
    }

    // Login Function
    public void Login()
    {
        string email = loginEmailField.text;
        string password = loginPasswordField.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Login failed: " + task.Exception);
                signupError.text = "wrong email or password or an error idk";
                return;
            }
            if (task.IsCompleted)
            {
                AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
                userId = result.User.UserId;
                Debug.Log("l");
                StartGame();
            }
        });
    }

    // Update Username (if needed for display purposes)
    private void UpdateUsername(string username)
    {
        var userProfile = new UserProfile { DisplayName = username };
        auth.CurrentUser.UpdateUserProfileAsync(userProfile).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Update username failed: " + task.Exception);
                return;
            }
            Debug.Log("Username updated successfully.");
        });
    }

    public void Switch()
    {
        SignIn = !SignIn;
        if (SignIn == true)
        {
            SignInCanvas.gameObject.SetActive(true);
            SignUpCanvas.gameObject.SetActive(false);
        }
        else
        {
            SignInCanvas.gameObject.SetActive(false);
            SignUpCanvas.gameObject.SetActive(true);
        }
    }

    public void StartGame()
    {
        LoginUI.enabled = false;
        GameUI.enabled = true;
        gameManager.StartGame();
        Debug.Log("game start!");
    }
}