using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
using Runtime.Services.Game.GameContent.Actors.ActorModels.SO;
using Runtime.Services.Game.GameContent.Actors.ActorModules.AI;
using Runtime.Services.Game.GameContent.Actors.ActorViews;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.View;
using UnityEngine.AI;

namespace Runtime.Services.Game.GameContent.Actors.ActorModels
{
    public class AIModel : ActorModel
    {
        #region methodes
        public AIModel(AIMovementDataSo movementDataSo,AIDetectionDataSo detectionSo ,Animator animator, NavMeshAgent agent, Transform rcOrigin, PlayerStateMachine player, LayerMask excludedLayers, float repairTime, Transform[] waypoints)
        {
            movementData =  movementDataSo;
            detectionData = detectionSo;
            _agentRef = agent;
            _animatorRef = animator; 
            _rcOrigin = rcOrigin;
            _player = player;
            _excludedLayers = excludedLayers;
            _repairTime = repairTime;
            

            _isSuspicious = false;
            _isPlayerDetected = false; 
            
            this.waypoints = new Vector3[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                this.waypoints[i] = waypoints[i].position;
            }

            notImmediateRepeatCount = movementData.NotImmediateRepeatCount >= this.waypoints.Length
                ? this.waypoints.Length - 1 : movementData.NotImmediateRepeatCount;
            _excludedWaypoints = new int[notImmediateRepeatCount];
        }
        #endregion
        
        #region fields
        
        public NavMeshAgent _agentRef;
        public Animator _animatorRef;
        public PlayerStateMachine _player;
        public Transform _rcOrigin;
        public AIMovementDataSo movementData;
        public AIDetectionDataSo detectionData;
        public mTransform _currentWaypoint;
        public LayerMask _excludedLayers; 
        public Vector3 _lastKnownPlayerPosition;
        public IGrabbable _currentGrabbable;
        public IPossessable _currentPossessable;
        public Vector3[] waypoints;
        public int[] _excludedWaypoints;
        public int notImmediateRepeatCount;
        public float _repairTime; 
        //To Remove
        public float _waitTimer;
        //To Remove
        public float _repairTimer;
        public bool _isSuspicious;
        public bool _isRepairing;
        public bool _isPlayerDetected;
        

        #endregion
    }
}
