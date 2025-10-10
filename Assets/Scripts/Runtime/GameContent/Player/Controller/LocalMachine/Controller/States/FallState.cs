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
            if (OnJump())
                return 1;
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            if (OnGrounded())
                return 1;
            
            playerModel.HandleGravity(goRef);
            playerModel.Move(playerModel.currentMoveMultiplier);
            
            //TODO maybe ranger ca dans une Func d'update graph
            playerModel.graph.transform.rotation = Quaternion.Slerp(playerModel.graph.transform.rotation, Quaternion.LookRotation(playerModel.lastLookDir), playerModel.data.moveData.graphRotationSpeed * Time.fixedDeltaTime);
            
            playerModel.Look();
            
            return 0;
        }
        
        private bool OnJump()
        {
            if (playerModel.coyoteTime <= 0
                || playerModel.jumpBufferTime <= 0)
                return false;
                
            stateMachine.TrySwitchState("jump", (int)playerModel.data.activeStates);
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
    }
}