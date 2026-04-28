using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Settings")]
    public float range = 5f;
    public float fireRate = 1f;
    public int damage = 10;
    
    private Vector3Int gridPosition;

    public void Setup(Vector3Int pos)
    {
        Debug.Log($"Setting up tower ({pos.x}, {pos.y}, {pos.z})");
        gridPosition = pos;
    }
    
    // public void SellTower()
    // {
    //     TowerManager.Instance.UnregisterTower(gridPosition);
    //     Destroy(gameObject);
    // }

    private void OnDrawGizmosSelected()
    {
        // Радиус в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}