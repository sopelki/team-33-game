using System;
using UnityEngine;

namespace Core
{
    public class TickManager : MonoBehaviour
    {
        public static TickManager Instance;

        public float tickInterval = 1f;
        private float timer;

        public event Action OnTick;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= tickInterval)
            {
                timer -= tickInterval;
                OnTick?.Invoke();
            }
        }
    }
}