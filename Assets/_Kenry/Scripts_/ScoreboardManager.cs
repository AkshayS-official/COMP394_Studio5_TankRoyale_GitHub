using UnityEngine;
using TMPro;

public class ScoreboardManager : MonoBehaviour
{
    public GameObject scoreboardPanel;          // The UI panel that contains the scoreboard
    public TextMeshProUGUI currentTimeText;     // Text to display current survival time
    public TextMeshProUGUI bestTimeText;        // Text to display best time
    
    private const string BEST_TIME_KEY = "BestSurvivalTime";

    private void Start()
    {
        // Hide the scoreboard at the start
        if (scoreboardPanel != null)
        {
            scoreboardPanel.SetActive(false);
        }
    }

    public void ShowScoreboard(float currentTime)
    {
        // Get the best time from PlayerPrefs
        float bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY, 0f);
        
        // Check if current time is better than best time
        if (currentTime > bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat(BEST_TIME_KEY, bestTime);
            PlayerPrefs.Save();
        }
        
        // Update the UI texts
        currentTimeText.text = "Time Survived: " + FormatTime(currentTime);
        bestTimeText.text = "Best Time: " + FormatTime(bestTime);
        
        // Show the scoreboard panel
        scoreboardPanel.SetActive(true);
        
        // Optional: Pause the game
        Time.timeScale = 0f;
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetBestTime()
    {
        PlayerPrefs.DeleteKey(BEST_TIME_KEY);
        PlayerPrefs.Save();
        bestTimeText.text = "Best Time: 00:00";
    }
}
