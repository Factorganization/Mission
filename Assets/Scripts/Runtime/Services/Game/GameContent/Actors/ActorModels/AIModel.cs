using Runtime.Services.Game.GameContent.Actors.ActorModels.SO;
using Runtime.Services.Game.GameContent.Actors.ActorViews;
using UnityEngine.AI;

namespace Runtime.Services.Game.GameContent.Actors.ActorModels
{
    public class AIModel : ActorModel
    {
        #region methodes
        public AIModel(AIMovementDataSo movementDataSo, Animator animator, NavMeshAgent agent)
        {
            movementData =  movementDataSo;
            _agentRef = agent;
            _animatorRef = animator; 

            _isSuspicious = false;
            _isPlayerDetected = false; 
            
            if (movementData.NotImmediateRepeatCount >= movementData.waypoints.Length)
                movementData.NotImmediateRepeatCount = movementData.waypoints.Length - 1;
            _excludedWaypoints = new int[movementData.NotImmediateRepeatCount];
        }
        #endregion
        
        #region fields
        
        public AIMovementDataSo movementData;
        public NavMeshAgent _agentRef;
        public Animator _animatorRef;
        public mTransform _currentWaypoint;
        public Vector3 _lastKnownPlayerPosition;
        public int[] _excludedWaypoints;
        public float _waitTimer;
        public float _repairTimer;
        public bool _isSuspicious;
        public bool _isRepairing;
        public bool _isPlayerDetected;

        #endregion
    }
}
