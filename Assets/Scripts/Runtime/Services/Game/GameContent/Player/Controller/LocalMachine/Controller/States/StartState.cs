using System.Collections;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Utils.Coroutines;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Controller.States;

public class StartState : BasePlayerState
{
    #region constructors
    
    public StartState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
    {
    }
    
    #endregion

    #region methodes

    public override void OnEnterState()
    {
        ServiceLocator.Instance.Get<GameService>().StartCoroutine(OnCoroutine());
    }

    public override IEnumerator OnCoroutine()
    {
        yield return new WaitForSceneLoaded();
        
        yield return new WaitForSeconds(1f);
        stateMachine.ForceState("idle");
    }

    #endregion
}