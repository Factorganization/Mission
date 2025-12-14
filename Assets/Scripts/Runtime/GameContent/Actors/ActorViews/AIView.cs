using Runtime.GameContent.Actors.ActorControllers;
using Runtime.GameContent.Actors.ActorModels;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime.GameContent.Actors.ActorViews
{
    public class AIView : ActorView
    {
        #region methodes
        
        private void Awake()
        {
            _aiModel = new AIModel(aiMovementDataSo);
            _aiModel.transform = transform;
            agent.speed = _aiModel.movementData.patrolSpeed;
            agent.angularSpeed = _aiModel.movementData.rotateSpeed;
            UpdateMovementData();
        }
        
        private void Start()
        {
            _aiModel._currentWaypoint = new mTransform();
            AIController.SetCurrentWaypoint(_aiModel, aiMovementDataSo.waypoints[0]);
        }

        private void Update()
        {
            //Check if Caught
            if (Vector3.Distance(transform.position, playerTrans.position) < 1 && aiDetection.IsPlayerSpotted)
            {
                gameOver.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            
            //Drop Object // Check Distance Collectable
            if (aiDetection.CurrentObject != null)
                if (Vector3.Distance(transform.position, aiDetection.CurrentObject.OriginPos) < distanceToCollectable)
                {
                    aiDetection.DropObject();
                    AIController.SelectRandomWaypoint(_aiModel);
                }
            
            //Must put AI Detection into MVC
            if (aiDetection.CurrentObject != null)
                AIController.SetCurrentWaypoint(_aiModel,  aiDetection.CurrentObject.OriginPos);

            //Check if Repairable
            if (aiDetection.CurrentPossessable != null)
            {
                AIController.SetCurrentWaypoint(_aiModel,
                    aiDetection.CurrentPossessable.Transform.position -
                    ((transform.position - aiDetection.CurrentPossessable.Transform.position).normalized) * 2);
                
                // Check Distance Possessable
                if (Vector3.Distance(gameObject.transform.position, aiDetection.CurrentPossessable.Transform.position) <
                    distanceToPossessable)
                {
                    //repair sfx
                    aiDetection.CurrentPossessable.Destroyed = false;
                    aiDetection.ForgetPossessable();
                    AIController.SelectRandomWaypoint(_aiModel);
                }
            }

            //Suspicious behaviour
            if (aiDetection.IsSuspicious)
                AIController.SetCurrentWaypoint(_aiModel, aiDetection.LastKnownPlayerPosition);
            agent.isStopped = aiDetection.IsSuspicious;
            
            if (aiDetection && aiDetection.IsSuspicious && !aiDetection.IsPlayerSpotted)
                return;

            if (aiDetection && aiDetection.IsPlayerSpotted)
            {
                agent.isStopped = false;
                aiDetection.DropObject();
                agent.speed = _aiModel.movementData.chaseSpeed;
            }
            else
            {
                agent.speed = _aiModel.movementData.patrolSpeed;
            }
            
            _aiUpdateSetPositionDelay = aiDetection.IsPlayerSpotted ? 0.01f : 0.2f;

            AIController.MoveToWaypoint(_aiModel);
            
            // Update every 0.2 second
            _updateAgentTimer +=  Time.deltaTime;
            if (_updateAgentTimer >= _aiUpdateSetPositionDelay)
            {
                _updateAgentTimer = 0;
                
                if (_aiModel._currentWaypoint.position != Vector3.zero)
                    agent.SetDestination(_aiModel._currentWaypoint.position);
            }
        
            //Display Player
            transform.position = _aiModel.transform.position;
            transform.rotation = _aiModel.transform.rotation;
        
            //If position is null return
            if (_aiModel._currentWaypoint.position != Vector3.zero)
                return;
        
            //Timer Logic
            _aiModel._waitTimer += Time.deltaTime;
            if (!(_aiModel._waitTimer >= aiMovementDataSo.waitDelay)) return;
        
            AIController.SelectRandomWaypoint(_aiModel);
            _aiModel._waitTimer = 0;
        }

        private void UpdateMovementData()
        {
            aiMovementDataSo.waypoints = new Vector3[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                aiMovementDataSo.waypoints[i] = waypoints[i].position;
            }
        }


        #endregion
        
        #region fields
        
        [SerializeField] private float distanceToPossessable; 
        [SerializeField] private float distanceToCollectable;
        
        [SerializeField] private Transform[]  waypoints;
        
        [SerializeField] private AIMovementDataSo aiMovementDataSo;
        [SerializeField] private AIDetection aiDetection;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform playerTrans;
        [SerializeField] private GameObject gameOver; 

        private int _index;
        private AIModel _aiModel;
        private float _updateAgentTimer;
        private float _aiUpdateSetPositionDelay = 0.2f;

        #endregion
    }
}
