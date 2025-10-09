using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Utils.BaseMachine;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller.States
{
    public sealed class FallState : BasePlayerState
    {
        #region constructors
        
        public FallState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {
        }
        
        #endregion
        
        #region methodes

        public override void OnEnterState()
        {
            playerModel.isGrounded = false;
        }
        
        public override sbyte OnUpdate()
        {
            playerModel.HandleInputGather();
            playerModel.HandleRotateInputGather();
            
            playerModel.coyoteTime -= Time.deltaTime;
            OnJump();
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            OnGrounded();
            
            playerModel.HandleGravity(goRef);
            playerModel.Move(playerModel.currentMoveMultiplier);
            playerModel.Look();
            
            return 0;
        }
        
        private void OnJump()
        {
            if (playerModel.coyoteTime > 0
                && playerModel.jumpBufferTime > 0)
                stateMachine.TrySwitchState("jump", (int)playerModel.data.activeStates);
        }
        
        private void OnGrounded()
        {
            if (playerModel.CheckGround(goRef))
                stateMachine.TrySwitchState("move", (int)playerModel.data.activeStates);
        }

        #endregion
    }
}