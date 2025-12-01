using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Shared.Utils.BaseMachine;
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
            var mono = playerModel.HandleMonoInputGather();

            switch (mono)
            {
                case 1:
                    if (playerModel.OnTryPossess() == 1)
                    {
                        if (stateMachine.TrySwitchState("possess", (int)playerModel.data.activeStates))
                            return 1;
                        
                        playerModel.currentPossessedObject = null;
                    }
                    break;
                
                case 6:
                    var tg = playerModel.OnTryGrab(out var gb);
                    switch (tg)
                    {
                        case 1 when playerModel.currentGrabbedObject is not null:
                            playerModel.ResetGrabbedObjectState();
                            playerModel.SetGrabbedObjectState(gb);
                            break;
                        
                        case 1:
							playerModel.SetGrabbedObjectState(gb);
                            break;
                        
                        case 0 when playerModel.currentGrabbedObject is not null:
							playerModel.ResetGrabbedObjectState();
                            break;
                    }
                    break;
                
                case 4:
                    if (playerModel.TryInteractGrabbedObject())
						//TODO des feedbacks ?
						break;
                    break;
                
                case 5:
                    playerModel.throwTimer += Time.deltaTime;
                    if (playerModel.throwTimer > playerModel.data.interactData.throwTimer)
                    {
                        playerModel.throwTimer = 0;
                        playerModel.TryThrowGrabbedObject();
                    }
                    break;
                
                case 7:
                    playerModel.throwTimer = 0;
                    break;
            }

            if (playerModel.OnJump())
            {
                if (stateMachine.TrySwitchState("jump", (int)playerModel.data.activeStates))
                    return 1;
            }
            if (!playerModel.CheckGround(goRef))
            {
                if (stateMachine.TrySwitchState("fall", (int)playerModel.data.activeStates))
                    return 1;
            }
            if (playerModel.OnIdle())
            {
                if (stateMachine.TrySwitchState("idle", (int)playerModel.data.activeStates))
                    return 1;
            }
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            playerModel.HandleRotateInputGather();
            playerModel.SetGrabbedObjectLocalPos(); //TODO cleanup callback plutot que verif a la frame
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