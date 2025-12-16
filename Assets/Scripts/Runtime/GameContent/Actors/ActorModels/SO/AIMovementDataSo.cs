namespace Runtime.GameContent.Actors.ActorModels
{
    [CreateAssetMenu(fileName = "IAMovementSO", menuName = "IA/Movement")]
    public sealed class AIMovementDataSo : ScriptableObject
    {
        public float patrolSpeed; 
        public float chaseSpeed;
        public float rotateSpeed;
        public float waitDelay;

        [HideInInspector] 
        public Vector3[] waypoints;
        
    }
}

