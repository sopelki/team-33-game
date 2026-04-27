using System;
using Logic.Castle;
using UI;
using UnityEngine;

namespace Core
{
    public class GameInitializer : MonoBehaviour
    {
        [Header("Castle Settings")]
        [SerializeField] private int startGold = 300;
        [SerializeField] private int startFood = 100;
        [SerializeField] private int startHp = 500;
        
        [Header("Scene References")]
        [SerializeField] private CastleUI castleUI;
        [SerializeField] private TickManager tickManager; 
        
        private CastleModel castleModel;
        private CastleSystem castleSystem;
        // private UnitManager unitManager;
        
        private void Awake()
        {
            castleModel = new CastleModel(startHp, startGold, startFood);
            castleSystem = new CastleSystem(castleModel);
            // unitManager = new UnitManager();

            if (tickManager != null)
                tickManager.OnTick += castleSystem.Tick;
            // TickManager.Instance.OnTick += unitManager.Tick;
        }
        
        private void Start()
        {
            // 3. Передаем данные в UI
            if (castleUI != null)
                castleUI.Initialize(castleModel);

            // 4. Находим все объекты магазина на сцене и передаем им систему
            var shopItems = FindObjectsByType<ShopToFieldItem>();
            foreach (var item in shopItems)
            {
                item.Construct(castleSystem);
            }
            var slots = FindObjectsByType<DropSlot>();
            foreach (var slot in slots)
            {
                slot.Construct(castleSystem);
            }
        }

    }
}
