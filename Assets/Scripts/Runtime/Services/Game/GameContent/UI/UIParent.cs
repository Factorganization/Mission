namespace Runtime.Services.Game.GameContent.UI
{
    public class UIParent : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            _isOpen = false;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
    
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            _isOpen = false;
        }
        #endregion

        #region Fields

        [SerializeField] protected bool _isOpen;

        #endregion
    }
}