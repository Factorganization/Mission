using System.Collections.Generic;

namespace Runtime.Services.Game.GameContent.UI.PauseMenu
{
    public class QuestPage : UIParent
    {
        #region Functions

        public void QuestOpen()
        {
            if (IsOpen) return;
            IsOpen = true;
        }

        public void QuestClose()
        {
            if (!IsOpen) return;
            IsOpen = false;
        }
        
        public override void Show()
        {
            base.Show();
            _animator.Play("QuestPageAppear");
        }

        public override void Hide()
        {
            _animator.Play("QuestPageDisappear");
        }

        #endregion

        #region Fields
        
        // Temporary quest data list for testing
        [SerializeField] private Animation _animator;
        private bool _isOpen;
        
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (_isOpen == value) return;
                _isOpen = value;
                if (_isOpen) Show();
                else Hide();
            }
        }

        #endregion
    }
}