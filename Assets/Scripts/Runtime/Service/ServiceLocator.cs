using Runtime.Utils.Singleton;

namespace Runtime.Service
{
    public class ServiceLocator : Singleton<ServiceLocator>
    {
        #region methodes

        public ServiceLocator Register<T>(T service) where T : AService
        {
            _serviceManager.Register(service);
            return this;
        }

        public ServiceLocator Register(Type type, object service)
        {
            _serviceManager.Register(type, service);
            return this;
        }

        public T Get<T>() where T : AService
        {
            if (TryGetService(out T service))
                return service;
                
            throw new ArgumentException($"Service of type {typeof(T).FullName} not found");
        }

        public bool TryGet<T>(out T service) where T : AService
        {
            service = null;
            return TryGetService(out service);
        }

        private bool TryGetService<T>(out T service) where T : AService
        {
            return _serviceManager.TryGet(out service);
        }

        #endregion
    
        #region fields

        private readonly ServiceManager _serviceManager = new();

        #endregion
    }
}