using GameContent.Player.Controller.BaseMachine;
using GameContent.Player.Controller.LocalMachine.Model;
using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.Controller.States
{
    public class FallState : BasePlayerState
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
            
            Debug.Log(playerModel.inputDir.sqrMagnitude);
            HandleInputGather();
            HandleRotateInputGather();
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            OnGrounded();
            
            HandleGravity();
            Move(playerModel.currentMoveMultiplier);
            Look();
            
            return 0;
        }
        
        private void OnGrounded()
        {
            if (CheckGround())
                stateMachine.SwitchState("move");
        }

        #endregion
    }
}