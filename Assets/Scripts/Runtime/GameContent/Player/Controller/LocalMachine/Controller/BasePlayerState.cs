using System.Collections;
using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Utils.BaseMachine;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller
{
    public class BasePlayerState : BaseState
    {
        #region

        protected BasePlayerState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go)
        {
            playerModel = model;
        }
        
        #endregion

        #region methodes
        
        public override void OnInit(GenericStateMachine machine)
        {
            playerModel.HandleInputGather();
        }

        public override void OnEnterState()
        {
        }

        public override sbyte OnUpdate()
        {
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            return 0;
        }

        public override void OnExitState()
        {
        }

        public override IEnumerator OnCoroutine()
        {
            yield return null;
        }
        
        #endregion

        #region fields
        
        protected readonly PlayerModel playerModel;

        #endregion
    }
}