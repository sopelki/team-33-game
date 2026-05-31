using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TextMesh_Pro.Examples___Extras.Scripts
{
    public class TMP_TextSelector_A : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Camera m_Camera;

        private bool m_isHoveringObject;
        private int m_lastCharIndex = -1;
        private int m_lastWordIndex = -1;
        private int m_selectedLink = -1;
        private TextMeshPro m_TextMeshPro;

        private void Awake()
        {
            m_TextMeshPro = gameObject.GetComponent<TextMeshPro>();
            m_Camera = Camera.main;

            m_TextMeshPro.ForceMeshUpdate();
        }


        private void LateUpdate()
        {
            m_isHoveringObject = false;

            if (TMP_TextUtilities.IsIntersectingRectTransform(m_TextMeshPro.rectTransform, Input.mousePosition,
                    Camera.main))
                m_isHoveringObject = true;

            if (m_isHoveringObject)
            {
                #region Example of Character Selection

                var charIndex =
                    TMP_TextUtilities.FindIntersectingCharacter(m_TextMeshPro, Input.mousePosition, Camera.main, true);
                if (charIndex != -1 && charIndex != m_lastCharIndex &&
                    (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                {
                    m_lastCharIndex = charIndex;

                    var meshIndex = m_TextMeshPro.textInfo.characterInfo[charIndex].materialReferenceIndex;

                    var vertexIndex = m_TextMeshPro.textInfo.characterInfo[charIndex].vertexIndex;

                    var c = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255),
                        (byte)Random.Range(0, 255), 255);

                    var vertexColors = m_TextMeshPro.textInfo.meshInfo[meshIndex].colors32;

                    vertexColors[vertexIndex + 0] = c;
                    vertexColors[vertexIndex + 1] = c;
                    vertexColors[vertexIndex + 2] = c;
                    vertexColors[vertexIndex + 3] = c;

                    m_TextMeshPro.textInfo.meshInfo[meshIndex].mesh.colors32 = vertexColors;
                }

                #endregion

                #region Example of Link Handling

                var linkIndex = TMP_TextUtilities.FindIntersectingLink(m_TextMeshPro, Input.mousePosition, m_Camera);

                if (linkIndex == -1 && m_selectedLink != -1 || linkIndex != m_selectedLink)
                    m_selectedLink = -1;

                if (linkIndex != -1 && linkIndex != m_selectedLink)
                {
                    m_selectedLink = linkIndex;

                    var linkInfo = m_TextMeshPro.textInfo.linkInfo[linkIndex];

                    Vector3 worldPointInRectangle;

                    RectTransformUtility.ScreenPointToWorldPointInRectangle(m_TextMeshPro.rectTransform,
                        Input.mousePosition, m_Camera, out worldPointInRectangle);

                    switch (linkInfo.GetLinkID())
                    {
                        case "id_01":
                            break;
                        case "id_02":
                            break;
                    }
                }

                #endregion


                #region Example of Word Selection

                var wordIndex = TMP_TextUtilities.FindIntersectingWord(m_TextMeshPro, Input.mousePosition, Camera.main);
                if (wordIndex != -1 && wordIndex != m_lastWordIndex)
                {
                    m_lastWordIndex = wordIndex;

                    var wInfo = m_TextMeshPro.textInfo.wordInfo[wordIndex];

                    var wordPOS = m_TextMeshPro.transform.TransformPoint(m_TextMeshPro.textInfo
                        .characterInfo[wInfo.firstCharacterIndex].bottomLeft);
                    wordPOS = Camera.main.WorldToScreenPoint(wordPOS);

                    var vertexColors = m_TextMeshPro.textInfo.meshInfo[0].colors32;

                    var c = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255),
                        (byte)Random.Range(0, 255), 255);
                    for (var i = 0; i < wInfo.characterCount; i++)
                    {
                        var vertexIndex = m_TextMeshPro.textInfo.characterInfo[wInfo.firstCharacterIndex + i]
                            .vertexIndex;

                        vertexColors[vertexIndex + 0] = c;
                        vertexColors[vertexIndex + 1] = c;
                        vertexColors[vertexIndex + 2] = c;
                        vertexColors[vertexIndex + 3] = c;
                    }

                    m_TextMeshPro.mesh.colors32 = vertexColors;
                }

                #endregion
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter()");
            m_isHoveringObject = true;
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit()");
            m_isHoveringObject = false;
        }
    }
}