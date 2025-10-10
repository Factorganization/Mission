using Runtime.GameContent.Actors.ActorModels;
using UnityEngine;

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
