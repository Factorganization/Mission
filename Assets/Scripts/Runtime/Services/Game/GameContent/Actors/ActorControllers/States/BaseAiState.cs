using System.Collections;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States
{
    public class BaseAiState : BaseState
    {
        #region

        protected BaseAiState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go)
        {
            aiModel = model;
        }
        
        #endregion

        #region methodes
        
        public override void OnInit(GenericStateMachine machine)
        {
        }

        public override void OnEnterState()
        {
        }

        public override sbyte OnUpdate()
        {
            //Update Destination
            _agentDestinationUpdateDelay = aiModel._isPlayerDetected
                ? aiModel.movementData.DestinationUpdateDelayPatrol
                : aiModel.movementData.DestinationUpdateDelayChase;
            
            _agentDestinationUpdateTimer += Time.deltaTime;
            if (_agentDestinationUpdateTimer >= _agentDestinationUpdateDelay)
            {
                _agentDestinationUpdateTimer = 0;

                if (aiModel._currentWaypoint.position != Vector3.zero)
                    aiModel._agentRef.SetDestination(aiModel._currentWaypoint.position);
            }
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            return 0;
        }

        public override void OnExitState()
        {
        }

        public override IEnumerator OnCoroutine()
        {
            yield return null;
        }
        
        #endregion

        #region fields
        
        protected readonly AIModel aiModel;
        
        protected float _agentDestinationUpdateTimer;
        protected float _agentDestinationUpdateDelay;

        #endregion
    }
}