using System.Collections;
using UnityEngine;

namespace Runtime.Utils.BaseMachine
{
    public abstract class BaseState
    {
        #region constructor

        protected BaseState(GenericStateMachine machine, GameObject go)
        {
            stateMachine = machine;
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

        protected readonly GenericStateMachine stateMachine;
        
        protected readonly GameObject goRef;

        #endregion
    }
}