using System.Collections;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Services.Game.GameContent.Actors.ActorViews;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States
{
    public class AIMoveState : BaseAiState
    {
        public AIMoveState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go, model, state)
        {
        }

        public override void OnInit(GenericStateMachine machine)
        {
        }

        public override void OnEnterState()
        {

        }

        public override sbyte OnUpdate()
        {
            base.OnUpdate();
            if (aiModel._currentPossessable != null)
            {
                AIController.SetCurrentWaypoint(aiModel, aiModel._currentPossessable.Transform.position);
                
                if (Vector3.Distance(aiModel.transform.position, aiModel._currentPossessable.Transform.position) < 2)
                    stateMachine.SwitchState("repair");
            }

            if (aiModel._currentGrabbable != null)
            {
                AIController.SetCurrentWaypoint(aiModel, aiModel._currentGrabbable.Transform.position);
                
                if (Vector3.Distance(aiModel.transform.position, aiModel._currentGrabbable.Transform.position) < 2)
                    stateMachine.SwitchState("bbgrabbable");
                
            }
            
            if (aiModel._currentWaypoint.position == Vector3.zero)
            {
                stateMachine.SwitchState("idle");
            }
            
            AIController.MoveToWaypoint(aiModel);
            
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
        
        #region fields
        

        #endregion
    }
}

