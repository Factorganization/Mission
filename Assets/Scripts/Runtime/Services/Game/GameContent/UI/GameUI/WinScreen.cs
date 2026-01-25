namespace Runtime.Services.Game.GameContent.UI.GameUI
{
    public class WinScreen : UIParent
    {
        #region Functions
        
        public override void Show()
        {
            if (!_isOpen)
            {
                base.Show();
            }
            
            //StartCoroutine(AnimationExtensions.Play(_winScreenAnimator, _winScreenAnimator.clip.name, false, null));
            _isOpen = true;
        }

        #endregion
        
        #region Fields
        
        [SerializeField] private Animation _winScreenAnimator;
        
        #endregion
        
    }
}