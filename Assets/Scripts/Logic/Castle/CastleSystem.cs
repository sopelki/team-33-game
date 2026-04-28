using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Castle
{
    public class CastleSystem
    {
        private readonly CastleModel model;

        public CastleSystem(CastleModel model)
        {
            this.model = model;
        }

        public void Tick()
        {
            ProduceResources();
        }

        private void ProduceResources()
        {
            var changed = false;
            foreach (var building in model.Buildings)
            {
                if (building.Data.type == BuildingType.Farm)
                {
                    model.Food += building.Production;
                    changed = true;
                }
                // TODO: Добавить логику для других BuildingType
            }

            if (changed)
                model.Changed();
        }

        public bool TryBuyBuilding(BuildingData data)
        {
            if (!TrySpendGold(data))
                return false;

            var instance = new BuildingInstance(data);
            model.Buildings.Add(instance);

            return true;
        }

        public bool TryBuyTower(BuildingData data) => TrySpendGold(data);

        private bool TrySpendGold(BuildingData data)
        {
            if (model.Gold < data.baseCost)
                return false;

            model.Gold -= data.baseCost;
            model.Changed();
            return true;
        }
    }
}