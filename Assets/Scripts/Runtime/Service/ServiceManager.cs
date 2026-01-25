using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runtime.Service;

public class ServiceManager : IDisposable, IAsyncDisposable
{
    #region properties

    public IEnumerable<object> RegisteredServices => _services.Values;

    #endregion
    
    #region constructors

    ~ServiceManager()
    {
        ReleaseUnmanagedResources();
    }

    #endregion
    
    #region methodes
    
    #region services

    public bool TryGet<T>(out T service) where T : AService
    {
        var t = typeof(T);

        if (_services.TryGetValue(t, out var obj))
        {
            service = obj as T;
            return true;
        }
        
        service = null;
        return false;
    }

    public T Get<T>() where T : AService
    {
        var t = typeof(T);

        if (_services.TryGetValue(t, out var obj))
            return obj as T;
        
        throw new ArgumentException($"Service {t} not found");
    }

    public ServiceManager Register<T>(T service) where T : AService
    {
        var t = typeof(T);
        
        if (!_services.TryAdd(t, service))
            Debug.LogError($"Service {t} already registered");
        
        return this;
    }

    public ServiceManager Register(Type type, object service)
    {
        if (!type.IsInstanceOfType(service))
            throw new ArgumentException($"Service {type} is not an instance of type {service.GetType()}");
        
        if (!_services.TryAdd(type, service))
            Debug.LogError($"Service {type} already registered");
        
        return this;
    }

    #endregion
    
    #region dispose
    
    private void ReleaseUnmanagedResources()
    {
        
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    #endregion
    
    #endregion

    #region fields

    private readonly Dictionary<Type, object> _services = new();
    

    #endregion
}