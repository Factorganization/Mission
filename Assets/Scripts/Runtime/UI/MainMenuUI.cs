using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_customizeApp != null)
                _customizeApp.onClick.AddListener(() => _customizeContainer.Show());
            
            if (_settingsApp != null)
                _settingsApp.onClick.AddListener(() => _settingsContainer.Show());
        
            if (_mailApp != null)
                _mailApp.onClick.AddListener(() => _mailContainer.Show());
            
            _mailContainer.Hide();
            _settingsContainer.Hide();
            //_customizeContainer.Hide();
        }

        #endregion

        #region Fields

        [SerializeField] private Button _mailApp, _settingsApp, _customizeApp, _quitApp;
    
        [SerializeField] private UIParent _mailContainer, _settingsContainer, _customizeContainer;
    
        #endregion
    }
}
