using Runtime.Service;
using Runtime.Services.Cursor;
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
            Time.timeScale = 1f;
            ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            await ServiceLocator.Instance.Get<SceneService>().LoadSceneGroup(0);
        }
        
        public void RestartLevel()
        {
            GameManager.Instance.ReloadScene();
        }
        
        public void GameOver()
        {
            // Show end game UI
            ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            _gameOverUI.SetActive(true);
            Time.timeScale = 0f;
        }
        
        public void WinGame()
        {
            // Show win game UI
            ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            _winUI.SetActive(true);
            Time.timeScale = 0f;
        }

        public void SetMissionPos(int i)
        {
            QuestPage.SetMissionPos(i);
        }

        #endregion

        #region Fields

        [SerializeField] private GameObject _gameOverUI;
        [SerializeField] private GameObject _winUI;
        [SerializeField] private QuestPage _questPage;
        [SerializeField] private PauseMenuUI _pauseMenuUI;

        public QuestPage QuestPage => _questPage;
        public PauseMenuUI PauseMenuUI => _pauseMenuUI;
        
        #endregion
    }
}