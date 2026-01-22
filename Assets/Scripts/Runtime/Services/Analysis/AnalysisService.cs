using Runtime.Service;
using Runtime.Services.Cursor;
using Runtime.Services.Game.GameSystems;
using Runtime.Services.Scene;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Runtime.Services.Analysis
{
    public class AnalysisService : AService
    {
        #region methodes

        public override bool Init()
        {
            return true;
        }

        public override void Tick()
        {
            if (!allowDebug)
                return;
            
            text.text = "";
            if (cheatCanvas.gameObject.activeSelf)
                ServiceLocator.Instance.Get<CursorService>().SetActive(true);
            
            if (loadedInput.action.WasPressedThisFrame())
                _loadedDebug = !_loadedDebug;

            if (perfInput.action.WasPressedThisFrame())
                graphy.gameObject.SetActive(!graphy.gameObject.activeSelf);

            if (cheatInput.action.WasPressedThisFrame())
            {
                cheatCanvas.gameObject.SetActive(!cheatCanvas.gameObject.activeSelf);
                var c = ServiceLocator.Instance.Get<CursorService>();
                if (cheatCanvas.gameObject.activeSelf)
                {
                    _mousePreviousState = c.MouseVisible;
                    c.SetActive(true);
                }
                else
                    c.SetActive(ServiceLocator.Instance.Get<SceneService>().CurrentActiveSceneGroup == 0); // et au pire blk fallait pas cheat voila
            }
            
            if (_loadedDebug)
            {
                text.text += "Managers : \n";

                if (GameManager.Instance)
                    text.text += "GameManager\n";
                if (LevelGenerator.Generator)
                    text.text += "LevelGenerator\n";
                if (ElementManager.Element)
                    text.text += "ElementManager\n";
                if (MissionManager.Manager)
                    text.text += "MissionManager\n";
                
                text.text += "Scenes : \n";
                                                                    
                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    text.text += $"{SceneManager.GetSceneAt(i).name}\n";
                }
            }
            else
                text.text = "";
        }

        #endregion

        #region fields

        [SerializeField] private TMP_Text text;

        [SerializeField] private Canvas graphy;

        [SerializeField] private Canvas cheatCanvas;
        
        [SerializeField] private InputActionReference loadedInput;
        
        [SerializeField] private InputActionReference perfInput;
        
        [SerializeField] private InputActionReference cheatInput;
        
        [SerializeField] private bool allowDebug;
        
        private bool _loadedDebug;

        private bool _mousePreviousState;

        #endregion
    }
}