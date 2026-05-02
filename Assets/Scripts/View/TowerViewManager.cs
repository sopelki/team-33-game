using UnityEngine;
using Logic.Tower;
using System.Collections.Generic;
using HexagonScripts;

namespace View
{
    public class TowerViewManager : MonoBehaviour
    {
        private TowersModel model;
        private readonly Dictionary<TowerModel, TowerView> views = new();

        public void Initialize(TowersModel modelToInitialize)
        {
            model = modelToInitialize;
            model.OnChanged += HandleTowerAdded;
        }

        private void OnDestroy()
        {
            if (model != null)
                model.OnChanged -= HandleTowerAdded;
        }

        private void HandleTowerAdded(TowerModel towerModel)
        {
            var viewGo = Instantiate(towerModel.Data.viewPrefab, towerModel.WorldPosition, Quaternion.identity);
            var view = viewGo.GetComponent<TowerView>();
            
            views.Add(towerModel, view);
            
            Debug.Log($"TowerView created for tower at {towerModel.GridPosition}");
        }
    }
}