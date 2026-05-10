using UnityEngine;
using UnityEngine.SceneManagement;
using Logic.Castle;

namespace MenuScripts
{
    public class GameOverMenu : MonoBehaviour
    {
        public GameObject gameOverPanel;
        
        [SerializeField] private Core.GameInitializer gameInitializer;  // ← Добавь ссылку
        
        private CastleModel model;
        
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

        public void OpenGameOver()
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void RestartGame()
        {
            // if (gameInitializer != null)
            //     gameInitializer.Cleanup();
            
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadMainMenu()
        {
            // if (gameInitializer != null)
            //     gameInitializer.Cleanup();
            
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }

        private void OnDestroy()
        {
            if (model != null)
                model.OnChanged -= CheckGameOver;
        }
    }
}