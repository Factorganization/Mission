using System.Threading.Tasks;

namespace Runtime.Service;

public class ServiceManager : IDisposable, IAsyncDisposable
{
    #region constructors

    ~ServiceManager()
    {
        ReleaseUnmanagedResources();
    }

    #endregion
    
    #region methodes
    
    
    
    #region dispose
    
    private void ReleaseUnmanagedResources()
    {
        // TODO release unmanaged resources here
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
}