using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainScenes_
{
    public class MainMenu : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        public void Menu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void TipsIn()
        {
            SceneManager.LoadScene("Tips");
        }

        public void TipsOut()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void PlayGame()
        {
        
            SceneManager.LoadScene("Test Scene (Kenry)");
        }
        
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
            Debug.Log("Game Quit!");
            Application.Quit();
        }
    }
}
