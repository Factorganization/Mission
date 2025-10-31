using UnityEngine;

namespace Runtime.GameContent.Actors.ActorModels
{
    public class IAModel : ActorModel
    {
        #region methodes
        public IAModel(AIMovementDataSo movementDataSo)
        {
            movementData =  movementDataSo;

            _isSuspicious = false;
            _isPlayerDetected = false; 
        }
        #endregion
        
        #region fields
        
        public AIMovementDataSo movementData;
    
        public Vector3 _currentWaypoint;
        public float _waitTimer;
        
        public bool _isSuspicious;
        public bool _isPlayerDetected;
        public Vector3 _lastKnownPlayerPosition;
        
        #endregion
    }
}
