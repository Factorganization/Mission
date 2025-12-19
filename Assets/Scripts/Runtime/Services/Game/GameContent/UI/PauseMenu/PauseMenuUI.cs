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

        private void OpenPauseMenu()
        {
            Show();
            // Time.timeScale = 0f; // Pause the game
        }
        
        private void ReturnToMainMenu()
        {
            // SceneManager.LoadScene("MainMenu");
        }

        public override void Hide()
        {
            base.Hide();
            // Time.timeScale = 1f; // Resume the game
        }
        

        #endregion
        
        #region Fields
        
        [SerializeField] private Button _resumeButton, _settingsButton, _quitButton;
        [SerializeField] private Settings _settingsUI;
        [SerializeField] private QuestPage _questPage;
        
        #endregion
    }
}