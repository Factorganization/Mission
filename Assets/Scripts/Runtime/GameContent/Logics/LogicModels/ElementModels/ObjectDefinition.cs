using Runtime.GameContent.Logics.LogicModels.MissionModels;
using TMPro;

namespace Runtime.GameContent.Logics.LogicModels.ElementModels
{
    [Serializable]
    public struct ObjectDefinition
    {
        public ObjectType @object; //Tu me laisses l'appeler object >:(
        
        public BoxCollider col;
        
        public LayerMask blockLayer;

        public ElementDuration durations;
    
        public float elementApplicationDistance;
    
        public float destructionApplicationDistance;
        
        public VFXReferences vfxReferences;
        
        public DebugInfo debugInfo;
    }

    [Serializable]
    public struct DebugInfo
    {
        public TMP_Text text;

        public bool debug;
    }
}