using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Player.Controller.LocalMachine.View;
using Runtime.Management.GameManagement;
using UnityEditor;

namespace Runtime.GameContent.Actors.ActorModules.AI
{
    public class AIDetection : MonoBehaviour
    {
        //To change later
    
        #region methodes
    
        private void Update()
        {
            DetectPlayer();
        
            if (!IsPlayerSpotted && CurrentPossessable == null)
                DetectDamagedItem();
        
            if (!IsPlayerSpotted && CurrentObject == null && CurrentPossessable == null)
                DetectObject();
            else
            {
                //Move object
                SetGrabbedObjectLocalPos();
            }
        
            //reset sus timer
            if (_detectionTimer > 0)
            {
                _forgetTimer += Time.deltaTime;
                if (_forgetTimer >= timeToForget) 
                {
                    _detectionTimer = 0;
                    _forgetTimer = 0;
                    IsPlayerSpotted = false;
                }
            }

            IsSuspicious = _detectionTimer > 0;
        }

        private void DetectPlayer()
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position);
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);
            if ((((angleToPlayer < unawareDetectionAngle / 2 || (angleToPlayer < awareDetectionAngle && IsPlayerSpotted)) && (directionToPlayer.magnitude <= unawareDetectionDistance) || directionToPlayer.magnitude <= awareDetectionDistance && IsPlayerSpotted) ||
                 Vector3.Distance(player.transform.position, transform.position) < sixthSensDetectionDistance) && player.IsVisible)
            {
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, awareDetectionDistance))
                    return; 
                if (hit.transform != player.transform)
                    return;
            
                _forgetTimer = 0;
            
                if (_detectionTimer < timeToDetect)
                {
                    Debug.Log("Suspicious");
                    _detectionTimer += Time.deltaTime;
                    transform.LookAt(player.transform.position);
                    transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
                }
                else
                {
                    Debug.Log("Player Spotted");
                    IsPlayerSpotted = true;
                    LastKnownPlayerPosition = player.transform.position;
                }
            }

            if (IsPlayerSpotted && Vector3.Distance(transform.position, player.transform.position) <= sixthSensDetectionDistance)
            {
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, unawareDetectionDistance))
                    return;
                if (hit.transform != player.transform)
                    return;
            
                LastKnownPlayerPosition = player.transform.position;
                IsPlayerSpotted = true;
            }
        }
    
        private void DetectObject()
        {
            if (CurrentObject != null)
                return;
        
            foreach (IGrabbable grabbable in levelGenerator.Grabbables)
            {
                var directionToGrabbable = (grabbable.Transform.position - transform.position);
                float angleToGrabbable = Vector3.Angle(transform.forward, directionToGrabbable.normalized);

                if (angleToGrabbable < unawareDetectionAngle / 2 && directionToGrabbable.magnitude <= unawareDetectionAngle)
                {
                    RaycastHit hit;
                    if (!Physics.Raycast(transform.position, directionToGrabbable.normalized, out hit))
                        continue;
                    if (hit.transform != grabbable.Transform)
                        continue;
                    
                    if (Vector3.Distance(grabbable.OriginPos, grabbable.Transform.position) > 0.5f)
                    {
                        CurrentObject = grabbable;
                        return;
                    }
                }
            }  
        }

        public void DetectDamagedItem()
        {
            if  (CurrentPossessable != null)
                return;
        
            foreach (IPossessable possessable in levelGenerator.Possessables)
            {
                var directionToPossessable = (new Vector3(possessable.Transform.position.x, 0, possessable.Transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
                float angleToPossess = Vector3.Angle(transform.forward, directionToPossessable);

                if (angleToPossess < unawareDetectionAngle / 2 && directionToPossessable.magnitude <= unawareDetectionAngle)
                {
                    RaycastHit hit;
                    if (!Physics.Raycast(transform.position, directionToPossessable.normalized, out hit))
                        continue; 
                    if  (hit.transform.root != possessable.Transform)
                        continue;

                    if (possessable.Destroyed)
                    {
                        Debug.Log("Damaged Object spotted");
                        DropObject();
                        CurrentPossessable = possessable;
                    }
                }
            }
        }

        public void DropObject()
        {
            if (CurrentObject == null)
                return;
            CurrentObject.Transform.parent = null;
            CurrentObject.Rigidbody.isKinematic = false;
            CurrentObject = null;
        }

        public void ForgetPossessable()
        {
            if (CurrentPossessable == null)
                return;
            CurrentPossessable =  null;
        }
        
        private void SetGrabbedObjectLocalPos()
        {
            if (CurrentObject is null)
                return;
            
            if (CurrentObject.Transform.parent != gameObject.transform)
                CurrentObject.Transform.SetParent(gameObject.transform);
            
            if (CurrentObject.Transform.localPosition.sqrMagnitude < 0.005f)
                return;
            
            CurrentObject.Transform.localPosition += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(CurrentObject.Transform.localPosition, Vector3.zero, 0.1f);
            if (CurrentObject.Transform.localPosition.sqrMagnitude < 0.005f)
                CurrentObject.Transform.localPosition = Vector3.zero;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 rightBoundary = Quaternion.Euler(0, awareDetectionAngle / 2, 0) * transform.forward * awareDetectionAngle;
            Vector3 leftBoundary = Quaternion.Euler(0, -awareDetectionAngle / 2, 0) * transform.forward * awareDetectionAngle;
        
            Gizmos.color = new Color(1, 1, 0, 0.2f);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Handles.color = new Color(1, 1, 0, 0.1f);
            Handles.DrawSolidArc(transform.position, Vector3.up, leftBoundary, awareDetectionAngle, awareDetectionDistance);
        
            rightBoundary = Quaternion.Euler(0, unawareDetectionAngle / 2, 0) * transform.forward * unawareDetectionAngle;
            leftBoundary = Quaternion.Euler(0, -unawareDetectionAngle / 2, 0) * transform.forward * unawareDetectionAngle;

            Gizmos.color = new Color(1, 0.5f, 0, 0.2f);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Handles.color = new Color(1, 1, 0, 0.1f);
            Handles.DrawSolidArc(transform.position, Vector3.up, leftBoundary, unawareDetectionAngle, unawareDetectionDistance);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sixthSensDetectionDistance);
        }
#endif
    
        #endregion
    
        #region fields

        public bool IsSuspicious { private set; get; } = false; 
        public bool IsPlayerSpotted { private set; get; } = false;
        public Vector3 LastKnownPlayerPosition { private set; get; } = Vector3.zero;
        public IGrabbable CurrentObject { private set; get; } = null;
        public IPossessable CurrentPossessable { private set; get; } = null;
    
        [SerializeField] private float unawareDetectionAngle = 45f;
        [SerializeField] private float awareDetectionAngle = 45f;
        [SerializeField] private float unawareDetectionDistance = 10f;
        [SerializeField] private float awareDetectionDistance = 10f;
    
        [SerializeField] private float sixthSensDetectionDistance = 6f;
        [SerializeField] private float timeToDetect = 3f;
        [SerializeField] private float timeToForget = 5f;
    
        [SerializeField] private PlayerStateMachine player;
        [SerializeField] private LevelGenerator levelGenerator;
         
        private float _detectionTimer = 0f;
        private float _forgetTimer = 0f;
    
    
        #endregion
    }
}