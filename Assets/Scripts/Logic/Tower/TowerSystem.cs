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
            // Debug.Log(monsters.Count);
            
            foreach (var tower in towersModel.Towers)
            {
                if (Time.time < tower.NextFireTime)
                    continue;

                var target = FindTarget(tower, monsters);
                if (target == null)
                    continue;

                Vector3 interceptPoint;

                if (!tower.Data.projectileData.isHoming)
                {
                    interceptPoint = CalculateInterceptPoint(
                        tower.WorldPosition,
                        target.WorldPosition,
                        target.CurrentVelocity,
                        tower.Data.projectileData.speed
                    );
                }
                else
                    interceptPoint = target.WorldPosition;

                var projectile = new ProjectileModel(
                    tower.WorldPosition,
                    target,
                    tower.Data.projectileData,
                    interceptPoint
                );

                projectileSystem.CreateProjectile(projectile);

                tower.NextFireTime =
                    Time.time + 1f / tower.Data.fireRate;
            }
        }

        public bool TryPlaceTower(TowerData data, Vector3Int cellPos, Vector3 worldPos)
        {
            if (towersModel.Towers.Any(t => t.GridPosition == cellPos))
            {
                Debug.Log("Cell is occupied!");
                return false;
            }

            if (!castleSystem.TrySpendGold(data.baseCost))
                return false;

            var tower = new TowerModel(data, cellPos, worldPos);
            towersModel.AddTower(tower);
            return true;
        }

        private static Vector3 CalculateInterceptPoint(
            Vector3 shooterPos,
            Vector3 targetPos,
            Vector3 targetVelocity,
            float projectileSpeed)
        {
            var toTarget = targetPos - shooterPos;

            var a = Vector3.Dot(targetVelocity, targetVelocity) - projectileSpeed * projectileSpeed;
            var b = 2f * Vector3.Dot(targetVelocity, toTarget);
            var c = Vector3.Dot(toTarget, toTarget);

            var discriminant = b * b - 4f * a * c;

            if (discriminant < 0f || Mathf.Abs(a) < 0.001f)
                return targetPos;

            var sqrt = Mathf.Sqrt(discriminant);

            var t1 = (-b + sqrt) / (2f * a);
            var t2 = (-b - sqrt) / (2f * a);

            var t = Mathf.Min(t1, t2);

            if (t < 0f)
                t = Mathf.Max(t1, t2);

            if (t < 0f)
                return targetPos;

            return targetPos + targetVelocity * t;
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