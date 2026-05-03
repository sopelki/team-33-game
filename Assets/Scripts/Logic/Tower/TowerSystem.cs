using System.Collections.Generic;
using System.Linq;
using Logic.Castle;
using Logic.Monster;
using Logic.Projectile;
using UnityEngine;

namespace Logic.Tower
{
    public class TowerSystem : Interfaces.ITickable
    {
        private readonly CastleSystem castleSystem;
        private readonly TowersModel towersModel;
        private readonly ProjectileSystem projectileSystem;
        private readonly MonsterSystem monsterSystem;

        public TowerSystem(
            CastleSystem castleSystem,
            TowersModel towersModel,
            MonsterSystem monsterSystem,
            ProjectileSystem projectileSystem)
        {
            this.castleSystem = castleSystem;
            this.towersModel = towersModel;
            this.monsterSystem = monsterSystem;
            this.projectileSystem = projectileSystem;
        }

        public void Tick()
        {
            var monsters = monsterSystem.GetAllMonsters();
            Debug.Log(monsters.Count);
            foreach (var tower in towersModel.Towers)
            {
                if (Time.time < tower.NextFireTime)
                    continue;

                var target = FindTarget(tower, monsters);
                if (target == null)
                    continue;

                var projectile = new ProjectileModel(
                    tower.WorldPosition,
                    target,
                    tower.Data.projectileData
                );

                projectileSystem.CreateProjectile(projectile);
                Debug.Log($"Projectile Created: {projectile}");
                tower.NextFireTime =
                    Time.time + 1f / tower.Data.fireRate;
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

        private static MonsterModel FindTarget(
            TowerModel tower,
            IReadOnlyList<MonsterModel> monsters)
        {
            MonsterModel closest = null;
            var minDist = float.MaxValue;

            foreach (var monster in monsters)
            {
                if (monster.IsDead)
                    continue;

                var dist = Vector3.Distance(
                    tower.WorldPosition,
                    monster.WorldPosition);

                // Debug.Log($"TowerPos: {tower.WorldPosition}, MonsterPos: {monster.WorldPosition}, Dist: {dist}");

                if (dist > tower.Data.range)
                    continue;

                if (dist < minDist)
                {
                    minDist = dist;
                    closest = monster;
                }
            }

            // Debug.Log($"Target found: {closest}, dist: {minDist}, range: {tower.Data.range}");
            return closest;
        }
    }
}