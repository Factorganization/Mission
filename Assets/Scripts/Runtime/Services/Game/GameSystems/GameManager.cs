using Runtime.Service;
using Runtime.Services.Cursor;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.View;
using Runtime.Services.Scene;

namespace Runtime.Services.Game.GameSystems
{
    public class GameManager : MonoBehaviour
    {
        #region properties

        public PlayerStateMachine Player => player;

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

        public void EndGame()
        {
            // Show end game UI
        }
        
        #endregion

        #region fields

        [SerializeField] private PlayerStateMachine player;

        #endregion
    }
}