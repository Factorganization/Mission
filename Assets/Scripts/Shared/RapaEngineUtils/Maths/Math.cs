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

            public static class SimpleQuinticEase
            {
                public static float FSimpleQuinticEaseOut(float current, float target)
                {
                    return Mathf.Lerp(current, target, 1 - Mathf.Pow(1 - current, 5f));
                }
                
                public static Vector2 V2SimpleQuinticEaseOut(Vector2 current, Vector2 target)
                {
                    var temp = target - current;
                    return target - new Vector2(Mathf.Pow(temp.x, 5), Mathf.Pow(temp.y, 5));
                }
                
                public static Vector3 V3SimpleQuinticEaseOut(Vector3 current, Vector3 target)
                {
                    var temp = target - current;
                    return target - new Vector3(Mathf.Pow(temp.x, 5), Mathf.Pow(temp.y, 5), 0);
                }
            }
        }
    }
}