using System.Collections;
using Runtime.Services.Audio;
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
            _audioService = ServiceLocator.Instance.Get<AudioService>();
        }

        public override void OnEnterState()
        {
        }

        public override sbyte OnUpdate()
        {
            if (AIController.DetectPlayer(aiModel))
            {
                stateMachine.SwitchState("suspicious");
                return 0;
            }
            if (AIController.DetectDestroyedPossessable(aiModel))
            {
                stateMachine.SwitchState("move");
                return 0;
            }
            if (AIController.DetectGrabbable(aiModel))
            {
                stateMachine.SwitchState("move");
                return 0;
            }
            
            AIController.UpdateAgent(aiModel);

            if (_timer >= _waitTime)
            {
                if (aiModel._demon)
                    _audioService.PlayOneShot(_audioService.Atlas.sfx.pnj.demon.demonSuspicious, aiModel.transform.position);
                else
                    _audioService.PlayOneShot(aiModel._male ? _audioService.Atlas.sfx.pnj.male.maleSuspicious : _audioService.Atlas.sfx.pnj.female.femaleSuspicious, aiModel.transform.position);
                
                _timer = 0;
            }
                
            
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
        
        protected AudioService _audioService;
        protected readonly AIModel aiModel;
        protected float _timer;
        protected float _waitTime = 20f;
        
        #endregion
    }
}