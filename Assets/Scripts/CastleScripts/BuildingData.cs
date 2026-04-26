using CastleScrripts;
using UnityEngine;

namespace CastleScripts
{
    [CreateAssetMenu(menuName = "Buildings/Building Data")]
    public class BuildingData : ScriptableObject
    {
        public BuildingType type;
        public int baseProduction;
        public int baseCost;
    }
}