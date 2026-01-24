using System.Collections;
using Runtime.Services.Audio;
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
        aiModel._agentRef.isStopped = true;
        _repairTimer = 0; 
        aiModel._animatorRef.SetBool("ac_isRepairing", true);
        var a = ServiceLocator.Instance.Get<AudioService>();
        a.PlayOneShot(a.Atlas.sfx.pnj.repair, aiModel.transform.position);
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
        aiModel._animatorRef.SetBool("ac_isRepairing", false);
    }

    public override IEnumerator OnCoroutine()
    {
        yield return null;
    }
    
    #region fields
    
    private float _repairTimer; 

    #endregion
}