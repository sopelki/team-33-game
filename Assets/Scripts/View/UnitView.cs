using Logic.Unit;
using UnityEngine;

namespace View
{
    public class UnitView : MonoBehaviour
    {
        private UnitModel model;

        public void Initialize(UnitModel model)
        {
            this.model = model;
            transform.position = model.WorldPosition;
        }

        public void SetPosition(Vector3 worldPos)
        {
            transform.position = worldPos;
        }

        private void Update()
        {
            if (model != null)
                transform.position = model.WorldPosition;
        }
    }

}