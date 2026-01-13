using System.Collections.Generic;

namespace Runtime.Services.Game.GameContent.UI.PauseMenu
{
    public class QuestPage : UIParent
    {
        #region Functions

        public void QuestOpenOrClose()
        {
            if (IsOpen)
            {
                Hide();
                IsOpen = false;
            }
            else
            {
                Show();
                IsOpen = true;
            }
        }
        
        public override void Show()
        {
            base.Show();
            _animator.Play("OpenQuestPage");
        }

        public override void Hide()
        {
            _animator.Play("CloseQuestPage");
        }

        #endregion

        #region Fields
        
        // Temporary quest data list for testing
        [SerializeField] private Animation _animator;
        
        public bool IsOpen { get; private set; }
        
        #endregion
    }
}