using UnityEngine;

namespace Logic.Tower
{
    public class TowerModel
    {
        public TowerData Data { get; }
        public Vector3Int GridPosition { get; }
        public Vector3 WorldPosition { get; }
        public float NextFireTime { get; set; }

        public TowerModel(TowerData data, Vector3Int gridPos, Vector3 worldPos)
        {
            Data = data;
            GridPosition = gridPos;
            WorldPosition = worldPos;
        }
    }
}