using GameContent.Player.Controller.LocalMachine.Model;
using GameContent.Player.Controller.BaseMachine;
using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.Controller.States
{
    public class PossessState : BasePlayerState
    {
        #region constructors
        
        public PossessState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go, model, state)
        {
        }
        
        #endregion
    }
}