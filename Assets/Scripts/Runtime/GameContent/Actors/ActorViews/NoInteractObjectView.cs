using System;
using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using Runtime.GameContent.Logics.LogicModels.ElementModels;
using Runtime.GameContent.Logics.LogicModels.MissionModels;
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
        
        public BoxCollider Collider => col;
        
        public float ElementApplicationDistance => elementApplicationDistance;
        
        public ElementFlag Flag1 { get; set; }

        public ElementFlag Flag2 => flag;

        public ElementFlag Flag3
        {
	        get => Flag1;
	        set => Flag1 = value;
        }
        
        public RoomType RoomType { get; set; } = RoomType.House;
        
        public bool Active
        {
            get => true;
            set { }
        }

        public bool[] MissionDone => _missionDone;

        public VFXReferences VFX => vfxReferences;
        
        #endregion
        
        #region methodes

        private void Start()
        {
	        _missionDone = new bool[Enum.GetValues(typeof(ElementFlag)).Length];
	        
	        _resolveInteractions = new[]
	        {
		        new ElementInteractionDataPair{ flag = 0b0011, callback = WetAndBurn },
		        new ElementInteractionDataPair{ flag = 0b0101, callback = WetAndElec },
	        };
	        
	        _nextInteractions = new []
	        {
		        new ElementInteractionDataPair{ flag = 0b00100010, callback = BurnToBurn },
		        new ElementInteractionDataPair{ flag = 0b00101000, callback = BurnToExplode },
		        //new ElementInteractionDataPair{ flag = 0b01000010, callback = ElectricToBurn },
		        new ElementInteractionDataPair{ flag = 0b01000100, callback = ElectricToElectric },
		        new ElementInteractionDataPair{ flag = 0b01001000, callback = ElectricToExplode },
		        new ElementInteractionDataPair{ flag = 0b00010001, callback = WetToWet },
	        };
        }

        private void Update()
        {
	        if ((Flag3 & ElementFlag.CanBeWet) != 0 && !_missionDone[0])
	        {
		        _missionDone[0] = true;
		        MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanBeWet, RoomType));
	        }
	        if ((Flag3 & ElementFlag.CanBurn) != 0 && !_missionDone[1])
	        {
		        _missionDone[1] = true;
		        MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanBurn, RoomType));
	        }
	        if ((Flag3 & ElementFlag.CanConduct) != 0 && !_missionDone[2])
	        {
		        _missionDone[2] = true;
		        MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanConduct, RoomType));
	        }
	        if ((Flag3 & ElementFlag.CanExplode) != 0 && !_missionDone[3])
	        {
		        _missionDone[3] = true;
		        MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanExplode, RoomType));
	        }
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
			if (!_missionDone[1])
			{
				_missionDone[1] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanBurn, RoomType));
			}
		}

		private void BurnToExplode(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanExplode;
			Explode(data.holder2);
			if (_missionDone[3])
			{
				_missionDone[3] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanExplode, RoomType));
			}
		}

		private void ElectricToBurn(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanBurn;
			if (!_missionDone[1])
			{
				_missionDone[1] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanBurn, RoomType));
			}
		}

		private void ElectricToElectric(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanConduct;
			if (!_missionDone[2])
			{
				_missionDone[2] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanConduct, RoomType));
			}
		}

		private void ElectricToExplode(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanExplode;
			Explode(data.holder2);
			if (_missionDone[3])
			{
				_missionDone[3] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanExplode, RoomType));
			}
		}

		private void WetToWet(ElementInteractionData data)
		{
			data.holder2.Flag3 |= ElementFlag.CanBeWet;
			if (!_missionDone[0])
			{
				_missionDone[0] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanBeWet, RoomType));
			}
		}

		#endregion

		private void Explode(IElementHolder holder)
		{
			//TODO add raycasts

			foreach (var e in LevelGenerator.Generator.ElementHolders)
			{
				if (Vector3.Distance(e.Transform.position, holder.Transform.position) > 5f)
					continue;
				
				Physics.Linecast(e.Transform.position + e.Collider.center, holder.Transform.position + holder.Collider.center, out var hit, blockLayer);
				if (hit.transform is not null && !hit.transform.TryGetComponent<IElementHolder>(out _))
					continue;

				if ((e.Flag2 & ElementFlag.CanBurn) == 0)
					continue;

				e.Flag3 |= ElementFlag.CanBurn;
				SetParticleOverride(e, ElementFlag.CanBurn, true);

				if (!e.MissionDone[1])
				{
					_missionDone[1] = true;
					MissionManager.Manager.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanExplode, RoomType));
				}
			}
		}

        #endregion

        #region fields

        [SerializeField] private ObjectType @object; //Tu me laisses l'appeler object >:(

        [SerializeField] private ElementFlag flag;

        [SerializeField] private BoxCollider col;
        
        [SerializeField] private LayerMask blockLayer;
        
        [SerializeField] private float elementApplicationDistance;
        
        [SerializeField] private VFXReferences vfxReferences;

        private ElementInteractionDataPair[] _resolveInteractions;

        private ElementInteractionDataPair[] _nextInteractions;

        private bool[] _missionDone;

        #endregion
    }
}