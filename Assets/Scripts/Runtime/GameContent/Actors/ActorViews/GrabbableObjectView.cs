using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    [Pooled]
    public class GrabbableObjectView : ActorView, IGrabbable, IElementHolder
    {
        #region properties

        public Transform Transform => transform;

		public Rigidbody Rigidbody => rb;

        public ElementFlag Flag1 => element;

        public ElementFlag Flag2 { get; private set; }

		public Vector3 OriginPos { get; private set; }

		public bool Active => true;

        #endregion

        #region methodes

        public void Start()
        {
			_meshRenderer = GetComponentInChildren<MeshRenderer>();
			OriginPos = transform.position;
        }

        public void CheckOtherElement(ElementFlag elementFlag)
        {
            
        }

        #endregion

        #region fields

        [SerializeField] private Rigidbody rb;

        [SerializeField] private ElementFlag element;

        private MeshRenderer _meshRenderer;

        #endregion
    }
}