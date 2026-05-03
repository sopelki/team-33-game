using System.Linq;
using Interfaces;
using UnityEngine;
using Logic.Monster;

namespace Logic.Unit
{
    public class UnitAttackStrategy : IAttackStrategy
    {
        private readonly UnitModel unit;
        private readonly MonsterSystem monsterSystem;

        private float currentCooldown;
        private bool isAttacking;

        public bool IsAttacking => isAttacking;

        public UnitAttackStrategy(
            UnitModel unit,
            MonsterSystem monsterSystem)
        {
            this.unit = unit;
            this.monsterSystem = monsterSystem;
        }

        public void Tick()
        {
            isAttacking = false;

            if (currentCooldown > 0f)
            {
                currentCooldown -= Core.TickManager.Instance.tickInterval;
                return;
            }

            var targets = monsterSystem.GetAllMonsters()
                .Where(m => !m.IsDead)
                .Where(m =>
                    Vector3.Distance(
                        m.WorldPosition,
                        unit.WorldPosition) <= unit.UnitData.attackRadius)
                .ToList();

            if (targets.Count == 0)
                return;

            foreach (var monster in targets)
            {
                monster.TakeDamage(unit.GetAttack());
            }

            currentCooldown = unit.UnitData.attackCooldown;
            isAttacking = true;
        }
    }
}