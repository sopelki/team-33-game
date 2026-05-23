using UnityEngine;

namespace MenuScripts
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject settingsPanel;
        [SerializeField]
        private GameObject pausePanel;
        [SerializeField]
        private GameObject gameOverPanel;
        [SerializeField]
        private GameObject gameWonPanel;
        [SerializeField]
        private GameObject mainMenuPanel;

        private GameObject lastPanel;

        public void OpenSettings()
        {
            if (pausePanel != null && pausePanel.activeSelf)
                lastPanel = pausePanel;

            else if (gameOverPanel != null && gameOverPanel.activeSelf)
                lastPanel = gameOverPanel;

            else if (gameWonPanel != null && gameWonPanel.activeSelf)
                lastPanel = gameWonPanel;

            else if (mainMenuPanel != null && mainMenuPanel.activeSelf)
                lastPanel = mainMenuPanel;

            if (lastPanel != null)
                lastPanel.SetActive(false);

            settingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            PlayerPrefs.Save();
            Debug.Log("Settings Saved to Disk");

            if (settingsPanel != null)
                settingsPanel.SetActive(false);

            if (lastPanel != null)
                lastPanel.SetActive(true);
        }
    }
}