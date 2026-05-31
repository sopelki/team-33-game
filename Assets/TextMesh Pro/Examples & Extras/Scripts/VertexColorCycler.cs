using System.Collections;
using TMPro;
using UnityEngine;

namespace TextMesh_Pro.Examples___Extras.Scripts
{
    public class VertexColorCycler : MonoBehaviour
    {
        private TMP_Text m_TextComponent;

        private void Awake()
        {
            m_TextComponent = GetComponent<TMP_Text>();
        }


        private void Start()
        {
            StartCoroutine(AnimateVertexColors());
        }


        /// <summary>
        ///     Method to animate vertex colors of a TMP Text object.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimateVertexColors()
        {
            m_TextComponent.ForceMeshUpdate();

            var textInfo = m_TextComponent.textInfo;
            var currentCharacter = 0;

            Color32[] newVertexColors;
            Color32 c0 = m_TextComponent.color;

            while (true)
            {
                var characterCount = textInfo.characterCount;

                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }

                var materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                var vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

                if (textInfo.characterInfo[currentCharacter].isVisible)
                {
                    c0 = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255),
                        255);

                    newVertexColors[vertexIndex + 0] = c0;
                    newVertexColors[vertexIndex + 1] = c0;
                    newVertexColors[vertexIndex + 2] = c0;
                    newVertexColors[vertexIndex + 3] = c0;

                    m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                }

                currentCharacter = (currentCharacter + 1) % characterCount;

                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}