using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Runtime.Services.Scene.SceneSystems
{
    public class SceneGroupManager : IDisposable, IAsyncDisposable
    {
        #region constructors
        
        ~SceneGroupManager()
        {
            ReleaseUnmanagedResources();
        }
        
        #endregion
        
        #region methodes
        
        public async Task LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes = false)
        {
            if (_loading)
                return;

            _loading = true;
            Debug.Log("Loading scenes");
            
            _activeSceneGroup = group;
            var loadedScenes = new List<string>();

            await UnloadScenes();

            var sceneCount = SceneManager.sceneCount;

            for (var i = 0; i < sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }
            
            var operationGroup = new AsyncOperationGroup(_activeSceneGroup.scenes.Count);

            foreach (var scene in _activeSceneGroup.scenes)
            {
                /*if (!reloadDupScenes && loadedScenes.Contains(scene.Name))
                    continue;*/
                
                var operation = SceneManager.LoadSceneAsync(scene.reference.Path, LoadSceneMode.Additive);

                await Task.Delay(TimeSpan.FromSeconds(2.5f));
                
                operationGroup.operations.Add(operation);
                
                OnSceneLoaded.Invoke(scene.Name);
            }
            
            // Wait until all AsyncOperations in the group are done
            while (!operationGroup.IsDone)
            {
                progress?.Report(operationGroup.Progress);
                await Task.Delay(100);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(0.1f));

            Debug.Log("Cleaning Scenes");
            
            var actuallyLoadedScenes = new HashSet<string>();
            var toRemove = new List<string>();
            
            for (var i = 0; i < SceneManager.loadedSceneCount; i++)
            {
                if (!actuallyLoadedScenes.Add(SceneManager.GetSceneAt(i).name))
                {
                    toRemove.Add(SceneManager.GetSceneAt(i).name);
                }
            }

            foreach (var scene in toRemove)
            {
                await SceneManager.UnloadSceneAsync(scene);
            }
            
           /*var activeScene = SceneManager.GetSceneByName(_activeSceneGroup.FindSceneNameByType(SceneType.ActiveScene));
            
            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }*/
            
            OnSceneGroupLoaded.Invoke();
            
            _loading = false;
            Debug.Log("Ending Loading");
        }

        public async Task UnloadScenes()
        {
            var scenes = new List<string>();
            var activeScene = SceneManager.GetActiveScene().name;
            
            var sceneCount = SceneManager.sceneCount;
            
            for (var i = 0; i < sceneCount; i++)
            {
                var sceneAt = SceneManager.GetSceneAt(i);
                if (!sceneAt.isLoaded)
                    continue;
                
                var sceneName = sceneAt.name;
                
                if (sceneName.Equals(activeScene) || sceneName == "Bootstrapper")
                    continue;
                
                scenes.Add(sceneName);
            }
            
            // Create an AsyncOperationGroup
            var operationGroup = new AsyncOperationGroup(scenes.Count);

            foreach (var scene in scenes)
            {
                var operation = SceneManager.UnloadSceneAsync(scene);
                
                if (operation == null)
                    continue;
                
                operationGroup.operations.Add(operation);
                
                OnSceneUnloaded.Invoke(scene);
            }
            
            // Wait until all AsyncOperations in the group are done

            while (!operationGroup.IsDone)
            {
                await Task.Delay(100);
            }
        }

        private void ReleaseUnmanagedResources()
        {
            _activeSceneGroup = null;
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

        #region fields

        public event Action<string> OnSceneLoaded = delegate { };
        
        public event Action<string> OnSceneUnloaded = delegate { };
        
        public event Action OnSceneGroupLoaded = delegate { };

        private SceneGroup _activeSceneGroup;

        private bool _loading;

        #endregion
    }

    public readonly struct AsyncOperationGroup
    {
        #region properties

        public float Progress => operations.Count == 0 ? 0 : operations.Average(o => o.progress);
        
        public bool IsDone => operations.All(o => o.isDone);
        
        #endregion
        
        #region methodes
        
        public AsyncOperationGroup(int initialCapacity)
        {
            operations = new List<AsyncOperation>(initialCapacity);
        }
        
        #endregion

        #region fields

        public readonly List<AsyncOperation> operations;

        #endregion
    }
}