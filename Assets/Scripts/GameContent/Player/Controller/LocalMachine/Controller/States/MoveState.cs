using GameContent.Player.Controller.LocalMachine.Model;
using UnityEngine;
using Utils.BaseMachine;

namespace GameContent.Player.Controller.LocalMachine.Controller.States
{
    public sealed class MoveState : BasePlayerState
    {
        #region constructors
        
        public MoveState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            playerModel.Move(playerModel.currentMoveMultiplier);
            playerModel.isGrounded = true;
            //TODO anims
        }

        public override sbyte OnUpdate()
        {
            playerModel.HandleInputGather();
            playerModel.HandleRotateInputGather();
            
            OnFall();
            OnIdle();
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            playerModel.HandleGravity(goRef);
            playerModel.Move(playerModel.currentMoveMultiplier);
            
            //TODO maybe ranger ca dans une Func d'update graph
            playerModel.graph.transform.rotation = Quaternion.Slerp(playerModel.graph.transform.rotation, Quaternion.LookRotation(playerModel.lastLookDir), playerModel.data.moveData.graphRotationSpeed * Time.fixedDeltaTime);
            
            playerModel.Look();
            
            return 0;
        }
        
        private void OnIdle()
        {
            if (playerModel.inputDir.sqrMagnitude < 0.1f)
                stateMachine.SwitchState("idle");
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