using UnityEngine;
using Logic.Monster;

namespace View
{
    public class MonsterView : MonoBehaviour
    {
        private MonsterModel model;

        public void Initialize(MonsterModel model)
        {
            this.model = model;
            transform.position = model.WorldPosition;
        }

        public void SetPosition(Vector3 worldPos)
        {
            transform.position = worldPos;
        }
    }
}