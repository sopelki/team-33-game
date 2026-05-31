using System.Collections;
using TMPro;
using UnityEngine;

namespace TextMesh_Pro.Examples___Extras.Scripts
{
    public class TextConsoleSimulator : MonoBehaviour
    {
        private bool hasTextChanged;
        private TMP_Text m_TextComponent;

        private void Awake()
        {
            m_TextComponent = gameObject.GetComponent<TMP_Text>();
        }


        private void Start()
        {
            StartCoroutine(RevealCharacters(m_TextComponent));
        }


        private void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
        }

        private void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
        }


        private void ON_TEXT_CHANGED(Object obj)
        {
            hasTextChanged = true;
        }


        /// <summary>
        ///     Method revealing the text one character at a time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RevealCharacters(TMP_Text textComponent)
        {
            textComponent.ForceMeshUpdate();

            var textInfo = textComponent.textInfo;

            var totalVisibleCharacters = textInfo.characterCount;
            var visibleCount = 0;

            while (true)
            {
                if (hasTextChanged)
                {
                    totalVisibleCharacters = textInfo.characterCount;
                    hasTextChanged = false;
                }

                if (visibleCount > totalVisibleCharacters)
                {
                    yield return new WaitForSeconds(1.0f);
                    visibleCount = 0;
                }

                textComponent.maxVisibleCharacters = visibleCount;

                visibleCount += 1;

                yield return null;
            }
        }


        /// <summary>
        ///     Method revealing the text one word at a time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RevealWords(TMP_Text textComponent)
        {
            textComponent.ForceMeshUpdate();

            var totalWordCount = textComponent.textInfo.wordCount;
            var totalVisibleCharacters =
                textComponent.textInfo.characterCount;
            var counter = 0;
            var currentWord = 0;
            var visibleCount = 0;

            while (true)
            {
                currentWord = counter % (totalWordCount + 1);

                if (currentWord == 0)
                    visibleCount = 0;
                else if (currentWord < totalWordCount)
                    visibleCount = textComponent.textInfo.wordInfo[currentWord - 1].lastCharacterIndex + 1;
                else if (currentWord == totalWordCount)
                    visibleCount = totalVisibleCharacters;

                textComponent.maxVisibleCharacters = visibleCount;

                if (visibleCount >= totalVisibleCharacters)
                    yield return new WaitForSeconds(1.0f);

                counter += 1;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}