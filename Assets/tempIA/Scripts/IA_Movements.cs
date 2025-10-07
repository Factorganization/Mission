using UnityEngine;
using Random = UnityEngine.Random;

public class IA_Movements : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float waitDelay = 4f;
    
    private Transform _currentWaypoint;
    private float _waitTimer = 0;

    private void Start()
    {
        SelectWaypoint();
    }

    private void Update()
    {
        if (RotateToWaypoint())
        {
            MoveToWaypoint();
        }

        if (_currentWaypoint)
            return;
                
        _waitTimer += Time.deltaTime;
        if (_waitTimer >= waitDelay)
        {
            SelectWaypoint();
            _waitTimer = 0;
        }
    }
    
    public void SetCurrentWaypoint(Transform waypoint)
    {
        _currentWaypoint = waypoint;
    }

    private void MoveToWaypoint()
    {
        if (!_currentWaypoint)
            return;
        
        transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _currentWaypoint.position) < 0.1f)
            _currentWaypoint = null;
    }

    private bool RotateToWaypoint()
    {
        if (!_currentWaypoint)
            return true;
        
        Quaternion newRotation = Quaternion.LookRotation((_currentWaypoint.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_currentWaypoint.position - transform.position)) < 1 ) 
            return true;
        return false;
    }

    private void SelectWaypoint()
    {
        if  (waypoints.Length == 0) return;
        _currentWaypoint = waypoints[Random.Range(0, waypoints.Length)];
    }
}
