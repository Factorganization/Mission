using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.MainMenu
{
    public class Credits : UIParent
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _closeButton.onClick.AddListener(() => Hide());
        }

        public override void Show()
        {
            base.Show();
            //_animator.Play("OpenCredits");
        }
        
        
        public override void Hide()
        {
            base.Hide();
            //_animator.Play("CloseCredits");
        }

        #endregion

        #region Fields

        [SerializeField] private Button _closeButton;
        [SerializeField] private Animation _animator;

        #endregion
    }
}

