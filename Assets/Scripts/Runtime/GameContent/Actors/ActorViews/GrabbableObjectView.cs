using System;
using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using Runtime.Management.GameManagement;
using Shared.Utils.Listing;
using TMPro;
using UnityEngine;


namespace Runtime.GameContent.Actors.ActorViews
{
	[Pooled, SelectionBase, RequireComponent(typeof(Rigidbody))]
	public class GrabbableObjectView : ActorView, IGrabbable, IElementHolder
	{
		#region properties

		public Transform Transform => transform;

		public Rigidbody Rigidbody => _rb;

		public ElementFlag Flag1 { get; set; }

		public ElementFlag Flag2 => element;

		public Vector3 OriginPos { get; private set; }

		public bool Active { get; set; }
		
		public VFXReferences VFX => vfxReferences;

		#endregion

		#region methodes

		public void Start()
		{
			_meshRenderer = GetComponentInChildren<MeshRenderer>();
			_rb = GetComponent<Rigidbody>();
			OriginPos = transform.position;
			Active = true;
		}

		public void Update()
		{
			text.text = $"{(Active ? "<color=green>Active</color>" : "<color=red>Inactive</color>")}\n {Convert.ToString((int)Flag1, 2).PadLeft(4, '0')} \n {Convert.ToString((int)Flag2, 2).PadLeft(4, '0')}";
		}

		public bool Action()
		{
			Active = !Active;
			return true;
		}

		public void CheckOtherElement(IElementHolder holder)
		{
			foreach (var i in ResolveInteractions)
			{
				var key = GetKey(i);

				if (((int)(Flag1 | holder.Flag1) & key) == key)
					i.callback.Invoke(new(this, holder));
			}

			if (Active && holder.Active)
			{
				foreach (var i in NextInteractions)
				{
					var key = GetKey(i);
					var f1M = (int)Flag1 & key;
					var hf2M = (int)holder.Flag2 & key;
					var hf1M = (int)holder.Flag1 & key;
					var f2M = (int)Flag2 & key;
					
					if ((f1M | hf2M) == key && f1M != 0 && hf2M != 0)
						i.callback.Invoke(new(this, holder));
                
					if ((hf1M | f2M) == key &&  hf1M != 0 && f2M != 0)
						i.callback.Invoke(new(holder, this));
				}
			}
			
			SetParticle(this);
			SetParticle(holder);
			ResetFlags(this);
			ResetFlags(holder);
		}

		private static void ResetFlags(IElementHolder holder)
		{
			//TODO separation pour elec et water
			
			holder.Flag1 |= ElementFlag.CanExplode;
			holder.Flag1 &= ~ElementFlag.CanExplode;
		}

		protected static void SetParticle(IElementHolder holder)
		{
			if ((holder.Flag1 & ElementFlag.CanBeWet) != 0 && !holder.VFX.waterParticles.isPlaying)
				holder.VFX.waterParticles.Play();
			else if ((holder.Flag1 & ElementFlag.CanBeWet) == 0)
				holder.VFX.waterParticles.Stop();

			if ((holder.Flag1 & ElementFlag.CanBurn) != 0 && !holder.VFX.fireParticles.isPlaying)
				holder.VFX.fireParticles.Play();
			else if ((holder.Flag1 & ElementFlag.CanBurn) == 0)
				holder.VFX.fireParticles.Stop();

			if ((holder.Flag1 & ElementFlag.CanConduct) != 0 && !holder.VFX.electricParticles.isPlaying)
				holder.VFX.electricParticles.Play();
			else if ((holder.Flag1 & ElementFlag.CanConduct) == 0)
				holder.VFX.electricParticles.Stop();

			if ((holder.Flag1 & ElementFlag.CanExplode) != 0 && !holder.VFX.explosionParticles.isPlaying)
				holder.VFX.explosionParticles.Play();
			else if ((holder.Flag1 & ElementFlag.CanExplode) == 0)
				holder.VFX.explosionParticles.Stop();
		}

		private static int GetKey(ElementInteractionDataPair data) => data.flag;

		#region F11 Comparisions

		private static void WetAndBurn(ElementInteractionData data)
		{
			data.holder1.Flag1 |= ElementFlag.CanBurn;
			data.holder1.Flag1 &= ~ElementFlag.CanBurn;
			data.holder2.Flag1 |= ElementFlag.CanBurn;
			data.holder2.Flag1 &= ~ElementFlag.CanBurn;
			
			if (data.holder1 is IPossessable && (data.holder1.Flag1 & ElementFlag.CanBurn) != 0)
				data.holder1.Active = false;
			if (data.holder2 is IPossessable && (data.holder1.Flag1 & ElementFlag.CanBurn) != 0)
				data.holder2.Active = false;
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
			Debug.Log(data.holder2);
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
				SetParticle(e);
			}
		}

		#endregion

		#region fields

		[SerializeField] private VFXReferences vfxReferences;

		[SerializeField] private ElementFlag element;
		
		[SerializeField] private TMP_Text text;

		private static ElementInteractionDataPair[] ResolveInteractions =
		{
			new(){ flag = 0b0011, callback = WetAndBurn },
			new(){ flag = 0b0101, callback = WetAndElec },
		};
		
		private static ElementInteractionDataPair[] NextInteractions =
		{
			new(){ flag = 0b0010, callback = BurnToBurn },
			new(){ flag = 0b1010, callback = BurnToExplode },
			new(){ flag = 0b0110, callback = ElectricToBurn },
			new(){ flag = 0b0100, callback = ElectricToElectric },
			new(){ flag = 0b1100, callback = ElectricToExplode },
			new(){ flag = 0b0001, callback = WetToWet },
		};
		
		private Rigidbody _rb;

        private MeshRenderer _meshRenderer;

        #endregion
    }
}