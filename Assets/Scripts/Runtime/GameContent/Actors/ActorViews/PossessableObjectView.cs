using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    [Pooled]
    public class PossessableObjectView : ActorView, IPossessable, ISource
    {
        #region properties

        public Transform Transform => transform;
        
        public bool Possessed { get; set; }

        public ElementFlag SourceElement => sourceElement;

        public ElementFlag ReceptorElement => receptorElement;

        public bool Active { get; set; }

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

        public void ReactToElement(ElementFlag elementFlag)
        {
            
        }

        #endregion

        #region fields

        [SerializeField] private ElementFlag sourceElement;
        
        [SerializeField] private ElementFlag receptorElement;

        #endregion
    }
}