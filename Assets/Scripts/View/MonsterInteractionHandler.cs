using Logic.Monster;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
    public class MonsterInteractionHandler : MonoBehaviour
    {
        [SerializeField]
        private int damagePerClick = 5;
        private MonsterModel model;

        public void Setup(MonsterModel monsterModel) => model = monsterModel;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (model is { IsDead: false })
                model.TakeDamage(damagePerClick);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (model is { IsDead: false } && Misc.GlobalCursorManager.Instance)
                Misc.GlobalCursorManager.Instance.SetAttack();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Misc.GlobalCursorManager.Instance != null)
                Misc.GlobalCursorManager.Instance.SetDefault();
        }
    }
}