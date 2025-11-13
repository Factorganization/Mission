using UnityEngine;

namespace Shared.RapaEngineUtils.Maths
{
    public static class Math
    {
        public static class EasingFunction
        {
            public static class SimpleQuadraticEase
            {
                public static float FSimpleQuadraticEaseOut(float current, float target, float quadSpeedMultiplier)
                {
                    return (target - current) * quadSpeedMultiplier;
                }

                public static Vector2 V2SimpleQuadraticEaseOut(Vector2 current, Vector2 target, float quadSpeedMultiplier)
                {
                    return (target - current) * quadSpeedMultiplier;
                }

                public static Vector3 V3SimpleQuadraticEaseOut(Vector3 current, Vector3 target, float quadSpeedMultiplier)
                {
                    return (target - current) * quadSpeedMultiplier;
                }
            }
        }
    }
}