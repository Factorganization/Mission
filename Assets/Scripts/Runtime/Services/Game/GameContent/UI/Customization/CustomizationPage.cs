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
            _closeButton.onClick.AddListener(Show);
        }

        public override void Show()
        {
            base.Show();
            if (!_isOpen)
            {
                StartCoroutine(AnimationExtensions.Play(_animator, "OpenCustomPage", true, null));
                _isOpen = true;
            }
            else
            {
                StartCoroutine(AnimationExtensions.Play(_animator, "CloseCustomPage", true, Hide));
                _isOpen = false;
            }
        }

        #endregion

        #region Fields

        [SerializeField] private Button _closeButton;
        [SerializeField] private Animation _animator;

        #endregion
    }
}