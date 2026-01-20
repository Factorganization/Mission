using System.Collections;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States
{
    public class AIMoveState : BaseState
    {
        public AIMoveState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go)
        {
        }

        public override void OnInit(GenericStateMachine machine)
        {
        }

        public override void OnEnterState()
        {
        }

        public override sbyte OnUpdate()
        {
            stateMachine.SwitchState("AIIdleState");
            
            Debug.Log("MoveAI");
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
    }
}