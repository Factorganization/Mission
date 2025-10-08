using System;
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
            Root = root ?? throw new ArgumentNullException(nameof(root));
            SetVisualElements();
            RegisterButtonCallbacks();
            if (_hideOnAwake)
            {
                HideMailPage();
            }
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

        public void ShowMailPage()
        {
            Root.AddToClassList("research-up");
        }

        public void HideMailPage()
        {
            Root.RemoveFromClassList("research-up");
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
                HideMailPage();
            }
        }
    }
}
