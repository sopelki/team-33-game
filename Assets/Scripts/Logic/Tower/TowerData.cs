using Interfaces;
using Logic.Projectile;
using UnityEngine;

namespace Logic.Tower
{
    [CreateAssetMenu(menuName = "Towers/Tower Data")]
    public class TowerData : ScriptableObject, ITooltipProvider
    {
        public TowerType type;
        public int baseCost;
        public float range;
        public float fireRate;
        public ProjectileData projectileData;
        public GameObject viewPrefab;
        public int targetsCount = 1;
        public string description;

        // TODO: допилить инфу
        public TooltipContent GetTooltipContent()
        {
            return new TooltipContent
            {
                Title = type.ToString(),
                Description = description,
                Cost = $"Цена: {baseCost}",
                SpecialInfo = $"Урон: {projectileData.damage}"
            };
        }
    }
}