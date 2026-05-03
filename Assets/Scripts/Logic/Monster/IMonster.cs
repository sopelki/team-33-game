using UnityEngine;

namespace Logic.Monster
{
    public interface IMonster
    {
        UnityEngine.Vector3 WorldPosition { get; }
        Vector2Int CurrentHex { get; }
        bool IsDead { get; }

        void TakeDamage(int damage);
    }
}