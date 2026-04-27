using Logic.Castle;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CastleUI: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI foodText;
        
        private CastleModel model;

        public void Initialize(CastleModel castleModel)
        {
            model = castleModel;
            model.OnChanged += UpdateUI;
            UpdateUI();
        }
        
        private void OnDestroy()
        {
            if (model != null) 
                model.OnChanged -= UpdateUI;
        }
        
        private void UpdateUI()
        {
            hpText.text = "HP: " + model.Hp;
            goldText.text = "Gold: " + model.Gold;
            foodText.text = "Food: " + model.Food;
        }
    }
}