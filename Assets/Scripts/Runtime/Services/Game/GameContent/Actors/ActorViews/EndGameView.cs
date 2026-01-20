using Runtime.Services.Game.GameContent.Actors.ActorControllers;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Controller;
using Runtime.Services.Game.GameSystems;
using Vector3 = UnityEngine.Vector3;

namespace Runtime.Services.Game.GameContent.Actors.ActorViews
{
    public class EndGameView : ActorView
    {
        #region methodes

        void Start()
        {
            graph.SetActive(false);
            _egModel = new EndGameModel();
        }

        private void Update()
        {
            if (!_egModel._active)
                return;

            var p = GameManager.Instance.Player;
            p.PlayerModel.SetEndLevel(Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position) <= 2f);
        }

        public void OnActivate()
        {
            graph.SetActive(true);
            EndGameController.SetActive(_egModel, true);
        }
        
        //peu appueyer sur interact

        #endregion

        #region fields

        [SerializeField] private GameObject graph;

        private EndGameModel _egModel; 

    

        #endregion
    }
}