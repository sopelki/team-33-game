using System.Collections.Generic;
using Field;
using Logic.Castle;
using Logic.Monster;
using Logic.Projectile;
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
        private ProjectileViewManager projectileViewManager;
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

        [Header("Monster Settings")]
        [SerializeField]
        private List<MonsterData> availableMonsters;
        [SerializeField]
        private MonsterViewManager monsterViewManager;

        private MonsterSystem monsterSystem;
        private MonsterSpawner monsterSpawner;

        private CastleModel castleModel;
        private CastleSystem castleSystem;

        private TowersModel towersModel;
        private TowerSystem towerSystem;

        private UnitSystem unitSystem;

        private ProjectileSystem projectileSystem;

        private Field.Field field;
        private static readonly List<Vector2Int> spawnHexes = new()
        {
            new Vector2Int(28, -17),
        };

        private static readonly Vector2Int castleHex = new(-30, 20);

        private void Awake()
        {
            field = SaveLoadManager.LoadMapFromFile();
            if (field == null)
            {
                Debug.LogError("Level file not found! Game cannot start.");
                return;
            }
            cameraSetup.FitToGrid();
            
            monsterSystem = new MonsterSystem();
            projectileSystem = new ProjectileSystem(monsterSystem);
            unitSystem = new UnitSystem(
                monsterSystem,
                field,
                tilemap
            );


            monsterSpawner = new MonsterSpawner(
                spawnHexes,
                castleHex,
                field,
                monsterSystem,
                unitSystem,
                availableMonsters,
                tilemap
            );


            castleModel = new CastleModel(startHp, startGold, startFood);
            castleSystem = new CastleSystem(
                castleModel,
                unitSystem,
                soldierData,
                field,
                tilemap
            );

            towersModel = new TowersModel();
            towerSystem = new TowerSystem(castleSystem, towersModel, monsterSystem, projectileSystem);

            if (tickManager == null)
                return;

            tickManager.OnTick += castleSystem.Tick;
            tickManager.OnTick += towerSystem.Tick;
            tickManager.OnTick += unitSystem.Tick;
            tickManager.OnTick += monsterSystem.Tick;
            tickManager.OnTick += monsterSpawner.Tick;
            tickManager.OnTick += projectileSystem.Tick;

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

            if (monsterViewManager != null)
                monsterViewManager.Initialize(monsterSystem);

            if (castleUI != null)
                castleUI.Initialize(castleModel);

            if (projectileViewManager != null)
                projectileViewManager.Initialize(projectileSystem);

            var shopItems = FindObjectsByType<ShopToFieldItem>();
            foreach (var item in shopItems)
                item.Construct(towerSystem);

            var slots = FindObjectsByType<DropSlot>();
            foreach (var slot in slots)
                slot.Construct(castleSystem);
        }
    }
}