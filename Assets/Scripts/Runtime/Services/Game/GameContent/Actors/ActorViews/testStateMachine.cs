using Runtime.Services.Game.GameContent.Actors.ActorControllers;
using Runtime.Services.Game.GameContent.Actors.ActorControllers.AIControllerStates;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Services.Game.GameContent.Actors.ActorModels.SO;
using Runtime.Services.Game.GameContent.Actors.ActorModules.AI;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorViews
{
    public class testStateMachine : MonoBehaviour
    {

        #region methodes
        
        private void Awake()
        {
            _aiModel = new AIModel(aiMovementDataSo);
            _aiModel.transform = transform;
        }

        private void Start()
        {
            _stateMachine = new GenericStateMachine(1);
            
            //var idle = new AIIdleState(_stateMachine, gameObject);
            var move = new AIMoveState(_stateMachine, gameObject);

            //_stateMachine.SetCallBacks(0, "AIIdleState", idle.OnInit, idle.OnEnterState, idle.OnUpdate, idle.OnFixedUpdate, idle.OnExitState, null);
            _stateMachine.SetCallBacks(0, "AIMoveState", move.OnInit, move.OnEnterState, move.OnUpdate, move.OnFixedUpdate, move.OnExitState, null);
            
            _stateMachine.InitMachine();
            _stateMachine.StartMachine();

            _aiModel._currentWaypoint = new mTransform();
            AIController.SetCurrentWaypoint(_aiModel, aiMovementDataSo.waypoints[0]);
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
        }
        
        private void FixedUpdate()
        {
            _stateMachine.FixedUpdateMachine();
        }

        #endregion

        #region fields
        private GenericStateMachine _stateMachine;
        
        [SerializeField] private AIMovementDataSo aiMovementDataSo;
        [SerializeField] private AIDetection aiDetection;
        [SerializeField] private UnityEngine.AI.NavMeshAgent agent;

        private int _index;
        private AIModel _aiModel;
        private float _updateAgentTimer;
        private float _aiUpdateSetPositionDelay = 0.2f;

        #endregion
    }

    public class mTransform
    {
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 scale = Vector3.one;
    }
}