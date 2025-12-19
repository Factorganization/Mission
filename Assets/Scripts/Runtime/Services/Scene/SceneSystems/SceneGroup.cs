using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;

namespace Runtime.Services.Scene.SceneSystems
{
    [Serializable]
    public class SceneGroup
    {
        #region methodes

        public string FindSceneNameByType(SceneType sceneType)
        {
            return scenes.FirstOrDefault(scene => scene.sceneType == sceneType)?.reference.Name;
        }

        #endregion
        
        #region fields
        
        public string groupName;
        
        public List<ScenaData> scenes;
        
        #endregion
    }
    
    [Serializable]
    public class ScenaData
    {
        #region fields
        
        public string Name => reference.Name;
        
        public SceneReference reference;
        
        public SceneType sceneType;
        
        #endregion
    }
    
    public enum SceneType
    {
        ActiveScene,
        MainMenu,
        UserInterface,
        HUD,
        Cinematic,
        Environment
    }
}