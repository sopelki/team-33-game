using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ITooltipProvider provider;
        private Coroutine delayCoroutine;
        [SerializeField]
        private float delay = 0.5f;

        public void SetContent(ITooltipProvider tooltipProvider) => provider = tooltipProvider;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (provider == null)
                return;
            delayCoroutine = StartCoroutine(ShowWithDelay());
        }

        public void OnPointerExit(PointerEventData eventData) => StopDisplay();

        private IEnumerator ShowWithDelay()
        {
            yield return new WaitForSecondsRealtime(delay);
            TooltipUI.Instance.Show(provider.GetTooltipContent());
        }

        public void StopDisplay()
        {
            if (delayCoroutine != null) StopCoroutine(delayCoroutine);
            if (TooltipUI.Instance != null) TooltipUI.Instance.Hide();
        }

        private void OnDisable() => StopDisplay();
    }
}