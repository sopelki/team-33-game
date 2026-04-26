using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Settings")]
    public float range = 5f;
    public float fireRate = 1f;
    public int damage = 10;
    
    private Vector3Int gridPosition;

    // Метод для инициализации (вызывается при спавне)
    public void Setup(Vector3Int pos)
    {
        Debug.Log($"Setting up tower ({pos.x}, {pos.y}, {pos.z})");
        gridPosition = pos;
    }

    // // Пример логики удаления башни
    // public void SellTower()
    // {
    //     TowerManager.Instance.UnregisterTower(gridPosition);
    //     Destroy(gameObject);
    // }

    private void OnDrawGizmosSelected()
    {
        // Рисуем радиус атаки в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}