using UnityEngine;

public class NextScene : MonoBehaviour
{
    public static bool backToGame =false;
    public static double savedTime = 0;
    public static NextScene Instance;
    public static bool menuClicked = false;
    public static GameObject creditMenu;
    public static GameObject startMenu;
    void Start()
    {
        Instance = this;
    }

    
    public void nextScene()
    {                             
        if(Manager.level == 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene Lvl1");   
        }
        else if (Manager.level == 2)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene Lvl2");   
        }
        else if(Manager.level == 3)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene Lvl3");   
        }
    }

    public void GameOverScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver"); 
    }

    public void gameScene()
    {
        if(Manager.level < 3 && !Manager.GameOver)
        {
            backToGame = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");  
        }
        else if (Manager.level == 3 || Manager.GameOver)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");    
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            gameScene();            
        }
    }
}
