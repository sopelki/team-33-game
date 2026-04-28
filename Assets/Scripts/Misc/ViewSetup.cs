using UnityEngine;

namespace Misc
{
    public class ViewSetup : MonoBehaviour
    {
    
        [Header("References")]
        public CameraSetup cameraSetup;

        public FieldGenerator fieldGenerator;

        private void Start()
        {
            Initialize();
        }

        [ContextMenu("Setup Field and Camera")]
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
}