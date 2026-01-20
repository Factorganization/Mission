using Runtime.Services.Game.GameContent.Actors.ActorControllers;
using Runtime.Services.Game.GameContent.Actors.ActorControllers.States;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Services.Game.GameContent.Actors.ActorModels.SO;
using Runtime.Services.Game.GameContent.Actors.ActorModules.AI;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.View;
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
            _aiModel = new AIModel(aiMovementDataSo ,aiDetectionDataSo ,refData.animator, refData.agent, rcOrigin, player, excludedLayers)
            {
                transform = transform
            };

            _stateMachine = new GenericStateMachine(Enum.GetNames(typeof(AIControllerState)).Length);

           var idle = new AIIdleState(_stateMachine, gameObject, _aiModel, AIControllerState.Idle);
           var move = new AIMoveState(_stateMachine, gameObject, _aiModel, AIControllerState.Move);
           var suspicious = new AISuspiciousState(_stateMachine, gameObject, _aiModel, AIControllerState.Suspicious);
           var chase = new AIChaseState(_stateMachine, gameObject, _aiModel, AIControllerState.Chase);
           
           _stateMachine.SetCallBacks(SetId((int)AIControllerState.Idle), "idle", idle.OnInit, idle.OnEnterState,
               idle.OnUpdate, idle.OnFixedUpdate, idle.OnExitState, idle.OnCoroutine);
           _stateMachine.SetCallBacks(SetId((int)AIControllerState.Move), "move", move.OnInit, move.OnEnterState,
               move.OnUpdate, move.OnFixedUpdate, move.OnExitState, move.OnCoroutine);
           _stateMachine.SetCallBacks(SetId((int)AIControllerState.Suspicious), "suspicious", suspicious.OnInit, suspicious.OnEnterState,
               suspicious.OnUpdate, suspicious.OnFixedUpdate, suspicious.OnExitState, suspicious.OnCoroutine);
           _stateMachine.SetCallBacks(SetId((int)AIControllerState.Chase), "chase", chase.OnInit, chase.OnEnterState,
               chase.OnUpdate, chase.OnFixedUpdate, chase.OnExitState, chase.OnCoroutine);
           
           //Set AI Position to first waypoint
           _aiModel._currentWaypoint = new mTransform();
           AIController.SetCurrentWaypoint(_aiModel, aiMovementDataSo.waypoints[0]);
            _stateMachine.InitMachine();
        }

        private void Start()
        {
            UpdateMovementData();
            
            _stateMachine.StartMachine();
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
            
            //Display Player
            transform.position = _aiModel.transform.position;
            transform.rotation = _aiModel.transform.rotation;
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
        
        //To move in model constructor
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
        

        [Header("Detection")]
        [SerializeField] private AIDetectionDataSo aiDetectionDataSo;
        [SerializeField] private Transform rcOrigin;
        [SerializeField] private LayerMask excludedLayers;
        [SerializeField] private PlayerStateMachine player;
        
        [SerializeField]
        private References refData;
        
        private AIModel _aiModel; 
        private GenericStateMachine _stateMachine; 
        
        [Serializable]
        private struct References
        {
            [SerializeField] internal NavMeshAgent agent;
            [SerializeField] internal Animator animator;
        }
        #endregion
    }
    
    public class mTransform
    {
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 scale = Vector3.one;
    }
    
}


