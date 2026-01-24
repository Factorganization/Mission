using System.Collections;
using Runtime.Services.Audio;
using Runtime.Services.Game.GameContent.Actors.ActorModels;
using Runtime.Services.Game.GameSystems;
using Shared.Utils.BaseMachine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States
{
    public class AIChaseState : BaseAiState
    {
        public AIChaseState(GenericStateMachine machine, GameObject go, AIModel model, AIControllerState state) : base(machine, go, model, state)
        {
        }

        public override void OnInit(GenericStateMachine machine)
        {
        }

        public override void OnEnterState()
        {
            aiModel._agentRef.speed = aiModel.movementData.chaseSpeed;
            aiModel._animatorRef.SetBool("ac_isRunning", true);
            var a = ServiceLocator.Instance.Get<AudioService>();
            a.PlayOneShot(aiModel._male ? a.Atlas.sfx.pnj.male.maleChase : a.Atlas.sfx.pnj.female.femaleChase, aiModel.transform.position);
        }

        public override sbyte OnUpdate()
        {
            if (Vector3.Distance(aiModel.transform.position, aiModel._player.transform.position) <= 1f)
            {
                var p = aiModel._player;
                aiModel._animatorRef.SetBool("ac_playerCaught", true);
                p.StateMachine.ForceState("locked");
                GameManager.Instance.GameUIMgr.GameOver();
            }
            
            if (_forgetTimer >= aiModel.detectionData.timeToForget)
            {
                _forgetTimer = 0;
                stateMachine.SwitchState("idle");
            }
            
            AIController.UpdateAgent(aiModel);
            if (AIController.DetectPlayer(aiModel))
            {
                aiModel._currentWaypoint.position = aiModel._lastKnownPlayerPosition;
                _forgetTimer = 0;
            }
            else _forgetTimer += Time.deltaTime;
            
            if (Vector3.Distance(aiModel.transform.position, aiModel._lastKnownPlayerPosition) < 1f)
            {
                stateMachine.SwitchState("suspicious");
            }
            
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            return 0;
        }

        public override void OnExitState()
        {
            aiModel._agentRef.speed = aiModel.movementData.patrolSpeed;
            aiModel._animatorRef.SetBool("ac_isRunning", false);
        }

        public override IEnumerator OnCoroutine()
        {
            yield return null;
        }
        
        #region fields

        private float _forgetTimer; 

        #endregion
    }
}