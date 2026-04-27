using Misc;
using UnityEngine;

public class ViewSetup : MonoBehaviour
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

        fieldGenerator.LoadAndDraw();
        cameraSetup.FitToGrid();
    }
}