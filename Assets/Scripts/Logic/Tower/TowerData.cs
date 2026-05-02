using UnityEngine;

namespace Logic.Tower
{
    [CreateAssetMenu(menuName = "Towers/Tower Data")]
    public class TowerData : ScriptableObject
    {
        public TowerType type;
        public int baseCost;
        public float range;
        public float fireRate;
        public int damage;
        public GameObject viewPrefab;
    }
}