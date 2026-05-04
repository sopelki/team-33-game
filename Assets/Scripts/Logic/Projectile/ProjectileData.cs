using UnityEngine;

namespace Logic.Projectile
{
    [CreateAssetMenu(menuName = "Projectiles/Projectile Data")]
    public class ProjectileData : ScriptableObject
    {
        [Header("Visual")]
        public GameObject prefab;
        public float xOffset;
        public float yOffset;

        [Header("Stats")]
        public float speed = 5f;
        public int damage = 1;

        [Header("Optional")]
        public float aoeRadius;
        public bool isHoming;
        public float maxTravelDistance = 20f;
    }
}