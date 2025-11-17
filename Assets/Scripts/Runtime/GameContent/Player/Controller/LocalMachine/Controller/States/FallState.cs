using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Utils.BaseMachine;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller.States
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
                    playerModel.TryThrowGrabbedObject();
                    break;
            }
            
            playerModel.coyoteTime -= Time.deltaTime;
            if (playerModel.OnJump())
            {
                stateMachine.TrySwitchState("jump", (int)playerModel.data.activeStates);
                return 1;
            }
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            playerModel.HandleRotateInputGather();
            playerModel.SetGrabbedObjectLocalPos(); //TODO cleanup callback plutot que verif a la frame
            playerModel.SetCameraPivotLocalPos(Vector3.zero);

            if (playerModel.CheckGround(goRef))
            {
                stateMachine.TrySwitchState("move", (int)playerModel.data.activeStates);
                return 1;
            }
            
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