using GameContent.Player.Controller.LocalMachine.Model;
using UnityEngine;
using Utils.BaseMachine;

namespace GameContent.Player.Controller.LocalMachine.Controller.States
{
    public sealed class JumpState : BasePlayerState
    {
        #region constructors
        
        public JumpState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            playerModel.rb.linearVelocity = new Vector3(playerModel.rb.linearVelocity.x, 0, playerModel.rb.linearVelocity.z);
            
            playerModel.jumpBufferTime = 0;
            
            playerModel.castAddLength = 0;
            playerModel.rb.AddForce(Vector3.up * playerModel.data.jumpData.jumpStrength, ForceMode.VelocityChange);
            playerModel.isGrounded = false;
        }

        public override sbyte OnUpdate()
        {
            _jumpCounter += Time.deltaTime;
            
            playerModel.HandleInputGather();
            playerModel.HandleRotateInputGather();
            
            OnFall();
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            playerModel.Move(playerModel.currentMoveMultiplier);
            playerModel.Look();
            
            if (_jumpCounter < GameConstants.AntiGroundGrabJumpTimer)
                return 0;
         
            OnGrounded();
            playerModel.HandleGravity(goRef);
            return 0;
        }

        public override void OnExitState()
        {
            _jumpCounter = 0;
        }
        
        private void OnFall()
        {
            if (playerModel.rb.linearVelocity.y < 0)
                stateMachine.SwitchState("fall");
        }

        private void OnGrounded()
        {
            if (playerModel.CheckGround(goRef))
                stateMachine.SwitchState("move");
        }

        #endregion

        #region fields

        private float _jumpCounter;

        #endregion
    }
}