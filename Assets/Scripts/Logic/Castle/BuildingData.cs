using Interfaces;
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
        public string description;


        // TODO: допилить инфу
        public TooltipContent GetTooltipContent()
        {
            return new TooltipContent
            {
                Title = type.ToString(),
                Description = description,
                Cost = $"Цена: {baseCost}",
                SpecialInfo = $"Скорость производства ***: {baseProduction}"
            };
        }
    }
}