using System;
using Runtime.GameContent.Player.Controller.LocalMachine.Controller.States;
using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Utils.BaseMachine;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.View
{
    [SelectionBase]
    public class PlayerStateMachine : MonoBehaviour
    {
        #region methodes

        private void Awake()
        {
            _playerModel = new PlayerModel(dataSo, referenceData.rb, referenceData.graph, referenceData.cam, referenceData.animator);
            _stateMachine = new GenericStateMachine(Enum.GetNames(typeof(ControllerState)).Length);

            var idle = new IdleState(_stateMachine, gameObject, _playerModel, ControllerState.Idle);
            var move = new MoveState(_stateMachine, gameObject, _playerModel, ControllerState.Move);
            var jump = new JumpState(_stateMachine, gameObject, _playerModel, ControllerState.Jump);
            var fall = new FallState(_stateMachine, gameObject, _playerModel, ControllerState.Fall);
            var interact = new InteractState(_stateMachine, gameObject, _playerModel, ControllerState.Interact);
            var possess =  new PossessState(_stateMachine, gameObject, _playerModel, ControllerState.Possess);
            var menu = new MenuState(_stateMachine, gameObject, _playerModel, ControllerState.Menu);
            var locked = new LockedState(_stateMachine, gameObject, _playerModel, ControllerState.Locked);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Idle), "idle", idle.OnInit, idle.OnEnterState,
                idle.OnUpdate, idle.OnFixedUpdate, idle.OnExitState, idle.OnCoroutine);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Move), "move", move.OnInit, move.OnEnterState,
                move.OnUpdate, move.OnFixedUpdate, move.OnExitState, move.OnCoroutine);

            _stateMachine.SetCallBacks(SetId((int)ControllerState.Jump), "jump", jump.OnInit, jump.OnEnterState,
                jump.OnUpdate, jump.OnFixedUpdate, jump.OnExitState, jump.OnCoroutine);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Fall), "fall", fall.OnInit, fall.OnEnterState,
                fall.OnUpdate, fall.OnFixedUpdate, fall.OnExitState, fall.OnCoroutine);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Interact), "interact", interact.OnInit, interact.OnEnterState,
                interact.OnUpdate, interact.OnFixedUpdate, interact.OnExitState, interact.OnCoroutine);

            _stateMachine.SetCallBacks(SetId((int)ControllerState.Possess), "possess", possess.OnInit, possess.OnEnterState,
                possess.OnUpdate, possess.OnFixedUpdate, possess.OnExitState, possess.OnCoroutine);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Menu), "menu", menu.OnInit, menu.OnEnterState,
                menu.OnUpdate, menu.OnFixedUpdate, menu.OnExitState, menu.OnCoroutine);
            
            _stateMachine.SetCallBacks(SetId((int)ControllerState.Locked), "locked", locked.OnInit, locked.OnEnterState,
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

        private static int SetId(int state)
        {
            var i = 0;
            while (state != 1)
            {
                i++;
                state >>= 1;
            }
            return i;
        }

        #endregion

        #region fields

        [SerializeField] private PlayerDataSo dataSo;

        [SerializeField] private ReferenceData referenceData;

        private GenericStateMachine _stateMachine;

        private PlayerModel _playerModel;

        private float NON
        {
            set
            {
                if (dataSo.cameraData.maxPitchAngle <= 89)
                    return;
                
                var color = Color.red;
                dataSo.cameraData.maxPitchAngle = 89;
                Debug.LogError(
                    $"<color=#{(byte)(color.r * 255f):X2}{(byte)(color.g * 255f):X2}{(byte)(color.b * 255f):X2}>par pitié depassez pas 89</color>");
            }
        }

        [Serializable]
        private class ReferenceData
        {
            [SerializeField] internal Rigidbody rb;
                    
            [SerializeField] internal Transform cam;
            
            [SerializeField] internal Transform graph;
                    
            [SerializeField] internal Animator animator;
        }
        
        #endregion
    }
}