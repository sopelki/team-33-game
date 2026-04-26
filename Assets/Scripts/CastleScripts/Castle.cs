using System;
using System.Collections.Generic;
using CastleScrripts;
using UnityEngine;

namespace CastleScripts
{
    public class Castle : MonoBehaviour
    {
        public int HP = 100;
        public int WallLevel = 1;

        public int Gold;
        public int Food;

        private readonly List<BuildingInstance> buildings = new();
        public event Action OnResourcesChanged;

        public BuildingInstance TryAddBuilding(BuildingData data)
        {
            if (Gold < data.baseCost)
                return null;

            Gold -= data.baseCost;
            OnResourcesChanged?.Invoke();

            var instance = new BuildingInstance(data);
            buildings.Add(instance);

            return instance;
        }

        // public void RemoveBuilding(BuildingInstance building)
        // {
        //     buildings.Remove(building);
        // }

        public void TickProduction()
        {
            foreach (var building in buildings)
            {
                if (building.Data.type == BuildingType.Farm)
                {
                    Food += building.Production;
                    OnResourcesChanged?.Invoke();
                }
            }
        }
        
        public bool TryBuy(BuildingData data)
        {
            return TrySpendGold(data.baseCost);
        }

        private bool TrySpendGold(int amount)
        {
            if (Gold < amount)
                return false;

            Gold -= amount;
            OnResourcesChanged?.Invoke();
            return true;
        }
    }
}