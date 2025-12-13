using System.Collections;
using Shared.Utils.BaseMachine;

namespace Runtime.GameContent.Actors.ActorControllers.AIControllerStates
{
    public class AIIdleState : BaseState
    {
        public AIIdleState(GenericStateMachine machine, GameObject go) : base(machine, go)
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
                stateMachine.SwitchState("AIMoveState");
            
            Debug.Log("IdleAI");
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