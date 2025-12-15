using UnityEngine.UI;

namespace Runtime.GameContent.UI.LoadingScreen
{
    public class LoadingScreen : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            // _barFill.fillAmount = 
        }

        private void Initialize()
        {
            _barFill.fillAmount = 0f;
        }

        #endregion
        
        #region Fields

        [SerializeField] private Image _barFill;

        #endregion
    }
}