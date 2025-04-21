using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        if(Manager.level == 3 || Manager.GameOver)
        {
            Manager.level = 0;
            Manager.gameRunning = false;
            Manager.GameOver = false;
           
            if (NextScene.menuClicked)
            {
                GameObjectActive.instance.startMenu.SetActive(false);
                GameObjectActive.instance.creditMenu.SetActive(true);
                NextScene.menuClicked = false;

            }
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

    public void ToggleNASACollection()
    {
        

        // Update button label text
        if (!Manager.NASACollection)
        {
           GameObjectActive.instance.buttonLabelText.text = "NASA Tracks";
            Manager.NASACollection = true;
        }
        else if (Manager.NASACollection)
        {
            
           GameObjectActive.instance.buttonLabelText.text = "Regular Tracks";
            Manager.NASACollection = false;
        }
        Debug.Log("NASA Collection: " + Manager.NASACollection);
    }
}
