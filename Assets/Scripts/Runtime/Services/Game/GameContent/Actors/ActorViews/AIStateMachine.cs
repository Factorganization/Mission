using Runtime.Services.Game.GameContent.Actors.ActorControllers;
using Runtime.Services.Game.GameContent.Actors.ActorControllers.States;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Services.Game.GameContent.Actors.ActorModels.SO;
using Shared.Utils.BaseMachine;
using UnityEngine.AI;

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
            _aiModel = new AIModel(aiMovementDataSo, refData.animator, refData.agent);
            _aiModel.transform = transform; 
            
           _stateMachine = new GenericStateMachine(Enum.GetNames(typeof(AIControllerState)).Length);

           var idle = new AIIdleState(_stateMachine, gameObject, _aiModel, AIControllerState.Idle);
           var move = new AIMoveState(_stateMachine, gameObject, _aiModel, AIControllerState.Move);
           var suspicious = new AISuspiciousState(_stateMachine, gameObject, _aiModel, AIControllerState.Suspicious);
           
           _stateMachine.SetCallBacks(SetId((int)AIControllerState.Idle), "idle", idle.OnInit, idle.OnEnterState,
               idle.OnUpdate, idle.OnFixedUpdate, idle.OnExitState, idle.OnCoroutine);
           _stateMachine.SetCallBacks(SetId((int)AIControllerState.Move), "move", move.OnInit, move.OnEnterState,
               move.OnUpdate, move.OnFixedUpdate, move.OnExitState, move.OnCoroutine);
           _stateMachine.SetCallBacks(SetId((int)AIControllerState.Suspicious), "suspicious", suspicious.OnInit, suspicious.OnEnterState,
               suspicious.OnUpdate, suspicious.OnFixedUpdate, suspicious.OnExitState, suspicious.OnCoroutine);
           
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
        
        private void UpdateMovementData()
        {
            aiMovementDataSo.waypoints = new Vector3[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                aiMovementDataSo.waypoints[i] = waypoints[i].position;
            }
        }

        #endregion

        #region fields
        
        [Header("Movements")]
        [SerializeField] private Transform[]  waypoints;
        [SerializeField] private AIMovementDataSo aiMovementDataSo;

        [Header("")]
        [SerializeField]
        private References refData;
        
        private AIModel _aiModel; 
        private GenericStateMachine _stateMachine; 

        #endregion
        
        [Serializable]
        private struct References
        {
            [SerializeField] internal NavMeshAgent agent;
            [SerializeField] internal Animator animator; 

        }
        
    }
    
    public class mTransform
    {
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 scale = Vector3.one;
    }
    
}


