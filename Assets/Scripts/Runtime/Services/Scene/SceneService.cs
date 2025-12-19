using System.Threading.Tasks;
using UnityEngine.UI;
using Runtime.Services.SceneService.SceneSystems;
using Runtime.Service;

namespace Runtime.Services.SceneService
{
    public class SceneService : AService
    {
        #region properties

        public bool Loaded => !_isLoading;

        #endregion
        
        #region methodes
        
        #region unity events
        
        public override bool Init()
        {
            manager.OnSceneLoaded += sceneName => Debug.Log("Loaded : " + sceneName);
            manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded : " + sceneName);
            manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");
            return true;
        }

        public override async void Begin()
        {
            await LoadSceneGroup(0);
        }

        public override void Tick()
        {
            if (!_isLoading)
                return;
            
            var currentFillAmount = loadingBar.fillAmount;
            var progressDifference = Mathf.Abs(currentFillAmount - _targetProgress);
            var dynamicSpeed = progressDifference * fillSpeed;
            
            loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, _targetProgress, Time.deltaTime * dynamicSpeed);
        }
        
        #endregion

        #region load methodes
        
        public async Task LoadSceneGroup(int index)
        {
            loadingBar.fillAmount = 0f;
            _targetProgress = 1f;

            if (index < 0 || index >= sceneGroups.Length)
            {
                Debug.LogError("Invalid scene group index : " + index);
                return;
            }
            
            var progress = new LoadingProgress();
            progress.Progressed += target => _targetProgress = Mathf.Max(target, _targetProgress);
            
            EnableLoadingCanvas();
            await manager.LoadScenes(sceneGroups[index], progress);
            EnableLoadingCanvas(false);
        }

        public async Task LoadSceneGroup(string groupName)
        {
            for (var i = 0; i < sceneGroups.Length; i++)
            {
                var s = sceneGroups[i];
                if (s.groupName != groupName)
                    continue;
                
                await LoadSceneGroup(i);
                return;
            }
            
            Debug.LogError("Invalid scene group name : " + groupName);
        }
        
        private void EnableLoadingCanvas(bool enable = true)
        {
            _isLoading = enable;
            loadingCanvas.enabled = enable;
            loadingCamera.enabled = enable;
        }
        
        #endregion
        
        #endregion

        #region fields

        public readonly SceneGroupManager manager = new();
        
        [SerializeField] private Image loadingBar;
        
        [SerializeField] private float fillSpeed = 0.5f;
        
        [SerializeField] private Canvas loadingCanvas;
        
        [SerializeField] private Camera loadingCamera;
        
        [SerializeField] private SceneGroup[] sceneGroups;

        private float _targetProgress;

        private bool _isLoading;
        
        #endregion
    }
    
    public class LoadingProgress : IProgress<float>
    {
        #region methodes
        
        public void Report(float value)
        {
            Progressed?.Invoke(value / Ratio);
        }
        
        #endregion
        
        #region fields
        
        public event Action<float> Progressed;

        private const float Ratio = 1f;
        
        #endregion
    }
}