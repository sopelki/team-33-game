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

                if (p.Data.isHoming)
                    UpdateHoming(p, step, i);
                else
                    UpdateStraight(p, step, i);
            }
        }

        private void UpdateStraight(ProjectileModel p, float step, int index)
        {
            var toTargetBefore = p.TargetPoint - p.Position;

            p.Position += p.Direction * p.Data.speed * step;

            var toTargetAfter = p.TargetPoint - p.Position;

            if (toTargetAfter.magnitude <= 0.3f)
            {
                TryApplyDamage(p);
                Remove(index);
                return;
            }

            if (Vector3.Dot(toTargetBefore, toTargetAfter) < 0f)
            {
                Remove(index);
                return;
            }

            if (p.Data.maxTravelDistance > 0f &&
                Vector3.Distance(p.StartPosition, p.Position) >= p.Data.maxTravelDistance)
                Remove(index);
        }

        private void UpdateHoming(ProjectileModel p, float step, int index)
        {
            if (p.Target == null || p.Target.IsDead)
            {
                p.Data.isHoming = false;
                return;
            }

            var dir = (p.Target.WorldPosition - p.Position).normalized;

            p.Position += dir * p.Data.speed * step;

            var dist =
                Vector3.Distance(p.Position, p.Target.WorldPosition);

            if (dist <= 0.3f)
            {
                TryApplyDamage(p);
                Remove(index);
                return;
            }

            if (p.Data.maxTravelDistance > 0f &&
                Vector3.Distance(p.StartPosition, p.Position) >= p.Data.maxTravelDistance)
                Remove(index);
        }

        private void TryApplyDamage(ProjectileModel p)
        {
            if (p.Target == null)
                return;

            if (p.Data.aoeRadius <= 0f)
            {
                if (!p.Target.IsDead)
                    p.Target.TakeDamage(p.Data.damage);
                return;
            }

            foreach (var monster in monsterSystem.GetAllMonsters())
            {
                if (monster.IsDead)
                    continue;

                var dist =
                    Vector3.Distance(monster.WorldPosition, p.Position);

                if (dist <= p.Data.aoeRadius)
                {
                    Debug.Log("ApplyDamage: " + p.Data.damage);
                    monster.TakeDamage(p.Data.damage);
                }
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