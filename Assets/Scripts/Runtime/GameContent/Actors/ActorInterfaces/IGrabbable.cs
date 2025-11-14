using UnityEngine;

namespace Runtime.GameContent.Actors.ActorInterfaces
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

		/// <summary>
		/// Trigger an action when grabbing an object
		/// </summary>
		/// <returns></returns>
		public bool Action();
    }
}