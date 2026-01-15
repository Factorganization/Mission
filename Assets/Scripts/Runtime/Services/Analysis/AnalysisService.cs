using Runtime.Service;
using Runtime.Services.Game.GameSystems;
using TMPro;
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
            
            if (managerDebug)
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
            }
            else
                text.text = "";

            if (sceneDebug)
            {
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
        
        [SerializeField] private bool managerDebug;

        [SerializeField] private bool sceneDebug;

        #endregion
    }
}