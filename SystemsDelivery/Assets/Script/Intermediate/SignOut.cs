using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignOut : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad = "Main";
    public void SignOuts()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene(_sceneToLoad);
    }
}
