using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Runtime.Services.SceneService.SceneSystems
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
            _activeSceneGroup = group;
            var loadedScenes = new List<string>();

            await UnloadScenes();

            var sceneCount = SceneManager.sceneCount;

            for (var i = 0; i < sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }
            
            var totalScenesToLoad = _activeSceneGroup.scenes.Count;
            
            var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

            for (var i = 0; i < totalScenesToLoad; i++)
            {
                var sceneData = group.scenes[i];
                
                if (reloadDupScenes == false && loadedScenes.Contains(sceneData.Name))
                    continue;
                
                var operation = SceneManager.LoadSceneAsync(sceneData.reference.Path, LoadSceneMode.Additive);

                await Task.Delay(TimeSpan.FromSeconds(2.5f));
                
                operationGroup.operations.Add(operation);
                
                OnSceneLoaded.Invoke(sceneData.Name);
            }
            
            // Wait until all AsyncOperations in the group are done
            while (!operationGroup.IsDone)
            {
                progress?.Report(operationGroup.Progress);
                await Task.Delay(100);
            }
            
            var activeScene = SceneManager.GetSceneByName(_activeSceneGroup.FindSceneNameByType(SceneType.ActiveScene));
            
            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }
            
            OnSceneGroupLoaded.Invoke();
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