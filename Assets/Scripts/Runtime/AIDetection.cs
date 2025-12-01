using Runtime.GameContent.Actors.ActorControllers;
using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.Management.GameManagement;
using UnityEditor;
using UnityEngine;

namespace Runtime
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
            if (CurrentObject != null)
            {
                if (Vector3.Distance(CurrentObject.Transform.position, transform.position) < 0.5f)
                {
                    CurrentObject.Transform.position = transform.position + transform.forward;
                    CurrentObject.Rigidbody.isKinematic = true;
                }
            }
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
        Vector3 directionToPlayer = (player.position - transform.position);
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);

        if (angleToPlayer < detectionAngle / 2 && directionToPlayer.magnitude <= detectionDistance)
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionDistance))
                return; 
            if (hit.transform != player)
                return;
                
            _forgetTimer = 0;
            
            if (_detectionTimer < timeToDetect)
            {
                Debug.Log("Suspicious");
                _detectionTimer += Time.deltaTime;
                transform.LookAt(player.position);
            }
            else
            {
                Debug.Log("Player Spotted");
                IsPlayerSpotted = true;
                LastKnownPlayerPosition = player.position;
            }
        }

        if (IsPlayerSpotted && Vector3.Distance(transform.position, player.position) <= sixthSensDetectionDistance)
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionDistance))
                return;
            if (hit.transform != player)
                return;
            
            LastKnownPlayerPosition = player.position;
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
            float  angleToGrabbable = Vector3.Angle(transform.forward, directionToGrabbable.normalized);

            if (angleToGrabbable < detectionAngle / 2 && directionToGrabbable.magnitude <= detectionDistance)
            {
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, directionToGrabbable.normalized, out hit))
                    continue;
                if (hit.transform != grabbable.Transform)
                    continue;

                if (Vector3.Distance(grabbable.OriginPos, grabbable.Transform.position) > 0.1f)
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
        
        DropObject();
        
        foreach (IPossessable possessable in levelGenerator.Possessables)
        {
            var directionToPossessable = (possessable.Transform.position - transform.position);
            float angleToPossess = Vector3.Angle(transform.forward, directionToPossessable.normalized);

            if (angleToPossess < detectionAngle / 2 && directionToPossessable.magnitude <= detectionDistance)
            {
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, directionToPossessable.normalized, out hit))
                    continue; 
                if  (hit.transform != possessable.Transform)
                    continue;
                
                if (possessable.Destroyed)
                    CurrentPossessable = possessable;
            }
        }
    }

    public void DropObject()
    {
        if (CurrentObject == null)
            return;
        CurrentObject.Rigidbody.isKinematic = false;
        CurrentObject = null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionDistance;
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionDistance;

        Gizmos.color = new Color(1, 1, 0, 0.2f);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Handles.color = new Color(1, 1, 0, 0.1f);
        Handles.DrawSolidArc(transform.position, Vector3.up, leftBoundary, detectionAngle, detectionDistance);
        if (IsPlayerSpotted)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sixthSensDetectionDistance);
        }
    }
#endif
    
    #endregion
    
    #region fields

    public bool IsSuspicious { private set; get; } = false; 
    public bool IsPlayerSpotted { private set; get; } = false;
    public Vector3 LastKnownPlayerPosition { private set; get; } = Vector3.zero;
    public IGrabbable CurrentObject { private set; get; } = null;
    public IPossessable CurrentPossessable { private set; get; } = null;
    
    [SerializeField] private float detectionAngle = 45f;
    [SerializeField] private float detectionDistance = 10f;
    [SerializeField] private float sixthSensDetectionDistance = 6f;
    [SerializeField] private float timeToDetect = 3f;
    [SerializeField] private float timeToForget = 5f;
    
    [SerializeField] private Transform player;
    [SerializeField] private LevelGenerator levelGenerator;
         
    private float _detectionTimer = 0f;
    private float _forgetTimer = 0f;
    
    
    #endregion
}
}