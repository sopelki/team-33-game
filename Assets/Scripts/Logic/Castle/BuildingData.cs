using Interfaces;
using Misc;
using UnityEngine;

namespace Logic.Castle
{
    [CreateAssetMenu(menuName = "Buildings/Building Data")]
    public class BuildingData : ScriptableObject, ITooltipProvider
    {
        public BuildingType type;
        public int baseProduction;
        public int baseCost;
        public GameObject viewPrefab;
        [TextArea]
        public string description;

        [Header("Localisation")]
        [SerializeField]
        private string effectLabel = "Производство ресурсов";

        public TooltipContent GetTooltipContent(bool isBought = false)
        {
            var priceInfo = isBought
                ? string.Empty
                : $"Цена: <color=#FFEE58>{baseCost} золота</color>";

            return new TooltipContent
            {
                Title = $"<color=#FFD700><b>{type.GetRussianName()}</b></color>",
                Description = $"<color=#BDBDBD>{description}</color>",
                Cost = priceInfo,
                SpecialInfo = $"{effectLabel}: <color=#66BB6A>+{baseProduction}</color>"
            };
        }
    }
}