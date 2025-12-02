using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI display;
    public ScoreboardManager scoreboardManager;  // Reference to the scoreboard

    float elapsed;
    bool stopped;

    void Update()
    {
        if (stopped) return;

        elapsed += Time.deltaTime;
        UpdateText();
    }

    void UpdateText()
    {
        int m = Mathf.FloorToInt(elapsed / 60f);
        int s = Mathf.FloorToInt(elapsed % 60f);
        display.text = $"{m:00}:{s:00}";
    }

    public void StopTimerOnDeath()
    {
        if (stopped) return;  // Prevent calling multiple times
        
        stopped = true;
        
        // Show the scoreboard with the final time
        if (scoreboardManager != null)
        {
            scoreboardManager.ShowScoreboard(elapsed);
        }
    }

    public float GetScore()
    {
        return elapsed;
    }
}