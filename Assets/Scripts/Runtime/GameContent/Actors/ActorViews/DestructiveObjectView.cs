using Runtime.GameContent.Actors.ActorInterfaces;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    [Pooled]
    public class DestructiveObjectView : ActorView, IPossessable
    {
        #region properties

        public Transform Transform => transform;
        
        public bool Possessed { get; set; }

        #endregion

        #region methodes

        public bool Action()
        {
            Debug.Log("Destructive Action");
            return true;
        }

        #endregion
    }
}