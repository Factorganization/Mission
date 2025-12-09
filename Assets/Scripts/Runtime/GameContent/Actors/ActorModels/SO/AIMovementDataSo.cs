using UnityEngine;

namespace Runtime.GameContent.Actors.ActorModels
{
    [CreateAssetMenu(fileName = "IAMovementSO", menuName = "IA/Movement")]
    public sealed class AIMovementDataSo : ScriptableObject
    {
        public float moveSpeed;
        public float rotateSpeed;
        public float waitDelay;

        [HideInInspector] 
        public Vector3[] waypoints;
        
    }
}

