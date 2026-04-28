using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuScripts
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Game is exited.");
        }
        
        public void Settings()
        {
            Application.Quit();
            Debug.Log("Game is exited.");
        }
    }
}