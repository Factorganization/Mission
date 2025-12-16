using Runtime.GameContent.Actors.ActorControllers;
using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicModels.ElementModels;
using Shared.Utils.Listing;

namespace Runtime.GameContent.Actors.ActorViews
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
			base.Update();
			if (!IsResetingPos)
				return;
			
			Transform.position += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(Transform.position, OriginPos, 0.1f);
			if (Vector3.Distance(Transform.position, OriginPos) > 0.1f)
			{
				Transform.position = OriginPos;
				IsResetingPos = false;
			}
		}

		#endregion
		
		#region grabbable
		
		public bool Action()
		{
			Active = !Active;
			return Active;
		}
		
		#endregion

		#endregion

		#region fields

		[SerializeField] private ElementFlag element;
		
		private Rigidbody _rb;

		#endregion
	}
}