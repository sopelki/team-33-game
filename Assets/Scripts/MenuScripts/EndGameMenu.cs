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
        private float panelDelay = 0.5f;
        [SerializeField]
        private float fadeDuration = 1.5f;

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

            if (gameOverPanel)
                gameOverPanel.SetActive(false);

            if (gameWonPanel)
                gameWonPanel.SetActive(false);
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
            if (!panel)
                yield break;

            if (clip && audioSource)
            {
                AudioManager.Instance.StopMusic();
                audioSource.PlayOneShot(clip);
            }

            yield return new WaitForSecondsRealtime(panelDelay);

            var cg = panel.GetComponent<CanvasGroup>();

            if (!cg)
                cg = panel.AddComponent<CanvasGroup>();

            cg.alpha = 0;
            panel.SetActive(true);
            UIBlocker.BlockAll();

            Time.timeScale = 0f;

            var elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                cg.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
                yield return null;
            }

            cg.alpha = 1f;
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
    }
}