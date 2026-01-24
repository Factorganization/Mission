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
            _closeButton.onClick.AddListener(Show);
        }

        public override void Show()
        {
            base.Show();
            if (!_isOpen)
            {
                StartCoroutine(AnimationExtensions.Play(_animator, "OpenCredits", true, null));
                _isOpen = true;
            }
            else
            {
                StartCoroutine(AnimationExtensions.Play(_animator, "CloseCredits", true, Hide));
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

