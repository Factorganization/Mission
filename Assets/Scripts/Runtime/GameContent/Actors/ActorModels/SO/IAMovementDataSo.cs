using UnityEngine;

namespace Runtime.GameContent.Actors.ActorModels
{
    [CreateAssetMenu(fileName = "IAMovementSO", menuName = "IA/Movement")]
    public sealed class IAMovementDataSo : ScriptableObject
    {
        public float moveSpeed;
        public float rotateSpeed;
        public Vector3[] waypoints;
        public float waitDelay; 
    }
}