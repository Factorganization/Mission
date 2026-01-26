using Runtime.Services.Audio;
using Runtime.Services.Cursor;
using Runtime.Services.Data;
using Runtime.Services.Game.GameContent.UI.PauseMenu;
using Runtime.Services.Game.GameSystems;
using Runtime.Services.Scene;
using TMPro;

namespace Runtime.Services.Game.GameContent.UI.GameUI
{
    public class GameUIMgr : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            _gameOverUI.Hide();
            _winUI.Hide();
            UpdateDevilDollars();
            UpdateMalicePoints();
        }

        public async void ReturnToMainMenu()
        {
            ElementManager.Element.TempMalice = 0;
            MissionManager.Manager.TempDD = 0;
            ServiceLocator.Instance.Get<AudioService>().StopMusicSmooth();
            Time.timeScale = 1f;
            ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            await ServiceLocator.Instance.Get<SceneService>().LoadSceneGroup(0);
        }

        public async void ReturnAfterWin()
        {
            ServiceLocator.Instance.Get<AudioService>().StopMusicSmooth();
            Time.timeScale = 1f;
            ServiceLocator.Instance.Get<DataService>().AddMoney(MissionManager.Manager.TempDD);
            ServiceLocator.Instance.Get<DataService>().AddMalicePoints(ElementManager.Element.TempMalice);
            ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            await ServiceLocator.Instance.Get<SceneService>().LoadSceneGroup(0);
        }
        
        public void RestartLevel()
        {
            ElementManager.Element.TempMalice = 0;
            MissionManager.Manager.TempDD = 0;
            GameManager.Instance.ReloadScene();
        }
        
        public void GameOver()
        {
            // Show end game UI
            _gameOverUI.Show();
            ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            Time.timeScale = 0f;
        }
        
        public void WinGame()
        {
            // Show win game UI
            _winUI.Show();
            ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            Time.timeScale = 0f;
        }

        public void UpdateDevilDollars()
        {
            _devildollarsText.text = MissionManager.Manager.TempDD.ToString();
        }
        
        public void UpdateMalicePoints()
        {
            _malicePointsText.text = ElementManager.Element.TempMalice.ToString();
        }

        public void SetMissionPos(int i)
        {
            QuestPage.SetMissionPos(i);
        }

        #endregion

        #region Fields

        [SerializeField] private LoseScreen _gameOverUI;
        [SerializeField] private WinScreen _winUI;
        [SerializeField] private QuestPage _questPage;
        [SerializeField] private PauseMenuUI _pauseMenuUI;
        [SerializeField] private TextMeshProUGUI _devildollarsText, _malicePointsText;

        public QuestPage QuestPage => _questPage;
        public PauseMenuUI PauseMenuUI => _pauseMenuUI;
        
        #endregion
    }
}