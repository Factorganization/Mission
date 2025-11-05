using Runtime.GameContent.Actors.ActorControllers;
using Runtime.GameContent.Actors.ActorModels;
using UnityEngine;

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
            AIController.SelectRandomWaypoint(_aiModel);
        }

        private void Update()
        {
            if (aiDetection && aiDetection.IsSuspicious && !aiDetection.IsPlayerSpotted)
                return;
            if (aiDetection.IsSuspicious)
                AIController.SetCurrentWaypoint(_aiModel, aiDetection.LastKnownPlayerPosition);
            
            if (!aiDetection.IsPlayerSpotted)
            {         
                //Turn then move
                if (AIController.RotateToWaypoint(_aiModel))
                    AIController.MoveToWaypoint(_aiModel);
            }
            else
            {
                //Turn and move
                AIController.RotateToWaypoint(_aiModel);
                AIController.MoveToWaypoint(_aiModel);
            }
        
            transform.position = _aiModel.transform.position;
            transform.rotation = _aiModel.transform.rotation;
        
            if (_aiModel._currentWaypoint != Vector3.zero)
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
    
        private AIModel _aiModel;
        #endregion
    }
}
