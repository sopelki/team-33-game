using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace MenuScripts
{
    public class SettingsController : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField]
        private AudioMixer audioMixer;
        [SerializeField]
        private Slider masterSlider;
        [SerializeField]
        private Slider musicSlider;
        [SerializeField]
        private Slider sfxSlider;
        [SerializeField]
        private Slider uiSlider;

        [Header("Graphics")]
        [SerializeField]
        private TMP_Dropdown resolutionDropdown;
        [SerializeField]
        private Toggle fullscreenToggle;

        private Resolution[] resolutions;

        private void Start()
        {
            SetupResolutions();
            LoadSettings();
        }

        private void SetupResolutions()
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            var options = new List<string>();
            var currentResolutionIndex = 0;

            for (var i = 0; i < resolutions.Length; i++)
            {
                var option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = PlayerPrefs.GetInt("ResIndex", currentResolutionIndex);
            resolutionDropdown.RefreshShownValue();
        }

        public void SetResolution(int resolutionIndex)
        {
            var resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResIndex", resolutionIndex);
        }

        public void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MasterVol", volume);
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MusicVol", volume);
        }

        public void SetSfxVolume(float volume)
        {
            audioMixer.SetFloat("SfxVol", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SfxVol", volume);
        }
        
        public void SetUiVolume(float volume)
        {
            audioMixer.SetFloat("UiVol", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("UiVol", volume);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        }

        private void LoadSettings()
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 0.75f);
            musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 0.75f);
            sfxSlider.value = PlayerPrefs.GetFloat("SfxVol", 0.75f);
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

            SetMasterVolume(masterSlider.value);
            SetMusicVolume(musicSlider.value);
            SetSfxVolume(sfxSlider.value);
        }
    }
}