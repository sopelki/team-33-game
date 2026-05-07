using UnityEngine;

namespace Logic.Unit
{
    public abstract class Buff
    {
        public virtual int ModifyAttack(int baseValue) => baseValue;
        public virtual int ModifyMaxHealth(int baseValue) => baseValue;
        public virtual float ModifyMoveSpeed(float baseValue) => baseValue;
    }
    
    public class AttackPercentBuff : Buff
    {
        public override int ModifyAttack(int baseValue) 
        {
            return baseValue + Mathf.RoundToInt(baseValue * 0.25f);
        }
    }
    
    public class HealthPercentBuff : Buff
    {
        public override int ModifyMaxHealth(int baseValue) 
        {
            return baseValue + Mathf.RoundToInt(baseValue * 0.25f);
        }
    }
}