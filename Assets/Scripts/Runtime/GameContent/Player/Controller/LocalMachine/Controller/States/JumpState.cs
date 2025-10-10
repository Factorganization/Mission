using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Utils.BaseMachine;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller.States
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
            playerModel.coyoteTime = 0;
            
            playerModel.castAddLength = 0;
            playerModel.rb.AddForce(Vector3.up * playerModel.data.jumpData.jumpStrength, ForceMode.VelocityChange);
            playerModel.isGrounded = false;
        }

        public override sbyte OnUpdate()
        {
            _jumpCounter += Time.deltaTime;
            
            playerModel.HandleInputGather();
            playerModel.HandleRotateInputGather();

            if (OnFall())
                return 1;
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            playerModel.Move(playerModel.currentMoveMultiplier);
            
            //TODO maybe ranger ca dans une Func d'update graph
            playerModel.graph.transform.rotation = Quaternion.Slerp(playerModel.graph.transform.rotation, Quaternion.LookRotation(playerModel.lastLookDir), playerModel.data.moveData.graphRotationSpeed * Time.fixedDeltaTime);
            
            playerModel.Look();
            
            if (_jumpCounter < GameConstants.AntiGroundGrabJumpTimer)
                return 0;

            if (OnGrounded())
                return 1;
            
            playerModel.HandleGravity(goRef);
            return 0;
        }

        public override void OnExitState()
        {
            _jumpCounter = 0;
        }
        
        private bool OnFall()
        {
            if (playerModel.rb.linearVelocity.y >= 0)
                return false;
            
            stateMachine.TrySwitchState("fall", (int)playerModel.data.activeStates);
            return true;
        }

        private bool OnGrounded()
        {
            if (!playerModel.CheckGround(goRef))
                return false;
            
            stateMachine.TrySwitchState("move", (int)playerModel.data.activeStates);
            return true;
        }

        #endregion

        #region fields

        private float _jumpCounter;

        #endregion
    }
}