using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score Instance;

    public Text scoreText;       // UI Text for displaying score
    public Text multiplierText;  // UI Text for displaying multiplier

    private int currentScore = 0;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this when the player hits a note
    public void AddScore(int points)
    {
        int multiplier = (Manager.pointStreak / 5) + 1;
        currentScore += points * multiplier;
        UpdateUI();
    }

    // Update the UI text
    public void UpdateUI()
    {
        int currentMultiplier = (Manager.pointStreak / 5) + 1;
        scoreText.text = "Score: " + currentScore.ToString();
        multiplierText.text = "Multiplier: x" + currentMultiplier.ToString();
    }
}