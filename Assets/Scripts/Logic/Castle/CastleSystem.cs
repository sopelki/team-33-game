using System.Linq;
using Interfaces;
using Logic.Unit;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Logic.Castle
{
    public class CastleSystem : ITickable
    {
        private static readonly Vector2Int spawnHex = new(-30, 20); // Поменять, если нужна другая точка спавна

        private readonly CastleModel model;
        private readonly UnitSystem unitSystem;
        private readonly UnitData unitData;
        private readonly Field.Field field;
        private readonly Tilemap tilemap;
        private float resourceTimer;
        private float spawnTimer;
        private const float ResourceInterval = 1f;
        private const float SpawnInterval = 1f;


        public CastleSystem(
            CastleModel model,
            UnitSystem unitSystem,
            UnitData unitData,
            Field.Field field,
            Tilemap tilemap)
        {
            this.model = model;
            this.unitSystem = unitSystem;
            this.unitData = unitData;
            this.field = field;
            this.tilemap = tilemap;
        }

        public void Tick()
        {
            var dt = Core.TickManager.Instance.tickInterval;

            resourceTimer += dt;
            spawnTimer += dt;

            if (resourceTimer >= ResourceInterval)
            {
                resourceTimer = 0f;
                ProduceResources();
            }

            if (spawnTimer >= SpawnInterval)
            {
                spawnTimer = 0f;
                SpawnUnitsFromBarracks();
            }
        }

        private void SpawnUnitsFromBarracks()
        {
            var barracksCount = model.Buildings.Count(b => b.Data.type == BuildingType.Barracks);

            if (barracksCount == 0)
                return;

            for (var i = 0; i < barracksCount; i++)
            {
                if (model.Food < unitData.foodCost)
                    return;

                SpawnUnit();
            }
        }

        // TODO: Поменять логику использования еды юнитами (например, чтобы они тратили еду, находясь на поле)
        private void SpawnUnit()
        {
            var hex = field.GetHex(spawnHex);

            if (hex == null)
                return;

            var worldPos = tilemap.GetCellCenterWorld(hex.offset);
            unitSystem.SpawnUnit(worldPos, spawnHex, unitData);
            model.Food -= unitData.foodCost;
            model.CurrentUnits++;
            model.Changed();
        }


        private void ProduceResources()
        {
            var changed = false;
            foreach (var building in model.Buildings.Where(building => building.Data.type == BuildingType.Farm))
            {
                model.Food += building.Production;
                changed = true;
            }

            if (changed)
                model.Changed();
        }


        public bool TryBuyBuilding(BuildingData data)
        {
            if (!TrySpendGold(data.baseCost))
                return false;

            var instance = new BuildingModel(data);
            model.Buildings.Add(instance);
            
            if (data.type == BuildingType.Blacksmith)
            {
                unitSystem.AddBuff(new AttackPercentBuff());
                Debug.Log("Кузница построена: будущие юниты получат +25% к атаке");
            }
            else if (data.type == BuildingType.Hospital)
            {
                unitSystem.AddBuff(new HealthPercentBuff());
                Debug.Log("Больница построена: будущие юниты получат +25% к здоровью");
            }

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