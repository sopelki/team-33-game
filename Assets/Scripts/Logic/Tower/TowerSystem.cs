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
            var step = Core.TickManager.Instance.tickInterval;

            foreach (var tower in towersModel.Towers)
            {
                if (tower.CooldownTimer > 0)
                    tower.CooldownTimer -= step;

                if (tower.CooldownTimer > 0)
                    continue;

                var targets = FindTargets(tower, monsters);
                if (targets.Count == 0)
                {
                    tower.ShotsLeft = 0;
                    continue;
                }

                var target = targets[0];
                Shoot(tower, target);

                if (tower.ShotsLeft > 0)
                {
                    tower.ShotsLeft--;
                    tower.CooldownTimer = 0.2f; 
                }
                else
                {
                    tower.ShotsLeft = tower.Data.targetsCount - 1;
                    tower.CooldownTimer = 1f / tower.Data.fireRate;
                }
            }
        }

        private void Shoot(TowerModel tower, MonsterModel target)
        {
            var interceptPoint = CalculateInterceptPoint(
                tower.WorldPosition,
                target.WorldPosition,
                target.CurrentVelocity,
                tower.Data.projectileData.speed
            );

            var projectile = new ProjectileModel(
                tower.WorldPosition,
                target,
                tower.Data.projectileData,
                interceptPoint
            );

            projectileSystem.CreateProjectile(projectile);
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

        private static Vector3 CalculateInterceptPoint(Vector3 shooterPos, Vector3 targetPos, Vector3 targetVelocity,
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

        private static List<MonsterModel> FindTargets(TowerModel tower, IReadOnlyList<MonsterModel> monsters)
        {
            return monsters
                .Where(m => !m.IsDead)
                .Select(m => new { Monster = m, Distance = Vector3.Distance(tower.WorldPosition, m.WorldPosition) })
                .Where(x => x.Distance <= tower.Data.range)
                .OrderBy(x => x.Distance)
                .Take(tower.Data.targetsCount)
                .Select(x => x.Monster)
                .ToList();
        }
    }
}