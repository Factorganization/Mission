using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Actors.ActorViews;
using Shared.Utils.Listing;
using UnityEngine;

namespace Runtime.Management.GameManagement
{
    public class LevelGenerator : MonoBehaviour
    {
        #region properties

        public ActorList<IPossessable> Possessables => _possessables;

        #endregion
        
        #region methodes

        private void Start()
        {
            _possessables = new ActorList<IPossessable>();
            
            Pooler.PoolWithAttribute(_possessables, FindObjectsByType<ActorView>(FindObjectsSortMode.None));
        }

        #endregion

        #region fields

        private ActorList<IPossessable> _possessables;
        
        #endregion
    }
}