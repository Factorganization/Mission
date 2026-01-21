using System.Collections;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States;

public class AIRepairState : BaseAiState
{
    public AIRepairState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go, model, state)
    {
       
    }

    public override void OnInit(GenericStateMachine machine)
    {
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering AI Repair State");
        aiModel._agentRef.isStopped = true;
        _repairTimer = 0; 
    }

    public override sbyte OnUpdate()
    {
        if (AIController.DetectPlayer(aiModel))
            stateMachine.SwitchState("suspicious");
        
        _repairTimer += Time.deltaTime;
        if (_repairTimer >= aiModel._repairTime)
        {
            _repairTimer = 0;
            aiModel._currentPossessable.Destroyed = false; 
            AIController.ForgetPossessable(aiModel);
            stateMachine.SwitchState("idle");
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
    
    private float _repairTimer; 

    #endregion
}