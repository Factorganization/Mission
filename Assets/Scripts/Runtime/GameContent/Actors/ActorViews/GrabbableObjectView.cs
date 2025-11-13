using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using Runtime.Management.GameManagement;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    [Pooled, SelectionBase]
    public class GrabbableObjectView : ActorView, IGrabbable, IElementHolder
    {
        #region properties

        public Transform Transform => transform;

		public Rigidbody Rigidbody => _rb;

        public ElementFlag Flag1 => element;

        public ElementFlag Flag2 { get; private set; }

		public Vector3 OriginPos { get; private set; }

		public bool Active => true;

        #endregion

        #region methodes

        public void Start()
        {
			_meshRenderer = GetComponentInChildren<MeshRenderer>();
			_rb = GetComponent<Rigidbody>();
			OriginPos = transform.position;
        }

        public void Update()
        {
	        foreach (var e in LevelGenerator.Generator.ElementHolders)
	        {
		        
	        }
        }

		public bool Action()
		{
			return false;
		}

		public void CheckOtherElement(ElementFlag elementFlag)
		{
			
		}

		protected void SetParticle(ElementFlag elementFlag)
		{
			if ((elementFlag & ElementFlag.CanBeWet) != 0)
				vfxReferences.waterParticles.Play();
			else 
				vfxReferences.waterParticles.Stop();
			
			if ((elementFlag & ElementFlag.CanBurn) != 0)
				vfxReferences.fireParticles.Play();
			else 
				vfxReferences.fireParticles.Stop();
			
			if ((elementFlag & ElementFlag.CanConduct) != 0)
				vfxReferences.electricParticles.Play();
			else 
				vfxReferences.electricParticles.Stop();
			
			if ((elementFlag & ElementFlag.CanExplode) != 0)
				vfxReferences.explosionParticles.Play();
			else 
				vfxReferences.explosionParticles.Stop();
			
		}

		#endregion

		#region fields

		[SerializeField] private VFXReferences vfxReferences;
		
		[SerializeField] private ElementFlag element;
		
		private Rigidbody _rb;

        private MeshRenderer _meshRenderer;

        [System.Serializable]
        private class VFXReferences
        {
	        [SerializeField] internal ParticleSystem fireParticles;
	        
	        [SerializeField] internal ParticleSystem waterParticles;
	        
	        [SerializeField] internal ParticleSystem electricParticles;
	        
	        [SerializeField] internal ParticleSystem explosionParticles;
        }

        #endregion
    }
}