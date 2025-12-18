using Runtime.Service;
using Runtime.Utils.Singleton;

namespace Runtime.Management.SceneManagement
{
    [DisallowMultipleComponent]
    public class Bootstrapper : Singleton<Bootstrapper>
    {
        #region methodes
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void Init()
        {
            try
            {
                Debug.Log("Bootstrapper...");
                //await SceneManager.LoadSceneAsync("SceneLoader", LoadSceneMode.Single);
            }
            catch (Exception e)
            {
                throw new Exception("Bootstrapper failed to load scene", e);
            }
        }
        
        #endregion

        #region fields

        [SerializeField] private AService[] services;

        #endregion
    }
}