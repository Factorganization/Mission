using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
using Runtime.Services.Game.GameContent.Actors.ActorViews;
using Runtime.Services.Game.GameContent.Logics.LogicInterfaces;
using Runtime.Services.Game.GameContent.Logics.LogicModels;
using Runtime.Services.Game.GameContent.Logics.LogicModels.ElementModels;
using Runtime.Services.Game.GameContent.Logics.LogicModels.MissionModels;
using Runtime.Services.Game.GameSystems;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers;

public abstract class ElementHolderController : ActorView, IElementHolder 
{
    #region properties

	public Transform Transform => transform;

	public ObjectType ObjectType => objectDefinition.@object;

	public RoomType RoomType { get; set; } = RoomType.House;

	public ElementDuration Durations => objectDefinition.durations;
	
	public VFXReferences VFX => objectDefinition.vfxReferences;

	public bool[] MissionDone => _missionDone;
    
	public float ElementApplicationDistance => objectDefinition.elementApplicationDistance;

	public abstract bool Active { get; set; }
	
	public abstract ElementFlag Flag1 { get; set; }

	public abstract ElementFlag Flag2 { get; }
    
	public abstract ElementFlag Flag3 { get; set; }

	public virtual BoxCollider Collider => objectDefinition.col;
	
	#endregion

	#region methodes

	#region Unity Events
	
	protected virtual void Start()
	{
		_missionDone = new bool[Enum.GetValues(typeof(ElementFlag)).Length + 1];
		
		_resolveInteractions = new[]
		{
			new ElementInteractionDataPair{ Flag = 0b0011, Callback = WetAndBurn },
			new ElementInteractionDataPair{ Flag = 0b0101, Callback = WetAndElec }
		};
		
		_nextInteractions = new []
		{
			new ElementInteractionDataPair{ Flag = 0b00100010, Callback = BurnToBurn },
			new ElementInteractionDataPair{ Flag = 0b00101000, Callback = BurnToExplode },
			//new ElementInteractionDataPair{ flag = 0b01000010, callback = ElectricToBurn },
			new ElementInteractionDataPair{ Flag = 0b01000100, Callback = ElectricToElectric },
			new ElementInteractionDataPair{ Flag = 0b01001000, Callback = ElectricToExplode },
			new ElementInteractionDataPair{ Flag = 0b00010001, Callback = WetToWet }
		};

		//Flag3 = Flag1;
		foreach (var p in objectDefinition.vfxReferences.waterParticles)
			p.Stop();
		objectDefinition.vfxReferences.waterPlaying = false;
		foreach (var p in objectDefinition.vfxReferences.fireParticles)
			p.Stop();
		objectDefinition.vfxReferences.firePlaying = false;
		foreach (var p in objectDefinition.vfxReferences.electricParticles)
			p.Stop();
		objectDefinition.vfxReferences.elecPlaying = false;
		foreach (var p in objectDefinition.vfxReferences.explosionParticles)
			p.Stop();
		objectDefinition.vfxReferences.explodePlaying = false;
	}
	
	protected virtual void Update()
	{
//#if UNITY_EDITOR
		if (objectDefinition.debugInfo.debug)
			objectDefinition.debugInfo.text.text = $"{(Active ? "<color=green>Active</color>" : "<color=red>Inactive</color>")}\n {Convert.ToString((int)Flag1, 2).PadLeft(4, '0')} \n {Convert.ToString((int)Flag2, 2).PadLeft(4, '0')}";
//#endif
		
		//checkup
		/*if (((int)Flag3 & 0b0011) == 0b0011)
		{
			Flag3 &= ~ElementFlag.CanBurn;
			objectDefinition.durations.fireTimer = 0;
			SetParticleOverride(this, ElementFlag.CanBurn, false);
		}
		*/ 
		/*if (!Active) //Partons du principe que un objet peut valider ses missions meme inactif
			return;*/
		
		if ((Flag3 & ElementFlag.CanBeWet) != 0)
		{
			objectDefinition.durations.waterTimer -= Time.deltaTime;

			if (objectDefinition.durations.waterTimer < 0)
			{
				Flag3 &= ~ElementFlag.CanBeWet;
				SetParticleOverride(this, ElementFlag.CanBeWet, false);
			}
		}
		if ((Flag3 & ElementFlag.CanBurn) != 0)
		{
			objectDefinition.durations.fireTimer -= Time.deltaTime;

			if (objectDefinition.durations.fireTimer < 0)
			{
				Flag3 &= ~ElementFlag.CanBurn;
				SetParticleOverride(this, ElementFlag.CanBurn, false);
			}
		}
		if ((Flag3 & ElementFlag.CanConduct) != 0)
		{
			objectDefinition.durations.electricityTimer -= Time.deltaTime;

			if (objectDefinition.durations.electricityTimer < 0)
			{
				Flag3 &= ~ElementFlag.CanConduct;
				SetParticleOverride(this, ElementFlag.CanConduct, false);
			}
		}
	}
	
	#endregion
	
	#region element holder implementation
	
	private static int GetKey(ElementInteractionDataPair data) => data.Flag;

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
	
	#endregion

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

		data.Holder1.Durations.fireTimer = 0;
		data.Holder2.Durations.fireTimer = 0;
	}

	private void WetAndElec(ElementInteractionData data)
	{
		data.Holder1.Flag3 |= ElementFlag.CanConduct;
		data.Holder2.Flag3 |= ElementFlag.CanConduct;
		
		data.Holder1.Durations.electricityTimer = data.Holder1.Durations.electricityDuration;
		data.Holder2.Durations.electricityTimer = data.Holder2.Durations.electricityDuration;
	}

	#endregion

	#region F12 Comparisons

	private void BurnToBurn(ElementInteractionData data)
	{
		data.Holder2.Durations.fireTimer = objectDefinition.durations.fireDuration; //TODO
		data.Holder2.Flag3 |= ElementFlag.CanBurn;
		
		if (!data.Holder2.MissionDone[1])
		{
			MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.ElementAffection, data.Holder2.ObjectType, ElementFlag.CanBurn, data.Holder2.RoomType));
			data.Holder2.MissionDone[1] = true;
		}
	}

	private void BurnToExplode(ElementInteractionData data)
	{
		data.Holder2.Flag3 |= ElementFlag.CanExplode;
		Explode(data.Holder2);
		
		if (!data.Holder2.MissionDone[3])
		{
			MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.ElementAffection, data.Holder2.ObjectType, ElementFlag.CanExplode, data.Holder2.RoomType));
			data.Holder2.MissionDone[3] = true;
		}
	}

	private void ElectricToBurn(ElementInteractionData data)
	{
		data.Holder2.Durations.fireTimer = objectDefinition.durations.fireDuration;
		data.Holder2.Flag3 |= ElementFlag.CanBurn;
		
		if (!data.Holder2.MissionDone[1])
		{
			MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.ElementAffection, data.Holder2.ObjectType, ElementFlag.CanBurn, data.Holder2.RoomType));
			data.Holder2.MissionDone[1] = true;
		}
	}

	private void ElectricToElectric(ElementInteractionData data)
	{
		data.Holder2.Durations.electricityTimer = objectDefinition.durations.electricityDuration;
		data.Holder2.Flag3 |= ElementFlag.CanConduct;
		
		if (!data.Holder2.MissionDone[2])
		{
			MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.ElementAffection, data.Holder2.ObjectType, ElementFlag.CanConduct, data.Holder2.RoomType));
			data.Holder2.MissionDone[2] = true;
		}
	}

	private void ElectricToExplode(ElementInteractionData data)
	{
		data.Holder2.Flag3 |= ElementFlag.CanExplode;
		Explode(data.Holder2);
		
		if (!data.Holder2.MissionDone[3])
		{
			MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.ElementAffection, data.Holder2.ObjectType, ElementFlag.CanExplode, data.Holder2.RoomType));
			data.Holder2.MissionDone[3] = true;
		}
	}

	private void WetToWet(ElementInteractionData data)
	{
		data.Holder2.Durations.waterTimer = objectDefinition.durations.waterDuration;
		data.Holder2.Flag3 |= ElementFlag.CanBeWet;
		
		if (!data.Holder2.MissionDone[0])
		{
			MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.ElementAffection, data.Holder2.ObjectType, ElementFlag.CanBeWet, data.Holder2.RoomType));
			data.Holder2.MissionDone[0] = true;
		}
	}

	#endregion

	#region instant methodes
	
	protected void Explode(IElementHolder holder)
	{
		foreach (var e in LevelGenerator.Generator.ElementHolders)
		{
			if (Vector3.Distance(e.Transform.position, holder.Transform.position) > objectDefinition.destructionApplicationDistance)
				continue;

			Physics.Linecast(e.Transform.position + e.Collider.center, holder.Transform.position + holder.Collider.center, out var hit, objectDefinition.blockLayer);
			if (hit.transform is not null && !hit.transform.TryGetComponent<IElementHolder>(out _))
				continue;
			
			if ((e.Flag2 & ElementFlag.CanBurn) == 0)
				continue;

			e.Flag3 |= ElementFlag.CanBurn;
			SetParticleOverride(e, ElementFlag.CanBurn, true);
			e.Durations.fireTimer = e.Durations.fireDuration;

			if (!e.MissionDone[1])
			{
				MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.ElementAffection, objectDefinition.@object, ElementFlag.CanBurn, RoomType));
				_missionDone[1] = true;
			}
		}
	}
	
	#endregion

	#endregion

	#region fields
	
	[SerializeField] protected ObjectDefinition objectDefinition;

	private ElementInteractionDataPair[] _resolveInteractions;

	private ElementInteractionDataPair[] _nextInteractions;

	private bool[] _missionDone;

	#endregion
}