using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Services.Game.GameSystems;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Controller.States
{
    public sealed class MenuState : BasePlayerState
    {
        #region constructors
        
        public MenuState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {
        }
        
        #endregion

        #region methodes

        public override sbyte OnUpdate()
        {
            if (playerModel.data.inputData.menuInput.action.WasPressedThisFrame())
            {
                GameManager.Instance.GameUIMgr.PauseMenuUI.Hide();
                stateMachine.TrySwitchState(playerModel.currentPossessedObject is not null ? "possess" : "idle", (int)playerModel.data.activeStates);
            }
            
            return base.OnUpdate();
        }

        #endregion
        
    }
}