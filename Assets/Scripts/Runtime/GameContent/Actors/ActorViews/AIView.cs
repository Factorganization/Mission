using System;
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
        }
        
        private void Start()
        {
            _aiModel._currentWaypoint = new mTransform();
            AIController.SetCurrentWaypoint(_aiModel, aiMovementDataSo.waypoints[0]);
        }

        private void Update()
        {
            if (aiDetection.IsSuspicious)
                AIController.SetCurrentWaypoint(_aiModel, aiDetection.LastKnownPlayerPosition);
            
            agent.isStopped = aiDetection.IsSuspicious;
            
            // Must put AI Detection into MVC
            if (aiDetection && aiDetection.IsSuspicious && !aiDetection.IsPlayerSpotted)
                return;

            if (aiDetection && aiDetection.IsPlayerSpotted)
                agent.isStopped = false;
            
            _aiUpdateSetPositionDelay = aiDetection.IsPlayerSpotted ? 0.01f : 0.2f;

            AIController.MoveToWaypoint(_aiModel);
            
            // Update every 0.2 second
            _updateAgentTimer +=  Time.deltaTime;
            if (_updateAgentTimer >= _aiUpdateSetPositionDelay)
            {
                if (_aiModel._currentWaypoint.position != Vector3.zero)
                {
                    agent.SetDestination(_aiModel._currentWaypoint.position);
                    _updateAgentTimer = 0;
                }
            }
        
            transform.position = _aiModel.transform.position;
            transform.rotation = _aiModel.transform.rotation;
        
            if (_aiModel._currentWaypoint.position != Vector3.zero)
                return;
        
            _aiModel._waitTimer += Time.deltaTime;
            if (!(_aiModel._waitTimer >= aiMovementDataSo.waitDelay)) return;
        
            AIController.SelectRandomWaypoint(_aiModel);
            _aiModel._waitTimer = 0;
        }
        
        #endregion
        
        #region fields
        
        [SerializeField] private AIMovementDataSo aiMovementDataSo;
        [SerializeField] private AIDetection aiDetection;
        [SerializeField] private NavMeshAgent agent;

        private int _index;
        private AIModel _aiModel;
        private float _updateAgentTimer;
        private float _aiUpdateSetPositionDelay = 0.2f;

        #endregion
    }

    public class mTransform
    {
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 scale = Vector3.one;
    }
}
