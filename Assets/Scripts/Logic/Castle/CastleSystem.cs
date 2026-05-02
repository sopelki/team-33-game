using System.Linq;
using HexagonScripts;
using Interfaces;
using Logic.Unit;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Logic.Castle
{
    public class CastleSystem : ITickable
    {
        private readonly CastleModel model;
        private readonly UnitSystem unitSystem;
        private readonly UnitData unitData;
        private readonly Field field;
        private readonly Vector2Int spawnHex;
        private readonly Tilemap tilemap;


        public CastleSystem(
            CastleModel model, 
            UnitSystem unitSystem,
            UnitData unitData,
            Field field,
            Tilemap tilemap)
        {
            this.model = model;
            this.unitSystem = unitSystem;
            this.unitData = unitData;
            this.field = field;
            this.tilemap = tilemap;
            
            spawnHex = GetTopLeftHex();
        }

        public void Tick()
        {
            ProduceResources();
            ConsumeFood();
            SpawnUnitsFromBarracks();
        }
        
        private void ConsumeFood()
        {
            int totalConsumption = model.CurrentUnits;

            if (model.Food >= totalConsumption)
                model.Food -= totalConsumption;
            else
                model.Food = 0;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void SpawnUnitsFromBarracks()
        {
            var barracksCount = model.Buildings.Count(b => b.Data.type == BuildingType.Barracks);

            if (barracksCount == 0)
                return;
            
            for (var i = 0; i < barracksCount; i++)
            {
                // Проверяем хватает ли еды на нового юнита
                if (model.Food <= 0)
                    return;

                SpawnUnit();
            }
        }
        
        private void SpawnUnit()
        {
            var hex = field.GetHex(spawnHex);

            if (hex == null)
                return;
            
            Debug.Log(field.GetHex(spawnHex).type);
            var worldPos = tilemap.GetCellCenterWorld(hex.offset);
            unitSystem.SpawnUnit(worldPos, spawnHex, unitData);
            model.CurrentUnits++;
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
        
        private Vector2Int GetTopLeftHex()
        {
            var topLeft = field.Hexagons.Values
                .OrderBy(h => h.offset.x)     // сначала самый левый
                .ThenByDescending(h => h.offset.y) // потом самый верхний
                .First();

            return topLeft.coordinates;
        }

        public bool TryBuyBuilding(BuildingData data)
        {
            if (!TrySpendGold(data.baseCost))
                return false;

            var instance = new BuildingModel(data);
            model.Buildings.Add(instance);

            return true;
        }

        public bool TrySpendGold(int price)
        {
            if (model.Gold < price)
                return false;

            model.Gold -= price;
            model.Changed();
            return true;
        }
    }
}