using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuScripts
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("MainScene");
        }

        public void QuitGame()
        {
            Application.Quit();
            // Debug.Log("Выход из игры"); // работает в редакторе
        }
    }
}