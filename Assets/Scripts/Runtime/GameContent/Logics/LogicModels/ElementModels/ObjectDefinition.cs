using Runtime.GameContent.Logics.LogicModels.MissionModels;

namespace Runtime.GameContent.Logics.LogicModels.ElementModels
{
    [Serializable]
    public struct ObjectDefinition
    {
        public ObjectType @object;
        
        public ElementFlag element;
    }
}