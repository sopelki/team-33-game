using UnityEngine;
using Logic.Monster;

namespace Logic.Projectile
{
    public class ProjectileModel
    {
        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 TargetPoint;
        public Vector3 StartPosition;

        public MonsterModel Target;
        public ProjectileData Data;

        public ProjectileModel(
            Vector3 startPos,
            MonsterModel target,
            ProjectileData data)
        {
            StartPosition = startPos;
            Position = startPos;
            Target = target;
            Data = data;

            TargetPoint = target.WorldPosition;
            Direction = (TargetPoint - startPos).normalized;
        }

        public ProjectileModel(
            Vector3 startPos,
            MonsterModel target,
            ProjectileData data,
            Vector3 interceptPoint)
        {
            StartPosition = startPos;
            Position = startPos;
            Target = target;
            Data = data;

            TargetPoint = interceptPoint;
            Direction = (TargetPoint - startPos).normalized;
        }
    }
}