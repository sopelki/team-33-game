using System.Collections;
using TMPro;
using UnityEngine;

namespace TextMesh_Pro.Examples___Extras.Scripts
{
    public class ShaderPropAnimator : MonoBehaviour
    {
        public AnimationCurve GlowCurve;

        public float m_frame;
        private Material m_Material;

        private Renderer m_Renderer;

        private void Awake()
        {
            m_Renderer = GetComponent<Renderer>();

            m_Material = m_Renderer.material;
        }

        private void Start()
        {
            StartCoroutine(AnimateProperties());
        }

        private IEnumerator AnimateProperties()
        {
            float glowPower;
            m_frame = Random.Range(0f, 1f);

            while (true)
            {
                glowPower = GlowCurve.Evaluate(m_frame);
                m_Material.SetFloat(ShaderUtilities.ID_GlowPower, glowPower);

                m_frame += Time.deltaTime * Random.Range(0.2f, 0.3f);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}