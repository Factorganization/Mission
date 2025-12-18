namespace Runtime.Service;

public abstract class AService : MonoBehaviour, IService
{
    public virtual bool Init()
    {
        return true;
    }

    public virtual void Tick()
    {
    }

    public virtual void Delete()
    {
    }
}