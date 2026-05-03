using UnityEngine;
using Vector3 = System.Numerics.Vector3;

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