using System.Collections;
using TMPro;
using UnityEngine;

namespace TextMesh_Pro.Examples___Extras.Scripts
{
    public class VertexJitter : MonoBehaviour
    {
        public float AngleMultiplier = 1.0f;
        public float SpeedMultiplier = 1.0f;
        public float CurveScale = 1.0f;
        private bool hasTextChanged;

        private TMP_Text m_TextComponent;

        private void Awake()
        {
            m_TextComponent = GetComponent<TMP_Text>();
        }


        private void Start()
        {
            StartCoroutine(AnimateVertexColors());
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
            if (obj == m_TextComponent)
                hasTextChanged = true;
        }

        /// <summary>
        ///     Method to animate vertex colors of a TMP Text object.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimateVertexColors()
        {
            m_TextComponent.ForceMeshUpdate();

            var textInfo = m_TextComponent.textInfo;

            Matrix4x4 matrix;

            var loopCount = 0;
            hasTextChanged = true;

            var vertexAnim = new VertexAnim[1024];
            for (var i = 0; i < 1024; i++)
            {
                vertexAnim[i].angleRange = Random.Range(10f, 25f);
                vertexAnim[i].speed = Random.Range(1f, 3f);
            }

            var cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

            while (true)
            {
                if (hasTextChanged)
                {
                    cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

                    hasTextChanged = false;
                }

                var characterCount = textInfo.characterCount;

                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }


                for (var i = 0; i < characterCount; i++)
                {
                    var charInfo = textInfo.characterInfo[i];

                    if (!charInfo.isVisible)
                        continue;

                    var vertAnim = vertexAnim[i];

                    var materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    var vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    var sourceVertices = cachedMeshInfo[materialIndex].vertices;

                    Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

                    Vector3 offset = charMidBasline;

                    var destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                    destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
                    destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
                    destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
                    destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

                    vertAnim.angle = Mathf.SmoothStep(-vertAnim.angleRange, vertAnim.angleRange,
                        Mathf.PingPong(loopCount / 25f * vertAnim.speed, 1f));
                    var jitterOffset = new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f), 0);

                    matrix = Matrix4x4.TRS(jitterOffset * CurveScale,
                        Quaternion.Euler(0, 0, Random.Range(-5f, 5f) * AngleMultiplier), Vector3.one);

                    destinationVertices[vertexIndex + 0] =
                        matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
                    destinationVertices[vertexIndex + 1] =
                        matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
                    destinationVertices[vertexIndex + 2] =
                        matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
                    destinationVertices[vertexIndex + 3] =
                        matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

                    destinationVertices[vertexIndex + 0] += offset;
                    destinationVertices[vertexIndex + 1] += offset;
                    destinationVertices[vertexIndex + 2] += offset;
                    destinationVertices[vertexIndex + 3] += offset;

                    vertexAnim[i] = vertAnim;
                }

                for (var i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                loopCount += 1;

                yield return new WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        ///     Structure to hold pre-computed animation data.
        /// </summary>
        private struct VertexAnim
        {
            public float angleRange;
            public float angle;
            public float speed;
        }
    }
}