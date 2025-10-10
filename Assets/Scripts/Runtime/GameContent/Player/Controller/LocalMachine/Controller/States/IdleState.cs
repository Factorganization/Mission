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

            if (OnJump())
                return 1;
            if (OnFall())
                return 1;
            if (OnMove())
                return 1;
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            playerModel.HandleGravity(goRef);
            playerModel.Move(playerModel.currentMoveMultiplier);
            playerModel.Look();

            return 0;
        }

        private bool OnMove()
        {
            if (playerModel.inputDir.sqrMagnitude <= 0.1f)
                return false;
            
            stateMachine.TrySwitchState("move", (int)playerModel.data.activeStates);
            return true;
        }
        
        private bool OnJump()
        {
            if (playerModel.jumpBufferTime <= 0)
                return false;
            
            stateMachine.TrySwitchState("jump", (int)playerModel.data.activeStates);
            return true;
        }
        
        private bool OnFall()
        {
            if (playerModel.CheckGround(goRef))
                return false;
            
            stateMachine.TrySwitchState("fall", (int)playerModel.data.activeStates);
            return true;
        }

        #endregion
    }
}