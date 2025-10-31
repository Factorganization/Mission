using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Utils.BaseMachine;
using UnityEngine;

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
            //TODO anims
        }

        public override sbyte OnUpdate()
        {
            playerModel.HandleContinuousInputGather();
            playerModel.HandleRotateInputGather();

            if (playerModel.HandleMonoInputGather() == 1)
            {
                var pg = playerModel.OnTryPossessGrab();
                
                switch (pg)
                {
                    case 1:
                        if (!stateMachine.TrySwitchState("possess", (int)playerModel.data.activeStates))
                        {
                            playerModel.currentPossessedObject = null;
                            return 1;
                        }
                        break;
                    
                    case 2:
                        playerModel.currentGrabbedObject.Rigidbody.isKinematic = true;
                        playerModel.currentGrabbedObject.Transform.SetParent(playerModel.grab, true);
                        return 1;
                    
                    case 0:
                        if (playerModel.currentGrabbedObject is not null)
                        {
                            playerModel.currentGrabbedObject.Rigidbody.isKinematic = false;
                            playerModel.currentGrabbedObject.Transform.SetParent(null, true);
                        }
                        break;
                }
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