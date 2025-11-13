using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Management.GameManagement;
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
                    var tg = playerModel.OnTryGrab();
                    switch (tg)
                    {
                        case 1:
                            //TODO cleanup ces merdes dans le controller
                            playerModel.currentGrabbedObject.Rigidbody.isKinematic = true;
                            playerModel.currentGrabbedObject.Transform.SetParent(playerModel.grab, true);
                            break;
                        
                        case 0 when playerModel.currentGrabbedObject is not null:
                            playerModel.currentGrabbedObject.Rigidbody.isKinematic = false;
                            playerModel.currentGrabbedObject.Transform.SetParent(null, true);
                            playerModel.currentGrabbedObject = null;
                            break;
                    }
                    break;
                
                case 4:
                    //TODO grab interaction, en fait retournement de situation y'en a pas donc faudra retirer et passet en callback de drop item
                    break;
                
                case 5:
                    playerModel.TryThrowGrabbedObject();
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
        
        private bool OnIdle()
        {
            if (playerModel.inputDir.sqrMagnitude >= 0.1f)
                return false;
            
            stateMachine.TrySwitchState("idle", (int)playerModel.data.activeStates);
            return true;

        }
        
        private bool OnJump()
        {
            if (playerModel.jumpBufferTime <= 0)
                return false;
            
            stateMachine.TrySwitchState("jump", (int)playerModel.data.activeStates);
            return true;
        }
        
        private bool OnFall()
        {
            if (playerModel.CheckGround(goRef))
                return false;
            
            stateMachine.TrySwitchState("fall", (int)playerModel.data.activeStates);
            return true;
        }
        
        private bool OnPossess()
        {
            var min = 100f;
            IPossessable tp = null;
            
            foreach (var p in LevelGenerator.Generator.Possessables)
            {
                var d = Vector3.Distance(p.Transform.position, playerModel.rb.position);

                if (d >= GameConstants.MaxPossessDistance || d > min)
                    continue;
                
                min = d;
                tp = p;
            }
            
            if (min > 2)
                return false;
            
            if (stateMachine.TrySwitchState("possess", (int)playerModel.data.activeStates))
                playerModel.currentPossesedObject = tp;
            
            return true;
        }

        #endregion
    }
}