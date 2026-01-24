using Runtime.Utils.Singleton;

namespace Runtime
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-1)]
    public class Bootstrapper : Singleton<Bootstrapper>
    {
        #region methodes
        
        protected override void Awake()
        {
            base.Awake();
            
            try
            {
                Debug.Log("Bootstrapper...");
                {
                    foreach (var s in services)
                    {
                        if (!s.Init())
                        {
                            Debug.LogError("Service didn't initialize properly");
                            continue;
                        }
                        serviceLocator.Register(s.GetType(), s);
                        Debug.Log($"Service registered: {s}");
                    }
                }
                
                Debug.Log("Bootstrapper loaded");
            }
            catch (Exception e)
            {
                throw new Exception("Bootstrapper failed to load scene", e);
            }
        }

        private void Start()
        {
            foreach (var s in services)
                s.Begin();
        }

        private void Update()
        {
            foreach (var s in services)
                s.Tick();
        }

        private void OnDestroy()
        {
            foreach (var s in services)
                s.Delete();
        }

        #endregion

        #region fields

        [SerializeField] private ServiceLocator serviceLocator;
        
        [SerializeField] private AService[] services;

        #endregion
    }
}