using Runtime.Service;
using Runtime.Services.Game.GameSystems;
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
            text.text = "";
            
            if (loadedInput.action.WasPressedThisFrame())
                loadedDebug = !loadedDebug;

            if (perfInput.action.WasPressedThisFrame())
                graphy.enabled = !graphy.enabled;
            
            if (cheatInput.action.WasPressedThisFrame())
                cheatCanvas.enabled = !cheatCanvas.enabled;
            
            if (loadedDebug)
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
        
        [SerializeField] private bool loadedDebug;

        #endregion
    }
}