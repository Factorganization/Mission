using Runtime.Service;
using Runtime.Services.Cursor;
using Runtime.Services.Scene;

namespace Runtime.Services.Game.GameSystems
{
    public class GameManager : MonoBehaviour
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

        #endregion

        #region fields

        [SerializeField] private string[] scenes;

        #endregion
    }
}