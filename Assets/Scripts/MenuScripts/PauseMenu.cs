using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuScripts
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pausePanel;

        public void OpenPause()
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}