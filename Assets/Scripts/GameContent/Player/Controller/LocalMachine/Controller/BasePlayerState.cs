using System.Collections;
using GameContent.Player.Controller.BaseMachine;
using GameContent.Player.Controller.LocalMachine.Model;
using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.Controller
{
    public class BasePlayerState : BaseState
    {
        #region properties

        public ControllerState StateFlag { get; }

        #endregion
        
        #region 
        
        public BasePlayerState(GameObject go, PlayerModel model, ControllerState state, PlayerStateMachine machine) : base(go)
        {
            playerModel = model;
            StateFlag = state;
            playerMachine = machine;
        }
        
        #endregion

        #region methodes
        
        public override void OnInit(GenericStateMachine machine)
        {
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

        protected readonly PlayerStateMachine playerMachine;
        
        protected PlayerModel playerModel;

        #endregion
    }
}