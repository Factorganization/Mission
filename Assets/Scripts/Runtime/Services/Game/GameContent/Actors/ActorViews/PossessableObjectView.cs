using Runtime.Services.Audio;
using Runtime.Services.Data;
using Runtime.Services.Game.GameContent.Actors.ActorControllers;
using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
using Runtime.Services.Game.GameContent.Logics.LogicInterfaces;
using Runtime.Services.Game.GameContent.Logics.LogicModels.ElementModels;
using Runtime.Services.Game.GameContent.Logics.LogicModels.MissionModels;
using Runtime.Services.Game.GameSystems;
using Shared.Utils.Listing;

namespace Runtime.Services.Game.GameContent.Actors.ActorViews
{
	[Pooled, SelectionBase]
	public class PossessableObjectView : ElementHolderController, IPossessable
	{
		#region properties
		
		#region element holder
		
		public override ElementFlag Flag1
		{
			get => sourceElement;
			set { }
		}

		public override ElementFlag Flag2 => receptorElement;
        
		public override ElementFlag Flag3 { get; set; }

		public override bool Active
		{
			get => _active /*&& !Destroyed*/;
			set => _active = value;
		}

		#endregion
		
		#region possessable
		
		public bool Possessed
		{
			get => _possessed;
			set
			{
				_possessed = value;

				if (_destroyed)
					return;
				
				if (_possessed)
				{
					SetModel(2);
					return;
				}

				SetModel(1);
			}
		}

		public bool Destroyed
		{
			get => _destroyed;
			set
			{
				_destroyed = value;
				if (_destroyed)
				{
					SetModel(3);
					return;
				}

				SetModel(1);
				Active = false;
				Flag3 = Flag1;
				exploded = false;
				_alreadyExploded = false;
				
				foreach (var p in VFX.waterParticles)
					p.Stop();
				VFX.waterPlaying = false;
				foreach (var p in VFX.fireParticles)
					p.Stop();
				VFX.firePlaying = false;
				foreach (var p in VFX.electricParticles)
					p.Stop();
				VFX.elecPlaying = false;
				foreach (var p in VFX.explosionParticles)
					p.Stop();
				VFX.explodePlaying = false;
			}
		}

		public Vector3 TargetPosition
		{
			get => aiTargetPosition.position;
			private set => aiTargetPosition.position = value;
		}

		public bool Possessable
		{
			get => _possessable;
			set
			{
				_possessable = value;

				if (!_possessable)
				{
					indic.gameObject.SetActive(false);
					return;
				}
				
				indic.gameObject.SetActive(true);
			}
		}
		
		#endregion

		#endregion

		#region methodes

		#region unity events

		public bool AtOriginPos { get; set; }

		protected override void Start()
		{
			base.Start();

			Active = false;
			AtOriginPos = true;
			Possessed = false;
			Destroyed = destroyedAtStart;

			if (aiTargetPosition == null)
				aiTargetPosition = Transform;
		}

		private void FixedUpdate()
		{
			if (_possessable)
				indic.LookAt(GameManager.Instance.Player.UiOverLayCam);
		}
		
		#endregion

		#region possessable
		
		public void Action()
		{
			Active = !Active;

			if (!Active)
			{
				Flag3 = Flag1;
				foreach (var p in VFX.waterParticles)
					p.Stop();
				VFX.waterPlaying = false;
				foreach (var p in VFX.fireParticles)
					p.Stop();
				VFX.firePlaying = false;
				foreach (var p in VFX.electricParticles)
					p.Stop();
				VFX.elecPlaying = false;
				foreach (var p in VFX.explosionParticles)
					p.Stop();
				VFX.explodePlaying = false;
				return;
			}

			SetParticle(this);
		}

		public void DestructiveAction()
		{
			Destroyed = true;
			Active = true;
			Flag3 = Flag1;

			if ((Flag3 & ElementFlag.CanBurn) != 0)
				objectDefinition.durations.fireTimer = objectDefinition.durations.fireDuration;
			if ((Flag3 & ElementFlag.CanBeWet) != 0)
				objectDefinition.durations.waterTimer = objectDefinition.durations.waterDuration;
			if ((Flag3 & ElementFlag.CanConduct) != 0)
				objectDefinition.durations.electricityTimer = objectDefinition.durations.electricityDuration;
			
			//TODO how ?
			//TODO what ?
			SetParticle(this);

			if ((Flag1 & ElementFlag.CanBeWet) != 0 && !MissionDone[0])
			{
				MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.ElementAffection, objectDefinition.@object, ElementFlag.CanBeWet, RoomType));
				MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.Action, objectDefinition.@object, ElementFlag.CanBeWet, RoomType));
				MissionDone[0] = true;
			}
			if ((Flag1 & ElementFlag.CanBurn) != 0 && !MissionDone[1])
			{
				MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.ElementAffection, objectDefinition.@object, ElementFlag.CanBurn, RoomType));
				MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.Action, objectDefinition.@object, ElementFlag.CanBurn, RoomType));
				MissionDone[1] = true;
			}
			if ((Flag1 & ElementFlag.CanConduct) != 0 && !MissionDone[2])
			{
				MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.ElementAffection, objectDefinition.@object, ElementFlag.CanConduct, RoomType));
				MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.Action, objectDefinition.@object, ElementFlag.CanConduct, RoomType));
				MissionDone[2] = true;
			}
			if ((Flag1 & ElementFlag.CanExplode) != 0 && !MissionDone[^1])
			{
				MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.ElementAffection, objectDefinition.@object, ElementFlag.CanExplode, RoomType));
				MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.Action, objectDefinition.@object, ElementFlag.CanExplode, RoomType));
				MissionDone[^1] = true;
			}
            
			if ((Flag3 & ElementFlag.CanExplode) != 0)
				Explode(this);
		}

		private void SetModel(int i)
		{
			baseModel.SetActive(i == 1);
			possessedModel.SetActive(i == 2);
			destroyedModel.SetActive(i == 3);
		}

		#endregion

		#region elements

		protected override void Explode(IElementHolder holder)
		{
			base.Explode(holder);
			
			if (_alreadyExploded)
				return;
			
			_alreadyExploded = true;
			
			ElementManager.Element.TempMalice += 20;
			impulseSource?.GenerateImpulseAt(Transform.position, Vector3.one);
			var a = ServiceLocator.Instance.Get<AudioService>();
			a.PlayOneShot(a.Atlas.sfx.effects.fire.bigExplosion, Transform.position);
		}

		#endregion
		
		#endregion

		#region fields

		[SerializeField] private ElementFlag sourceElement;

		[SerializeField] private ElementFlag receptorElement;

		[SerializeField] private GameObject baseModel;

		[SerializeField] private GameObject possessedModel;

		[SerializeField] private GameObject destroyedModel;

		[SerializeField] private Transform aiTargetPosition;

		[SerializeField] private Transform indic;
			
		[SerializeField] private bool destroyedAtStart;

		private bool _active;

		private bool _possessed;

		private bool _destroyed;
		
		private bool _alreadyExploded;

		private bool _possessable;

		#endregion
	}
}