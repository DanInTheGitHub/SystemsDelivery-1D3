using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load Scene When User Authenticacated
/// </summary>
public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private string _sceneToLoad = "GameScene";

    private void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChange;
    }

    private void HandleAuthStateChange(object sender, EventArgs e)
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
        void OnDestroy()
        {
            FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChange;
        }
    }
}
