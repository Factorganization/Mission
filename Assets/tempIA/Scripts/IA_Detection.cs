using UnityEditor;
using UnityEngine;

public class IA_Detection : MonoBehaviour
{
    [SerializeField] private float detectionAngle = 45f;
    [SerializeField] private float detectionDistance = 10f;
    [SerializeField] private float timeToDetect = 3f;
    [SerializeField] private float timeToForget = 5f;

    [SerializeField] private Transform player;

    private float _detectionTimer = 0f;
    private float _forgetTimer = 0f;
    
    private void Update()
    {
        DetectPlayer();
        
        //reset sus timer
        if (_detectionTimer > 0)
        {
            _forgetTimer  += Time.deltaTime;
            if (_forgetTimer >= timeToForget) 
            {
                _detectionTimer = 0;
                _forgetTimer = 0;
            }
        }
    }

    private void DetectPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < detectionAngle / 2 && directionToPlayer.magnitude <= detectionDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionDistance))
            {
                if (hit.transform == player)
                {
                    if (_detectionTimer < timeToDetect)
                    {
                        _detectionTimer += Time.deltaTime;
                        _forgetTimer = 0;
                        Debug.Log("Suspicious");
                    }
                    else Debug.Log("Player Spotted");
                }
            }
        }
    }

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
    }
}

