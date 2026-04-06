using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("References")]
    public CameraSetup cameraSetup;

    public FieldGenerator fieldGenerator;

    private void Start()
    {
        Initialize();
    }

    [ContextMenu("Initialize Game")]
    public void Initialize()
    {
        if (cameraSetup == null || fieldGenerator == null)
        {
            Debug.LogWarning("GameInitializer: references are not assigned.");
            return;
        }

        fieldGenerator.GenerateAndDraw();
        cameraSetup.FitToGrid();
    }
}