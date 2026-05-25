using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DialogueAnimator : MonoBehaviour
    {
        [Header("Компоненты UI")]
        [SerializeField] private Image characterAvatar;      // Ссылка на Image аватарки
        [SerializeField] private TextMeshProUGUI dialogueText; // Ссылка на TMP текст диалога

        [Header("Спрайты для анимации рта")]
        [SerializeField] private Sprite mouthClosedSprite;
        [SerializeField] private Sprite mouthOpenSprite;

        [Header("Настройки скорости")]
        [SerializeField] private float textSpeed = 0.05f;      // Скорость появления букв
        [SerializeField] private float mouthToggleSpeed = 0.15f; // Как быстро шевелится рот

        private bool isTalking = false;

        // Метод для запуска фразы
        public void PrintPhrase(string phrase)
        {
            StopAllCoroutines();
            StartCoroutine(TypeText(phrase));
            StartCoroutine(AnimateMouth());
        }

        // Корутина эффекта печатной машинки
        private IEnumerator TypeText(string phrase)
        {
            dialogueText.text = "";
            isTalking = true;

            foreach (char letter in phrase.ToCharArray())
            {
                dialogueText.text += letter;
                // Тут можно добавить воспроизведение короткого пиксельного звука "блып-блып"
                yield return new WaitForSecondsRealtime(textSpeed); 
            }

            isTalking = false; // Текст закончился, рот замирает
            characterAvatar.sprite = mouthClosedSprite;
        }

        // Корутина шевеления рта
        private IEnumerator AnimateMouth()
        {
            while (isTalking)
            {
                // Переключаем спрайты туда-сюда
                characterAvatar.sprite = characterAvatar.sprite == mouthClosedSprite ? mouthOpenSprite : mouthClosedSprite;
                yield return new WaitForSecondsRealtime(mouthToggleSpeed);
            }
            characterAvatar.sprite = mouthClosedSprite;
        }
    }
}