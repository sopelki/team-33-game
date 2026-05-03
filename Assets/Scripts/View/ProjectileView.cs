using UnityEngine;
using Logic.Projectile;

namespace View
{
    public class ProjectileView : MonoBehaviour
    {
        private ProjectileModel model;
        private Vector3 lastVisualPosition;

        [Header("Arc Settings")]
        [SerializeField]
        private float arcHeight = 2f;

        public void Initialize(ProjectileModel projectileModel)
        {
            model = projectileModel;
            lastVisualPosition = model.StartPosition;
        }

        private void Update()
        {
            if (model == null)
                return;

            var t = model.TravelProgress;
            var groundPosition = Vector3.Lerp(model.StartPosition, model.TargetPoint, t);

            if (model.Data.isHoming)
            {
                transform.position = model.Position;
                RotateTowards(model.Target?.WorldPosition ?? model.Position);
            }
            else
            {
                var height = arcHeight * 4f * t * (1f - t);

                var currentVisualPosition = groundPosition + Vector3.up * height;
                transform.position = currentVisualPosition;

                ApplyRotation(currentVisualPosition);

                lastVisualPosition = currentVisualPosition;
            }
        }

        private void ApplyRotation(Vector3 currentPos)
        {
            var diff = currentPos - lastVisualPosition;
            if (diff.magnitude < 0.1f)
                return;

            var angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void RotateTowards(Vector3 targetPosition)
        {
            var dir = (targetPosition - transform.position).normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}