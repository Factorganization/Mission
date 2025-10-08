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
        
        public bool isDead = false;

        public bool isGrounded = true;
        
        public Vector2 inputDir;
        
        public Vector2 lookDir;
        
        public Vector3 targetDir = Vector3.zero;

        public Vector3 tempLinearVelocity = Vector3.zero;

        public Vector3 acceleration = Vector3.zero;

        public float camYaw = 0;

        public float camPitch = 0;

        public float castAddLength = 0;

        public float vertVelocity = 0;

        public float currentMoveMultiplier = 1;
        
        public float currentHeightTarget;

        #endregion

        #region graphs

        public Transform graph;
                
        public Animator animator;

        public Vector3 lastLookDir;

        #endregion

        #endregion
    }
}