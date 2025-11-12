using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    [Pooled, SelectionBase]
    public class PossessableObjectView : ActorView, IPossessable, IElementHolder
    {
        #region properties

        public Transform Transform => transform;

		public ElementFlag Flag1 => sourceElement;

        public ElementFlag Flag2 => receptorElement;
        

        public bool Active { get; private set; }
        
        public bool Possessed { get; set; }

		public bool Destroyed { get; private set; }

		#endregion

		#region methodes

		private void Start()
		{
			Possessed = false;
			Destroyed = false;
			Active = false;
		}

		public void Action()
		{
			Debug.Log("Action");
			Active = !Active;

			//TODO CHANGER CA
			transform.localScale = Active ? new Vector3(1, 5, 1) : new Vector3(1, 1, 1);
		}

        public void DestructiveAction()
        {
            Debug.Log("DestructiveAction");
        }

        public void CheckOtherElement(ElementFlag elementFlag)
        {
            
        }

        #endregion

        #region fields

        [SerializeField] private ElementFlag sourceElement;
        
        [SerializeField] private ElementFlag receptorElement;

        #endregion
    }
}