using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Intro Scene");
        Manager.GameOver = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleNASACollection()
    {
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
