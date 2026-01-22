using Runtime.Services.Game.GameContent.UI;
using UnityEngine;

namespace Runtime.Services.Game.GameContent.UI.GameUI
{
    public class WinScreen : UIParent
    {
        #region Functions
        
        public override void Show()
        {
            StartCoroutine(AnimationExtensions.Play(_winScreenAnimator, "OpenWinScreen", false, () => base.Show()));
        }

        #endregion
        
        #region Fields
        
        [SerializeField] private Animation _winScreenAnimator;
        
        #endregion
        
    }
}