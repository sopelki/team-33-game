using System.Collections.Generic;
using UnityEngine;
using Logic.Monster;

namespace View
{
    public class MonsterViewManager : MonoBehaviour
    {
        [SerializeField] private Transform parent;

        private readonly Dictionary<MonsterModel, MonsterView> views = new();
        private MonsterSystem system;

        public void Initialize(MonsterSystem system)
        {
            this.system = system;

            system.OnMonsterCreated += HandleCreated;
            system.OnMonsterDied += HandleDied;
        }

        private void HandleCreated(MonsterModel model)
        {
            var prefab = model.Data.prefab;

            var go = Instantiate(
                prefab,
                model.WorldPosition,
                Quaternion.identity,
                parent
            );

            var view = go.GetComponent<MonsterView>();
            view.Initialize(model);

            views.Add(model, view);
        }

        private void HandleDied(MonsterModel model)
        {
            if (!views.TryGetValue(model, out var view))
                return;

            Destroy(view.gameObject);
            views.Remove(model);
        }

        private void Update()
        {
            foreach (var pair in views)
            {
                pair.Value.UpdateView();
            }
        }
    }
}