using Runtime.Services.Audio;
using Runtime.Services.Game.GameContent.Logics.LogicModels.MissionModels;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Services.Game.GameSystems;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Controller.States
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
            
            playerModel.SetAnimParam(playerModel.isWalking, true);
        }

        public override sbyte OnUpdate()
        {
            playerModel.HandleContinuousInputGather();
            playerModel.HandleRotateInputGather();
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
                        
                        var t = playerModel.currentGrabbedObject.ObjectType;
                        var a = ServiceLocator.Instance.Get<AudioService>();

                        switch (t)
                        {
                            // Book type ? Also for PQ ?
                            case ObjectType.I_Paper : 
                                a.PlayOneShot(a.Atlas.sfx.objects.book.bookPick, playerModel.currentGrabbedObject.Transform.position);
                                break;
                            
                            // Also for Glass Type ? also for Pans ? 
                            case ObjectType.I_Cookware : 
                                a.PlayOneShot(a.Atlas.sfx.objects.cutlery.cutleryPick, playerModel.currentGrabbedObject.Transform.position);
                                break;
                            
                            // Jerrican type ?
                            case ObjectType.I_Bucket : 
                                a.PlayOneShot(a.Atlas.sfx.objects.jerrican.jerricanPick, playerModel.currentGrabbedObject.Transform.position);
                                break;
                            
                            // Metal type ?
                            case ObjectType.I_Tool :
                                a.PlayOneShot(a.Atlas.sfx.objects.metal.metalPick, playerModel.currentGrabbedObject.Transform.position);
                                break;
                            
                            // Phone ? 
                            case ObjectType.I_Device : 
                                a.PlayOneShot(a.Atlas.sfx.objects.phone.phonePick, playerModel.currentGrabbedObject.Transform.position);
                                break;
                            
                            case ObjectType.I_Cloth :
                                a.PlayOneShot(a.Atlas.sfx.objects.tshirt.tshirtPick, playerModel.currentGrabbedObject.Transform.position);
                                break;
                            
                            default:
                                a.PlayOneShot(a.Atlas.sfx.objects.wood.woodPick, playerModel.currentGrabbedObject.Transform.position);
                                break;
                            
                        }
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
                
                //TODO
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