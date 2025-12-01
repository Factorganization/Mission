using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using Runtime.GameContent.Logics.LogicModels.ElementModels;
using Runtime.Management.GameManagement;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    [Pooled]
    public class NoInteractObjectView : ActorView, INoInteract
    {
        #region proprties
        
        public Transform Transform => transform;
        
        public ElementFlag Flag1 { get; set; }

        public ElementFlag Flag2 => flag;

        public ElementFlag Flag3
        {
	        get => Flag1;
	        set => Flag1 = value;
        }
        
        public bool Active
        {
            get => true;
            set { }
        }

        public VFXReferences VFX => vfxReferences;
        
        public void CheckOtherElement(IElementHolder holder)
        {
            foreach (var i in ResolveInteractions)
			{
				var key = GetKey(i);

				if (((int)(Flag3 | holder.Flag3) & key) == key)
					i.callback.Invoke(new ElementInteractionData(this, holder));
			}

			if (Active && holder.Active)
			{
				foreach (var i in NextInteractions)
				{
					var key = GetKey(i);
					
					if (((((int)Flag3 << 4) | (int)holder.Flag2) & key) == key)
						i.callback.Invoke(new ElementInteractionData(this, holder));
                
					if (((((int)holder.Flag3 << 4) | (int)Flag2) & key) == key)
						i.callback.Invoke(new ElementInteractionData(holder, this));
				}
			}
			
			SetParticle(this);
			SetParticle(holder);
		}

		protected static void SetParticle(IElementHolder holder)
		{
			if ((holder.Flag3 & ElementFlag.CanBeWet) != 0 && !holder.VFX.waterPlaying)
			{
				holder.VFX.waterParticles.Play();
				holder.VFX.waterPlaying = true;
			}
			else if ((holder.Flag3 & ElementFlag.CanBeWet) == 0)
			{
				holder.VFX.waterParticles.Stop();
				holder.VFX.waterPlaying = false;
			}

			if ((holder.Flag3 & ElementFlag.CanBurn) != 0 && !holder.VFX.firePlaying)
			{
				holder.VFX.fireParticles.Play();
				holder.VFX.firePlaying = true;
			}
			else if ((holder.Flag3 & ElementFlag.CanBurn) == 0)
			{
				holder.VFX.fireParticles.Stop();
				holder.VFX.firePlaying = false;
			}

			if ((holder.Flag3 & ElementFlag.CanConduct) != 0 && !holder.VFX.elecPlaying)
			{
				holder.VFX.electricParticles.Play();
				holder.VFX.elecPlaying = true;
			}
			else if ((holder.Flag3 & ElementFlag.CanConduct) == 0)
			{
				holder.VFX.electricParticles.Stop();
				holder.VFX.elecPlaying = false;
			}

			if ((holder.Flag3 & ElementFlag.CanExplode) != 0 && !holder.VFX.explodePlaying)
			{
				holder.VFX.explosionParticles.Play();
				holder.VFX.explodePlaying = true;
			}
			else if ((holder.Flag3 & ElementFlag.CanExplode) == 0)
			{
				holder.VFX.explosionParticles.Stop();
				holder.VFX.explodePlaying = false;
			}
		}

		protected static void SetParticleOverride(IElementHolder holder, ElementFlag flag, bool active)
		{
			if (flag == ElementFlag.CanBeWet && active)
				holder.VFX.waterParticles.Play();
			else if (flag == ElementFlag.CanBeWet && !active)
				holder.VFX.waterParticles.Stop();
			
			if (flag == ElementFlag.CanBurn && active)
				holder.VFX.fireParticles.Play();
			else if (flag == ElementFlag.CanBurn && !active)
				holder.VFX.fireParticles.Stop();
			
			if (flag == ElementFlag.CanConduct && active)
				holder.VFX.electricParticles.Play();
			else if (flag == ElementFlag.CanConduct && !active)
				holder.VFX.electricParticles.Stop();
			
			if (flag == ElementFlag.CanExplode && active)
				holder.VFX.explosionParticles.Play();
			else if (flag == ElementFlag.CanExplode && !active)
				holder.VFX.explosionParticles.Stop();
		}

		private static int GetKey(ElementInteractionDataPair data) => data.flag;

		#region F11 Comparisions

		private static void WetAndBurn(ElementInteractionData data)
		{
			data.holder1.Flag3 |= ElementFlag.CanBurn;
			data.holder1.Flag3 &= ~ElementFlag.CanBurn;
			data.holder2.Flag3 |= ElementFlag.CanBurn;
			data.holder2.Flag3 &= ~ElementFlag.CanBurn;

			if (data.holder1 is IPossessable && (data.holder1.Flag3 & ElementFlag.CanBurn) != 0)
				data.holder1.Active = false;
			if (data.holder2 is IPossessable && (data.holder1.Flag3 & ElementFlag.CanBurn) != 0)
				data.holder2.Active = false;
		}

		private static void WetAndElec(ElementInteractionData data)
		{
			data.holder1.Flag3 |= ElementFlag.CanConduct;
			data.holder2.Flag3 |= ElementFlag.CanConduct;
		}

		#endregion

		#region F12 Comparisons

		private static void BurnToBurn(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanBurn;
		}

		private static void BurnToExplode(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanExplode;
			Explode(data.holder2);
		}

		private static void ElectricToBurn(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanBurn;
		}

		private static void ElectricToElectric(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanConduct;
		}

		private static void ElectricToExplode(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanExplode;
			Explode(data.holder2);
		}

		private static void WetToWet(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanBeWet;
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

				e.Flag3 |= ElementFlag.CanBurn;
				SetParticleOverride(e, ElementFlag.CanBurn, true);
			}
		}

        #endregion

        #region fields

        private static ElementInteractionDataPair[] ResolveInteractions =
        {
	        new(){ flag = 0b0011, callback = WetAndBurn },
	        new(){ flag = 0b0101, callback = WetAndElec },
        };

		private static ElementInteractionDataPair[] NextInteractions =
		{
			new(){ flag = 0b00100010, callback = BurnToBurn },
			new(){ flag = 0b00101000, callback = BurnToExplode },
			new(){ flag = 0b01000010, callback = ElectricToBurn },
			new(){ flag = 0b01000100, callback = ElectricToElectric },
			new(){ flag = 0b01001000, callback = ElectricToExplode },
			new(){ flag = 0b00010001, callback = WetToWet },
		};

        [SerializeField] private ElementFlag flag;
        
        [SerializeField] private VFXReferences vfxReferences;

        #endregion
    }
}