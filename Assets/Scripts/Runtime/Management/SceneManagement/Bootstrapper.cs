using Runtime.Utils;
using UnityEngine.SceneManagement;

namespace Runtime.Management.SceneManagement
{
    public class Bootstrapper : PersistentSingleton<Bootstrapper>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private async void Init()
        {
            Debug.Log("Bootstrapper...");
            await SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
        }
    }
}