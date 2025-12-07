using Runtime.GameContent.Actors.ActorInterfaces;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Model
{
    public sealed class PlayerModel
    {
        #region constuctors

        public PlayerModel(PlayerDataSo data, Rigidbody rb, Transform graph, Transform cam, Transform grab, Animator animator)
        {
            this.data = data;
            this.rb = rb;
            this.graph = graph;
            this.cam = cam;
            this.grab = grab;
            this.animator = animator;
        }

        #endregion

        #region fields

        #region logics

        public readonly PlayerDataSo data;
        
        public readonly Rigidbody rb;

        public readonly Transform cam;

        public readonly Transform grab;

        public IPossessable currentPossessedObject = null;
        
        public IGrabbable currentGrabbedObject = null;
        
        public Vector2 inputDir;
        
        public Vector2 lookDir;

        public Vector3 targetLookDir;
        
        public Vector3 targetDir = Vector3.zero;

        public Vector3 tempLinearVelocity = Vector3.zero;

        public Vector3 acceleration = Vector3.zero;

        public float camYaw = 0;

        public float camPitch = 0;

        public float castAddLength = 0;

        public float vertVelocity = 0;

        public float currentMoveMultiplier = 1;
        
        public float currentHeightTarget;
        
        public float jumpBufferTime = 0;
        
        public float coyoteTime = 0;

        public float throwTimer = 0;
        
        public bool isVisible = true;
        
        public bool isDead = false;

        public bool isGrounded = true;
        
        public bool isCrouching = false;
        
        public bool isUsingMouse = false;

        #endregion

        #region graphs

        public Transform graph;
                
        public Animator animator;

        public Vector3 lastLookDir;

        #endregion

        #endregion
    }
}