using System.Collections.Generic;
using UI.Modals.MailSignet;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Modals.MailPage
{
    public class MailPage : UIView
    {
        #region Functions
        public MailPage(VisualElement root, MailLevel[] mailSO)
        {
            _hideOnAwake = true;
            _isOverlay = true;
            
            _mailLevel = Resources.Load<VisualTreeAsset>("Mail");
            Debug.Log(_mailLevel);
            _mailSO = mailSO;
            
            _popupLevel = new PopupLevel.PopupLevel(
                root.Q<VisualElement>("popup-level"));
            
            Initialize(root);
        }
        
        public override void Dispose()
        {
            UnregisterButtonCallbacks();   
            _mails.Clear();
            _scrollView.Clear();
        }
        
        protected override void RegisterButtonCallbacks()
        {
            _closeButton.RegisterCallback<ClickEvent>(Hide);
            _scrollView.RegisterCallback<PointerLeaveEvent>(HandleLoseMouse);
        }
        
        private void UnregisterButtonCallbacks()
        {
            foreach (Mail mail in _mails)
            {
                mail.Root.UnregisterCallback<ClickEvent, MailLevel>(HandleUIClick);
            }
            
            _closeButton.UnregisterCallback<ClickEvent>(Hide);
        }
        
        protected override void SetVisualElements()
        {
            _mails = new List<Mail>(_mailSO.Length);
            _scrollView = Root.Q<ScrollView>();
            _closeButton = Root.Q<Button>("close-button");

            foreach (MailLevel mail in _mailSO)
            {
                VisualElement uiMailRoot = new();
                _mailLevel.CloneTree(uiMailRoot);
                
                Mail mailElement = new Mail(uiMailRoot, mail.Subject);
                
                mailElement.Root.RegisterCallback<ClickEvent, MailLevel>(HandleUIClick, mail);
                mailElement.Root.RegisterCallback<MouseDownEvent>(HandleMouseDownDrag);
                mailElement.Root.RegisterCallback<MouseUpEvent>(HandleMouseUpDrag);
                mailElement.Root.RegisterCallback<MouseMoveEvent>(HandleMouseMove);
                
                _scrollView.Add(mailElement.Root);
                _mails.Add(mailElement);
            }
            
            _scrollView.ScrollTo(_mails[0].Root);
        }

        private void HandleUIClick(ClickEvent clickEvent, MailLevel mailLevel)
        {
            OnMailSelected?.Invoke(mailLevel);
        }
        
        private void HandleMouseMove(MouseMoveEvent evt)
        {
            if (!_isDragging) return;

            // ideally we'd use some kind of velocity on this, but this gets us started
            _scrollView.scrollOffset -= evt.mouseDelta;
        }
        
        private void Hide(ClickEvent clickEvent)
        {
            Hide();
            Root.pickingMode = PickingMode.Ignore;
        }

        public void OpenPopupLevel(string title, string sender, string description)
        {
            _popupLevel.Show();
            _popupLevel.SetMissionDetails(title, sender, description);
        }
        
        private void HandleLoseMouse(PointerLeaveEvent evt) => _isDragging = false;
        private void HandleMouseUpDrag(MouseUpEvent evt) => _isDragging = false;
        private void HandleMouseDownDrag(MouseDownEvent evt) => _isDragging = true;
        
        #endregion

        #region Fields
        
        private VisualTreeAsset _mailLevel;
        private Button _closeButton;
        private ScrollView _scrollView;
        private MailLevel[] _mailSO;
        private PopupLevel.PopupLevel _popupLevel;
        private bool _isDragging;

        public PopupLevel.PopupLevel PopupLevel => _popupLevel;
        public delegate void MailSelectedEvent(MailLevel mail);
        public event MailSelectedEvent OnMailSelected;
        
        private List<Mail> _mails = new List<Mail>();
        
        #endregion
        
    }

}

