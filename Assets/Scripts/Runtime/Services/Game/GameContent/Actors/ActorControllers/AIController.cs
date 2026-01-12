using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Services.Game.GameContent.Actors.ActorModels.SO;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers
{
    public static class AIController
    {
        #region methodes
        public static void SetCurrentWaypoint(AIModel model, Vector3 waypoint)
        {
            model._currentWaypoint.position = waypoint;
        }

        public static void SelectRandomWaypoint(AIModel model)
        {
            model._currentWaypoint.position = model.movementData.waypoints[Random.Range(0, model.movementData.waypoints.Length)];
        }
        
        public static void SelectRandomNoImmediateRepeatWaypoint(AIModel model)
        {
            if (model.movementData.WaypointChoiceType == WaypointChoiceType.RandomNoImmediateRepeat)
            {
                int newIndex = Random.Range(0, model.movementData.waypoints.Length);
                
                while (Array.IndexOf(model._excludedWaypoints, newIndex) != -1)
                {
                    newIndex = (newIndex + 1) % model.movementData.waypoints.Length;
                }
                SetCurrentWaypoint(model, model.movementData.waypoints[newIndex]);
                model._excludedWaypoints[_currentExclusionIndex] = newIndex;
                _currentExclusionIndex = (_currentExclusionIndex + 1) % model.movementData.NotImmediateRepeatCount;
            }
        }
        
        public static void SelectSequentialWaypoint(AIModel model)
        {
            int nextIndex = (_currentIndex + 1) % model.movementData.waypoints.Length;
            model._currentWaypoint.position = model.movementData.waypoints[nextIndex];
            _currentIndex = nextIndex;
        }
        
        public static void SelectReverseSequentialWaypoint(AIModel model)
        {
            int nextIndex = (_currentIndex - 1 + model.movementData.waypoints.Length) % model.movementData.waypoints.Length;
            model._currentWaypoint.position = model.movementData.waypoints[nextIndex];
            _currentIndex = nextIndex;
        }
        
        public static void SelectNextWaypoint(AIModel model)
        {
            switch (model.movementData.WaypointChoiceType)
            {
                case WaypointChoiceType.Random:
                    SelectRandomWaypoint(model);
                    break;
                case WaypointChoiceType.RandomNoImmediateRepeat:
                    SelectRandomNoImmediateRepeatWaypoint(model);
                    break;
                case WaypointChoiceType.Sequential:
                    SelectSequentialWaypoint(model);
                    break;
                case WaypointChoiceType.ReverseSequential:
                    SelectReverseSequentialWaypoint(model);
                    break;
                default:
                    SelectRandomWaypoint(model);
                    break;
            }
        }

        public static void MoveToWaypoint(AIModel model)
        {
            if (model._currentWaypoint.position == Vector3.zero)
                return;
            
            if (Vector3.Distance(model.transform.position, model._currentWaypoint.position) < 1f)
            {
                model._currentWaypoint.position = Vector3.zero;
            }
        }
        
        public static bool RotateToWaypoint(AIModel model)
        {
            if (model._currentWaypoint.position == Vector3.zero)
                return true;

            Quaternion newRotation = Quaternion.LookRotation((model._currentWaypoint.position - model.transform.position).normalized);
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, newRotation, model.movementData.rotateSpeed * Time.deltaTime);

            if (Quaternion.Angle(model.transform.rotation, newRotation) < 1)
                return true;
            return false;
        }

        private static int _currentExclusionIndex = 0;
        private static int _currentIndex = 0;

        #endregion
    }
}
