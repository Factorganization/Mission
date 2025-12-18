namespace Runtime.Service;

public interface IService
{
    public bool Init();
    
    public void Tick();
    
    public void Delete();
}