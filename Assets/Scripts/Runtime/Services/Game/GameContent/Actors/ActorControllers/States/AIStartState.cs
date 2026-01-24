using System.Collections;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Utils.Coroutines;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States;

public class AIStartState : BaseAiState
{
    #region constructors

    public AIStartState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go, model, state)
    {
    }

    #endregion

    #region methodes

    public override void OnEnterState()
    {
        ServiceLocator.Instance.Get<GameService>().OnCoroutine(OnCoroutine());
    }
    
    public override sbyte OnUpdate()
    {
        return 0;
    }

    public override IEnumerator OnCoroutine()
    {
        yield return new WaitForSceneLoaded();
        
        yield return new WaitForSeconds(1f);
        stateMachine.ForceState("idle");
    }

    #endregion
}