using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Services.Game.GameContent.Actors.ActorModels.SO;
using Runtime.Services.Game.GameSystems;

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
            model._currentWaypoint.position = model.waypoints[Random.Range(0, model.waypoints.Length)];
        }
        
        public static void SelectRandomNoImmediateRepeatWaypoint(AIModel model)
        {
            if (model.movementData.WaypointChoiceType == WaypointChoiceType.RandomNoImmediateRepeat)
            {
                int newIndex = Random.Range(0, model.waypoints.Length);
                
                while (Array.IndexOf(model._excludedWaypoints, newIndex) != -1)
                {
                    newIndex = (newIndex + 1) % model.waypoints.Length;
                }
                SetCurrentWaypoint(model, model.waypoints[newIndex]);
                model._excludedWaypoints[_currentExclusionIndex] = newIndex;
                _currentExclusionIndex = (_currentExclusionIndex + 1) % model._excludedWaypoints.Length;
            }
        }
        
        public static void SelectSequentialWaypoint(AIModel model)
        {
            int nextIndex = (_currentIndex + 1) % model.waypoints.Length;
            model._currentWaypoint.position = model.waypoints[nextIndex];
            _currentIndex = nextIndex;
        }
        
        public static void SelectReverseSequentialWaypoint(AIModel model)
        {
            int nextIndex = (_currentIndex - 1 + model.waypoints.Length) % model.waypoints.Length;
            model._currentWaypoint.position = model.waypoints[nextIndex];
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
        
        public static bool RotateToPlayer(AIModel model)
        {
            if (model._currentWaypoint.position == Vector3.zero)
                return true;
            Vector3 playerPos = model._player.transform.position;
            playerPos.y = model.transform.position.y;
            Quaternion newRotation = Quaternion.LookRotation((playerPos - model.transform.position).normalized);
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, newRotation, model.movementData.rotateSpeed * Time.deltaTime);

            if (Quaternion.Angle(model.transform.rotation, newRotation) < 1)
                return true;
            return false;
        }
        
        public static bool DetectPlayer(AIModel model)
        {
            if (!model._player.IsVisible)
                return false;
            
            Vector3 directionToPlayer = model._player.transform.position - model._rcOrigin.transform.position;
            float angleToPlayer = Vector3.Angle(model.transform.forward, directionToPlayer.normalized);

            if (directionToPlayer.magnitude > model.detectionData.sixthSensDetectionDistance)
            {
                if (model._isSuspicious)
                {
                    if (directionToPlayer.magnitude > model.detectionData.awareDetectionDistance)
                        return false;
                    if (angleToPlayer > model.detectionData.awareDetectionAngle / 2)
                        return false;
                }
                else
                {
                    if (directionToPlayer.magnitude > model.detectionData.unawareDetectionDistance)
                        return false;
                    if (angleToPlayer > model.detectionData.unawareDetectionAngle / 2)
                        return false;
                }
            }
            
            RaycastHit hit;
            if (!Physics.Raycast(model._rcOrigin.position, directionToPlayer.normalized, out hit, model.detectionData.awareDetectionDistance, ~model._excludedLayers))
                return false;
            if (hit.transform != model._player.transform)
                return false; 
            
            model._lastKnownPlayerPosition = model._player.transform.position;
            model._isSuspicious = true;
            return true;
        }

        public static bool DetectDestroyedPossessable(AIModel model)
        {
            if (model._currentPossessable != null)
                return false;

            foreach (var possessable in LevelGenerator.Generator.Possessables)
            {
                var directionToPossessable = possessable.Transform.position - model._rcOrigin.transform.position;
                var angleToPossessable = Vector3.Angle(directionToPossessable.normalized, model.transform.forward);

                if (angleToPossessable < model.detectionData.unawareDetectionAngle / 2 &&
                    directionToPossessable.magnitude <= model.detectionData.unawareDetectionDistance ||
                    directionToPossessable.magnitude <= model.detectionData.sixthSensDetectionDistance)
                {
                    RaycastHit hit;
                    if (!Physics.Raycast(model._rcOrigin.position, directionToPossessable.normalized, out hit, model.detectionData.unawareDetectionAngle, ~model._excludedLayers))
                        continue;
                    if  (hit.transform.root != possessable.Transform)
                        continue;

                    if (possessable.Destroyed)
                    {
                        DropObject(model);
                        model._currentPossessable = possessable;
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool DetectGrabbable(AIModel model)
        {
            if (model._currentGrabbable != null)
                return false;
        
            foreach (var grabbable in LevelGenerator.Generator.Grabbables)
            {
                var directionToGrabbable = (grabbable.Transform.position - model.transform.position);
                float angleToGrabbable = Vector3.Angle(model.transform.forward, directionToGrabbable.normalized);

                if (angleToGrabbable < model.detectionData.unawareDetectionAngle / 2 && directionToGrabbable.magnitude <= model.detectionData.unawareDetectionDistance || directionToGrabbable.magnitude <= model.detectionData.sixthSensDetectionDistance)
                {
                    RaycastHit hit;
                    if (!Physics.Raycast(model.transform.position, directionToGrabbable.normalized, out hit, model.detectionData.unawareDetectionDistance, ~model._excludedLayers))
                        continue;
                    if (hit.transform != grabbable.Transform)
                        continue;
                    
                    if ((Vector3.Distance(grabbable.OriginPos, grabbable.Transform.position) > 0.5f) && !grabbable.Grabbed)
                    {
                        model._currentGrabbable = grabbable;
                        return true;
                    }
                }
            }
            return false;
        }

        public static void UpdateAgent(AIModel model)
        {
            //Update Destination
            _agentDestinationUpdateDelay = model._isPlayerDetected
                ? model.movementData.DestinationUpdateDelayPatrol
                : model.movementData.DestinationUpdateDelayChase;
            
            _agentDestinationUpdateTimer += Time.deltaTime;
            if (_agentDestinationUpdateTimer >= _agentDestinationUpdateDelay)
            {
                _agentDestinationUpdateTimer = 0;

                if (model._currentWaypoint.position != Vector3.zero)
                    model._agentRef.SetDestination(model._currentWaypoint.position);
            }
        }

        public static void DropObject(AIModel model)
        {
            if (model._currentGrabbable == null)
                return; 
            model._currentGrabbable.Transform.parent = null;
            model._currentGrabbable.Rigidbody.isKinematic = false;
            model._currentGrabbable.Rigidbody.useGravity = true;
            model._currentGrabbable = null;
        }

        public static void ForgetPossessable(AIModel model)
        {
            if (model._currentPossessable == null)
                return;
            model._currentPossessable = null;
        }

        private static int _currentExclusionIndex = 0;
        private static int _currentIndex = 0;
        
        private static float _agentDestinationUpdateTimer;
        private static float _agentDestinationUpdateDelay;

        #endregion
    }
}
