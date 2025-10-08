using GameContent.Player.Controller.LocalMachine.Model;
using GameContent.Player.Controller.BaseMachine;
using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.Controller.States
{
    public class MoveState : BasePlayerState
    {
        #region constructors
        
        public MoveState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            Move(playerModel.currentMoveMultiplier);
            playerModel.isGrounded = true;
            //TODO anims
        }

        public override sbyte OnUpdate()
        {
            HandleInputGather();
            HandleRotateInputGather();
            
            OnFall();
            OnIdle();
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            HandleGravity();
            Move(playerModel.currentMoveMultiplier);
            
            //TODO maybe ranger ca dans une Func d'update graph
            playerModel.graph.transform.rotation = Quaternion.Slerp(playerModel.graph.transform.rotation, Quaternion.LookRotation(playerModel.lastLookDir), playerModel.data.moveData.graphRotationSpeed * Time.fixedDeltaTime);
            
            Look();
            
            return 0;
        }
        
        private void OnIdle()
        {
            if (playerModel.inputDir.sqrMagnitude < 0.1f)
                stateMachine.SwitchState("idle");
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