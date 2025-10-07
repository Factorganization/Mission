using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UI.Modals.ResearchPopUp
{
    public class ResearchPopUp : UIView
    {
        private Button _mailButton;
        private MailPage.MailPage _mailPage;

        public ResearchPopUp(VisualElement root) : base(root)
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
            _mailButton.RegisterCallback<ClickEvent>(OpenMailPage);
        }
        
        protected override void SetVisualElements()
        {
            _mailButton = Root.Q<Button>("mail-button");
        }

        private void UnregisterButtonCallbacks()
        {
            _mailButton.UnregisterCallback<ClickEvent>(OpenMailPage);
        }

        private void Hide(ClickEvent clickEvent)
        {
            Hide();
        }
        
        public void SetMailPage(MailPage.MailPage mailPage)
        {
            _mailPage = mailPage;
        }

        private void OpenMailPage(ClickEvent clickEvent)
        {
            if (_mailPage != null)
            {
                _mailPage.Show();
            }
        }
    }
}
