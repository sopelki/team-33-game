using TMPro;
using UnityEngine;

namespace TextMesh_Pro.Examples___Extras.Scripts
{
    public class SimpleScript : MonoBehaviour
    {
        private const string label = "The <#0050FF>count is: </color>{0:2}";
        private float m_frame;

        private TextMeshPro m_textMeshPro;


        private void Start()
        {
            m_textMeshPro = gameObject.AddComponent<TextMeshPro>();

            m_textMeshPro.autoSizeTextContainer = true;

            m_textMeshPro.fontSize = 48;

            m_textMeshPro.alignment = TextAlignmentOptions.Center;

            m_textMeshPro.textWrappingMode = TextWrappingModes.NoWrap;
        }


        private void Update()
        {
            m_textMeshPro.SetText(label, m_frame % 1000);
            m_frame += 1 * Time.deltaTime;
        }
    }
}