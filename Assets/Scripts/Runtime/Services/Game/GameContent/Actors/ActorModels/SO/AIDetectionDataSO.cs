namespace Runtime.Services.Game.GameContent.Actors.ActorModels.SO
{
    [CreateAssetMenu(fileName = "IADetectionSO", menuName = "IA/Detection")]
    public sealed class AIDetectionDataSo : ScriptableObject
    {
        public float unawareDetectionAngle;
        public float unawareDetectionDistance;
        public float awareDetectionAngle;
        public float awareDetectionDistance;
        public float sixthSensDetectionDistance;
        public float detectionTime;
        public float timeToForget; 
    }
}