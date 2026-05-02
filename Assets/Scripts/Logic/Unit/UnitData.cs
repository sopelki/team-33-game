using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.Unit
{
    [CreateAssetMenu(menuName = "Units/Unit Data")]
    public class UnitData : ScriptableObject
    {
        [Header("Stats")]
        public int maxHealth = 100;
        public int attack = 10;
        public float moveSpeed = 2f;

        [Header("View")]
        public GameObject unitPrefab;
    }
}