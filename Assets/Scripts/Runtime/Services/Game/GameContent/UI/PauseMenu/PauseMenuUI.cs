using Runtime.Service;
using Runtime.Services.Cursor;
using Runtime.Services.Scene;
using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.PauseMenu
{
    public class PauseMenuUI : UIParent
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            //Hide();
            if (_resumeButton != null)
                _resumeButton.onClick.AddListener(Hide);
            if (_settingsButton != null && _settingsUI != null)
                _settingsButton.onClick.AddListener(() => _settingsUI.Show());
            if (_quitButton != null)
                _quitButton.onClick.AddListener(ReturnToMainMenu);
        }

        public void OpenPauseMenu()
        {
            Show();
            Time.timeScale = 0f;
        }
        
        private async void ReturnToMainMenu()
        {
            ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            await ServiceLocator.Instance.Get<SceneService>().LoadSceneGroup(0);
        }

        public override void Hide()
        {
            base.Hide();
            Time.timeScale = 1f;
        }
        
        #endregion
        
        #region Fields
        
        [SerializeField] private Button _resumeButton, _settingsButton, _quitButton;
        [SerializeField] private Settings _settingsUI;
        [SerializeField] private QuestPage _questPage;
        
        #endregion
    }
}