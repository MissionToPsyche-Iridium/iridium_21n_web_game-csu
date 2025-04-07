using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Intro Scene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
