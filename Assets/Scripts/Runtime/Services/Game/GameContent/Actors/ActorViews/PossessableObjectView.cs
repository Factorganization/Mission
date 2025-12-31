using Runtime.Services.Game.GameContent.Actors.ActorControllers;
using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
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
		
		#endregion

		#endregion

		#region methodes

		#region unity events
		
		protected override void Start()
		{
			base.Start();

			Active = false;
			Possessed = false;
			Destroyed = destroyedAtStart;
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
			Possessed = false;
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

			if (!MissionDone[^1])
			{
				if (MissionManager.Manager.TryGetAndSetMission(new MissionModel(MissionType.Action, objectDefinition.@object, ElementFlag.CanExplode, RoomType)))
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
		
		#endregion

		#region fields

		[SerializeField] private ElementFlag sourceElement;

		[SerializeField] private ElementFlag receptorElement;

		[SerializeField] private GameObject baseModel;

		[SerializeField] private GameObject possessedModel;

		[SerializeField] private GameObject destroyedModel;

		[SerializeField] private bool destroyedAtStart;

		private bool _active;

		private bool _possessed;

		private bool _destroyed;

		#endregion
	}
}