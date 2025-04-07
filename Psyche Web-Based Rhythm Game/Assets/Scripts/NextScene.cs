using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
  private int sceneCount = 0;
    public static NextScene Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    public void nextScene()
    {
       // UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);                                  
               // yield return Manager.Instance.nextLevel();
               
                //add so that it goes back to the game
        if(Manager.level == 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene Lvl1");   
        }
        else if (Manager.level == 2)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene Lvl2");   
        }
    }

    public void gameScene()
    {
        if(Manager.level < 3)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");  
            Invoke(nameof(Manager.Instance.nextLevel), 0.3f);
        }
        else if (Manager.level == 3)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Credits"); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            gameScene();            
        }
    }
}
