using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
using Shared.Utils.Listing;

namespace Runtime.Services.Game.GameContent.Actors.ActorViews
{
	[Pooled, SelectionBase]
	public class SpawnerObjectView : ActorView
	{
		#region fields

		[SerializeField] private IGrabbable grabbable;

		#endregion
	}
}