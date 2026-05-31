using UnityEngine;
using UnityEngine.InputSystem;

namespace MenuScripts
{
    public class GameInputController : MonoBehaviour
    {
        [Header("Ссылки на меню")]
        [SerializeField]
        private PauseMenu pauseMenu;
        [SerializeField]
        private SettingsMenu settingsMenu;
        [SerializeField]
        private HelpMenu helpMenu;

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                HandleEscape();
        }

        private void HandleEscape()
        {
            if (helpMenu != null && helpMenu.IsOpen)
            {
                helpMenu.CloseHelp();
                return;
            }

            if (settingsMenu != null && settingsMenu.IsOpen)
            {
                settingsMenu.CloseSettings();
                return;
            }

            if (pauseMenu != null)
            {
                if (pauseMenu.IsOpen)
                    pauseMenu.ResumeGame();
                else
                    pauseMenu.OpenPause();
            }
        }
    }
}