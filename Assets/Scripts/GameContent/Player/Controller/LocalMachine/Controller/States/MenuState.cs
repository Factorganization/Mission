using GameContent.Player.Controller.LocalMachine.Model;
using GameContent.Player.Controller.BaseMachine;
using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.Controller.States
{
    public class MenuState : BasePlayerState
    {
        #region constructors
        
        public MenuState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {
        }
        
        #endregion
    }
}