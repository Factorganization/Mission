using Runtime.Services.Cursor;
using Runtime.Services.Game.GameSystems;
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
            if (_resumeButton != null)
                _resumeButton.onClick.AddListener(Hide);
            if (_settingsButton != null && _settingsUI != null)
                _settingsButton.onClick.AddListener(() => _settingsUI.Show());
            if (_quitButton != null)
                _quitButton.onClick.AddListener(ReturnToMainMenu);
        }

        public void OpenPauseMenu()
        {
            if (!_isOpen)
            {
                base.Show();
            }

            if (_isOpen)
            {
                Hide();
                _isOpen = false;
                return;
            }
           
            StartCoroutine(AnimationExtensions.Play(_pauseMenuAnimator, "OpenPauseMenu", false, null));
            _isOpen = true;
            ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            Time.timeScale = 0f;

            var playerModel = GameManager.Instance.Player;
            
            playerModel.StateMachine.TrySwitchState(playerModel.PlayerModel.currentPossessedObject is not null ? "possess" : "idle", (int)playerModel.PlayerModel.data.activeStates);
        }
        
        private async void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            await ServiceLocator.Instance.Get<SceneService>().LoadSceneGroup(0);
        }

        public override void Hide()
        {
            StartCoroutine(AnimationExtensions.Play(_pauseMenuAnimator, "ClosePauseMenu", false, null));
            _isOpen = false;
            ServiceLocator.Instance.Get<CursorService>().SetActive(false);
            Time.timeScale = 1f;
            
            var playerModel = GameManager.Instance.Player;
            
            playerModel.StateMachine.TrySwitchState(playerModel.PlayerModel.currentPossessedObject is not null ? "possess" : "idle", (int)playerModel.PlayerModel.data.activeStates);
        }
        
        #endregion
        
        #region Fields
        
        [SerializeField] private Button _resumeButton, _settingsButton, _quitButton;
        [SerializeField] private Settings _settingsUI;
        [SerializeField] private Animation _pauseMenuAnimator;
        
        #endregion
    }
}