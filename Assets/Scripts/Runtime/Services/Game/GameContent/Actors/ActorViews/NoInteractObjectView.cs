using Runtime.Service;
using Runtime.Services.Audio;
using Runtime.Services.Game.GameContent.Actors.ActorControllers;
using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
using Runtime.Services.Game.GameContent.Logics.LogicInterfaces;
using Runtime.Services.Game.GameContent.Logics.LogicModels.ElementModels;
using Shared.Utils.Listing;

namespace Runtime.Services.Game.GameContent.Actors.ActorViews
{
	[Pooled, SelectionBase]
	public class NoInteractObjectView : ElementHolderController, INoInteract
	{
		#region proprties
        
		public override ElementFlag Flag1 { get; set; }

		public override ElementFlag Flag2 => flag;

		public override ElementFlag Flag3
		{
			get => Flag1;
			set => Flag1 = value;
		}

		public bool AtOriginPos { get; set; }

		public override bool Active
		{
			get => true;
			set { }
		}
        
		#endregion

		#region methodes

		protected override void Explode(IElementHolder holder)
		{
			base.Explode(holder);
			
			if (_alreadyExploded)
				return;
			
			_alreadyExploded = true;
			
			impulseSource?.GenerateImpulseAt(Transform.position, Vector3.one);
			var a = ServiceLocator.Instance.Get<AudioService>();
			a.PlayOneShot(a.Atlas.sfx.effects.fire.bigExplosion, Transform.position);
		}

		#endregion
		
		#region fields

		[SerializeField] private ElementFlag flag;
		
		private bool _alreadyExploded;

		#endregion
	}
}