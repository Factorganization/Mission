using System;
using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using Runtime.GameContent.Logics.LogicModels.ElementModels;
using Runtime.GameContent.Logics.LogicModels.MissionModels;
using Runtime.Management.GameManagement;
using Shared.Utils.Listing;
using TMPro;
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
        
        public ElementFlag Flag3 { get; set; }

        public bool Active
        {
	        get => _active /*&& !Destroyed*/;
	        set => _active = value;
        }

        public bool Possessed { get; set; }

        public bool Destroyed
        {
	        get => _destroyed;
	        set
	        {
		        _destroyed = value;
		        if (_destroyed)
			        return;
		        
		        _active = false;
		        Flag3 = Flag1;
		        foreach (var p in vfxReferences.waterParticles)
			        p.Stop();
		        vfxReferences.waterPlaying = false;
		        foreach (var p in vfxReferences.fireParticles)
			        p.Stop();
		        vfxReferences.firePlaying = false;
		        foreach (var p in vfxReferences.electricParticles)
			        p.Stop();
		        vfxReferences.elecPlaying = false;
		        foreach (var p in vfxReferences.explosionParticles)
			        p.Stop();
		        vfxReferences.explodePlaying = false;
	        }
        }

		public VFXReferences VFX => vfxReferences;

		#endregion

		#region methodes

		private void Start()
		{
			_resolveInteractions = new[]
			{
				new ElementInteractionDataPair{ flag = 0b0011, callback = WetAndBurn },
				new ElementInteractionDataPair{ flag = 0b0101, callback = WetAndElec }
			};
			
			_nextInteractions = new []
			{
				new ElementInteractionDataPair{ flag = 0b00100010, callback = BurnToBurn },
				new ElementInteractionDataPair{ flag = 0b00101000, callback = BurnToExplode },
				new ElementInteractionDataPair{ flag = 0b01000010, callback = ElectricToBurn },
				new ElementInteractionDataPair{ flag = 0b01000100, callback = ElectricToElectric },
				new ElementInteractionDataPair{ flag = 0b01001000, callback = ElectricToExplode },
				new ElementInteractionDataPair{ flag = 0b00010001, callback = WetToWet }
			};

			Possessed = false;
			Destroyed = destroyedAtStart;
			_active = false;
			Flag3 = Flag1;
		}
		
		public void Update()
		{
			if (debug)
				text.text = $"{(Active ? "<color=green>Active</color>" : "<color=red>Inactive</color>")}\n {Convert.ToString((int)Flag1, 2).PadLeft(4, '0')} \n {Convert.ToString((int)Flag2, 2).PadLeft(4, '0')}";
		}

		public void Action()
		{
			_active = !_active;

			if (!_active)
			{
				Flag3 = Flag1;
				foreach (var p in vfxReferences.waterParticles)
					p.Stop();
				vfxReferences.waterPlaying = false;
				foreach (var p in vfxReferences.fireParticles)
					p.Stop();
				vfxReferences.firePlaying = false;
				foreach (var p in vfxReferences.electricParticles)
					p.Stop();
				vfxReferences.elecPlaying = false;
				foreach (var p in vfxReferences.explosionParticles)
					p.Stop();
				vfxReferences.explodePlaying = false;
				return;
			}

			SetParticle(this);
		}

		public void DestructiveAction()
		{
            Destroyed = true;
            //TODO how ?
            _active = true;
            Flag3 = Flag1;
            //TODO what ?
            SetParticle(this);
            
            if ((Flag3 & ElementFlag.CanExplode) != 0)
	            Explode(this);
        }

		public void CheckOtherElement(IElementHolder holder)
		{
			foreach (var i in _resolveInteractions)
			{
				var key = GetKey(i);

				if (((int)(Flag3 | holder.Flag3) & key) == key)
					i.callback.Invoke(new ElementInteractionData(this, holder));
			}

			if (Active && holder.Active)
			{
				foreach (var i in _nextInteractions)
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

		#region graphics methodes
		
		protected static void SetParticle(IElementHolder holder)
		{
			if ((holder.Flag3 & ElementFlag.CanBeWet) != 0 && !holder.VFX.waterPlaying)
			{
				foreach (var p in holder.VFX.waterParticles)
					p.Play();
				holder.VFX.waterPlaying = true;
			}
			else if ((holder.Flag3 & ElementFlag.CanBeWet) == 0)
			{
				foreach (var p in holder.VFX.waterParticles)
					p.Stop();
				holder.VFX.waterPlaying = false;
			}

			if ((holder.Flag3 & ElementFlag.CanBurn) != 0 && !holder.VFX.firePlaying)
			{
				foreach (var p in holder.VFX.fireParticles)
					p.Play();
				holder.VFX.firePlaying = true;
			}
			else if ((holder.Flag3 & ElementFlag.CanBurn) == 0)
			{
				foreach (var p in holder.VFX.fireParticles)
					p.Stop();
				holder.VFX.firePlaying = false;
			}

			if ((holder.Flag3 & ElementFlag.CanConduct) != 0 && !holder.VFX.elecPlaying)
			{
				foreach (var p in holder.VFX.electricParticles)
					p.Play();
				holder.VFX.elecPlaying = true;
			}
			else if ((holder.Flag3 & ElementFlag.CanConduct) == 0)
			{
				foreach (var p in holder.VFX.electricParticles)
					p.Stop();
				holder.VFX.elecPlaying = false;
			}

			if ((holder.Flag3 & ElementFlag.CanExplode) != 0 && !holder.VFX.explodePlaying)
			{
				foreach (var p in holder.VFX.explosionParticles)
					p.Play();
				holder.VFX.explodePlaying = true;
			}
			else if ((holder.Flag3 & ElementFlag.CanExplode) == 0)
			{
				foreach (var p in holder.VFX.explosionParticles)
					p.Stop();
				holder.VFX.explodePlaying = false;
			}
		}

		protected static void SetParticleOverride(IElementHolder holder, ElementFlag flag, bool active)
		{
			if (flag == ElementFlag.CanBeWet && active)
				foreach (var p in holder.VFX.waterParticles)
					p.Play();
			else if (flag == ElementFlag.CanBeWet && !active)
				foreach (var p in holder.VFX.waterParticles)
					p.Stop();
			
			if (flag == ElementFlag.CanBurn && active)
				foreach (var p in holder.VFX.fireParticles)
					p.Play();
			else if (flag == ElementFlag.CanBurn && !active)
				foreach (var p in holder.VFX.fireParticles)
					p.Stop();
			
			if (flag == ElementFlag.CanConduct && active)
				foreach (var p in holder.VFX.electricParticles)
					p.Play();
			else if (flag == ElementFlag.CanConduct && !active)
				foreach (var p in holder.VFX.electricParticles)
					p.Stop();
			
			if (flag == ElementFlag.CanExplode && active)
				foreach (var p in holder.VFX.explosionParticles)
					p.Play();
			else if (flag == ElementFlag.CanExplode && !active)
				foreach (var p in holder.VFX.explosionParticles)
					p.Stop();
		}

		#endregion
		
		private static int GetKey(ElementInteractionDataPair data) => data.flag;

		#region F11 Comparisions

		private void WetAndBurn(ElementInteractionData data)
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

		private void WetAndElec(ElementInteractionData data)
		{
			data.holder1.Flag3 |= ElementFlag.CanConduct;
			data.holder2.Flag3 |= ElementFlag.CanConduct;
		}

		#endregion

		#region F12 Comparisons

		private void BurnToBurn(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanBurn;
			MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanBurn, _roomType));
		}

		private void BurnToExplode(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanExplode;
			Explode(data.holder2);
		}

		private void ElectricToBurn(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanBurn;
		}

		private void ElectricToElectric(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanConduct;
		}

		private void ElectricToExplode(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanExplode;
			Explode(data.holder2);
		}

		private void WetToWet(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanBeWet;
		}

		#endregion

		private void Explode(IElementHolder holder)
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

		[SerializeField] private VFXReferences vfxReferences;
		
		[SerializeField] private ObjectType @object;

        [SerializeField] private ElementFlag sourceElement;

        [SerializeField] private ElementFlag receptorElement;

        [SerializeField] private TMP_Text text;

        [SerializeField] private bool debug;

        [SerializeField] private bool destroyedAtStart;
        
        private RoomType _roomType = RoomType.House;

        private ElementInteractionDataPair[] _resolveInteractions;

        private ElementInteractionDataPair[] _nextInteractions;

		private bool _active;
		
		private bool _destroyed;

        #endregion
    }
}