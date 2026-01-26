using System.Collections;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Services.Game.GameSystems;
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
        playerModel.graph.gameObject.SetActive(false);
        ServiceLocator.Instance.Get<GameService>().StartCoroutine(OnCoroutine());
    }

    public override sbyte OnUpdate()
    {
        playerModel.HandleContinuousInputGather();
        var mono = playerModel.HandleMonoInputGather();

        switch (mono)
        {
            case 8:
                GameManager.Instance.GameUIMgr.SetMissionPos(1);
                break;
                
            case 9:
                GameManager.Instance.GameUIMgr.SetMissionPos(0);
                break;
        }
        
        return 0;
    }

    public override sbyte OnFixedUpdate()
    {
        playerModel.HandleRotateInputGather();
        playerModel.Look();
        
        return 0;
    }

    public override IEnumerator OnCoroutine()
    {
        yield return new WaitForSceneLoaded();

        yield return new WaitForSeconds(1f);
        playerModel.possessParticles.Play();
        yield return new WaitForSeconds(0.75f);
        playerModel.graph.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(1f);
        
        stateMachine.ForceState("idle");
    }

    #endregion
}