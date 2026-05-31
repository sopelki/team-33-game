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
        [SerializeField]
        private EndGameMenu endGameMenu;

        [Header("Настройки скорости")]
        [SerializeField]
        private float speedUpMultiplier = 3.0f;
        [SerializeField]
        private FadePanel speedUpIcon;

        private bool isSpedUp;

        private void Start()
        {
            if (speedUpIcon)
                speedUpIcon.Hide(0);
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                HandleEscape();

            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                HandleSpeedToggle();

            if (isSpedUp && !IsAnyMenuOpen() && Mathf.Approximately(Time.timeScale, 1f))
            {
                Time.timeScale = speedUpMultiplier;

                if (speedUpIcon && speedUpIcon.CurrentAlpha < 0.1f)
                    speedUpIcon.Show();
            }
        }

        private void HandleEscape()
        {
            if (helpMenu && helpMenu.IsOpen)
            {
                helpMenu.CloseHelp();
                return;
            }
            
            if (settingsMenu && settingsMenu.IsOpen)
            {
                settingsMenu.CloseSettings();
                return;
            }
            
            if (pauseMenu && pauseMenu.IsOpen)
            {
                pauseMenu.ResumeGame();
                return;
            }
            
            if (endGameMenu && endGameMenu.IsAnyEndGameOpen)
                return;

            if (pauseMenu)
                pauseMenu.OpenPause();
        }

        private void HandleSpeedToggle()
        {
            if (IsAnyMenuOpen()) return;

            isSpedUp = !isSpedUp;
            Time.timeScale = isSpedUp ? speedUpMultiplier : 1f;

            if (speedUpIcon != null)
            {
                if (isSpedUp) speedUpIcon.Show();
                else speedUpIcon.Hide();
            }
        }

        private bool IsAnyMenuOpen()
        {
            var help = helpMenu && helpMenu.IsOpen;
            var settings = settingsMenu && settingsMenu.IsOpen;
            var pause = pauseMenu && pauseMenu.IsOpen;
            var end = endGameMenu && endGameMenu.IsAnyEndGameOpen;

            return help || settings || pause || end;
        }
    }
}