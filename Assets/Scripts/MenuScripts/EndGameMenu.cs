using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Audio;
using Logic.Castle;
using Misc;

namespace MenuScripts
{
    [RequireComponent(typeof(AudioSource))]
    public class EndGameMenu : MonoBehaviour
    {
        public GameObject gameOverPanel;
        public GameObject gameWonPanel;

        [Header("Settings")]
        [SerializeField]
        private float panelDelay = 1.5f;

        [Header("Audio")]
        [SerializeField]
        private AudioClip gameOverSound;
        [SerializeField]
        private AudioClip gameWonSound;

        [Header("References")]
        [SerializeField]
        private Core.GameInitializer gameInitializer;

        private CastleModel model;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.ignoreListenerPause = true;
        }

        public void Initialize(CastleModel castleModel)
        {
            model = castleModel;
            model.OnChanged += CheckGameOver;
        }

        private void CheckGameOver()
        {
            if (model.IsDead)
                OpenGameOver();
        }

        public void OpenGameOver() => StartCoroutine(EndGameSequence(gameOverPanel, gameOverSound));

        public void OpenWinMenu() => StartCoroutine(EndGameSequence(gameWonPanel, gameWonSound));

        private IEnumerator EndGameSequence(GameObject panel, AudioClip clip)
        {
            if (clip && audioSource)
            {
                AudioManager.Instance.StopMusic();
                audioSource.PlayOneShot(clip);
            }

            yield return new WaitForSecondsRealtime(panelDelay);

            OpenMenu(panel);
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }

        private static void OpenMenu(GameObject menu)
        {
            if (menu == null) return;

            UIBlocker.BlockAll();
            menu.SetActive(true);

            Time.timeScale = 0f;
        }

        private void OnDestroy()
        {
            if (model != null)
                model.OnChanged -= CheckGameOver;
        }
    }
}