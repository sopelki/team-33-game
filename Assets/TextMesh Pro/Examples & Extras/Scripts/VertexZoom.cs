using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TextMesh_Pro.Examples___Extras.Scripts
{
    public class VertexZoom : MonoBehaviour
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
            var cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();

            var modifiedCharScale = new List<float>();
            var scaleSortingOrder = new List<int>();

            hasTextChanged = true;

            while (true)
            {
                if (hasTextChanged)
                {
                    cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();

                    hasTextChanged = false;
                }

                var characterCount = textInfo.characterCount;

                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }

                modifiedCharScale.Clear();
                scaleSortingOrder.Clear();

                for (var i = 0; i < characterCount; i++)
                {
                    var charInfo = textInfo.characterInfo[i];

                    if (!charInfo.isVisible)
                        continue;

                    var materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    var vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    var sourceVertices = cachedMeshInfoVertexData[materialIndex].vertices;

                    Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

                    Vector3 offset = charMidBasline;

                    var destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                    destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
                    destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
                    destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
                    destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

                    var randomScale = Random.Range(1f, 1.5f);

                    modifiedCharScale.Add(randomScale);
                    scaleSortingOrder.Add(modifiedCharScale.Count - 1);

                    matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, Vector3.one * randomScale);

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

                    var sourceUVs0 = cachedMeshInfoVertexData[materialIndex].uvs0;
                    var destinationUVs0 = textInfo.meshInfo[materialIndex].uvs0;

                    destinationUVs0[vertexIndex + 0] = sourceUVs0[vertexIndex + 0];
                    destinationUVs0[vertexIndex + 1] = sourceUVs0[vertexIndex + 1];
                    destinationUVs0[vertexIndex + 2] = sourceUVs0[vertexIndex + 2];
                    destinationUVs0[vertexIndex + 3] = sourceUVs0[vertexIndex + 3];

                    var sourceColors32 = cachedMeshInfoVertexData[materialIndex].colors32;
                    var destinationColors32 = textInfo.meshInfo[materialIndex].colors32;

                    destinationColors32[vertexIndex + 0] = sourceColors32[vertexIndex + 0];
                    destinationColors32[vertexIndex + 1] = sourceColors32[vertexIndex + 1];
                    destinationColors32[vertexIndex + 2] = sourceColors32[vertexIndex + 2];
                    destinationColors32[vertexIndex + 3] = sourceColors32[vertexIndex + 3];
                }

                for (var i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    scaleSortingOrder.Sort((a, b) => modifiedCharScale[a].CompareTo(modifiedCharScale[b]));

                    textInfo.meshInfo[i].SortGeometry(scaleSortingOrder);

                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    textInfo.meshInfo[i].mesh.SetUVs(0, textInfo.meshInfo[i].uvs0);
                    textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;

                    m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}