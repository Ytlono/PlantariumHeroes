using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MyGameProject
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject settingsPanel;
        private ActiveState settingsState;

        [SerializeField] private Scrollbar camSensitivityScrollbar;
        [SerializeField] private Text CamSensitivityValueText;

        [SerializeField] private Scrollbar volumeScrollbar;
        [SerializeField] private Text VolumeValueText;

        [SerializeField] private Dropdown resolutionDropdown;
        [SerializeField] private Dropdown qualityDropdown;

        private CameraController cameraController;
        private SoundBackground soundBackground;
        private Resolution[] resolutions;

        private const float MinSensitivity = 0.1f;
        private const float MaxSensitivity = 30.0f;

        public GameObject SettingsPanelUI
        {
            get { return settingsPanel; }
            set { settingsPanel = value; }
        }

        public ActiveState SettingsState
        {
            get { return settingsState; }
            set { settingsState = value; }
        }

        public Scrollbar CamSensitivityScrollbar
        {
            get { return camSensitivityScrollbar; }
            set { camSensitivityScrollbar = value; }
        }

        public Scrollbar VolumeScrollBar
        {
            get { return volumeScrollbar; }
            set { volumeScrollbar = value; }
        }

        public Dropdown ResolutionDropdown
        {
            get { return resolutionDropdown; }
            set { resolutionDropdown = value; }
        }

        public Dropdown QualityDropdown
        {
            get { return qualityDropdown; }
            set { qualityDropdown = value; }
        }

        public CameraController CameraController
        {
            get { return cameraController; }
            private set { cameraController = value; }
        }

        public SoundBackground SoundBackground
        {
            get { return soundBackground; }
            private set { soundBackground = value; }
        }

        public Resolution[] Resolutions
        {
            get { return resolutions; }
            set { resolutions = value; }
        }

        private void Start()
        {
            CameraController = FindAnyObjectByType<CameraController>();
            SoundBackground = FindAnyObjectByType<SoundBackground>();

            SettingsPanelUI.SetActive(false);
            SettingsState = ActiveState.OFF;

            CamSensitivityScrollbar.value = Mathf.InverseLerp(MinSensitivity, MaxSensitivity, CameraController.GetSensitivity());
            AdjustCameraSensitivity();
            AdjustSoundInGame();    
            InitializeResolutionDropdown();
        }

        private void InitializeResolutionDropdown()
        {
            ResolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            Resolutions = Screen.resolutions;
            int currentResolutionIndex = 0;

            for (int i = 0; i < Resolutions.Length; i++)
            {
                string option = $"{Resolutions[i].width} x {Resolutions[i].height}";
                options.Add(option);
                if (Resolutions[i].width == Screen.currentResolution.width && Resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            ResolutionDropdown.AddOptions(options);
            ResolutionDropdown.value = currentResolutionIndex;
            ResolutionDropdown.RefreshShownValue();

            ResolutionDropdown.onValueChanged.AddListener(SetResolution);

            SetResolution(currentResolutionIndex);
            LoadSettings(currentResolutionIndex);
        }

        public void ToggleShopPanel()
        {
            if (!IsShopActive())
            {
                SetShopActive(true);
                SettingsState = ActiveState.ON;
            }
            else
            {
                SetShopActive(false);
                SettingsState = ActiveState.OFF;
            }
        }

        public bool IsShopActive()
        {
            return SettingsState == ActiveState.ON;
        }

        public void SetShopActive(bool isActive)
        {
            SettingsPanelUI.SetActive(isActive);
        }

        public void AdjustCameraSensitivity()
        {
            CamSensitivityValueText.text = CamSensitivityScrollbar.value.ToString();
            float scaledSensitivity = Mathf.Lerp(MinSensitivity, MaxSensitivity, CamSensitivityScrollbar.value);
            CameraController.AdjustSensitivity(scaledSensitivity);
        }

        public void AdjustSoundInGame()
        {
            VolumeValueText.text = VolumeScrollBar.value.ToString();
            SoundBackground.ChangeVolume(VolumeScrollBar.value);
            
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = Resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetFloat("CamSensitivityScrollbarValue", CamSensitivityScrollbar.value);
            PlayerPrefs.SetFloat("VolumeScrollBarValue", VolumeScrollBar.value);
            PlayerPrefs.SetInt("QualitySettingsPreference", QualityDropdown.value);
            PlayerPrefs.SetInt("ResolutionSettingsPreference", ResolutionDropdown.value);
            PlayerPrefs.SetInt("FullScreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        }

        public void LoadSettings(int currentResolutionIndex)
        {
            CamSensitivityScrollbar.value = PlayerPrefs.GetFloat("CamSensitivityScrollbarValue", 0.5f);
            VolumeScrollBar.value = PlayerPrefs.GetFloat("VolumeScrollBarValue", 0.7f);
            if (PlayerPrefs.HasKey("QualitySettingsPreference"))
            {
                QualityDropdown.value = PlayerPrefs.GetInt("QualitySettingsPreference");
            }
            else
            {
                QualityDropdown.value = 3;
            }

            if (PlayerPrefs.HasKey("ResolutionSettingsPreference"))
            {
                ResolutionDropdown.value = PlayerPrefs.GetInt("ResolutionSettingsPreference");
            }
            else
            {
                ResolutionDropdown.value = currentResolutionIndex;
            }

            if (PlayerPrefs.HasKey("FullScreenPreference"))
            {
                Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullScreenPreference"));
            }
            else
            {
                Screen.fullScreen = true;
            }
        }
    }
}
