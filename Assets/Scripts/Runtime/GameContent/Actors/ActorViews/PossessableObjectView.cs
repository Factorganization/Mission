using Runtime.GameContent.Actors.ActorControllers;
using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicModels.ElementModels;
using Runtime.GameContent.Logics.LogicModels.MissionModels;
using Runtime.Management.GameManagement;
using Shared.Utils.Listing;

namespace Runtime.GameContent.Actors.ActorViews
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
		
		public bool Possessed { get; set; }

		public bool Destroyed
		{
			get => _destroyed;
			set
			{
				_destroyed = value;
				if (_destroyed)
					return;
		        
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
				MissionDone[^1] = true;
				MissionManager.Manager?.TryGetMission(new MissionModel(MissionType.Action, objectDefinition.@object, ElementFlag.CanExplode, RoomType));
			}
            
			if ((Flag3 & ElementFlag.CanExplode) != 0)
				Explode(this);
		}

		#endregion
		
		#endregion

		#region fields

		[SerializeField] private ElementFlag sourceElement;

		[SerializeField] private ElementFlag receptorElement;
		
		[SerializeField] private bool destroyedAtStart;

		private bool _active;
		
		private bool _destroyed;

		#endregion
	}
}