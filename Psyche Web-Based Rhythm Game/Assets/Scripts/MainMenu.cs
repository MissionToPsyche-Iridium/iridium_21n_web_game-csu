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

    public void ToggleNASACollection()
    {
        

        // Update button label text
        if (!Manager.NASACollection)
        {
           GameObjectActive.instance.buttonLabelText.text = "NASA Collection";
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
