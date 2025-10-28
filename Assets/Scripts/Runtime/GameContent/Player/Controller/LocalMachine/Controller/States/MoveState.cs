using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Utils.BaseMachine;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller.States
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
            playerModel.coyoteTime = playerModel.data.jumpData.jumpCoyoteTime;
            //TODO anims
        }

        public override sbyte OnUpdate()
        {
            playerModel.HandleContinuousInputGather();
            playerModel.HandleRotateInputGather();
            
            if (playerModel.HandleMonoInputGather() == 1)
                if (playerModel.OnPossess())
                    if (!stateMachine.TrySwitchState("possess", (int)playerModel.data.activeStates))
                    {
                        playerModel.currentPossessedObject = null;
                        return 1;
                    }

            if (playerModel.OnJump())
            {
                stateMachine.TrySwitchState("jump", (int)playerModel.data.activeStates);
                return 1;
            }
            if (!playerModel.CheckGround(goRef))
            {
                stateMachine.TrySwitchState("fall", (int)playerModel.data.activeStates);
                return 1;
            }
            if (playerModel.OnIdle())
            {
                stateMachine.TrySwitchState("idle", (int)playerModel.data.activeStates);
                return 1;
            }
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            playerModel.SetCameraPivotLocalPos(Vector3.zero);
            playerModel.HandleGravity(goRef);
            playerModel.Move(playerModel.currentMoveMultiplier);
            
            //TODO maybe ranger ca dans une Func d'update graph
            playerModel.graph.transform.rotation = Quaternion.Slerp(playerModel.graph.transform.rotation, Quaternion.LookRotation(playerModel.lastLookDir), playerModel.data.moveData.graphRotationSpeed * Time.fixedDeltaTime);
            
            playerModel.Look();
            
            return 0;
        }

        #endregion
    }
}