using System;
using GameContent.Player.Controller.BaseMachine;
using GameContent.Player.Controller.LocalMachine.Controller.States;
using GameContent.Player.Controller.LocalMachine.Model;
using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.Controller
{
    public class PlayerStateMachine : MonoBehaviour
    {
        #region properties
        //to refactor, too much old code
        
        public PlayerDataSo DataSo => dataSo;
        
        public Transform Cam => cam;
        
        public Animator Animator => animator;

        public PlayerModel PlayerModel => _playerModel;

        #endregion

        #region methodes

        private void Awake()
        {
            _playerModel = new PlayerModel(dataSo, rb, graph, cam, animator);
            _stateMachine = new GenericStateMachine(Enum.GetNames(typeof(ControllerState)).Length);

            var idle = new IdleState(gameObject, _playerModel, ControllerState.Idle, this);
            var move = new MoveState(gameObject, _playerModel, ControllerState.Move, this);
            var interact = new InteractState(gameObject, _playerModel, ControllerState.Interact, this);
            var possess =  new PossessState(gameObject, _playerModel, ControllerState.Possess, this);
            var menu = new MenuState(gameObject, _playerModel, ControllerState.Menu, this);
            var locked = new LockedState(gameObject, _playerModel, ControllerState.Locked, this);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Idle, "idle", idle.OnInit, idle.OnEnterState,
                idle.OnUpdate, idle.OnFixedUpdate, idle.OnExitState, idle.OnCoroutine);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Move, "move", move.OnInit, move.OnEnterState,
                move.OnUpdate, move.OnFixedUpdate, move.OnExitState, move.OnCoroutine);

            _stateMachine.SetCallBacks((byte)ControllerState.Interact, "interact", interact.OnInit, interact.OnEnterState,
                interact.OnUpdate, interact.OnFixedUpdate, interact.OnExitState, interact.OnCoroutine);

            _stateMachine.SetCallBacks((byte)ControllerState.Possess, "possess", possess.OnInit, possess.OnEnterState,
                possess.OnUpdate, possess.OnFixedUpdate, possess.OnExitState, possess.OnCoroutine);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Menu, "menu", menu.OnInit, menu.OnEnterState,
                menu.OnUpdate, menu.OnFixedUpdate, menu.OnExitState, menu.OnCoroutine);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Locked, "locked", locked.OnInit, locked.OnEnterState,
                locked.OnUpdate, locked.OnFixedUpdate, locked.OnExitState, locked.OnCoroutine);
        }

        private void Start()
        {
            _stateMachine.InitMachine();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdateMachine();
        }

        #endregion

        #region fields

        [SerializeField] private PlayerDataSo dataSo;

        [SerializeField] private Rigidbody rb;
        
        [SerializeField] private Transform cam;

        [SerializeField] private Transform graph;
        
        [SerializeField] private Animator animator;
        
        private GenericStateMachine _stateMachine;

        private PlayerModel _playerModel;

        #endregion
    }
}