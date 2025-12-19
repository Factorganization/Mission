using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace Runtime.Services.GameService.GameContent.UI
{
    public class Settings : UIParent
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_closeSettingsButton != null)
                _closeSettingsButton.onClick.AddListener(Hide);
            if (_fullscreenToggle != null)
                _fullscreenToggle.onValueChanged.AddListener(ToggleFullscreen);
            if (_screenSizeDropdown != null)
                InitScreenSizeDropdown();
            if (_videoQualityDropdown != null)
                InitVideoQualityDropdown();
        }
        
        private void InitScreenSizeDropdown()
        {
            _screenSizeDropdown.ClearOptions();
            var options = new List<string>();
            foreach (var res in Screen.resolutions)
            {
                options.Add(res.width + " x " + res.height);
            }
            _screenSizeDropdown.AddOptions(options);
            _screenSizeDropdown.onValueChanged.AddListener(ScreenSizeChanged);
        }
        
        private void InitVideoQualityDropdown()
        {
            _videoQualityDropdown.ClearOptions();
            var options = new List<string>();
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                options.Add(QualitySettings.names[i]);
            }
            _videoQualityDropdown.AddOptions(options);
            _videoQualityDropdown.onValueChanged.AddListener(VideoQualityChanged);
        }
        
        private void ScreenSizeChanged(int index)
        {
            var res = Screen.resolutions[index];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }
        
        private void VideoQualityChanged(int index)
        {
            QualitySettings.SetQualityLevel(index);
        }
        
        private void ToggleFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        #endregion

        #region Fields
        
        [SerializeField] private Button _closeSettingsButton;
        [SerializeField] private TMP_Dropdown _videoQualityDropdown;
        [SerializeField] private TMP_Dropdown _screenSizeDropdown;
        [SerializeField] private Toggle _fullscreenToggle, _sfwModeToggle, _noUIToggle;
        
        #endregion
    }
}