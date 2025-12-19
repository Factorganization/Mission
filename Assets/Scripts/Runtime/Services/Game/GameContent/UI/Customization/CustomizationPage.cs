using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.Customization
{
    public class CustomizationPage : UIParent
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
            //_animator.Play("OpenCustomization");
        }

        #endregion

        #region Fields

        [SerializeField] private Button _closeButton;
        [SerializeField] private Animation _animator;

        #endregion
    }
}