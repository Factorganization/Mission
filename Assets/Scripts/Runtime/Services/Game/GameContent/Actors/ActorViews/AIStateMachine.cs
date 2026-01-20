using Runtime.Services.Game.GameContent.Actors.ActorControllers;
using Runtime.Services.Game.GameContent.Actors.ActorControllers.States;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Services.Game.GameContent.Actors.ActorModels.SO;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorViews
{
    [SelectionBase]
    public class AIStateMachine : MonoBehaviour
    {
        #region properties 

        public AIModel AIModel => _aiModel;

        #endregion

        #region methodes

        private void Awake()
        {
            _aiModel = new AIModel(aiMovementDataSo);
            _aiModel.transform = transform; 
            
           _stateMachine = new GenericStateMachine(Enum.GetNames(typeof(AIControllerState)).Length);

           var idle = new AIIdleState(_stateMachine, gameObject, _aiModel, AIControllerState.Idle);
           var move = new AIMoveState(_stateMachine, gameObject, _aiModel, AIControllerState.Move);
           
           _stateMachine.SetCallBacks(SetId((int)AIControllerState.Idle), "idle", idle.OnInit, idle.OnEnterState,
               idle.OnUpdate, idle.OnFixedUpdate, idle.OnExitState, idle.OnCoroutine);
           _stateMachine.SetCallBacks(SetId((int)AIControllerState.Move), "move", move.OnInit, move.OnEnterState,
               move.OnUpdate, move.OnFixedUpdate, move.OnExitState, move.OnCoroutine);

           //Set AI Position to first waypoint
           _aiModel._currentWaypoint = new mTransform();
           AIController.SetCurrentWaypoint(_aiModel, aiMovementDataSo.waypoints[0]);
        }

        private void Start()
        {
            _stateMachine.InitMachine();
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
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
        
        [SerializeField] private AIMovementDataSo aiMovementDataSo;

        private AIModel _aiModel; 
        private GenericStateMachine _stateMachine; 

        #endregion
    }
    
    public class mTransform
    {
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 scale = Vector3.one;
    }
}


