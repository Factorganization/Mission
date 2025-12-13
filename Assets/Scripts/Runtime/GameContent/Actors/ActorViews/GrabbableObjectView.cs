using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using Runtime.GameContent.Logics.LogicModels.ElementModels;
using Runtime.GameContent.Logics.LogicModels.MissionModels;
using Runtime.Management.GameManagement;
using Shared.Utils.Listing;
using TMPro;

namespace Runtime.GameContent.Actors.ActorViews
{
	[Pooled, SelectionBase, RequireComponent(typeof(Rigidbody))]
	public class GrabbableObjectView : ActorView, IGrabbable, IElementHolder
	{
		#region properties

		public Transform Transform => transform;

		public BoxCollider Collider => col;
        
		public float ElementApplicationDistance => elementApplicationDistance;
		
		public Rigidbody Rigidbody => _rb;

		public ElementFlag Flag1 { get; set; }

		public ElementFlag Flag2 => element;

		public ElementFlag Flag3
		{
			get => Flag1;
			set => Flag1 = value;
		}

		public RoomType RoomType { get; set; } = RoomType.House;

		public Vector3 OriginPos { get; private set; }

		public bool Active { get; set; }

		public bool[] MissionDone => _missionDone;
		
		public VFXReferences VFX => vfxReferences;

		#endregion

		#region methodes

		public void Start()
		{
			_missionDone = new bool[Enum.GetValues(typeof(ElementFlag)).Length];
			
			_resolveInteractions = new[]
			{
				new ElementInteractionDataPair{ Flag = 0b0011, Callback = WetAndBurn },
				new ElementInteractionDataPair{ Flag = 0b0101, Callback = WetAndElec },
			};
			_nextInteractions = new[]
			{
				new ElementInteractionDataPair{ Flag = 0b00100010, Callback = BurnToBurn },
				new ElementInteractionDataPair{ Flag = 0b00101000, Callback = BurnToExplode },
				//new ElementInteractionDataPair{ flag = 0b01000010, callback = ElectricToBurn },
				new ElementInteractionDataPair{ Flag = 0b01000100, Callback = ElectricToElectric },
				new ElementInteractionDataPair{ Flag = 0b01001000, Callback = ElectricToExplode },
				new ElementInteractionDataPair{ Flag = 0b00010001, Callback = WetToWet },
			};
			
			_rb = GetComponent<Rigidbody>();
			OriginPos = transform.position;
			Active = true;
		}

		private void Update()
		{
			if (debug)
				text.text = $"{(Active ? "<color=green>Active</color>" : "<color=red>Inactive</color>")}\n {Convert.ToString((int)Flag1, 2).PadLeft(4, '0')} \n {Convert.ToString((int)Flag2, 2).PadLeft(4, '0')}";
			
			if ((Flag3 & ElementFlag.CanBeWet) != 0)
			{
				durations.waterTimer -= Time.deltaTime;

				if (durations.waterTimer < 0)
				{
					Flag3 &= ~ElementFlag.CanBeWet;
					SetParticleOverride(this, ElementFlag.CanBeWet, false);
				}

				if (!_missionDone[0])
				{
					_missionDone[0] = true;
					MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanBeWet, RoomType));
				}
			}
			if ((Flag3 & ElementFlag.CanBurn) != 0)
			{
				durations.fireTimer -= Time.deltaTime;

				if (durations.fireTimer < 0)
				{
					Flag3 &= ~ElementFlag.CanBurn;
					SetParticleOverride(this, ElementFlag.CanBurn, false);
				}
		        
				if (!_missionDone[1])
				{
					_missionDone[1] = true;
					MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanBurn, RoomType));
				}
			}
			if ((Flag3 & ElementFlag.CanConduct) != 0)
			{
				durations.electricityTimer -= Time.deltaTime;

				if (durations.electricityTimer < 0)
				{
					Flag3 &= ~ElementFlag.CanConduct;
					SetParticleOverride(this, ElementFlag.CanConduct, false);
				}
		        
				if (!_missionDone[2])
				{
					_missionDone[2] = true;
					MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanConduct, RoomType));
				}
			}
			if ((Flag3 & ElementFlag.CanExplode) != 0 && !_missionDone[3])
			{
				_missionDone[3] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanExplode, RoomType));
			}
		}

		public bool Action()
		{
			Active = !Active;
			return Active;
		}

		public void CheckOtherElement(IElementHolder holder)
		{
			foreach (var i in _resolveInteractions)
			{
				var key = GetKey(i);

				if (((int)(Flag3 | holder.Flag3) & key) == key)
					i.Callback.Invoke(new ElementInteractionData(this, holder));
			}

			if (Active && holder.Active)
			{
				foreach (var i in _nextInteractions)
				{
					var key = GetKey(i);
					
					if (((((int)Flag3 << 4) | (int)holder.Flag2) & key) == key)
						i.Callback.Invoke(new ElementInteractionData(this, holder));
                
					if (((((int)holder.Flag3 << 4) | (int)Flag2) & key) == key)
						i.Callback.Invoke(new ElementInteractionData(holder, this));
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

		private static int GetKey(ElementInteractionDataPair data) => data.Flag;

		#region F11 Comparisions

		private void WetAndBurn(ElementInteractionData data)
		{
			data.Holder1.Flag3 |= ElementFlag.CanBurn;
			data.Holder1.Flag3 &= ~ElementFlag.CanBurn;
			data.Holder2.Flag3 |= ElementFlag.CanBurn;
			data.Holder2.Flag3 &= ~ElementFlag.CanBurn;
			
			if (data.Holder1 is IPossessable && (data.Holder1.Flag3 & ElementFlag.CanBurn) != 0)
				data.Holder1.Active = false;
			if (data.Holder2 is IPossessable && (data.Holder1.Flag3 & ElementFlag.CanBurn) != 0)
				data.Holder2.Active = false;
		}

		private void WetAndElec(ElementInteractionData data)
		{
			data.Holder1.Flag3 |= ElementFlag.CanConduct;
			data.Holder2.Flag3 |= ElementFlag.CanConduct;
		}

		#endregion

		#region F12 Comparisons

		private void BurnToBurn(ElementInteractionData data)
		{
			durations.fireTimer = durations.fireDuration; 
			
			data.Holder2.Flag3 |= ElementFlag.CanBurn;
			if (!_missionDone[1])
			{
				_missionDone[1] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanBurn, RoomType));
			}
		}

		private void BurnToExplode(ElementInteractionData data)
		{
			data.Holder2.Flag3 |= ElementFlag.CanExplode;
			Explode(data.Holder2);
			if (_missionDone[3])
			{
				_missionDone[3] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanExplode, RoomType));
			}
		}

		private void ElectricToBurn(ElementInteractionData data)
		{
			durations.fireTimer = durations.fireDuration;
			
			data.Holder2.Flag3 |= ElementFlag.CanBurn;
			if (!_missionDone[1])
			{
				_missionDone[1] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanBurn, RoomType));
			}
		}

		private void ElectricToElectric(ElementInteractionData data)
		{
			durations.electricityTimer = durations.electricityDuration;
			
			data.Holder2.Flag3 |= ElementFlag.CanConduct;
			if (!_missionDone[2])
			{
				_missionDone[2] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanConduct, RoomType));
			}
		}

		private void ElectricToExplode(ElementInteractionData data)
		{
			data.Holder2.Flag3 |= ElementFlag.CanExplode;
			Explode(data.Holder2);
			if (_missionDone[3])
			{
				_missionDone[3] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanExplode, RoomType));
			}
		}

		private void WetToWet(ElementInteractionData data)
		{
			durations.waterTimer = durations.waterDuration;
			
			data.Holder2.Flag3 |= ElementFlag.CanBeWet;
			if (!_missionDone[0])
			{
				_missionDone[0] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.ElementAffection, @object, ElementFlag.CanBeWet, RoomType));
			}
		}

		#endregion

		private void Explode(IElementHolder holder)
		{
			foreach (var e in LevelGenerator.Generator.ElementHolders)
			{
				if (Vector3.Distance(e.Transform.position, holder.Transform.position) > destructionApplicationDistance)
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

		[SerializeField] private VFXReferences vfxReferences;
		
		[SerializeField] private ObjectDefinition objectDefinition;
		
		[SerializeField] private ObjectType @object;

		[SerializeField] private ElementFlag element;

		[SerializeField] private BoxCollider col;
		
		[SerializeField] private LayerMask blockLayer;

		[SerializeField] private ElementDuration durations;
		
		[SerializeField] private float elementApplicationDistance;
        
		[SerializeField] private float destructionApplicationDistance;
		
		[SerializeField] private TMP_Text text;

		[SerializeField] private bool debug;

		private ElementInteractionDataPair[] _resolveInteractions;

		private ElementInteractionDataPair[] _nextInteractions;

		private bool[] _missionDone;
		
		private Rigidbody _rb;

		#endregion
	}
}