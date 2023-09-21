using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseGame : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad = "Intermediate";
    public void CloseGames()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}
