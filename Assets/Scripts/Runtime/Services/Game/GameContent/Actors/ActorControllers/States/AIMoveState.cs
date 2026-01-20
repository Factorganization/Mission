using System.Collections;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Shared.Utils.BaseMachine;
using TMPro;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States
{
    public class AIMoveState : BaseAiState
    {
        public AIMoveState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go, model, state)
        {
        }

        public override void OnInit(GenericStateMachine machine)
        {
        }

        public override void OnEnterState()
        {
            Debug.Log("MoveAI");
        }

        public override sbyte OnUpdate()
        {
            base.OnUpdate();
            
            //test to remove
            stateMachine.SwitchState("AIIdleState");
            

            
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
        
        #region fields
        

        #endregion
    }
}