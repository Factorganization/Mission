using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Controller.States
{
    public sealed class LockedState : BasePlayerState
    {
        #region constructors
        
        public LockedState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {

        }
        
        #endregion
        
        #region methodes

        public override void OnEnterState()
        {
            playerModel.rb.linearVelocity = Vector3.zero;
            playerModel.rb.isKinematic = true;
        }

        #endregion
    }
}