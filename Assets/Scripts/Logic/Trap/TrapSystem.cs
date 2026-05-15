using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logic.Monster;
using Core;
using Logic.Castle;
using Logic.Trap.Logic.Trap;
using HexagonScripts;

namespace Logic.Trap
{
    public class TrapSystem
    {
        private readonly MonsterSystem monsterSystem;
        private readonly TrapsModel trapsModel;
        private readonly Field.Field field;
        private readonly CastleSystem castleSystem;

        public TrapSystem(MonsterSystem monsterSystem, TrapsModel trapsModel, Field.Field field, CastleSystem castleSys)
        {
            this.field = field;
            this.monsterSystem = monsterSystem;
            this.trapsModel = trapsModel;
            this.castleSystem = castleSys;
            TickManager.Instance.OnTick += Tick;
        }

        public List<Vector2Int> GetTrapOccupiedHexes(Vector2Int centerHex)
        {
            return new List<Vector2Int>
            {
                centerHex,
                centerHex + new Vector2Int(1, 0),
                centerHex + new Vector2Int(0, 1) 
            };
        }

        public bool CanPlaceTrap(TrapData data, Vector2Int axial)
        {
            if (castleSystem.Model.Gold < data.baseCost) return false;
            var hexes = GetTrapOccupiedHexes(axial);
            
            foreach (var h in hexes)
            {
                var hexObj = field.GetHex(h);
                
                if (hexObj == null) 
                    return false;
                if (hexObj.type != HexagonType.Path)
                    return false;
                
                if (trapsModel.Traps.Any(t => t.Hexes.Contains(h))) 
                    return false;
            }
            return true;
        }

        public bool TryPlaceTrap(TrapData data, Vector2Int hex)
        {
            if (!CanPlaceTrap(data, hex)) return false;
            
            if (castleSystem.TrySpendGold(data.baseCost))
            {
                var trap = new TrapModel(data, GetTrapOccupiedHexes(hex));
                trapsModel.AddTrap(trap);
                return true;
            }
            return false;
        }

        public void OnMonsterEnteredCell(Vector2Int hex, MonsterModel monster)
        {
            var trap = trapsModel.Traps.FirstOrDefault(t => t.Hexes.Contains(hex));
            if (trap == null || trap.IsTriggered) return;

            if (trap.Data.trapType == TrapType.SlowZone)
            {
                if (!trap.ActiveSlowDebuffs.ContainsKey(monster))
                {
                    var slow = new SlowDebuff(trap.Data.slowPercent);
                    trap.ActiveSlowDebuffs.Add(monster, slow);
                    monster.DebuffSystem.AddBuff(slow);
                }
            }
        }

        public void OnMonsterExitedCell(Vector2Int hex, MonsterModel monster)
        {
            var trap = trapsModel.Traps.FirstOrDefault(t => t.Hexes.Contains(hex));
            if (trap == null) return;
            if (trap.Hexes.Contains(monster.CurrentHex)) return;

            if (trap.Data.trapType == TrapType.SlowZone && trap.ActiveSlowDebuffs.TryGetValue(monster, out var slow))
            {
                monster.DebuffSystem.RemoveBuff(slow);
                trap.ActiveSlowDebuffs.Remove(monster);
            }
        }

        private void Tick()
        {
            var delta = TickManager.Instance.tickInterval;
            var monsters = monsterSystem.GetAllMonsters();
            foreach (var trap in trapsModel.Traps.ToList())
            {
                if (trap.IsTriggered) continue;
                if (trap.Data.trapType == TrapType.DamageZone) HandleDamageZone(trap, monsters, delta);
                else if (trap.Data.trapType == TrapType.BearTrap) HandleBearTrap(trap, monsters);
            }
        }

        private void HandleDamageZone(TrapModel trap, IReadOnlyList<MonsterModel> monsters, float delta)
        {
            trap.TickTimer += delta;
            if (trap.TickTimer < trap.Data.tickInterval) return;
            while (trap.TickTimer >= trap.Data.tickInterval)
            {
                trap.TickTimer -= trap.Data.tickInterval;
                foreach (var monster in monsters)
                    if (!monster.IsDead && trap.Hexes.Contains(monster.CurrentHex))
                        monster.TakeDamage(trap.Data.tickDamage);
            }
        }

        private void HandleBearTrap(TrapModel trap, IReadOnlyList<MonsterModel> monsters)
        {
            var inZone = monsters.Where(m => !m.IsDead && trap.Hexes.Contains(m.CurrentHex)).ToList();
            if (inZone.Count >= trap.Data.requiredMonsters)
            {
                foreach (var m in inZone) m.TakeDamage(trap.Data.criticalDamage);
                trap.Trigger(); 
                trapsModel.RemoveTrap(trap);
            }
        }
    }
}