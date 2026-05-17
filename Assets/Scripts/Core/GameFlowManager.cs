using UnityEngine;
using Logic.Tower;
using Logic.Trap;
using Logic.Castle;
using Logic.Monster;
using UI;

namespace Core
{
    public class GameFlowManager
    {
        private readonly WaveManager waveManager;
        private readonly TowerSystem towerSystem;
        private readonly TrapSystem trapSystem;
        private readonly CastleSystem castleSystem;
        private readonly HintUI hintUI;
        
        private bool gameStarted;
        private float timeSinceStart;
        private bool hasShownHint;
        private const float HintDelay = 10f;

        public GameFlowManager(
            WaveManager waveManager,
            TowerSystem towerSystem,
            TrapSystem trapSystem,
            CastleSystem castleSystem,
            HintUI hintUI)
        {
            this.waveManager = waveManager;
            this.towerSystem = towerSystem;
            this.trapSystem = trapSystem;
            this.castleSystem = castleSystem;
            this.hintUI = hintUI;
        }

        public void Initialize()
        {
            towerSystem.OnFirstTowerPlaced += OnFirstObjectPlaced;
            trapSystem.OnFirstTrapPlaced += OnFirstObjectPlaced;
            castleSystem.OnFirstBuildingPlaced += OnFirstObjectPlaced;
            
            timeSinceStart = 0f;
            hasShownHint = false;
            
            TickManager.Instance.OnTick += Tick;
            
            Debug.Log("GameFlowManager: Waiting for player action...");
        }

        private void Tick()
        {
            if (gameStarted)
                return;

            timeSinceStart += TickManager.Instance.tickInterval;
            
            if (!hasShownHint && timeSinceStart >= HintDelay)
            {
                hasShownHint = true;
                
                if (hintUI != null)
                    hintUI.ShowHint("Защитите королевство, пока на вас не напали монстры");
                
                Debug.Log($"Hint shown at {timeSinceStart}s");
            }
        }

        private void OnFirstObjectPlaced()
        {
            if (gameStarted)
                return;

            gameStarted = true;

            TickManager.Instance.OnTick -= Tick;

            towerSystem.OnFirstTowerPlaced -= OnFirstObjectPlaced;
            trapSystem.OnFirstTrapPlaced -= OnFirstObjectPlaced;
            castleSystem.OnFirstBuildingPlaced -= OnFirstObjectPlaced;

            if (hintUI != null)
                hintUI.HideHint();

            waveManager.StartGame();
            
            Debug.Log("GameFlowManager: Game started.");
        }
    }
}