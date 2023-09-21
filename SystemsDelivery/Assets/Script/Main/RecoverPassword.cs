using Firebase.Auth;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecoverPassword : MonoBehaviour
{
    [SerializeField] private Button _resetPasswordButton;

    void Reset()
    {
        _resetPasswordButton = GetComponent<Button>();
    }

    void Start()
    {
        _resetPasswordButton.onClick.AddListener(HandleResetPasswordButtonClicked);
    }

    void HandleResetPasswordButtonClicked()
    {
        string email = GameObject.Find("InputEmail").GetComponent<TMP_InputField>().text;

        StartCoroutine(ResetPassword(email));
    }

    private IEnumerator ResetPassword(string email)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var resetTask = auth.SendPasswordResetEmailAsync(email);

        yield return new WaitUntil(() => resetTask.IsCompleted);

        if (resetTask.IsCanceled)
        {
            Debug.LogError($"SendPasswordResetEmailAsync is canceled");
        }
        else if (resetTask.IsFaulted)
        {
            Debug.LogError($"SendPasswordResetEmailAsync encountered error" + resetTask.Exception);
        }
        else
        {
            Debug.Log("Password reset email sent successfully to: " + email);
        }
    }
}