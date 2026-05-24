using UnityEngine;
using UnityEngine.Audio;

namespace MenuScripts
{
    public class AudioInitializer : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer audioMixer;

        private void Start()
        {
            ApplyValue("MasterVol", 0.75f);
            ApplyValue("MusicVol", 0.75f);
            ApplyValue("SfxVol", 0.75f);
            ApplyValue("UiVol", 0.75f);
        }

        private void ApplyValue(string key, float defaultVal)
        {
            var volume = PlayerPrefs.GetFloat(key, defaultVal);
            var clampedVol = Mathf.Clamp(volume, 0.0001f, 1f);
            audioMixer.SetFloat(key, Mathf.Log10(clampedVol) * 20);
        }
    }
}