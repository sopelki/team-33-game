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
        public AudioClip monsterAttackSound;
        public AudioClip monsterDamageSound;

        [Header("Unit Attack Sounds")]
        public AudioClip unitMeleeAttackSound;
        public AudioClip unitRangeAttackSound;
        public AudioClip unitDamageSound;

        [Header("Background Music")]
        public AudioClip backgroundMusic;
    }
}