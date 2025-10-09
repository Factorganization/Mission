using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Utils.BaseMachine;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller.States
{
    public sealed class IdleState : BasePlayerState
    {
        #region constructors
        
        public IdleState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            playerModel.isGrounded = true;
            playerModel.coyoteTime = playerModel.data.jumpData.jumpCoyoteTime;
            //TODO anims
        }

        public override sbyte OnUpdate()
        {
            playerModel.HandleInputGather();
            playerModel.HandleRotateInputGather();

            OnJump();
            OnFall();
            OnMove();
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            playerModel.HandleGravity(goRef);
            playerModel.Move(playerModel.currentMoveMultiplier);
            playerModel.Look();

            return 0;
        }

        private void OnMove()
        {
            if (playerModel.inputDir.sqrMagnitude > 0.1f)
                stateMachine.SwitchState("move");
        }
        
        private void OnJump()
        {
            if (playerModel.jumpBufferTime > 0)
                stateMachine.SwitchState("jump");
        }
        
        private void OnFall()
        {
            if (playerModel.CheckGround(goRef))
                return;
            
            stateMachine.SwitchState("fall");
        }

        #endregion
    }
}