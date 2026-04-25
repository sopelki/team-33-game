using UnityEngine;
using UnityEngine.Audio;

namespace MenuScripts
{
    public class SettingsMenu : MonoBehaviour
    {
        public GameObject settingsPanel;

        public void OpenSettings()
        {
            settingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            settingsPanel.SetActive(false);
        }
    }
}