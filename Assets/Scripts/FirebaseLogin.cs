using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public class FirebaseLogin : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private GameObject loginButton;

    public void Login()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            UpdateStatus("Fill in all fields.", Color.red);
            return;
        }

        UpdateStatus("Connecting...", Color.white);
        if (loginButton != null) loginButton.SetActive(false);
        SignIn(email, password);
    }

    private async void SignIn(string email, string password)
    {
        const int signInTimeoutSeconds = 12;

        try
        {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            Task<AuthResult> loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
            Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(signInTimeoutSeconds));
            Task firstDone = await Task.WhenAny(loginTask, timeoutTask);

            if (firstDone == timeoutTask)
            {
                UpdateStatus(
                    "Connection timed out. Check internet, VPN, or firewall (Firebase must reach Google).",
                    Color.red);
                if (loginButton != null) loginButton.SetActive(true);
                return;
            }

            AuthResult result = await loginTask;
            UpdateStatus("Access Granted!", Color.green);

            string userEmail = result.User.Email;
            if (!string.IsNullOrEmpty(userEmail) && GameManager.Instance != null)
                GameManager.Instance.LoggedInEmail = userEmail;

            if (FirebaseManager.Instance != null)
                FirebaseManager.Instance.LoadAndStartGame();
            else
                SceneManager.LoadScene("CharacterSelect");
        }
        catch (Exception e)
        {
            string msg = GetLoginErrorMessage(e);
            UpdateStatus(msg, Color.red);
            Debug.LogError("Firebase Login Error: " + e);
            if (loginButton != null) loginButton.SetActive(true);
        }
    }

    static string GetLoginErrorMessage(Exception e)
    {
        if (e is FirebaseException fe)
        {
            switch ((AuthError)fe.ErrorCode)
            {
                case AuthError.WrongPassword:
                    return "Wrong password.";
                case AuthError.InvalidEmail:
                    return "Invalid email format.";
                case AuthError.UserNotFound:
                    return "No account for this email.";
                case AuthError.TooManyRequests:
                    return "Too many attempts. Try again later.";
                case AuthError.InvalidCredential:
                    return "Sign-in blocked (APK: add release SHA-1 in Firebase Console if using a release keystore).";
                default:
                    return string.IsNullOrEmpty(fe.Message) ? "Login failed." : fe.Message;
            }
        }

        return string.IsNullOrEmpty(e.Message) ? "Login failed." : e.Message;
    }

    private void UpdateStatus(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;
        }
    }
}