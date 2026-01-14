using Runtime.Service;
using Runtime.Services.Game.GameContent.Actors.ActorControllers;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Services.Game.GameContent.Actors.ActorModels.SO;
using Runtime.Services.Game.GameContent.Actors.ActorModules.AI;
using Runtime.Services.Cursor;
using Runtime.Services.Game.GameSystems;
using UnityEngine.AI;
using NUnit.Framework;

namespace Runtime.Services.Game.GameContent.Actors.ActorViews
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
            AIController.SelectNextWaypoint(_aiModel);
        }

        private void Update()
        {
            //Check if Caught
            if (Vector3.Distance(transform.position, playerTrans.position) < 1 && aiDetection.IsPlayerSpotted)
            {
                GameManager.Instance.GameUIMgr.GameOver();
                ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            }
            
            //Drop Object // Check Distance Collectable
            if (aiDetection.CurrentObject != null)
            {
                AIController.SetCurrentWaypoint(_aiModel,  aiDetection.CurrentObject.OriginPos);
                if (Vector3.Distance(transform.position, aiDetection.CurrentObject.OriginPos) < distanceToCollectable)
                {
                    aiDetection.CurrentObject.IsResetingPos = true;
                    aiDetection.DropObject();
                    AIController.SelectNextWaypoint(_aiModel);
                }
            }

            //Check if Repairable
            if (aiDetection.CurrentPossessable != null)
            {
                AIController.SetCurrentWaypoint(_aiModel,
                    aiDetection.CurrentPossessable.Transform.position -
                    ((transform.position - aiDetection.CurrentPossessable.Transform.position).normalized) * 2);
                
                // Check Distance Possessable
                if (Vector3.Distance(transform.position, aiDetection.CurrentPossessable.Transform.position) <
                    distanceToPossessable)
                {
                    //repair sfx
                    if (_aiModel._repairTimer < repairTime)
                    {
                        animator.SetBool("ac_isRepairing", true);
                        _aiModel._repairTimer += Time.deltaTime;
                        agent.isStopped = true;
                    }
                    else
                    {
                    aiDetection.CurrentPossessable.Destroyed = false;
                    aiDetection.ForgetPossessable();
                    AIController.SelectNextWaypoint(_aiModel);
                    agent.isStopped = false;
                    animator.SetBool("ac_isRepairing", false);
                    _aiModel._repairTimer = 0;
                    } 
                }
            }

            //Suspicious behaviour
            if (aiDetection.IsSuspicious)
                AIController.SetCurrentWaypoint(_aiModel, aiDetection.LastKnownPlayerPosition);
            agent.isStopped = aiDetection.IsSuspicious;
            animator.SetBool("ac_isSus", aiDetection.IsSuspicious);
            
            if (aiDetection && aiDetection.IsSuspicious && !aiDetection.IsPlayerSpotted)
                return;

            if (aiDetection && aiDetection.IsPlayerSpotted)
            {
                agent.isStopped = false;
                aiDetection.DropObject();
                agent.speed = _aiModel.movementData.chaseSpeed;
                animator.SetBool("ac_isWalking", false);
                animator.SetBool("ac_isRunning", true);
            }
            else
            {
                agent.speed = _aiModel.movementData.patrolSpeed;
                animator.SetBool("ac_isRunning", false);
                animator.SetBool("ac_isWalking", true);
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
        
            //If position is not null return
            if (_aiModel._currentWaypoint.position != Vector3.zero)
                return;
        
            //Timer Logic
            _aiModel._waitTimer += Time.deltaTime;
            if (!(_aiModel._waitTimer >= aiMovementDataSo.waitDelay)) return;
        
            AIController.SelectNextWaypoint(_aiModel);
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
        [SerializeField] private Animator animator; 
        [SerializeField] private float repairTime;

        private int _index;
        private AIModel _aiModel;
        private float _updateAgentTimer;
        private float _aiUpdateSetPositionDelay = 0.2f;

        #endregion
    }
}