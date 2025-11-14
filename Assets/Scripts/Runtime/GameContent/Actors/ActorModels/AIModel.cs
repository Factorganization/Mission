using Runtime.GameContent.Actors.ActorViews;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorModels
{
    public class AIModel : ActorModel
    {
        #region methodes
        public AIModel(AIMovementDataSo movementDataSo)
        {
            movementData =  movementDataSo;

            _isSuspicious = false;
            _isPlayerDetected = false; 
        }
        #endregion
        
        #region fields
        
        public AIMovementDataSo movementData;
    
        public mTransform _currentWaypoint;
        public float _waitTimer;
        
        public bool _isSuspicious;
        public bool _isPlayerDetected;
        public Vector3 _lastKnownPlayerPosition;
        
        #endregion
    }
}
