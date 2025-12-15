using Runtime.GameContent.Actors.ActorModels;

namespace Runtime.GameContent.Actors.ActorControllers
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
        
        #endregion
    }
}
