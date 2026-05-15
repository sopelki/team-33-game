using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class TooltipUI : MonoBehaviour
    {
        public static TooltipUI Instance;

        [SerializeField]
        private GameObject panel;
        [SerializeField]
        private TextMeshProUGUI titleText;
        [SerializeField]
        private TextMeshProUGUI descriptionText;
        [SerializeField]
        private TextMeshProUGUI priceText;
        [SerializeField]
        private TextMeshProUGUI statsText;
        [SerializeField]
        private int offsetX;
        [SerializeField]
        private int offsetY;

        private RectTransform rectTransform;
        private Canvas canvas;

        private void Awake()
        {
            Instance = this;
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            Hide();
        }

        public void Show(TooltipContent content)
        {
            panel.SetActive(true);

            titleText.text = content.Title;
            descriptionText.text = content.Description;
            priceText.text = content.Cost;
            statsText.text = content.SpecialInfo;

            UpdatePosition();
        }

        public void Hide() => panel.SetActive(false);

        private void Update()
        {
            if (panel.activeSelf)
                UpdatePosition();
        }

        private void UpdatePosition()
        {
            var mousePos = Mouse.current.position.ReadValue();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                mousePos,
                canvas.worldCamera,
                out var localPoint);

            rectTransform.localPosition = localPoint + new Vector2(offsetX, offsetY);
        }
    }
}