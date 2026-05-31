using TMPro;
using UnityEngine;

namespace TextMesh_Pro.Examples___Extras.Scripts
{
    public class TMP_ExampleScript_01 : MonoBehaviour
    {
        public enum objectType
        {
            TextMeshPro = 0,
            TextMeshProUGUI = 1
        }


        private const string k_label = "The count is <#0080ff>{0}</color>";

        public objectType ObjectType;
        public bool isStatic;
        private int count;

        private TMP_Text m_text;

        private void Awake()
        {
            if (ObjectType == 0)
                m_text = GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();
            else
                m_text = GetComponent<TextMeshProUGUI>() ?? gameObject.AddComponent<TextMeshProUGUI>();

            m_text.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");

            m_text.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");

            m_text.fontSize = 120;

            m_text.text = "A <#0080ff>simple</color> line of text.";

            var size = m_text.GetPreferredValues(Mathf.Infinity, Mathf.Infinity);

            m_text.rectTransform.sizeDelta = new Vector2(size.x, size.y);
        }


        private void Update()
        {
            if (!isStatic)
            {
                m_text.SetText(k_label, count % 1000);
                count += 1;
            }
        }
    }
}