using Runtime.GameContent.Actors.ActorModels;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorControllers
{
    public static class AIController
    {
        #region methodes
        public static void SetCurrentWaypoint(AIModel model, Vector3 waypoint)
        {
            model._currentWaypoint = waypoint;
        }

        public static void SelectRandomWaypoint(AIModel model)
        {
            model._currentWaypoint = model.movementData.waypoints[Random.Range(0, model.movementData.waypoints.Length)];
        }

        public static void MoveToWaypoint(AIModel model)
        {
            if (model._currentWaypoint == Vector3.zero)
                return; 
        
            model.transform.position = Vector3.MoveTowards(model.transform.position, model._currentWaypoint, model.movementData.moveSpeed*Time.deltaTime);

            if (Vector3.Distance(model.transform.position, model._currentWaypoint) < 0.1f)
                model._currentWaypoint = Vector3.zero;
        
        }

        public static bool RotateToWaypoint(AIModel model)
        {
            if (model._currentWaypoint == Vector3.zero)
                return true;

            Quaternion newRotation = Quaternion.LookRotation((model._currentWaypoint - model.transform.position).normalized);
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, newRotation, model.movementData.rotateSpeed * Time.deltaTime);

            if (Quaternion.Angle(model.transform.rotation, newRotation) < 1)
                return true;
            return false;
        }
        
        #endregion
    }
}
