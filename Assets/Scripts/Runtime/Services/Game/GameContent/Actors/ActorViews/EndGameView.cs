using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Controller;
using Runtime.Services.Game.GameSystems;
using Vector3 = UnityEngine.Vector3;

namespace Runtime.Services.Game.GameContent.Actors.ActorViews
{
    public class EndGameView : ActorView
    {
        #region methodes

        private void Update()
        {
            if (!_active)
                return;

            var p = GameManager.Instance.Player;
            p.PlayerModel.SetEndLevel(Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position) <= 2f);
        }

        public void OnActivate()
        {
            graph.SetActive(true);
            _active = true;
        }
        
        //TODO il é dan la room au dépar
        //mé il  é desactivé
        //if distance courte
        //peu appueyer sur interact
        //lui il sactiv kan missions done
        //MissionManager.ChackAndgema

        #endregion

        #region fields

        [SerializeField] private GameObject graph;

        private bool _active;

        #endregion
    }
}