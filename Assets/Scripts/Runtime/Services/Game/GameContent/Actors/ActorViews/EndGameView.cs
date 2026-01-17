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
        }

        private void Update()
        {
            if (!_EGModel._active)
                return;

            var p = GameManager.Instance.Player;
            p.PlayerModel.SetEndLevel(Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position) <= 2f);
        }

        public void OnActivate()
        {
            graph.SetActive(true);
            EndGameController.SetActive(_EGModel, true);
        }
        
        //peu appueyer sur interact

        #endregion

        #region fields

        [SerializeField] private GameObject graph;

        private EndGameModel _EGModel; 

    

        #endregion
    }
}