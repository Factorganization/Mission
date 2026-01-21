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
            _missionTargetPosition = new Vector2(-363.13f + i * 728.13f, 0);
        }

        #endregion

        #region Fields

        [SerializeField] private Image holder;
        
        // Temporary quest data list for testing
        private bool _isOpen;
        
        private Vector2 _missionTargetPosition;

        #endregion
    }
}