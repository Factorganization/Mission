namespace Runtime.Services.Game.GameContent.Actors.ActorInterfaces
{
    public interface IGrabbable
    {
        /// <summary>
        /// Transform of the grabbed object
        /// </summary>
        public Transform Transform { get; }
        
        /// <summary>
        /// RigidBody of the grabbed object
        /// </summary>
        public Rigidbody Rigidbody { get; }

		/// <summary>
		/// Spawn Position of the grabbable obj
		/// </summary>
		public Vector3 OriginPos { get; }
		public bool IsResetingPos { get; set; }

		/// <summary>
		/// Return True if object is active
		/// </summary>
		public bool Active { get; } 
		
		/// <summary>
		/// Return true if object is being grabbed
		/// </summary>
		public bool Grabbed { get; set; }
		
		/// <summary>
		/// Trigger an action when grabbing an object
		/// </summary>
		/// <returns></returns>
		public bool Action();
    }
}