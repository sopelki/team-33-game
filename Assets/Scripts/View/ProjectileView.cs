using UnityEngine;
using Logic.Projectile;

namespace View
{
    public class ProjectileView : MonoBehaviour
    {
        private ProjectileModel model;

        public void Initialize(ProjectileModel projectileModel)
        {
            model = projectileModel;
        }

        private void Update()
        {
            if (model != null)
                transform.position = model.Position;
        }
    }
}