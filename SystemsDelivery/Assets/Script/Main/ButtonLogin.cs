using Firebase.Auth;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;

public class ButtonLogin : MonoBehaviour
{
    [SerializeField] private Button _loginButton;
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _emailPasswordField;
    [SerializeField] private string _sceneToLoad = "Intermediate";

    public bool open;

    private FirebaseAuth _auth;

    void Start()
    {
        _loginButton.onClick.AddListener(HandleLoginButtonClicked);
        _auth = FirebaseAuth.DefaultInstance;
        open = false;
    }
    private void FixedUpdate()
    {
        if (open == true)
        {
            LoadScene();
        }
    }

    private void HandleLoginButtonClicked()
    {
        _auth.SignInWithEmailAndPasswordAsync(_emailInputField.text, _emailPasswordField.text)
            .ContinueWith(task => {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }
                else
                {
                    Debug.Log("User signed in successfully: " + _auth.CurrentUser.DisplayName);
                    open = true;
                }
            });
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneToLoad);
        Debug.Log("Algo Falla");
    }
}
