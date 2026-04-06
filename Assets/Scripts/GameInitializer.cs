using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("References")]
    public CameraSetup cameraSetup;

    public CreateMap createMap;

    private void Start()
    {
        Initialize();
    }

    [ContextMenu("Initialize Game")]
    public void Initialize()
    {
        if (cameraSetup == null || createMap == null)
        {
            Debug.LogWarning("GameInitializer: references are not assigned.");
            return;
        }

        createMap.GenerateGrid();
        cameraSetup.FitToGrid();
    }
}