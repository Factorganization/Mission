using Runtime.GameContent.Actors.ActorInterfaces;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    [Pooled]
    public class PossessableObjectView : ActorView, IPossessable
    {
        #region properties

        public Transform Transform => transform;
        
        public bool Possessed { get; set; }

        #endregion

        #region methodes

        public void Action()
        {
            Debug.Log("Action");
        }

        public void DestructiveAction()
        {
            Debug.Log("DestructiveAction");
        }

        #endregion
    }
}