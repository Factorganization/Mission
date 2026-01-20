using System.Collections;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States;

public class AIIdleState : BaseAiState
{
    public AIIdleState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go, model, state)
    {
       
    }

    public override void OnInit(GenericStateMachine machine)
    {
    }

    public override void OnEnterState()
    {
        Debug.Log("IdleAI");
    }

    public override sbyte OnUpdate()
    {
        base.OnUpdate();
        
        if (aiModel._currentWaypoint.position != Vector3.zero)
            stateMachine.SwitchState("move");
        
        _waitTimer += Time.deltaTime;
        if (_waitTimer >= aiModel.movementData.waitDelay)
        {
            AIController.SelectNextWaypoint(aiModel);
            _waitTimer = 0;
        }
        return 0;
    }

    public override sbyte OnFixedUpdate()
    {
        return 0;
    }

    public override void OnExitState()
    {
        _waitTimer = 0;
    }

    public override IEnumerator OnCoroutine()
    {
        yield return null;
    }
    
    #region fields
    
    private float _waitTimer;
    
    #endregion
}