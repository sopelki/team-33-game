using UnityEngine;

namespace Logic.Unit
{
    [CreateAssetMenu(menuName = "Units/Unit Data")]
    public class UnitData : ScriptableObject
    {
        [Header("Stats")]
        public int maxHealth = 100;
        public int attack = 10;
        public float moveSpeed = 20f;
        public int foodCost = 5;

        [Header("View")]
        public GameObject unitPrefab;
    }
}