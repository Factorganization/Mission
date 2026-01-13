using Runtime.Service;
using Runtime.Services.Game.GameContent.UI.PauseMenu;
using Runtime.Services.Game.GameSystems;
using Runtime.Services.Scene;

namespace Runtime.Services.Game.GameContent.UI.GameUI
{
    public class GameUIMgr : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
        
        }

        public async void ReturnToMainMenu()
        {
            var s = ServiceLocator.Instance.Get<SceneService>();
            await s.LoadSceneGroup(0);
        }
        
        public void RestartLevel()
        {
            GameManager.Instance.ReloadScene();
        }
        
        public void GameOver()
        {
            // Show end game UI
            _gameOverUI.SetActive(true);
        }
        
        public void WinGame()
        {
            // Show win game UI
            _winUI.SetActive(true);
        }

        #endregion

        #region Fields

        [SerializeField] private GameObject _gameOverUI;
        [SerializeField] private GameObject _winUI;
        [SerializeField] private QuestPage _questPage;

        public QuestPage QuestPage => _questPage;
        
        #endregion
    }
}