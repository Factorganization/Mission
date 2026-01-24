using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Controller.States;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.View
{
    [SelectionBase]
    public class PlayerStateMachine : MonoBehaviour
    {
        #region properties

        public GenericStateMachine StateMachine => _stateMachine;
        
        public PlayerModel PlayerModel => _playerModel;
        
        public bool IsVisible => _playerModel.isVisible;

        public Transform UiOverLayCam => referenceData.uiOverlayCam;

        #endregion

        #region methodes

        private void Awake()
        {
            _playerModel = new PlayerModel(dataSo, referenceData.rb, referenceData.col, referenceData.graph, referenceData.cam, referenceData.grab, referenceData.activeGrab, referenceData.animator, referenceData.possessParticles);
            _stateMachine = new GenericStateMachine(Enum.GetNames(typeof(ControllerState)).Length);

            var start = new StartState(_stateMachine, gameObject, _playerModel, ControllerState.Start);
            var idle = new IdleState(_stateMachine, gameObject, _playerModel, ControllerState.Idle);
            var move = new MoveState(_stateMachine, gameObject, _playerModel, ControllerState.Move);
            var jump = new JumpState(_stateMachine, gameObject, _playerModel, ControllerState.Jump);
            var fall = new FallState(_stateMachine, gameObject, _playerModel, ControllerState.Fall);
            var interact = new InteractState(_stateMachine, gameObject, _playerModel, ControllerState.Interact);
            var possess =  new PossessState(_stateMachine, gameObject, _playerModel, ControllerState.Possess);
            var menu = new MenuState(_stateMachine, gameObject, _playerModel, ControllerState.Menu);
            var locked = new LockedState(_stateMachine, gameObject, _playerModel, ControllerState.Locked);

            _stateMachine.SetCallBacks(SetId((int)ControllerState.Start), "start", start.OnInit, start.OnEnterState,
                start.OnUpdate, start.OnFixedUpdate, start.OnExitState, start.OnCoroutine);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Idle), "idle", idle.OnInit, idle.OnEnterState,
                idle.OnUpdate, idle.OnFixedUpdate, idle.OnExitState, idle.OnCoroutine);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Move), "move", move.OnInit, move.OnEnterState,
                move.OnUpdate, move.OnFixedUpdate, move.OnExitState, move.OnCoroutine);

            _stateMachine.SetCallBacks(SetId((int)ControllerState.Jump), "jump", jump.OnInit, jump.OnEnterState,
                jump.OnUpdate, jump.OnFixedUpdate, jump.OnExitState, jump.OnCoroutine);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Fall), "fall", fall.OnInit, fall.OnEnterState,
                fall.OnUpdate, fall.OnFixedUpdate, fall.OnExitState, fall.OnCoroutine);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Interact), "interact", interact.OnInit, interact.OnEnterState,
                interact.OnUpdate, interact.OnFixedUpdate, interact.OnExitState, interact.OnCoroutine);

            _stateMachine.SetCallBacks(SetId((int)ControllerState.Possess), "possess", possess.OnInit, possess.OnEnterState,
                possess.OnUpdate, possess.OnFixedUpdate, possess.OnExitState, possess.OnCoroutine);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Menu), "menu", menu.OnInit, menu.OnEnterState,
                menu.OnUpdate, menu.OnFixedUpdate, menu.OnExitState, menu.OnCoroutine);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Locked), "locked", locked.OnInit, locked.OnEnterState,
                locked.OnUpdate, locked.OnFixedUpdate, locked.OnExitState, locked.OnCoroutine);
            
            _stateMachine.InitMachine();
        }

        private void Start()
        {
            _stateMachine.StartMachine();
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdateMachine();
        }

        private static int SetId(int state)
        {
            var i = 0;
            while (state != 1)
            {
                i++;
                state >>= 1;
            }
            return i;
        }

        #endregion

        #region fields

        [SerializeField] private PlayerDataSo dataSo;

        [SerializeField] private ReferenceData referenceData;

        private GenericStateMachine _stateMachine;

        private PlayerModel _playerModel;

        [Serializable]
        private class ReferenceData
        {
            [SerializeField] internal Rigidbody rb;
            
            [SerializeField] internal Collider col;
                    
            [SerializeField] internal Transform cam;

            [SerializeField] internal Transform uiOverlayCam;
            
            [SerializeField] internal Transform graph;

            [SerializeField] internal Transform grab;

            [SerializeField] internal Transform activeGrab;
                    
            [SerializeField] internal Animator animator;
            
            [SerializeField] internal ParticleSystem possessParticles;
        }
        
        #endregion
    }
}