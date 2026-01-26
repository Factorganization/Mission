using Runtime.Service;
using Runtime.Services.Data;
using Runtime.Services.Game.GameContent.UI;
using Runtime.Services.Game.GameContent.UI.Customization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.MainMenu
{
    public class ConfirmPurchasePopup : UIParent
    {
        #region Functions

        private void Start()
        {
            _closeButton.onClick.AddListener(Show);
        }
        
        public override void Show()
        {
            base.Show();
            if (!_isOpen)
            {
                StartCoroutine(AnimationExtensions.Play(_animator, "OpenPurchasePopup", true, null));
                _purchaseText.text = "Are you sure ?";
                _isOpen = true;
                _bg.raycastTarget = true;
            }
            else
            {
                StartCoroutine(AnimationExtensions.Play(_animator, "ClosePurchasePopup", true, null));
                _isOpen = false;
                _bg.raycastTarget = false;
            }
        }

        #endregion

        #region Fields

        // Add any fields specific to the ConfirmPurchasePopup here
        [SerializeField] private Button _confirmButton, _closeButton;
        [SerializeField] private Animation _animator;
        [SerializeField] private Image _bg;
        [SerializeField] private TextMeshProUGUI _purchaseText;
        
        public Button ConfirmButton => _confirmButton;
        
        public TextMeshProUGUI PurchaseText => _purchaseText;

        #endregion
    }
}

