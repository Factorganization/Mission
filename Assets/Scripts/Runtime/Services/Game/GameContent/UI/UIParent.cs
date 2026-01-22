namespace Runtime.Services.Game.GameContent.UI
{
    public class UIParent : MonoBehaviour
    {
        #region Functions
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
    
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region Fields

        [SerializeField] private bool _isOpen;
        
        public bool isOpen => _isOpen;

        #endregion
    }
}