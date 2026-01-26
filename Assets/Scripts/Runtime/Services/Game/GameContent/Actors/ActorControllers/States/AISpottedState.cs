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
        aiModel._spottedPart.Play();
        aiModel._agentRef.isStopped = true;
        
        var a = ServiceLocator.Instance.Get<AudioService>();
        if (aiModel._demon)
            a.PlayOneShot(a.Atlas.sfx.pnj.demon.demonSpotPlayer, aiModel.transform.position);
        else
            a.PlayOneShot(aiModel._male ? a.Atlas.sfx.pnj.male.maleSpotPlayer : a.Atlas.sfx.pnj.female.femaleSpotPlayer, aiModel.transform.position);
        
        aiModel._animatorRef.SetTrigger("Spotted");
        isEntered = false;
    }

    public override sbyte OnUpdate()
    {
        if (!isEntered)
        {
            isEntered = aiModel._animatorRef.GetCurrentAnimatorStateInfo(0).IsName("Spotted");
        }
        else if (!aiModel._animatorRef.GetCurrentAnimatorStateInfo(0).IsName("Spotted"))
        {
            if (AIController.DetectPlayer(aiModel))
                aiModel._currentWaypoint.position = aiModel._lastKnownPlayerPosition;
            AIController.UpdateAgent(aiModel);
            stateMachine.SwitchState("chase");
        }
        return 0;
    }

    public override sbyte OnFixedUpdate()
    {
        return 0;
    }

    public override void OnExitState()
    {
        aiModel._spottedPart.Stop();
        aiModel._agentRef.isStopped = false;
    }

    public override IEnumerator OnCoroutine()
    {
        yield return null;
    }

    #region fields

    private bool isEntered = false;

    #endregion
}