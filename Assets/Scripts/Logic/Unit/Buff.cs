namespace Logic.Unit
{
    public abstract class Buff
    {
        public abstract int ModifyAttack(int baseValue);
        public abstract int ModifyHealth(int baseValue);
        public abstract float ModifyMoveSpeed(float baseValue);
    }
}