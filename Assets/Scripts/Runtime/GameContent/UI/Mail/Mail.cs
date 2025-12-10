using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MailSelectedEvent : UnityEvent<MailLevel> { }

namespace Runtime.GameContent.UI.Mail
{
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

        #endregion

        #region Fields

        [SerializeField] private MailLevel _mailLevel;
        [SerializeField] private TextMeshProUGUI _mailSubject;
        
        public MailSelectedEvent OnMailSelected = new MailSelectedEvent();

        #endregion
    }
}
