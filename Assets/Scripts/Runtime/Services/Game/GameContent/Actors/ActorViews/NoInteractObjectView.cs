using Runtime.Services.GameService.GameContent.Actors.ActorControllers;
using Runtime.Services.GameService.GameContent.Actors.ActorInterfaces;
using Runtime.Services.GameService.GameContent.Logics.LogicModels.ElementModels;
using Shared.Utils.Listing;

namespace Runtime.Services.GameService.GameContent.Actors.ActorViews
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
        
		public override bool Active
		{
			get => true;
			set { }
		}
        
		#endregion

		#region fields

		[SerializeField] private ElementFlag flag;

		#endregion
	}
}