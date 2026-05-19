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

        [Header("Unit Attack Sounds")]
        public AudioClip[] unitMeleeAttackSounds;
        public AudioClip[] unitRangeAttackSounds;
        public AudioClip[] unitDamageSounds;

        [Header("Background Music")]
        public AudioClip backgroundMusic;

        public AudioClip GetRandomClip(AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0)
                return null;

            return clips[Random.Range(0, clips.Length)];
        }
    }
}