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
        private float timeSinceLastHint;
        private bool hintCycleStarted;
        private const float HintStartDelay = 5f;
        private const float HintCycleInterval = 20f;

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
            timeSinceLastHint = 0f;
            hintCycleStarted = false;
            
            TickManager.Instance.OnTick += Tick;
            
            Debug.Log("GameFlowManager: Waiting for player action...");
        }

        private void Tick()
        {
            if (gameStarted)
                return;

            var deltaTime = TickManager.Instance.tickInterval;
            timeSinceStart += deltaTime;
            timeSinceLastHint += deltaTime;
            
            if (!hintCycleStarted && timeSinceStart >= HintStartDelay)
            {
                hintCycleStarted = true;
                if (hintUI != null)
                    hintUI.ShowHint("Защититесь от монстров до начала первой волны");
                timeSinceLastHint = -hintUI.displayDuration;
                Debug.Log("Hint cycle started");
            }
            
            if (hintCycleStarted && timeSinceLastHint >= HintCycleInterval)
            {
                if (hintUI != null)
                    hintUI.ShowHint("Пока вы не поставите башню, ловушку или здание игра не начнется");
                timeSinceLastHint = -hintUI.displayDuration;
                Debug.Log($"Hint repeated at {timeSinceStart}s, TimeSinceLastHint: {timeSinceLastHint}s");
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