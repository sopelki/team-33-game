using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "Audio/Sound Data")]
    public class SoundData : ScriptableObject
    {
        [Header("Building Sounds")]
        public AudioClip towerPlaceSound;
        public AudioClip trapPlaceSound;
        public AudioClip buildingPlaceSound;

        [Header("Monster Attack Sounds")]
        public AudioClip[] monsterAttackSounds;
        public AudioClip[] monsterDamageSounds;

        [Header("Tower Sounds")]
        public AudioClip[] archerTowerShootSounds;
        public AudioClip[] archerTowerHitSounds;
        public AudioClip[] mageTowerShootSounds;
        public AudioClip[] mageTowerHitSounds;

        [Header("Unit Attack Sounds")]
        public AudioClip[] unitMeleeAttackSounds;
        public AudioClip[] unitRangeAttackSounds;
        public AudioClip[] unitDamageSounds;

        [Header("Background Music")]
        public AudioClip backgroundMusic;
    }
}