using Runtime.Services.Game.GameContent.UI;
using UnityEngine;

namespace Runtime.Services.Game.GameContent.UI.MainMenu
{
    public class QuitPopup : UIParent
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_closeButton != null)
                _closeButton.onClick.AddListener(() => Show());
            if (_confirmButton != null)
                _confirmButton.onClick.AddListener(Application.Quit);
        }

        public override void Show()
        {
            base.Show();
            if (!_isOpen)
            {
                StartCoroutine(AnimationExtensions.Play(_animator, "OpenQuit", true, null));
                _isOpen = true;
            }
            else
            {
                StartCoroutine(AnimationExtensions.Play(_animator, "CloseQuit", true, Hide));
            }
        }

        #endregion
        
        #region Fields
        
        [SerializeField] private Animation _animator;
        [SerializeField] private UIButton _closeButton, _confirmButton;
        
        #endregion
    }
}