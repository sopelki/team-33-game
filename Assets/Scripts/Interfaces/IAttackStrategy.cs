namespace Interfaces
{
    public interface IAttackStrategy
    {
        void Tick();
        bool IsAttacking { get; }
    }
}