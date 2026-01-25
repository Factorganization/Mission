using Runtime.Services.Game.GameSystems;
using TMPro;

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
            
            _ddText.text = MissionManager.Manager.TempDD.ToString();
            _malicePointsText.text = ElementManager.Element.TempMalice.ToString();
        }

        #endregion
        
        #region Fields
        
        [SerializeField] private Animation _winScreenAnimator;
        [SerializeField] private TextMeshProUGUI _ddText, _malicePointsText;
        
        #endregion
        
    }
}