using UI;
using UnityEngine.UIElements;

namespace UI.Modals.MailPage
{
    public class MailPage : UIView
    {
        private Button _closeButton;
        
        public MailPage(VisualElement root) : base(root)
        {
            _hideOnAwake = true;
            _isOverlay = true;
            Initialize(root);
        }
        
        public override void Dispose()
        {
            UnregisterButtonCallbacks();   
        }
        
        protected override void RegisterButtonCallbacks()
        {
            _closeButton.RegisterCallback<ClickEvent>(Hide);
        }
        
        private void UnregisterButtonCallbacks()
        {
            _closeButton.UnregisterCallback<ClickEvent>(Hide);
        }
        
        protected override void SetVisualElements()
        {
            _closeButton = Root.Q<Button>("close-button");
        }
        
        private void Hide(ClickEvent clickEvent)
        {
            Hide();
        }
        
    }

}

