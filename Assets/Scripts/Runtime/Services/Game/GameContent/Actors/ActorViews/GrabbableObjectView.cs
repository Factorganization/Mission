using Runtime.Services.Game.GameContent.Actors.ActorControllers;
using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
using Runtime.Services.Game.GameContent.Logics.LogicModels.ElementModels;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Controller;
using Runtime.Services.Game.GameSystems;
using Shared.Utils.Listing;

namespace Runtime.Services.Game.GameContent.Actors.ActorViews
{
	[Pooled, SelectionBase, RequireComponent(typeof(Rigidbody))]
	public class GrabbableObjectView : ElementHolderController, IGrabbable
	{
		#region properties
		
		#region element holder

		public override ElementFlag Flag1 { get; set; }

		public override ElementFlag Flag2 => element;

		public override ElementFlag Flag3
		{
			get => Flag1;
			set => Flag1 = value;
		}
		
		public bool IsResetingPos { get; set; }
		public override bool Active { get; set; }
		
		#endregion
		
		#region grabbable
		
		public Rigidbody Rigidbody => _rb;
		
		public Vector3 OriginPos { get; private set; }
		
		#endregion
		
		#endregion

		#region methodes

		#region unity events
		
		protected override void Start()
		{
			base.Start();
			
			_rb = GetComponent<Rigidbody>();
			OriginPos = transform.position;
			Active = true;
			IsResetingPos = false; 
		}

		protected override void Update()
		{
#if UNITY_EDITOR
			if (objectDefinition.debugInfo.debug)
				objectDefinition.debugInfo.text.text = $"{(Active ? "<color=green>Active</color>" : "<color=red>Inactive</color>")}\n {Convert.ToString((int)Flag1, 2).PadLeft(4, '0')} \n {Convert.ToString((int)Flag2, 2).PadLeft(4, '0')}";
#endif

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

				if (objectDefinition.durations.fireTimer < -0.5f)
				{
					Transform.position = _spawnerRef ? _spawnerRef.SpawnPos.position : OriginPos;
					//TODO virer ca de la 
					var p = GameManager.Instance.Player.PlayerModel;
					p.ResetGrabbedObjectState();
					p.SetAnimParam(p.isHolding, false);
					p.SetAnimParam(p.isInteracting, false);
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
			
			if (IsResetingPos)
			{
				Transform.position += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(Transform.position, OriginPos, 0.1f);
				if (Vector3.Distance(Transform.position, OriginPos) > 0.1f)
				{
					Transform.position = OriginPos;
					IsResetingPos = false;
				}
			}
		}

		#endregion
		
		#region grabbable
		
		public bool Action()
		{
			Active = !Active;
			return Active;
		}

		public void SetSpawner(SpawnerObjectView spawner)
		{
			_spawnerRef = spawner;
		}
		
		#endregion

		#endregion

		#region fields

		[SerializeField] private ElementFlag element;
		
		private Rigidbody _rb;

		private SpawnerObjectView _spawnerRef;

		#endregion
	}
}