namespace Runtime.Utils.Singleton;

public class Singleton<T> : MonoBehaviour where T : Component
{
    #region properties

    public static bool HasInstance => instance is not null;
        
    public static T Current => instance;
        
    public static T Instance
    {
        get
        {
            if (instance is not null)
                return instance;
            
            instance = FindFirstObjectByType<T>();
            
            if (instance is not null)
                return instance;
            
            var obj = new GameObject
            {
                name = typeof(T).Name + "AutoCreated"
            };
            
            instance = obj.AddComponent<T>();
            return instance;
        }
    } 

    #endregion

    #region methodes

    protected virtual void Awake() => InitializeSingleton();

    protected virtual void InitializeSingleton()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (unparentOnAwake)
        {
            transform.SetParent(null);
        }

        if (instance is null)
        {
            instance = this as T;
            enabled = true;
        }
        else
        {
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }
        
    #endregion

    #region fields
        
    [Tooltip("if this is true, this singleton will auto detach if it finds itself parented on awake")]
    public bool unparentOnAwake = true;

    protected static T instance;

    #endregion
}