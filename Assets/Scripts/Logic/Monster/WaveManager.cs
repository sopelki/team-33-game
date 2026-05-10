using Interfaces;
using UnityEngine;

namespace Logic.Monster
{
    public class WaveManager : ITickable
    {
        private readonly MonsterSpawner spawner;
        private readonly MonsterSystem monsterSystem;

        private bool waitingForNextWave;
        private bool isDelaying;

        private float delayTimer;
        private readonly float delayBetweenWaves;

        public WaveManager(MonsterSpawner spawner, MonsterSystem monsterSystem, float delayBetweenWaves)
        {
            this.spawner = spawner;
            this.monsterSystem = monsterSystem;
            this.delayBetweenWaves = delayBetweenWaves;
        }

        public void Tick()
        {
            if (waitingForNextWave && monsterSystem.GetAllMonsters().Count == 0)
            {
                waitingForNextWave = false;
                isDelaying = true;
                delayTimer = delayBetweenWaves;
            }

            if (isDelaying)
            {
                delayTimer -= Core.TickManager.Instance.tickInterval;

                if (delayTimer <= 0f)
                {
                    isDelaying = false;
                    spawner.StartNextWave();
                    Debug.Log("Starting new wave");
                }
            }
        }

        public void OnWaveFinishedSpawning()
        {
            waitingForNextWave = true;
            Debug.Log("Wave finished");
        }
    }
}