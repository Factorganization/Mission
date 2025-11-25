using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Utils.BaseMachine;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller.States
{
    public sealed class JumpState : BasePlayerState
    {
        #region constructors
        
        public JumpState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            playerModel.rb.linearVelocity = new Vector3(playerModel.rb.linearVelocity.x, 0, playerModel.rb.linearVelocity.z);
            
            playerModel.jumpBufferTime = 0;
            playerModel.coyoteTime = 0;
            
            playerModel.castAddLength = 0;
            playerModel.rb.AddForce(Vector3.up * playerModel.data.jumpData.jumpStrength, ForceMode.VelocityChange);
            playerModel.isGrounded = false;
        }

        public override sbyte OnUpdate()
        {
            _jumpCounter += Time.deltaTime;
            
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
            
            if (playerModel.OnFall())
            {
                stateMachine.TrySwitchState("fall", (int)playerModel.data.activeStates);
                return 1;
            }
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            playerModel.HandleRotateInputGather();
            playerModel.SetGrabbedObjectLocalPos(); //TODO cleanup callback plutot que verif a la frame
            playerModel.SetCameraPivotLocalPos(Vector3.zero);
            playerModel.Move(playerModel.currentMoveMultiplier);
            
            //TODO maybe ranger ca dans une Func d'update graph
            playerModel.graph.transform.rotation = Quaternion.Slerp(playerModel.graph.transform.rotation, Quaternion.LookRotation(playerModel.lastLookDir), playerModel.data.moveData.graphRotationSpeed * Time.fixedDeltaTime);
            
            playerModel.Look();
            
            if (_jumpCounter < GameConstants.AntiGroundGrabJumpTimer)
                return 0;

            if (playerModel.CheckGround(goRef))
            {
                stateMachine.TrySwitchState("move", (int)playerModel.data.activeStates);
                return 1;
            }
            
            playerModel.HandleGravity(goRef);
            return 0;
        }

        public override void OnExitState()
        {
            _jumpCounter = 0;
        }

        #endregion

        #region fields

        private float _jumpCounter;

        #endregion
    }
}