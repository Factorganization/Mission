using UnityEngine;

namespace Runtime.GameContent.Actors.ActorInterfaces
{
    public interface IGrabbable
    {
        public Transform Transform { get; }
        
        public Rigidbody Rigidbody { get; }
    }
}