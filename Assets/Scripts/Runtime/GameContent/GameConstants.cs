namespace Runtime.GameContent
{
    public static class GameConstants
    {
        public const float FloatPointComparisonValue = 0.01f;

        public const float MaxInteractionAngle = 45f;
        
        public const float MaxPossessDistance = 4f;
        
        public const float ConstFixedDeltaTime = 0.02f;
        
        public const float AntiGroundGrabJumpTimer = 0.1f;

        public const float DestructiveActionTime = 0.5f;
        
        public static Vector3 VectorUpFilter = new(1, 0, 1);
    }
}