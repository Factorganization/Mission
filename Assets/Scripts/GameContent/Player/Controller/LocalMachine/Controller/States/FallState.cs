using GameContent.Player.Controller.LocalMachine.Model;
using UnityEngine;
using Utils.BaseMachine;

namespace GameContent.Player.Controller.LocalMachine.Controller.States
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
                stateMachine.SwitchState("jump");
        }
        
        private void OnGrounded()
        {
            if (playerModel.CheckGround(goRef))
                stateMachine.SwitchState("move");
        }

        #endregion
    }
}