using Runtime.GameContent.Actors.ActorModels;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    public class AIView : ActorView
    {
        [SerializeField] private IAMovementDataSo iaMovementDataSo;
    
        private IAModel _iaModel;
    
        private void Awake()
        {
            _iaModel = new IAModel(iaMovementDataSo);
            _iaModel.transform = transform;
        }

        private void Start()
        {
            IAController.SelectRandomWaypoint(_iaModel);
        }

        private void Update()
        {
            if (IAController.RotateToWaypoint(_iaModel))
                IAController.MoveToWaypoint(_iaModel);
        
            transform.position = _iaModel.transform.position;
            transform.rotation = _iaModel.transform.rotation;
        
            if (_iaModel._currentWaypoint != Vector3.zero)
                return;
        
            _iaModel._waitTimer += Time.deltaTime;
            if (!(_iaModel._waitTimer >= iaMovementDataSo.waitDelay)) return;
        
            IAController.SelectRandomWaypoint(_iaModel);
            _iaModel._waitTimer = 0;
        }
    }
}
