using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Modals.MailSignet
{
    public class Mail : UIView
    {
        private Label mailLabel;
        private string mailSubject;

        public Mail(VisualElement root, string text)
        {
            mailSubject = text;
            Initialize(root);
        }

        protected override void SetVisualElements()
        {
            mailLabel = Root.Q<Label>("mail-subject");
            
            if (mailLabel != null)
            {
                mailLabel.text = mailSubject;
            }
        }
        
        public override void Dispose()
        {
        }
    }
}
