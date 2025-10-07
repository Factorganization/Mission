using GameContent.Player.Controller.LocalMachine.Model;
using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.Controller.States
{
    public class IdleState : BasePlayerState
    {
        #region constructors
        
        public IdleState(GameObject go, PlayerModel model, ControllerState state, PlayerStateMachine machine) : base(go, model, state, machine)
        {
        }
        
        #endregion
    }
}