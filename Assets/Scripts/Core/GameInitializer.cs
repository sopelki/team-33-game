using Field;
using Logic.Castle;
using Logic.Tower;
using Logic.Unit;
using View;
using UI;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        [SerializeField]
        private CameraSetup cameraSetup;

        [Header("Unit Settings")]
        [SerializeField]
        private UnitData soldierData;
        [SerializeField]
        private UnitViewManager unitViewManager;

        [Header("Field")]
        [SerializeField]
        private FieldGenerator fieldGenerator;
        [SerializeField]
        private Tilemap tilemap;

        private CastleModel castleModel;
        private CastleSystem castleSystem;

        private TowersModel towersModel;
        private TowerSystem towerSystem;

        private UnitSystem unitSystem;
        private FreeMovementService movementService;

        private Field.Field field;

        private void Awake()
        {
            field = SaveLoadManager.LoadMapFromFile();
            if (field == null)
            {
                Debug.LogError("Level file not found! Game cannot start.");
                return;
            }
            cameraSetup.FitToGrid();
            
            movementService = new FreeMovementService(field, tilemap);
            unitSystem = new UnitSystem(movementService);

            castleModel = new CastleModel(startHp, startGold, startFood);
            castleSystem = new CastleSystem(
                castleModel,
                unitSystem,
                soldierData,
                field,
                tilemap
            );

            towersModel = new TowersModel();
            towerSystem = new TowerSystem(castleSystem, towersModel);

            if (tickManager == null)
                return;

            tickManager.OnTick += castleSystem.Tick;
            tickManager.OnTick += towerSystem.Tick;
            tickManager.OnTick += unitSystem.Tick;

            //TODO: Нужно ли это вообще?
            unitSystem.OnUnitDied += unit =>
            {
                castleModel.CurrentUnits--;
                castleModel.Changed();
            };
        }

        private void Start()
        {
            if (fieldGenerator != null)
                fieldGenerator.Initialize(field);

            if (unitViewManager != null)
                unitViewManager.Initialize(unitSystem);

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

        // private void Update()
        // {
        //     unitSystem?.Tick(Time.deltaTime);
        // }
    }
}