using UnityEngine.UI;

namespace Runtime.Services.SceneService.SceneSystems
{
    public class LoadingScreen : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            barFill.fillAmount = 0f;
        }

        #endregion
        
        #region Fields

        [SerializeField] private Image barFill;

        #endregion
    }
}