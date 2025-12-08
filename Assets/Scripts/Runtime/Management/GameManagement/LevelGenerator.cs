using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Actors.ActorViews;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using Runtime.GameContent.Logics.LogicModels.MissionModels;
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

            foreach (var e in _elementHolders)
            {
                foreach (var r in rooms)
                {
                    var b = new Bounds(r.collider.bounds.center, new Vector3(r.collider.bounds.size.x, 100, r.collider.bounds.size.z));
                    if (b.Contains(e.Transform.position))
                    {
                        e.RoomType = r.roomType;
                        break;
                    }
                    
                    e.RoomType = RoomType.House;
                }
            }
        }

        private void Update()
        {
            _delay += Time.deltaTime;

            if (_delay <= 0.25f)
                return;

            _delay = 0;

            foreach (var g in _grabbables)
            {
                if (g is not IElementHolder e)
                    continue;
                
                foreach (var r in rooms)
                {
                    var b = new Bounds(r.collider.bounds.center, new Vector3(r.collider.bounds.size.x, 100, r.collider.bounds.size.z));
                    if (b.Contains(e.Transform.position))
                    {
                        e.RoomType = r.roomType;
                        break;
                    }
                    
                    e.RoomType = RoomType.House;
                }
            }
        }

        #endregion

        #region fields

        private ActorList<IPossessable> _possessables;
        
        private ActorList<IGrabbable> _grabbables;
        
        private ActorList<IElementHolder> _elementHolders;
        
        [SerializeField] private RoomModel[] rooms;

        private float _delay;

        #endregion
    }
}