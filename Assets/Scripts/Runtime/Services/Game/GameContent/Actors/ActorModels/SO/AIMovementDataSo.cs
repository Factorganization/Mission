namespace Runtime.Services.Game.GameContent.Actors.ActorModels.SO
{
    [CreateAssetMenu(fileName = "IAMovementSO", menuName = "IA/Movement")]
    public sealed class AIMovementDataSo : ScriptableObject
    {
        public float patrolSpeed; 
        public float chaseSpeed;
        public float rotateSpeed;
        public float waitDelay;

        public WaypointChoiceType WaypointChoiceType;
        public int NotImmediateRepeatCount;

        [HideInInspector] 
        public Vector3[] waypoints;
        
    }
    
    public enum WaypointChoiceType
    {
        Random = 0,
        RandomNoImmediateRepeat = 1,
        Sequential = 2,
        ReverseSequential = 3
    }
}

