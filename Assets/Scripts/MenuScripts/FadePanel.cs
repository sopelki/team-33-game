using System.Collections;
using UnityEngine;

namespace MenuScripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadePanel : MonoBehaviour
    {
        [SerializeField]
        private float fadeDuration = 0.05f;
        private CanvasGroup canvasGroup;
        private Coroutine currentFade;
        
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void SetFadeDuration(float duration) => fadeDuration = duration;
        
        public float FadeDuration => fadeDuration;

        public void Show()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            if (currentFade != null)
                StopCoroutine(currentFade);
            currentFade = StartCoroutine(FadeRoutine(1f));
        }

        public void Hide()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            if (currentFade != null) StopCoroutine(currentFade);
            currentFade = StartCoroutine(FadeRoutine(0f));
        }

        private IEnumerator FadeRoutine(float targetAlpha)
        {
            var startAlpha = canvasGroup.alpha;
            var startTime = Time.realtimeSinceStartup;

            while (Time.realtimeSinceStartup - startTime < fadeDuration)
            {
                var elapsed = Time.realtimeSinceStartup - startTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
        }
    }
}