using GameContent.Player.Controller.LocalMachine.Model;
using GameContent.Player.Controller.BaseMachine;
using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.Controller.States
{
    public class IdleState : BasePlayerState
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
            //TODO anims
        }

        public override sbyte OnUpdate()
        {
            HandleInputGather();
            HandleRotateInputGather();

            OnMove();
            OnFall();
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            HandleGravity();
            Move(playerModel.currentMoveMultiplier);
            Look();

            return 0;
        }

        private void OnMove()
        {
            if (playerModel.inputDir.sqrMagnitude > 0.1f)
                stateMachine.SwitchState("move");
        }
        
        private void OnFall()
        {
            if (CheckGround())
                return;
            
            stateMachine.SwitchState("fall");
        }

        #endregion
    }
}