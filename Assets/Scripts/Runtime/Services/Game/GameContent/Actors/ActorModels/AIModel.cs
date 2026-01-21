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
            
            if (movementData.NotImmediateRepeatCount >= movementData.waypoints.Length)
                movementData.NotImmediateRepeatCount = movementData.waypoints.Length - 1;
            _excludedWaypoints = new int[movementData.NotImmediateRepeatCount];
            
            movementData.waypoints = new Vector3[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                movementData.waypoints[i] = waypoints[i].position;
            }
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
        public int[] _excludedWaypoints;
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
