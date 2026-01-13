using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model;
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
                //TODO close menu
                stateMachine.TrySwitchState("menu", (int)playerModel.data.activeStates);
            }
            
            return base.OnUpdate();
        }

        #endregion
    }
}