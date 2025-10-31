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
            _iaModel = new IAModel(aiMovementDataSo);
            _iaModel.transform = transform;
        }

        private void Start()
        {
            AIController.SelectRandomWaypoint(_iaModel);
        }

        private void Update()
        {
            if (aiDetection && aiDetection.IsSuspicious && !aiDetection.IsPlayerSpotted)
                return;
            if (aiDetection.IsSuspicious)
                AIController.SetCurrentWaypoint(_iaModel, aiDetection.LastKnownPlayerPosition);
            
            if (!aiDetection.IsPlayerSpotted)
            {         
                //Turn then move
                if (AIController.RotateToWaypoint(_iaModel))
                    AIController.MoveToWaypoint(_iaModel);
            }
            else
            {
                //Turn and move
                AIController.RotateToWaypoint(_iaModel);
                AIController.MoveToWaypoint(_iaModel);
            }

        
            transform.position = _iaModel.transform.position;
            transform.rotation = _iaModel.transform.rotation;
        
            if (_iaModel._currentWaypoint != Vector3.zero)
                return;
        
            _iaModel._waitTimer += Time.deltaTime;
            if (!(_iaModel._waitTimer >= aiMovementDataSo.waitDelay)) return;
        
            AIController.SelectRandomWaypoint(_iaModel);
            _iaModel._waitTimer = 0;
        }
        #endregion
        
        #region fields
        
        [SerializeField] private AIMovementDataSo aiMovementDataSo;
        [SerializeField] private AIDetection aiDetection;
    
        private IAModel _iaModel;
        #endregion
    }
}
