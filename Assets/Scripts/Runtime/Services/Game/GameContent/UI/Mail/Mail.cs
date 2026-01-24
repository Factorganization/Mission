using TMPro;
using UnityEngine.Events;

namespace Runtime.Services.Game.GameContent.UI.Mail
{
    [Serializable]
    public class MailSelectedEvent : UnityEvent<MailLevel> { }

    public class Mail : MonoBehaviour
    {
        #region Functions
        
        public void Bind(MailLevel mailLevel)
        {
            _mailLevel = mailLevel;
            _mailSubject.text = mailLevel.Subject;
        }

        public void OnSelected()
        {
            OnMailSelected.Invoke(_mailLevel);
        }

		public void UnlockMail()
        {
			_mailLevel.isMailUnlocked = true;
			_mailLevel.isMailNew = true;
		}	

        #endregion

        #region Fields

        [SerializeField] private MailLevel _mailLevel;
        [SerializeField] private TextMeshProUGUI _mailSubject;
        
        public MailSelectedEvent OnMailSelected = new MailSelectedEvent();

        #endregion
    }
}