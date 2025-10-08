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
        
        private void OnGrounded()
        {
            if (playerModel.CheckGround(goRef))
                stateMachine.SwitchState("move");
        }

        #endregion
    }
}