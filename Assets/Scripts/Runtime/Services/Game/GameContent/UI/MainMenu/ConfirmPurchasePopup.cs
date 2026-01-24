using Runtime.Service;
using Runtime.Services.Data;
using Runtime.Services.Game.GameContent.UI;
using Runtime.Services.Game.GameContent.UI.Customization;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.MainMenu
{
    public class ConfirmPurchasePopup : UIParent
    {
        #region Functions

        private void Start()
        {
            ServiceLocator.Instance.Get<DataService>().DevilDollars = 20;
            _closeButton.onClick.AddListener(Hide);
        }
        
        public override void Show()
        {
            base.Show();
            //StartCoroutine(AnimationExtensions.Play(_animator, "OpenConfirmPurchasePopup", true, null));
        }

        public override void Hide()
        {
            base.Hide();
            // StartCoroutine(AnimationExtensions.Play(_animator, "CloseConfirmPurchasePopup", true, null));
            _confirmButton.onClick.RemoveAllListeners();
        }

        #endregion

        #region Fields

        // Add any fields specific to the ConfirmPurchasePopup here
        [SerializeField] private Button _confirmButton, _closeButton;
        [SerializeField] private Animation _animator;
        
        public Button ConfirmButton => _confirmButton;

        #endregion
    }
}

