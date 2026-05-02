using System.Linq;
using Logic.Castle;
using UnityEngine;

namespace Logic.Tower
{
    public class TowerSystem : Interfaces.ITickable
    {
        private readonly CastleSystem castleSystem;
        private readonly TowersModel towersModel;

        public TowerSystem(CastleSystem castleSystem, TowersModel towersModel)
        {
            this.castleSystem = castleSystem;
            this.towersModel = towersModel;
        }

        public void Tick()
        {
            foreach (var tower in towersModel.Towers)
            {
                // Логика стрельбы (обновление NextFireTime в модели)
            }
        }

        public bool TryPlaceTower(TowerData data, Vector3Int cellPos, Vector3 worldPos)
        {
            if (towersModel.Towers.Any(t => t.GridPosition == cellPos)) 
            {
                Debug.Log("Cell occupied!");
                return false;
            }
            
            if (!castleSystem.TrySpendGold(data.baseCost))
                return false;

            var tower = new TowerModel(data, cellPos, worldPos);
            towersModel.AddTower(tower);
            return true;
        }
    }
}