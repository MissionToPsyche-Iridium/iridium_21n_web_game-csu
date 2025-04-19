using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        if(Manager.level == 3 || Manager.GameOver)
        {
            Manager.level = 0;
            GameObjectActive.instance.startMenu.SetActive(false);
            GameObjectActive.instance.creditMenu.SetActive(true);
        }
    }
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Intro Scene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
