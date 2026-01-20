using System.Collections;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States
{
    public class BaseAiState : BaseState
    {
        #region

        protected BaseAiState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go)
        {
            aiModel = model;
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
            if (AIController.DetectPlayer(aiModel))
                stateMachine.SwitchState("suspicious");
            
            AIController.UpdateAgent(aiModel);
            
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
        
        protected readonly AIModel aiModel;
        
        #endregion
    }
}