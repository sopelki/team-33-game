using UnityEngine;

namespace Logic.Castle
{
    [CreateAssetMenu(menuName = "Buildings/Building Data")]
    public class BuildingData : ScriptableObject
    {
        public BuildingType type;
        public int baseProduction;
        public int baseCost;
        public GameObject viewPrefab;
    }
}