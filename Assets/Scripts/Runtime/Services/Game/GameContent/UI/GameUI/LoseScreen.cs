using Runtime.Services.Game.GameContent.UI;
using UnityEngine;

namespace Runtime.Services.Game.GameContent.UI.GameUI
{
    public class LoseScreen : UIParent
    {
        #region Functions

        public override void Show()
        {
            StartCoroutine(AnimationExtensions.Play(_loseScreenAnimator, "OpenLoseScreen", false, () => base.Show()));
        }

        #endregion
        
        #region Fields
        
        [SerializeField] private Animation _loseScreenAnimator;
        
        #endregion
    }
}