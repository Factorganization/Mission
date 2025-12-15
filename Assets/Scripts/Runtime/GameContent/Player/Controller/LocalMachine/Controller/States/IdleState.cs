using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Shared.Utils.BaseMachine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller.States
{
    public sealed class IdleState : BasePlayerState
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
            playerModel.coyoteTime = playerModel.data.jumpData.jumpCoyoteTime;
            
            playerModel.SetAnimParam(playerModel.isWalking, false);
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
                    if (playerModel.currentGrabbedObject is not null)
                    {
                        playerModel.ResetGrabbedObjectState();
                        playerModel.SetAnimParam(playerModel.isHolding, false);
                        playerModel.SetAnimParam(playerModel.isInteracting, false);
                        break;
                    }
                    
                    var tg = playerModel.OnTryGrab(out var gb);
                    if (tg == 1)
                    {
                        playerModel.SetGrabbedObjectState(gb);
                        playerModel.SetAnimParam(playerModel.isHolding, true);
                        playerModel.SetAnimParam(playerModel.isInteracting, false);
                    }
                    break;
                
                case 4:
                    if (playerModel.TryInteractGrabbedObject())
                    {
                        playerModel.SetAnimParam(playerModel.isInteracting, true);
                        break;
                    }
                    playerModel.SetAnimParam(playerModel.isInteracting, false);
                    break;
                
                case 5:
                    playerModel.throwTimer += Time.deltaTime;
                    if (playerModel.throwTimer > playerModel.data.interactData.throwTimer)
                    {
                        playerModel.throwTimer = 0;
                        if (playerModel.TryThrowGrabbedObject())
                        {
                            playerModel.SetAnimParam(playerModel.isInteracting, false);
                            playerModel.SetAnimParam(playerModel.isHolding, false);
                            playerModel.SetAnimParam(playerModel.@throw);
                        }
                    }
                    break;
                
                case 7:
                    playerModel.throwTimer = 0;
                    break;
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
            if (playerModel.OnMove())
            {
                stateMachine.TrySwitchState("move", (int)playerModel.data.activeStates);
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
            playerModel.Look();

            return 0;
        }

        #endregion
    }
}