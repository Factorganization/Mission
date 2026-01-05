using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
using Runtime.Services.Game.GameContent.Actors.ActorViews;
using Runtime.Services.Game.GameContent.Logics.LogicInterfaces;
using Runtime.Services.Game.GameContent.Logics.LogicModels;
using Runtime.Services.Game.GameContent.Logics.LogicModels.MissionModels;
using Shared.Utils.Listing;

namespace Runtime.Services.Game.GameSystems
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
                    var b = new Bounds(r.col.bounds.center, new Vector3(r.col.bounds.size.x, 100, r.col.bounds.size.z));
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
                    var b = new Bounds(r.col.bounds.center, new Vector3(r.col.bounds.size.x, 100, r.col.bounds.size.z));
                    if (b.Contains(e.Transform.position))
                    {
                        e.RoomType = r.roomType;
                        break;
                    }
                    
                    e.RoomType = RoomType.House;
                }
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!debug)
                return;
            
            foreach (var r in rooms)
            {
                Gizmos.color = Color.red;
                var b = new Bounds(r.col.bounds.center, new Vector3(r.col.bounds.size.x, 100, r.col.bounds.size.z));
                Gizmos.DrawWireCube(b.center, b.size);
            }
        }
#endif

        #endregion

        #region fields

        private ActorList<IPossessable> _possessables;
        
        private ActorList<IGrabbable> _grabbables;
        
        private ActorList<IElementHolder> _elementHolders;
        
        [SerializeField] private RoomModel[] rooms;

        [SerializeField] private bool debug;
        
        private float _delay;

        #endregion
    }
}