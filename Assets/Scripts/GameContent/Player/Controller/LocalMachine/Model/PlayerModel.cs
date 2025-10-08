using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.Model
{
    public sealed class PlayerModel
    {
        #region constuctors

        public PlayerModel(PlayerDataSo data, Rigidbody rb, Transform graph, Transform cam, Animator animator)
        {
            this.data = data;
            this.rb = rb;
            this.graph = graph;
            this.cam = cam;
            this.animator = animator;
        }

        #endregion

        #region fields

        #region logics

        public PlayerDataSo data;
        
        public Rigidbody rb;

        public Transform cam;

        #endregion

        #region graphs

        public Transform graph;
                
        public Animator animator;

        #endregion

        #endregion
    }
}