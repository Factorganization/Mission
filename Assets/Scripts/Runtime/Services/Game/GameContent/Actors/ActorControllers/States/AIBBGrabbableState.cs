using System.Collections;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States;

public class AIBBGrabbableState : BaseAiState
{
    public AIBBGrabbableState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go, model, state)
    {
       
    }

    public override void OnInit(GenericStateMachine machine)
    {
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering AI Grab State");
        aiModel._currentGrabbable.Rigidbody.isKinematic = true;
        aiModel._currentGrabbable.Rigidbody.useGravity = false;
        aiModel._currentGrabbable.Transform.SetParent(aiModel.transform);

    }

    public override sbyte OnUpdate()
    {
        if (AIController.DetectPlayer(aiModel))
        {
            stateMachine.SwitchState("suspicious");
            return 0;
        }
        if (aiModel._currentGrabbable == null)
        {
            stateMachine.SwitchState("idle");
            return 0;
        }
        
        AIController.SetCurrentWaypoint(aiModel, aiModel._currentGrabbable.OriginPos);
        AIController.UpdateAgent(aiModel);
        
        if (Vector3.Distance(aiModel.transform.position, aiModel._currentGrabbable.OriginPos) < 2)
        {
            aiModel._currentGrabbable.IsResetingPos = true;
            AIController.DropObject(aiModel);
            aiModel._currentGrabbable.Transform.position = aiModel._currentGrabbable.OriginPos;
            stateMachine.SwitchState("idle");
            return 0;
        }
        
        if (aiModel._currentGrabbable.Transform.localPosition.sqrMagnitude < 0.005f)
            return 0;
            
        aiModel._currentGrabbable.Transform.localPosition += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(aiModel._currentGrabbable.Transform.localPosition, Vector3.zero, 0.1f);
        if (aiModel._currentGrabbable.Transform.localPosition.sqrMagnitude < 0.005f)
            aiModel._currentGrabbable.Transform.localPosition = Vector3.zero;
        return 0;
    }

    public override sbyte OnFixedUpdate()
    {
        return 0;
    }

    public override void OnExitState()
    {
        AIController.DropObject(aiModel);
    }

    public override IEnumerator OnCoroutine()
    {
        yield return null;
    }
}