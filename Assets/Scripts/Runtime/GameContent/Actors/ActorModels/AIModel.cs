using UnityEngine;

namespace Runtime.GameContent.Actors.ActorModels
{
    public class IAModel : ActorModel
    {
        public IAMovementDataSo movementData;
    
        public Vector3 _currentWaypoint;
        public float _waitTimer;
    
        public IAModel(IAMovementDataSo movementDataSo)
        {
            movementData =  movementDataSo;
        }
    }
}
