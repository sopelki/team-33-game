using System;
using System.Collections.Generic;
using Logic.Monster;
using UnityEngine;

namespace Logic.Projectile
{
    public class ProjectileSystem
    {
        private readonly List<ProjectileModel> projectiles = new();
        private readonly MonsterSystem monsterSystem;

        public event Action<ProjectileModel> OnProjectileCreated;
        public event Action<ProjectileModel> OnProjectileDestroyed;

        public ProjectileSystem(MonsterSystem monsterSystem)
        {
            this.monsterSystem = monsterSystem;
        }

        public void CreateProjectile(ProjectileModel projectile)
        {
            projectiles.Add(projectile);
            OnProjectileCreated?.Invoke(projectile);
        }

        public void Tick()
        {
            var step = Core.TickManager.Instance.tickInterval;

            for (var i = projectiles.Count - 1; i >= 0; i--)
            {
                var p = projectiles[i];

                if (p.Target == null || p.Target.IsDead)
                {
                    Remove(i);
                    continue;
                }

                Vector3 dir;

                if (p.Data.isHoming)
                    dir = (p.Target.WorldPosition - p.Position).normalized;
                else
                    dir = p.Direction;

                var distanceBefore =
                    Vector3.Distance(p.Position, p.Target.WorldPosition);

                p.Position += dir * p.Data.speed * step;

                var distanceAfter =
                    Vector3.Distance(p.Position, p.Target.WorldPosition);

                if (distanceAfter < 0.3f)
                {
                    ApplyDamage(p);
                    Remove(i);
                    continue;
                }

                if (!p.Data.isHoming && distanceAfter > distanceBefore)
                {
                    Remove(i);
                    continue;
                }

                p.TraveledDistance += p.Data.speed * step;

                if (p.Data.maxTravelDistance > 0f &&
                    p.TraveledDistance >= p.Data.maxTravelDistance)
                    Remove(i);
            }
        }

        private void ApplyDamage(ProjectileModel p)
        {
            if (p.Data.aoeRadius <= 0f)
            {
                p.Target.TakeDamage(p.Data.damage);
                return;
            }

            foreach (var monster in monsterSystem.GetAllMonsters())
            {
                if (monster.IsDead)
                    continue;

                var dist = Vector3.Distance(
                    monster.WorldPosition,
                    p.Position);

                if (dist <= p.Data.aoeRadius)
                    monster.TakeDamage(p.Data.damage);
            }
        }

        private void Remove(int index)
        {
            var p = projectiles[index];
            projectiles.RemoveAt(index);
            OnProjectileDestroyed?.Invoke(p);
        }
    }
}