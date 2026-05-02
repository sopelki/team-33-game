using JetBrains.Annotations;

namespace Interfaces
{
    public interface ITickable
    {
        [UsedImplicitly]
        void Tick();
    }
}