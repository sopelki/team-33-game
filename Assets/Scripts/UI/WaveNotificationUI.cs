using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WaveNotificationUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI waveText;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private float displayDuration = 3f;
        [SerializeField]
        private float fadeDuration = 0.5f;

        private Coroutine displayCoroutine;

        public void Initialize()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            
            canvasGroup.alpha = 0;
        }

        public void ShowWaveNotification(int waveNumber)
        {
            if (displayCoroutine != null)
                StopCoroutine(displayCoroutine);

            displayCoroutine = StartCoroutine(DisplayWaveCoroutine(waveNumber));
        }

        private IEnumerator DisplayWaveCoroutine(int waveNumber)
        {
            waveText.text = $"Началась волна {waveNumber}";

            yield return FadeCanvasGroup(0, 1, fadeDuration);

            yield return new WaitForSeconds(displayDuration);

            yield return FadeCanvasGroup(1, 0, fadeDuration);
        }

        private IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha, float duration)
        {
            var elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
                yield return null;
            }

            canvasGroup.alpha = endAlpha;
        }
    }
}