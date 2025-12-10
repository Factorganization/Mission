using UnityEngine;

namespace Runtime.GameContent.Actors.ActorInterfaces
{
    public interface IPossessable
    {
        /// <summary>
        /// Transform of the possessed object
        /// </summary>
        public Transform Transform { get; }
        
        /// <summary>
        /// Collider of the possessable object 
        /// </summary>
        public BoxCollider Collider { get; }
        
        /// <summary>
        /// True if the object is being possessed, False otherwise 
        /// </summary>
        public bool Possessed { get; set; }

		/// <summary>
		/// True if the object was destroyed and cant be used anymore
		/// </summary>
		public bool Destroyed { get; set; }
        
        /// <summary>
        /// Action that can be performed if the object is being possessed
        /// </summary>
        public void Action();

        /// <summary>
        /// Action that can be performed if the object is being possessed, will kick out the player of the object after the action is performed. Can destroy th object
        /// </summary>
        public void DestructiveAction();
    }
}