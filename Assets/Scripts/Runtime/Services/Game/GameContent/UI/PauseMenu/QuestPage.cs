using Runtime.Services.Audio;
using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.PauseMenu
{
    public class QuestPage : UIParent
    {
        #region Functions

        private void FixedUpdate()
        {
            holder.rectTransform.anchoredPosition += Math.EasingFunction.SimpleQuadraticEase.V2SimpleQuadraticEaseOut(holder.rectTransform.anchoredPosition, _missionTargetPosition, 0.5f);
        }

        public void SetMissionPos(int i)
        {
            _missionTargetPosition = new Vector2(-347.13f + i * 728.13f, 0);


            if (_currentI != i)
            {
                _currentI = i;
                var a = ServiceLocator.Instance.Get<AudioService>();
                a.PlayOneShot(a.Atlas.sfx.ui.uiPaperOpen, default);
            }
        }

        #endregion

        #region Fields

        [SerializeField] private Image holder;
        
        private Vector2 _missionTargetPosition;

        private int _currentI;

        #endregion
    }
}