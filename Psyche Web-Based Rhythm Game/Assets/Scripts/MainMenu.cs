using UnityEngine;

public class MainMenu : MonoBehaviour
{

    void Start()
    {
         if (!Manager.NASACollection)
        {
           GameObjectActive.instance.buttonLabelText.text = "Regular Tracks";
        }
        else if (Manager.NASACollection)
        {         
           GameObjectActive.instance.buttonLabelText.text = "NASA Tracks";
        }
    }
    public void PlayGame()
    {
        Manager.GameOver = false;
        Manager.level = 0; 
        NextScene.backToGame = false;
        Manager.midiLevel = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Intro Scene");
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
    }
}
