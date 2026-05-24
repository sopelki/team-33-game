using UnityEngine;

namespace MenuScripts
{
    public class SettingsMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private FadePanel menuBackground;

        [Header("Panels")]
        [SerializeField]
        private FadePanel settingsPanel;
        [SerializeField]
        private FadePanel pausePanel;
        [SerializeField]
        private FadePanel gameOverPanel;
        [SerializeField]
        private FadePanel gameWonPanel;
        [SerializeField]
        private FadePanel mainMenuPanel;

        private FadePanel lastPanel;

        public void OpenSettings()
        {
            lastPanel = FindActivePanel();

            if (lastPanel != null)
                lastPanel.Hide(lastPanel.FadeDuration);

            if (settingsPanel != null)
                settingsPanel.Show();
        }

        public void CloseSettings()
        {
            PlayerPrefs.Save();

            if (settingsPanel != null)
                settingsPanel.Hide();

            if (lastPanel != null)
            {
                if (menuBackground != null)
                {
                    var bgCanvas = menuBackground.GetComponent<CanvasGroup>();

                    if (bgCanvas != null && bgCanvas.alpha <= 0)
                        menuBackground.Show(lastPanel.FadeDuration);
                }

                lastPanel.Show(lastPanel.FadeDuration);
            }
        }

        private FadePanel FindActivePanel()
        {
            if (pausePanel != null && pausePanel.GetComponent<CanvasGroup>().alpha > 0)
                return pausePanel;

            if (gameOverPanel != null && gameOverPanel.GetComponent<CanvasGroup>().alpha > 0)
                return gameOverPanel;

            if (gameWonPanel != null && gameWonPanel.GetComponent<CanvasGroup>().alpha > 0)
                return gameWonPanel;

            if (mainMenuPanel != null && mainMenuPanel.GetComponent<CanvasGroup>().alpha > 0)
                return mainMenuPanel;

            return null;
        }
    }
}