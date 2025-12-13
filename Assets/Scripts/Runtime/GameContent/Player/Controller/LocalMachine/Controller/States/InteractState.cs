using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Shared.Utils.BaseMachine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller.States
{
    public sealed class InteractState : BasePlayerState
    {
        #region constructors
        
        public InteractState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {
        }
        
        #endregion
    }
}