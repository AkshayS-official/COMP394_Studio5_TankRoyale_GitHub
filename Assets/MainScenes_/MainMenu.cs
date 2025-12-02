using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        
        SceneManager.LoadScene("Main_Scene");
    }

    public void OpenSettings()
    {
        // Load your settings scene OR open a UI panel
        SceneManager.LoadScene("SettingsScene");
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }
}
