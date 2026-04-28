using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    // ключ: позиция клетки, значение: ссылка на башню
    private readonly Dictionary<Vector3Int, GameObject> occupiedCells = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    public bool IsCellOccupied(Vector3Int cellPos)
    {
        return occupiedCells.ContainsKey(cellPos);
    }

    public void RegisterTower(Vector3Int cellPos, GameObject tower)
    {
        Debug.Log($"Registering tower at {cellPos}.");
        occupiedCells.TryAdd(cellPos, tower);
    }

    public void UnregisterTower(Vector3Int cellPos)
    {
        occupiedCells.Remove(cellPos);
    }
}