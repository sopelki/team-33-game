using UnityEngine;
using Logic.Monster;

namespace Logic.Projectile
{
    public class ProjectileModel
    {
        public Vector3 Position;
        public MonsterModel Target;
        public ProjectileData Data;
        public Vector3 Direction;
        public float LastDistanceToTarget;
        public float TraveledDistance;

        public ProjectileModel(
            Vector3 startPos,
            MonsterModel target,
            ProjectileData data)
        {
            Position = startPos;
            Target = target;
            Data = data;
            Direction = (target.WorldPosition - startPos).normalized;
            LastDistanceToTarget =
                Vector3.Distance(startPos, target.WorldPosition);
            TraveledDistance = 0f;
        }
    }
}