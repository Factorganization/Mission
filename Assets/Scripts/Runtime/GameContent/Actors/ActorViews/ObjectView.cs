using Runtime.GameContent.Actors.ActorInterfaces;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    [Pooled]
    public class ObjectView : ActorView, IPossessable
    {
        #region properties

        public bool Possessed { get; set; }

        #endregion

        #region methodes

        public void Action()
        {
            Debug.Log("Action");
        }

        #endregion
    }
}