using Logic.Castle;
using Logic.Tower;
using View;
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
        [SerializeField] private TowerViewManager towerViewManager; // Ссылка на менеджер отрисовки башен
        
        private CastleModel castleModel;
        private CastleSystem castleSystem;
        
        private TowersModel towersModel;
        private TowerSystem towerSystem;
        
        private void Awake()
        {
            // 1. Инициализируем Замок
            castleModel = new CastleModel(startHp, startGold, startFood);
            castleSystem = new CastleSystem(castleModel);

            // 2. Инициализируем Башни
            towersModel = new TowersModel();
            towerSystem = new TowerSystem(castleSystem, towersModel);

            // 3. Подписываем системы на тики
            if (tickManager != null)
            {
                tickManager.OnTick += castleSystem.Tick;
                tickManager.OnTick += towerSystem.Tick;
            }
        }
        
        private void Start()
        {
            // Инициализируем визуальную часть башен
            if (towerViewManager != null)
                towerViewManager.Initialize(towersModel);

            // Инициализируем UI замка
            if (castleUI != null)
                castleUI.Initialize(castleModel);

            // Настраиваем предметы в магазине (теперь они работают с TowerSystem)
            var shopItems = FindObjectsByType<ShopToFieldItem>();
            foreach (var item in shopItems)
                item.Construct(towerSystem); 
            
            // Слоты для построек внутри замка (оставляем CastleSystem)
            var slots = FindObjectsByType<DropSlot>();
            foreach (var slot in slots)
                slot.Construct(castleSystem);
        }
    }
}