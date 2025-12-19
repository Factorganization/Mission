using Runtime.Services.GameService.GameContent.Logics.LogicModels.MissionModels;
using TMPro;

namespace Runtime.Services.GameService.GameContent.Logics.LogicModels.ElementModels;

[Serializable]
public struct ObjectDefinition
{
    /// <summary>
    /// Object category
    /// </summary>
    public ObjectType @object; //Tu me laisses l'appeler object >:(
        
    /// <summary>
    /// Collider used to detect holder
    /// </summary>
    public BoxCollider col;
        
    /// <summary>
    /// LayerMask used to detect holder
    /// </summary>
    public LayerMask blockLayer;

    /// <summary>
    /// Durations of the element application of the object
    /// </summary>
    public ElementDuration durations;
    
    /// <summary>
    /// Distance for normal application
    /// </summary>
    public float elementApplicationDistance;
    
    /// <summary>
    /// Distance for explosive application
    /// </summary>
    public float destructionApplicationDistance;
        
    /// <summary>
    /// Graph feedbacks 
    /// </summary>
    public VFXReferences vfxReferences;
        
    /// <summary>
    /// Debug states and support
    /// </summary>
    public DebugInfo debugInfo;
}

[Serializable]
public struct DebugInfo
{
    /// <summary>
    /// Debug text to show element states
    /// </summary>
    public TMP_Text text;

    /// <summary>
    /// debug toggle
    /// </summary>
    public bool debug;
}