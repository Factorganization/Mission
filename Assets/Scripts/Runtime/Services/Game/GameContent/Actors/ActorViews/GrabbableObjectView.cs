using Runtime.Services.Audio;
using Runtime.Services.Game.GameContent.Actors.ActorControllers;
using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
using Runtime.Services.Game.GameContent.Logics.LogicInterfaces;
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
		
		public override bool Active { get; set; }
		
		#endregion
		
		#region grabbable
		
		public Rigidbody Rigidbody => _rb;
		
		public Vector3 OriginPos { get; private set; }
		
		public bool Grabbed { get; set; }
		
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
		}

		protected override void Update()
		{
			base.Update();

			if ((Flag3 & ElementFlag.CanBurn) != 0)
			{
				_fireDestructionTimer += Time.deltaTime;

				if (_fireDestructionTimer > fireDestructionDuration)
				{
					Transform.position = _spawnerRef ? _spawnerRef.SpawnPos.position : OriginPos;

					if (Grabbed)
					{
						var p = GameManager.Instance.Player.PlayerModel;
						p.ResetGrabbedObjectState();
						p.SetAnimParam(p.isHolding, false);
						p.SetAnimParam(p.isInteracting, false);
					}

					Flag3 &= ~ElementFlag.CanBurn;
					Durations.fireTimer = 0;
					SetParticleOverride(this, ElementFlag.CanBurn, false);
				}
			}
			else
				_fireDestructionTimer = 0;
		}

		private void FixedUpdate()
		{
			SetSmoothPosition();
		}

		#endregion

		#region element holder

		protected override void Explode(IElementHolder holder)
		{
			base.Explode(holder);
			
			var p = GameManager.Instance.Player.PlayerModel;

			if (Grabbed)
			{
				p.ResetGrabbedObjectState();
				p.SetAnimParam(p.isHolding, false);
				p.SetAnimParam(p.isInteracting, false);
			}
			
			Transform.position = _spawnerRef ? _spawnerRef.SpawnPos.position : OriginPos;
			exploded = false;
			
			if (_alreadyExploded)
				return;

			_alreadyExploded = true;
			
			impulseSource?.GenerateImpulseAt(Transform.position, Vector3.one);
			var a = ServiceLocator.Instance.Get<AudioService>();
			a.PlayOneShot(a.Atlas.sfx.effects.fire.bigExplosion, Transform.position);
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

		public void StartSmoothPosition(Vector3 targetPos)
		{
			_returning = true;
			_targetPos = targetPos;
		}

		private void SetSmoothPosition()
		{
			if (!_returning)
				return;

			if (Grabbed)
			{
				_returning = false;
				return;
			}

			if (Vector3.Distance(Transform.position, _targetPos) < 0.2f)
			{
				Transform.position = _targetPos;
				_returning =  false;
				return;
			}
			
			Transform.position += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(Transform.position, _targetPos, 0.05f);
		}
		
		#endregion

		#endregion

		#region fields

		[SerializeField] private ElementFlag element;

		[SerializeField] private float fireDestructionDuration;
		
		private Rigidbody _rb;

		private SpawnerObjectView _spawnerRef;

		private Vector3 _targetPos;
		
		private float _fireDestructionTimer;

		private bool _returning;

		private bool _alreadyExploded;

		#endregion
	}
}