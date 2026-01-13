using Runtime.Service;
using Runtime.Services.Cursor;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.View;
using Runtime.Services.Game.GameContent.UI.GameUI;
using Runtime.Services.Scene;
using Runtime.Utils.Singleton;

namespace Runtime.Services.Game.GameSystems
{
    public class GameManager : Singleton<GameManager>
    {
        #region properties

        public PlayerStateMachine Player => player;
        public GameUIMgr GameUIMgr => gameUIMgr;

        #endregion
        
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
        
        #endregion

        #region fields

        [SerializeField] private PlayerStateMachine player;
        [SerializeField] private GameUIMgr gameUIMgr;
        #endregion
    }
}