using Runtime.Service;
using TMPro;

namespace Runtime.Services.Game.GameContent.UI
{
    public class Timer : MonoBehaviour
    {
        #region Functions
        
        void Update()
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;

                if (remainingTime < 0)
                {
                    remainingTime = 0;
                    ServiceLocator.Instance.Get<GameService>().GameManager.EndGame();
                }
            }
            
            int minutes = Mathf.FloorToInt(remainingTime / 60F);
            int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        
        #endregion
        
        #region Fields
        
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private float remainingTime;
        
        public float RemainingTime => remainingTime;
        
        #endregion
    }
}

