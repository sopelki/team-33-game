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
        [SerializeField]
        private int startGold = 300;
        [SerializeField]
        private int startFood = 100;
        [SerializeField]
        private int startHp = 500;

        [Header("Scene References")]
        [SerializeField]
        private CastleUI castleUI;
        [SerializeField]
        private TickManager tickManager;
        [SerializeField]
        private TowerViewManager towerViewManager;

        private CastleModel castleModel;
        private CastleSystem castleSystem;

        private TowersModel towersModel;
        private TowerSystem towerSystem;

        private void Awake()
        {
            castleModel = new CastleModel(startHp, startGold, startFood);
            castleSystem = new CastleSystem(castleModel);

            towersModel = new TowersModel();
            towerSystem = new TowerSystem(castleSystem, towersModel);

            if (tickManager == null)
                return;
            
            tickManager.OnTick += castleSystem.Tick;
            tickManager.OnTick += towerSystem.Tick;
        }

        private void Start()
        {
            if (towerViewManager != null)
                towerViewManager.Initialize(towersModel);

            if (castleUI != null)
                castleUI.Initialize(castleModel);

            var shopItems = FindObjectsByType<ShopToFieldItem>();
            foreach (var item in shopItems)
                item.Construct(towerSystem);

            var slots = FindObjectsByType<DropSlot>();
            foreach (var slot in slots)
                slot.Construct(castleSystem);
        }
    }
}