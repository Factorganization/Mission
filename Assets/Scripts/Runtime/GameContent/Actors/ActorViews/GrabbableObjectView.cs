using Runtime.GameContent.Actors.ActorInterfaces;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    [Pooled]
    public class GrabbableObjectView : ActorView, IGrabbable
    {
        #region properties
        
        public Transform Transform => transform;
        
        public Rigidbody Rigidbody => rb;
        
        #endregion
        
        #region methodes
        
        
        
        #endregion
        
        #region fields
        
        [SerializeField] private Rigidbody rb;
        
        #endregion
    }
}