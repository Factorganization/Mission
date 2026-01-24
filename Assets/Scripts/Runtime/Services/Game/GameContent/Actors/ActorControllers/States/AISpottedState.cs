using System.Collections;
using Runtime.Services.Audio;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States;

public class AISpottedState : BaseAiState
{
    public AISpottedState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go, model, state)
    {
       
    }

    public override void OnInit(GenericStateMachine machine)
    {
    }

    public override void OnEnterState()
    {
        aiModel._agentRef.isStopped = true;
        var a = ServiceLocator.Instance.Get<AudioService>();
        a.PlayOneShot(aiModel._male ? a.Atlas.sfx.pnj.male.maleSpotPlayer : a.Atlas.sfx.pnj.female.femaleSpotPlayer, aiModel.transform.position);
    }

    public override sbyte OnUpdate()
    {
        AIController.DetectPlayer(aiModel);
        if (!aiModel._animatorRef.GetCurrentAnimatorStateInfo(0).IsName("Spotted"))
            stateMachine.SwitchState("chase");
        
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
}