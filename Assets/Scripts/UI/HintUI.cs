using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HintUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI hintText;
        [SerializeField]
        private TextMeshProUGUI shadowText;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private float fadeInDuration = 0.5f;
        [SerializeField]
        private float fadeOutDuration = 0.5f;
        [SerializeField]
        private float targetOpacity = 0.75f;

        private Coroutine fadeCoroutine;
        
        public void Initialize()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            canvasGroup.alpha = 0;
        }

        public void ShowHint(string message)
        {
            if (hintText == null)
                return;
            
            if (shadowText == null)
                return;

            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            hintText.text = message;
            shadowText.text = message;
            
            fadeCoroutine = StartCoroutine(FadeIn());
        }

        public void HideHint()
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeOut());
        }

        private IEnumerator FadeIn()
        {
            var elapsed = 0f;

            while (elapsed < fadeInDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0, targetOpacity, elapsed / fadeInDuration);
                yield return null;
            }

            canvasGroup.alpha = targetOpacity;
        }

        private IEnumerator FadeOut()
        {
            var elapsed = 0f;

            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(targetOpacity, 0, elapsed / fadeOutDuration);
                yield return null;
            }

            canvasGroup.alpha = 0;
        }
    }
}