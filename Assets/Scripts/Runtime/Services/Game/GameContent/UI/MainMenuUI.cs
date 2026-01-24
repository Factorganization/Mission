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

        private void Update()
        {
            _backgroundImage.uvRect = new Rect(_backgroundImage.uvRect.position + new Vector2(0.2f, 0) * Time.deltaTime, _backgroundImage.uvRect.size);
        }

        private void Initialize()
        {
            if (_customizeApp != null)
                _customizeApp.onClick.AddListener(() => _customizeContainer.Show());
            
            if (_settingsApp != null)
                _settingsApp.onClick.AddListener(() => _settingsContainer.Show());
        
            if (_mailApp != null)
                _mailApp.onClick.AddListener(() => _mailContainer.Show());
            
            if (_creditsApp != null)
                _creditsApp.onClick.AddListener(() => _creditsContainer.Show());
            
            if (_quitApp != null)
                _quitApp.onClick.AddListener(() => _quitContainer.Show());
            
            _mailContainer.gameObject.SetActive(false);
            _settingsContainer.gameObject.SetActive(false);
            _customizeContainer.gameObject.SetActive(false);
            _creditsContainer.gameObject.SetActive(false);
            _quitContainer.gameObject.SetActive(false);
        }

        #endregion

        #region Fields

        [SerializeField] private Button _mailApp, _settingsApp, _customizeApp, _creditsApp, _quitApp;
    
        [SerializeField] private UIParent _mailContainer, _settingsContainer, _customizeContainer, _creditsContainer, _quitContainer;
            
        [SerializeField] private RawImage _backgroundImage;
    
        #endregion
    }
}