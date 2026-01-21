using Shared.Utils.Listing;

namespace Runtime.Services.Game.GameContent.Actors.ActorViews
{
	[Pooled, SelectionBase]
	public class SpawnerObjectView : ActorView
	{
		#region properties

		public Transform SpawnPos => spawnPos;

		#endregion
		
		#region methodes

		private void Start()
		{
			grabbable?.SetSpawner(this);
		}

		#endregion
		
		#region fields

		[SerializeField] private GrabbableObjectView grabbable;

		[SerializeField] private Transform spawnPos;

		#endregion
	}
}