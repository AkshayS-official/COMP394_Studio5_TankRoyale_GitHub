using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    
    public void PlayGame()
    {
        
        SceneManager.LoadScene("Test Scene (Kenry)");
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
