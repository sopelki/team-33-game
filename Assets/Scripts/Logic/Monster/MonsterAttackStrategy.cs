using System.Linq;
using Interfaces;
using UnityEngine;
using Logic.Unit;

namespace Logic.Monster
{
    public class MonsterAttackStrategy : IAttackStrategy
    {
        private readonly MonsterModel monster;
        private readonly UnitSystem unitSystem;

        private float currentCooldown;
        private UnitModel currentTarget;

        public bool IsAttacking => currentTarget != null;

        public MonsterAttackStrategy(
            MonsterModel monster,
            UnitSystem unitSystem)
        {
            this.monster = monster;
            this.unitSystem = unitSystem;
        }

        public void Tick()
        {
            if (currentTarget != null)
            {
                if (currentTarget.IsDead ||
                    Vector3.Distance(
                        currentTarget.WorldPosition,
                        monster.WorldPosition) > monster.Data.attackRadius)
                    currentTarget = null;
            }

            if (currentTarget == null)
            {
                currentTarget = unitSystem.GetAllUnits()
                    .Where(u => !u.IsDead)
                    .FirstOrDefault(u =>
                        Vector3.Distance(
                            u.WorldPosition,
                            monster.WorldPosition) <= monster.Data.attackRadius
                    );

                if (currentTarget == null)
                    return;
            }

            if (currentCooldown > 0f)
            {
                currentCooldown -= Core.TickManager.Instance.tickInterval;
                return;
            }

            currentTarget.TakeDamage(monster.Data.damage);
            currentCooldown = monster.Data.attackCooldown;
        }
    }
}