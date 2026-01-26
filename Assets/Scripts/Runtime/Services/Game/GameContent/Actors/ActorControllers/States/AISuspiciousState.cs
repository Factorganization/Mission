using System.Collections;
using Runtime.Services.Audio;
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
        aiModel._suspiciousPart.Play();
        aiModel._agentRef.isStopped = true;
        aiModel._animatorRef.SetBool("ac_isSus", true);
        
        var a = ServiceLocator.Instance.Get<AudioService>();
        if (aiModel._old)
            a.PlayOneShot(a.Atlas.sfx.pnj.demon.demonSuspicious, aiModel.transform.position);
        else if (aiModel._demon)
            a.PlayOneShot(a.Atlas.sfx.pnj.demon.demonSuspicious, aiModel.transform.position);
        else
            a.PlayOneShot(aiModel._male ? a.Atlas.sfx.pnj.male.maleSuspicious : a.Atlas.sfx.pnj.female.femaleSuspicious, aiModel.transform.position);

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
            stateMachine.SwitchState("move");
            return 0;
        }

        if (_detectionTimer >= aiModel.detectionData.detectionTime)
        {
            _detectionTimer = 0;
            _forgetTimer = 0;
            AIController.DropObject(aiModel);
            stateMachine.SwitchState("spotted");
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
        aiModel._suspiciousPart.Stop();
        aiModel._agentRef.isStopped = false;
        aiModel._animatorRef.SetBool("ac_isSus", false);
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