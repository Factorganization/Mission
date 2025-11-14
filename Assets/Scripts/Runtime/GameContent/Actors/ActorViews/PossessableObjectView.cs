using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using Runtime.Management.GameManagement;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    [Pooled, SelectionBase]
    public class PossessableObjectView : ActorView, IPossessable, IElementHolder
    {
        #region properties

        public Transform Transform => transform;

		public ElementFlag Flag1
		{
			get => sourceElement;
			set { }
		}

        public ElementFlag Flag2 => receptorElement;
        

        public bool Active => _active && !Destroyed;
        
        public bool Possessed { get; set; }

		public bool Destroyed { get; private set; }

		#endregion

		#region methodes

		private void Start()
		{
			Possessed = false;
			Destroyed = false;
			_active = false;
		}

		public void Action() => _active = !_active;

		public void DestructiveAction()
        {
            Debug.Log("DestructiveAction");
        }

		public void CheckOtherElement(IElementHolder holder)
		{
			foreach (var i in Interactions)
			{
				var key = GetKey(i);

				if (((int)(Flag1 | holder.Flag1) & key) == key)
					i.callback.Invoke(new(this, holder));
			}

			foreach (var i in Interactions)
			{
				var key = GetKey(i);

				if (((int)(Flag1 & holder.Flag2) & key) == key)
					i.callback.Invoke(new(this, holder));

				if (((int)(Flag2 & holder.Flag1) & key) == key)
					i.callback.Invoke(new(holder, this));
			}

			SetParticle(Flag1);
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

		private static int GetKey(ElementInteractionDataPair data) => data.flag;

		#region F11 Comparisions

		private static void WetAndBurn(ElementInteractionData data)
		{
			data.holder1.Flag1 &= ~ElementFlag.CanBurn;
			data.holder2.Flag1 &= ~ElementFlag.CanBurn;

		}

		private static void WetAndElec(ElementInteractionData data)
		{
			data.holder1.Flag1 |= ElementFlag.CanConduct;
			data.holder2.Flag1 |= ElementFlag.CanConduct;
		}

		#endregion


		#region F12 Comparisons

		private static void BurnToBurn(ElementInteractionData data)
		{
			data.holder2.Flag1 |= ElementFlag.CanBurn;
		}

		private static void BurnToExplode(ElementInteractionData data)
		{
			data.holder2.Flag1 |= ElementFlag.CanExplode;
			Explode(data.holder2);
		}

		private static void ElectricToBurn(ElementInteractionData data)
		{
			data.holder2.Flag1 |= ElementFlag.CanBurn;
		}

		private static void ElectricToElectric(ElementInteractionData data)
		{
			data.holder2.Flag1 |= ElementFlag.CanConduct;
		}

		private static void ElectricToExplode(ElementInteractionData data)
		{
			data.holder2.Flag1 |= ElementFlag.CanExplode;
			Explode(data.holder2);
		}

		private static void WetToWet(ElementInteractionData data)
		{
			data.holder2.Flag1 |= ElementFlag.CanBeWet;
		}

		#endregion

		private static void Explode(IElementHolder holder)
		{
			//TODO add raycasts

			foreach (var e in LevelGenerator.Generator.ElementHolders)
			{
				if (Vector3.Distance(e.Transform.position, holder.Transform.position) > 5f)
					continue;

				if ((e.Flag2 & ElementFlag.CanBurn) == 0)
					continue;

				e.Flag1 |= ElementFlag.CanBurn;
				//TODO, les particles call
			}
		}

        #endregion

        #region fields

		[SerializeField] private VFXReferences vfxReferences;

        [SerializeField] private ElementFlag sourceElement;
        
        [SerializeField] private ElementFlag receptorElement;

		private static ElementInteractionDataPair[] Interactions =
		{
			new(){ flag = 0b0011, callback = WetAndBurn },
			new(){ flag = 0b0101, callback = WetAndElec },
			new(){ flag = 0b0010, callback = BurnToBurn },
			new(){ flag = 0b1010, callback = BurnToExplode },
			new(){ flag = 0b0110, callback = ElectricToBurn },
			new(){ flag = 0b0100, callback = ElectricToElectric },
			new(){ flag = 0b1100, callback = ElectricToExplode },
			new(){ flag = 0b0001, callback = WetToWet },
		};

		private bool _active;

        #endregion
    }
}