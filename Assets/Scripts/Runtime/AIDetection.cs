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
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

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