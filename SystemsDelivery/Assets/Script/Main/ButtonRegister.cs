using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.Tilemaps;
using Firebase.Database;

public class ButtonRegister : MonoBehaviour
{

    [SerializeField]
    private Button _registrationButton;
    private Coroutine _registrationCorutine;

    public GameObject main;
    public GameObject register;

    private DatabaseReference mDatabaseRef;

    void Reset()
    {
        _registrationButton = GetComponent<Button>();
    }
    void Start()
    {
        _registrationButton.onClick.AddListener(HandleRegisterButtonClicked);
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }
    
    void HandleRegisterButtonClicked()
    {
        string email = GameObject.Find("InputEmail").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("InputPassword").GetComponent<TMP_InputField>().text;

        _registrationCorutine = StartCoroutine(RegisterUser(email, password));

    }

    private IEnumerator RegisterUser(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.IsCanceled)
        {
            Debug.LogError($"CreateUserWithEmailAndPasswordAsync is canceled");

        }
        else if (registerTask.IsFaulted)
        {
            Debug.LogError($"CreateUserWithEmailAndPasswordAsync encoutered error");
        }
        else
        {
            Firebase.Auth.AuthResult result = registerTask.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            string name = GameObject.Find("InputUsername").GetComponent<TMP_InputField>().text;
            mDatabaseRef.Child("Users").Child(result.User.UserId).Child("Username").SetValueAsync(name);
            mDatabaseRef.Child("Users").Child(result.User.UserId).Child("Score").SetValueAsync(0);

            HideRegisters();
        }
    }
    private void HideRegisters()
    {
        register.SetActive(false);
        main.SetActive(true);
    }
}