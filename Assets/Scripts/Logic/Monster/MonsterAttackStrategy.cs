using System.Linq;
using Interfaces;
using Logic.Castle;
using UnityEngine;
using Logic.Unit;
using View;

namespace Logic.Monster
{
    public class MonsterAttackStrategy : IAttackStrategy
    {
        private readonly MonsterModel monster;
        private readonly UnitSystem unitSystem;
        // private readonly CastleView castleView;

        private float currentCooldown;
        private IDamageable currentTarget;

        public bool IsAttacking => currentTarget != null;

        // public MonsterAttackStrategy(MonsterModel monster, UnitSystem unitSystem)
        // {
        //     this.monster = monster;
        //     this.unitSystem = unitSystem;
        // }

        public MonsterAttackStrategy(MonsterModel monsterModel, UnitSystem unitSystem)
        {
            monster = monsterModel;
            this.unitSystem = unitSystem;
        }

        public void Tick()
        {
            var castle = CastleSystem.Instance;
            if (castle == null) return;
            
            if (currentTarget != null)
            {
                Vector3 targetPos;
                if (currentTarget is UnitModel unit) 
                    targetPos = unit.WorldPosition;
                else
                {
                    if (castle.Model.WallWorldPositions.Count == 0)
                    {
                        currentTarget = null; 
                        return;
                    }
                    targetPos =  castle.Model.WallWorldPositions.OrderBy(p => Vector3.Distance(monster.WorldPosition, p)).First();
                }
                
                if (currentTarget.IsDead || Vector3.Distance(targetPos, monster.WorldPosition) > monster.Data.attackRadius)
                    currentTarget = null;
            }

            if (currentTarget == null)
            {
                var nearbyUnit = unitSystem.GetAllUnits()
                    .Where(u => !u.IsDead)
                    .FirstOrDefault(u => Vector3.Distance(u.WorldPosition, monster.WorldPosition) <= monster.Data.attackRadius);

                if (nearbyUnit != null)
                    currentTarget = nearbyUnit;
                // 2. Ищем, не подошли ли мы к ЛЮБОМУ из гексов замка
                else if (!castle.Model.IsDead && castle.Model.WallWorldPositions.Count > 0)
                {
                    foreach (var wallPos in castle.Model.WallWorldPositions)
                    {
                        if (Vector3.Distance(wallPos, monster.WorldPosition) <= monster.Data.attackRadius)
                        {
                            currentTarget = castle.Model;
                            break;
                        }
                    }
                }

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