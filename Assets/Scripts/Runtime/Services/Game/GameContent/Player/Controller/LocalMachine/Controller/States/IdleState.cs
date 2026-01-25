using Runtime.Services.Data;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Services.Game.GameSystems;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Controller.States
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
            playerModel.CheckGrab();
            playerModel.CheckPossessable();
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
                
                case 4:
                    if (playerModel.currentGrabbedObject is null && playerModel.canEndLevel)
                    {
                        ServiceLocator.Instance.Get<DataService>().SaveData();
                        GameManager.Instance.GameUIMgr.WinGame();
                        return 1;
                    }
                    
                    if (playerModel.TryInteractGrabbedObject())
                    {
                        playerModel.SetAnimParam(playerModel.isInteracting, true);
                        break;
                    }
                    playerModel.SetAnimParam(playerModel.isInteracting, false);
                    break;
                
                case 5:
                    if (playerModel.currentGrabbedObject is null)
                        break;

                    playerModel.throwTimer += Time.deltaTime;
                    if (playerModel.throwTimer > playerModel.data.interactData.throwTimer)
                    {
                        playerModel.throwTimer = 0;
                        if (playerModel.TryThrowGrabbedObject())
                        {
                            playerModel.canThrow = false;
                            playerModel.SetAnimParam(playerModel.isInteracting, false);
                            playerModel.SetAnimParam(playerModel.isHolding, false);
                            playerModel.SetAnimParam(playerModel.@throw);
                        }
                    }
                    break;
                
                case 6:
                    if (playerModel.currentGrabbedObject is not null)
                    {
                        playerModel.canThrow = true;
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
                
                case 7:
                    if (playerModel.currentGrabbedObject is null || !playerModel.canThrow)
                        break;

                    playerModel.canThrow = false;
                    playerModel.ResetGrabbedObjectState();
                    playerModel.SetAnimParam(playerModel.isHolding, false);
                    playerModel.SetAnimParam(playerModel.isInteracting, false);
                    playerModel.throwTimer = 0;
                    break;
                
                case 8:
                    GameManager.Instance.GameUIMgr.SetMissionPos(1);
                    break;
                
                case 9:
                    GameManager.Instance.GameUIMgr.SetMissionPos(0);
                    break;
                
                case 10:
                    stateMachine.TrySwitchState("menu", (int)playerModel.data.activeStates);
                    GameManager.Instance.GameUIMgr.PauseMenuUI.OpenPauseMenu();
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
            playerModel.SetGrabbedObjectLocalPos();
            playerModel.SetCameraPivotLocalPos(Vector3.zero);
            playerModel.HandleGravity(goRef);
            playerModel.Move(playerModel.currentMoveMultiplier);
            playerModel.Look();

            return 0;
        }

        #endregion
    }
}