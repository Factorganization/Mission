using System;
using GameContent.Player.Controller.BaseMachine;
using GameContent.Player.Controller.LocalMachine.Controller.States;
using GameContent.Player.Controller.LocalMachine.Model;
using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.View
{
    [SelectionBase]
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

            var idle = new IdleState(_stateMachine, gameObject, _playerModel, ControllerState.Idle);
            var move = new MoveState(_stateMachine, gameObject, _playerModel, ControllerState.Move);
            var fall = new FallState(_stateMachine, gameObject, _playerModel, ControllerState.Fall);
            var interact = new InteractState(_stateMachine, gameObject, _playerModel, ControllerState.Interact);
            var possess =  new PossessState(_stateMachine, gameObject, _playerModel, ControllerState.Possess);
            var menu = new MenuState(_stateMachine, gameObject, _playerModel, ControllerState.Menu);
            var locked = new LockedState(_stateMachine, gameObject, _playerModel, ControllerState.Locked);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Idle, "idle", idle.OnInit, idle.OnEnterState,
                idle.OnUpdate, idle.OnFixedUpdate, idle.OnExitState, idle.OnCoroutine);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Move, "move", move.OnInit, move.OnEnterState,
                move.OnUpdate, move.OnFixedUpdate, move.OnExitState, move.OnCoroutine);

            _stateMachine.SetCallBacks((byte)ControllerState.Fall, "fall", fall.OnInit, fall.OnEnterState,
                fall.OnUpdate, fall.OnFixedUpdate, fall.OnExitState, fall.OnCoroutine);
            
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
            NON = 0;
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

        private float NON
        {
            set
            {
                if (dataSo.cameraData.maxPitchAngle <= 89)
                    return;
                
                var color = Color.red;
                Debug.LogError(
                    $"<color=#{(byte)(color.r * 255f):X2}{(byte)(color.g * 255f):X2}{(byte)(color.b * 255f):X2}>par pitié depassez pas 89</color>");
            }
        }
        
        #endregion
    }
}