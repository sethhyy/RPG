using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public class FirebaseRegister : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text statusText;

    public GameObject LoginPanel;

    public void Register()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            statusText.text = "Email and password are required";
            return;
        }

        if (password.Length < 6)
        {
            statusText.text = "Password must be at least 6 characters";
            return;
        }

        statusText.text = "Connecting...";
        CreateUser(email, password);
    }

    async void CreateUser(string email, string password)
    {
        const int registerTimeoutSeconds = 12;

        try
        {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            Task<AuthResult> registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(registerTimeoutSeconds));
            Task firstDone = await Task.WhenAny(registerTask, timeoutTask);

            if (firstDone == timeoutTask)
            {
                statusText.text =
                    "Connection timed out. Check internet, VPN, or firewall (Firebase must reach Google).";
                return;
            }

            AuthResult result = await registerTask;
            FirebaseUser user = result.User;

            statusText.text = "Registration successful!";
            Debug.Log("Registered user: " + user.Email);

            // User is already signed in after registration — same flow as login (Firestore → map or CharacterSelect).
            if (!string.IsNullOrEmpty(user.Email) && GameManager.Instance != null)
                GameManager.Instance.LoggedInEmail = user.Email;

            if (FirebaseManager.Instance != null)
                FirebaseManager.Instance.LoadAndStartGame();
            else
                SceneManager.LoadScene("CharacterSelect");
        }
        catch (System.Exception e)
        {
            if (e is FirebaseException firebaseEx)
                statusText.text = GetErrorMessage((AuthError)firebaseEx.ErrorCode);
            else
                statusText.text = "Registration failed";

            Debug.LogError(e);
        }
    }

    string GetErrorMessage(AuthError error)
    {
        switch (error)
        {
            case AuthError.EmailAlreadyInUse:
                return "Email already registered";
            case AuthError.InvalidEmail:
                return "Invalid email format";
            case AuthError.WeakPassword:
                return "Weak password";
            case AuthError.InvalidCredential:
                return "Blocked on device (add APK signing SHA-1 in Firebase Console).";
            default:
                return "Registration failed";
        }
    }
}
