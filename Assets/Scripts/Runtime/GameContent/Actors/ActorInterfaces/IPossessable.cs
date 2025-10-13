using UnityEngine;

namespace Runtime.GameContent.Actors.ActorInterfaces
{
    public interface IPossessable
    {
        public Transform Transform { get; }
        
        public bool Possessed { get; set; }
        
        public bool Action();
    }
}