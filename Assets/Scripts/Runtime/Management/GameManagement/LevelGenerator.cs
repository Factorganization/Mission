using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Actors.ActorViews;
using Runtime.GameContent.Logics.LogicInterfaces;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.Management.GameManagement
{
    public class LevelGenerator : MonoBehaviour
    {
        #region properties

        public ActorList<IPossessable> Possessables => _possessables;
        
        public ActorList<IGrabbable> Grabbables => _grabbables;
        
        public ActorList<IElementHolder> ElementHolders => _elementHolders;
        
        public static LevelGenerator Generator { get; private set; }

        #endregion
        
        #region methodes

        private void Awake()
        {
            if (Generator is not null)
                Debug.LogWarning("LevelGenerator already instantiated");
            
            Generator = this;
        }
        
        private void Start()
        {
            _possessables = new ActorList<IPossessable>();
            _grabbables = new ActorList<IGrabbable>();
            _elementHolders = new ActorList<IElementHolder>();

            var actors = FindObjectsByType<ActorView>(FindObjectsSortMode.None);
            
            Pooler.PoolWithAttribute(_possessables, actors);
            Pooler.PoolWithAttribute(_grabbables, actors);
            Pooler.PoolWithAttribute(_elementHolders, actors);
        }

        #endregion

        #region fields

        private ActorList<IPossessable> _possessables;
        
        private ActorList<IGrabbable> _grabbables;
        
        private ActorList<IElementHolder> _elementHolders;

        #endregion
    }
}