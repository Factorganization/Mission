using System;
using Runtime.GameContent.Actors.ActorControllers.AIControllerStates;
using UnityEngine;
using Runtime.Utils.BaseMachine;

namespace Runtime.GameContent.Actors.ActorViews
{
    public class testStateMachine : MonoBehaviour
    {
        private void Start()
        {
            _stateMachine = new GenericStateMachine(2);
            
            var idle = new AIIdleState(_stateMachine, gameObject);
            var move = new AIMoveState(_stateMachine, gameObject);

            _stateMachine.SetCallBacks(0, "AIIdleState", idle.OnInit, idle.OnEnterState, idle.OnUpdate, idle.OnFixedUpdate, idle.OnExitState, null);
            _stateMachine.SetCallBacks(1, "AIMoveState", move.OnInit, move.OnEnterState, move.OnUpdate, move.OnFixedUpdate, move.OnExitState, null);
            
            _stateMachine.InitMachine();
            _stateMachine.StartMachine();
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
        }
        
        private void FixedUpdate()
        {
            _stateMachine.FixedUpdateMachine();
        }

        private GenericStateMachine _stateMachine;
    }
}