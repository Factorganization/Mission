using System.Collections;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States;

public class AISuspiciousState : BaseAiState
{
    public AISuspiciousState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go, model, state)
    {
       
    }

    public override void OnInit(GenericStateMachine machine)
    {
    }

    public override void OnEnterState()
    {
        aiModel._agentRef.isStopped = true;
    }

    public override sbyte OnUpdate()
    {
        if (AIController.DetectPlayer(aiModel))
        {
            AIController.RotateToPlayer(aiModel);
            _detectionTimer += Time.deltaTime;
            _forgetTimer = 0;
        }
        else
        {
            _forgetTimer += Time.deltaTime;
        }
        
        if (_forgetTimer >= aiModel.detectionData.timeToForget)
        {
            _detectionTimer = 0;
            _forgetTimer = 0;
            stateMachine.SwitchState("idle");
            return 0;
        }

        if (_detectionTimer >= aiModel.detectionData.detectionTime)
        {
            _detectionTimer = 0;
            _forgetTimer = 0;
            AIController.DropObject(aiModel);
            stateMachine.SwitchState("chase");
            return 0;
        }
        
        return 0;
    }

    public override sbyte OnFixedUpdate()
    {
        return 0;
    }

    public override void OnExitState()
    {
        aiModel._agentRef.isStopped = false;
    }

    public override IEnumerator OnCoroutine()
    {
        yield return null;
    }

    #region fields

    protected float _forgetTimer;
    protected float _detectionTimer;

    #endregion
}