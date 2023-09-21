using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPlay : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad = "Game";
    public void OpenGame()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}
