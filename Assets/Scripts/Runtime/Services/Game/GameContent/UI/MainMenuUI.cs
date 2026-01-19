using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
            Time.timeScale = 1.0f;
        }

        private void Initialize()
        {
            if (_customizeApp != null)
                _customizeApp.onClick.AddListener(() => _customizeContainer.Show());
            
            if (_settingsApp != null)
                _settingsApp.onClick.AddListener(() => _settingsContainer.Show());
        
            if (_mailApp != null)
                _mailApp.onClick.AddListener(() => _mailContainer.Show());
            
            if (_quitApp != null)
                _quitApp.onClick.AddListener(Application.Quit);
            
            _mailContainer.gameObject.SetActive(false);
            _settingsContainer.gameObject.SetActive(false);
            _customizeContainer.gameObject.SetActive(false);
        }

        #endregion

        #region Fields

        [SerializeField] private Button _mailApp, _settingsApp, _customizeApp, _quitApp;
    
        [SerializeField] private UIParent _mailContainer, _settingsContainer, _customizeContainer;
    
        #endregion
    }
}