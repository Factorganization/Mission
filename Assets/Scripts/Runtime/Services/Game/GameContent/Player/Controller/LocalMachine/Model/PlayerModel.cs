using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;

namespace Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model
{
    public sealed class PlayerModel
    {
        #region constuctors

        public PlayerModel(PlayerDataSo data, Rigidbody rb, Collider col, Transform graph, Transform cam, Transform grab, Transform activeGrab, Animator animator, ParticleSystem possessParticles)
        {
            this.data = data;
            this.rb = rb;
            this.col = col;
            this.graph = graph;
            this.cam = cam;
            this.grab = grab;
            this.activeGrab = activeGrab;
            this.animator = animator;
            this.possessParticles = possessParticles;
        }

        #endregion

        #region fields

        #region logics

        public readonly PlayerDataSo data;
        
        public readonly Rigidbody rb;

        public readonly Collider col;

        public readonly Transform cam;

        public readonly Transform grab;

        public readonly Transform activeGrab;

        public IPossessable possiblePossessedObject = null;
        
        public IPossessable currentPossessedObject = null;
        
        public IGrabbable possibleGrabbedObject = null;
        
        public IGrabbable currentGrabbedObject = null;
        
        public Vector2 inputDir;
        
        public Vector2 lookDir;

        public Vector3 targetLookDir;
        
        public Vector3 targetDir = Vector3.zero;

        public Vector3 tempLinearVelocity = Vector3.zero;

        public Vector3 acceleration = Vector3.zero;

        public float camYaw = 0;

        public float camPitch = 0;

        public float cYsD = 0;

        public float cPsD = 0;

        public float cVcY = 0;
        
        public float cVcP = 0;

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
        
        public bool canThrow = false;
        
        public bool canEndLevel = false;

        #endregion

        #region graphs

        public readonly ParticleSystem possessParticles;
        
        public readonly Transform graph;
                
        public readonly Animator animator;

        public Vector3 lastLookDir;
        
        public readonly int UpperBodyLayerID = Animator.StringToHash("UpperBody");
        
        public readonly int LowerBodyLayerID = Animator.StringToHash("LowerBody");
        
        public readonly int isWalking = Animator.StringToHash("isWalking");
        
        public readonly int isHolding = Animator.StringToHash("isHolding");
        
        public readonly int isInteracting = Animator.StringToHash("isInteracting");
        
        public readonly int @throw = Animator.StringToHash("throw");

        #endregion

        #endregion
    }
}