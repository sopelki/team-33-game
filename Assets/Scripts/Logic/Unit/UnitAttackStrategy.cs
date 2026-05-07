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

            if (currentCooldown > 0f)
            {
                currentCooldown -= Core.TickManager.Instance.tickInterval;
                isAttacking = true;
                return;
            }
            
            isAttacking = false;
            unit.AttackType = 0;

            var targets = monsterSystem.GetAllMonsters()
                .Where(m => !m.IsDead)
                .Where(m =>
                    Vector3.Distance(
                        m.WorldPosition,
                        unit.WorldPosition) <= unit.UnitData.attackRadius)
                .ToList();

            if (targets.Count == 0)
                return;
        
            var closest = targets
                .OrderBy(m => Vector3.Distance(m.WorldPosition, unit.WorldPosition))
                .First();
        
            var distance = Vector3.Distance(closest.WorldPosition, unit.WorldPosition);
            var meleeRange = unit.UnitData.attackRadius * 0.5f;
            unit.AttackType = distance <= meleeRange ? 1 : 2;

            foreach (var monster in targets)
                monster.TakeDamage(unit.GetAttack());

            currentCooldown = unit.UnitData.attackCooldown;
            isAttacking = true;
        }
    }
}