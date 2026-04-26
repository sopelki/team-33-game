using TMPro;
using UnityEngine;

namespace CastleScripts
{
    public class CastleUI: MonoBehaviour
    {
        [SerializeField] private Castle castle;

        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI foodText;

        private void Start()
        {
            castle.OnResourcesChanged += UpdateUI;
            UpdateUI();
        }

        private void UpdateUI()
        {
            hpText.text = "HP: " + castle.HP;
            goldText.text = "Gold: " + castle.Gold;
            foodText.text = "Food: " + castle.Food;
        }
        private void Update()
        {
            hpText.text = "HP: " + castle.HP;
            goldText.text = "Gold: " + castle.Gold;
            foodText.text = "Food: " + castle.Food;
        }
    }
}