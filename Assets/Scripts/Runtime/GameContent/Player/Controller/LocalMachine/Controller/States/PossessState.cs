using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Utils.BaseMachine;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller.States
{
    public sealed class PossessState : BasePlayerState
    {
        #region constructors
        
        public PossessState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            _destructTimer = 0;
            playerModel.targetDir = Vector3.zero;
            playerModel.rb.linearVelocity = Vector3.zero;
            playerModel.cam.SetParent(playerModel.currentPossessedObject.Transform, true);
            playerModel.isVisible = false;
            playerModel.graph.gameObject.SetActive(false);
        }

        public override sbyte OnUpdate()
        {
            playerModel.HandleRotateInputGather();
            var mono = playerModel.HandleMonoInputGather();

            switch (mono)
            {
                case 1:
                    if (stateMachine.TrySwitchState("idle", (int) playerModel.data.activeStates))
                        return 1;
                    break;
                
                case 2:
                    _destructTimer += Time.deltaTime;
                    if (_destructTimer > GameConstants.DestructiveActionTime)
                    {
                        playerModel.OnDestructiveAction();
                        
                        if (stateMachine.TrySwitchState("idle", (int) playerModel.data.activeStates))
                            return 1;
                    }
                    break;
                
                case 3:
                    _destructTimer = 0;
                    playerModel.OnAction();
                    break;
            }
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            playerModel.SetCameraPivotLocalPos(Vector3.zero);
            playerModel.Look();
            
            return 0;
        }

        public override void OnExitState()
        {
            playerModel.currentPossessedObject = null;
            playerModel.cam.SetParent(playerModel.rb.transform, true);
            playerModel.isVisible = true;
            playerModel.graph.gameObject.SetActive(true);
        }

        #endregion
        
        #region fields

        private float _destructTimer;

        #endregion
    }
}