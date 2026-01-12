using Runtime.Service;
using Runtime.Services.Cursor;
using Runtime.Services.Scene;
using Runtime.Utils.Singleton;

namespace Runtime.Services.Game.GameSystems
{
    public class GameManager : Singleton<GameManager>
    {
        #region methodes

        private void Start()
        {
            ServiceLocator.Instance.Get<CursorService>().SetActive(false);
        }

        public async void ReloadScene()
        {
			var s = ServiceLocator.Instance.Get<SceneService>();
            await s.LoadSceneGroup(s.CurrentActiveSceneGroup);
        }

        public void EndGame()
        {
            // Show end game UI
        }
        
        #endregion

        #region fields

        [SerializeField] private string[] scenes;
        
        #endregion
    }
}