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
            playerModel.targetDir = Vector3.zero;
            playerModel.rb.linearVelocity = Vector3.zero;
            playerModel.cam.SetParent(null, true);
            playerModel.isVisible = false;
            playerModel.graph.gameObject.SetActive(false);
        }

        public override sbyte OnUpdate()
        {
            playerModel.HandleRotateInputGather();
            if (playerModel.HandleMonoInputGather() == 2)
                if (OnAction())
                    return 1;
            
            if (playerModel.HandleMonoInputGather() == 1)
                if (OnLeavePossession())
                    return 1;
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            playerModel.SetCameraPivotPos(playerModel.currentPossessedObject.Transform.position);
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

        private bool OnAction()
        {
            if (!playerModel.currentPossessedObject.Action())
                return false;
            
            stateMachine.TrySwitchState("idle", (int) playerModel.data.activeStates);
            return true;
        }

        private bool OnLeavePossession()
        {
            stateMachine.TrySwitchState("idle", (int) playerModel.data.activeStates);
            
            return true;
        }

        #endregion
    }
}