using UnityEngine;

namespace Logic.Monster
{
    [CreateAssetMenu(menuName = "Monsters/Monster Data")]
    public class MonsterData : ScriptableObject
    {
        [Header("Stats")]
        public int maxHealth = 100;
        public int damage = 10;
        public float moveSpeed = 3f;
        
        [Header("Attack")]
        public float attackRadius = 1f;
        public float attackCooldown = 1f;

        [Header("View")]
        public GameObject prefab;
    }
}