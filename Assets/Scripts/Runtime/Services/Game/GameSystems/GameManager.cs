using Runtime.Service;
using Runtime.Services.Cursor;

namespace Runtime.Services.Game.GameSystems
{
    public class GameManager : MonoBehaviour
    {
        #region methodes

        private void Start()
        {
            ServiceLocator.Instance.Get<CursorService>().SetActive(false);
        }

        public void ReloadScene()
        {
            
        }

        #endregion

        #region fields

        [SerializeField] private string[] scenes;

        #endregion
    }
}