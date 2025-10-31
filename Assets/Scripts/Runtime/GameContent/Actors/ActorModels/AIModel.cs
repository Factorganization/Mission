using UnityEngine;

namespace Runtime.GameContent.Actors.ActorModels
{
    public class IAModel : ActorModel
    {

        #region methodes
        public IAModel(IAMovementDataSo movementDataSo)
        {
            movementData =  movementDataSo;
        }
        #endregion
        
        #region fields
        
        public IAMovementDataSo movementData;
    
        public Vector3 _currentWaypoint;
        public float _waitTimer;
        
        #endregion
    }
}
