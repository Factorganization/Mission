using UnityEngine;
using UnityEngine.UI;

namespace Runtime.GameContent.UI
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
        }

        #endregion

        #region Fields
        
        [SerializeField] private Button _closeSettingsButton;
        
        #endregion
    }
}