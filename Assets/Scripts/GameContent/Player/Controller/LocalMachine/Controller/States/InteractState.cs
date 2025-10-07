using GameContent.Player.Controller.LocalMachine.Model;
using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.Controller.States
{
    public class InteractState : BasePlayerState
    {
        #region constructors
        
        public InteractState(GameObject go, PlayerModel model, ControllerState state, PlayerStateMachine machine) : base(go, model, state, machine)
        {
        }
        
        #endregion
    }
}