using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score Instance;

    public Text scoreText;       
    public Text multiplierText;  

    private int currentScore = 0;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        int multiplier = (Manager.pointStreak / 5) + 1;
        currentScore += points * multiplier;
        UpdateUI();
    }


    public void UpdateUI()
    {
        int currentMultiplier = (Manager.pointStreak / 5) + 1;
        scoreText.text = "Score: " + currentScore.ToString();
        multiplierText.text = "Multiplier: x" + currentMultiplier.ToString();
    }
}
