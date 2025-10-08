using System.Collections;
using UnityEngine;

namespace GameContent.Player.Controller.BaseMachine
{
    public abstract class BaseState
    {
        #region constructor

        protected BaseState(GameObject go)
        {
            goRef = go;
        }
        
        #endregion

        #region methodes

        public abstract void OnInit(GenericStateMachine machine);

        public abstract void OnEnterState();

        public abstract sbyte OnUpdate();

        public abstract sbyte OnFixedUpdate();

        public abstract void OnExitState();

        public abstract IEnumerator OnCoroutine();

        #endregion
        
        #region fields

        protected GenericStateMachine stateMachine;
        
        protected readonly GameObject goRef;

        #endregion
    }
}