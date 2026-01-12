using Runtime.Service;
using Runtime.Services.Game.GameSystems;

namespace Runtime.Services.Game
{
    public class GameService : AService
    {
        #region properties

        public GameManager GameManager => gameManager;
        
        public LevelGenerator LevelManager => levelManager;
        
        public ElementManager ElementManager => elementManager;
        
        public MissionManager MissionManager => missionManager;

        #endregion
        
        #region fields

        [SerializeField] private GameManager gameManager;
        
        [SerializeField] private LevelGenerator levelManager;
        
        [SerializeField] private ElementManager elementManager;
        
        [SerializeField] private MissionManager missionManager;

        #endregion
    }
}