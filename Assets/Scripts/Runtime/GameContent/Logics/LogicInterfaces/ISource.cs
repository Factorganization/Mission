using Runtime.GameContent.Logics.LogicModels;

namespace Runtime.GameContent.Logics.LogicInterfaces
{
    public interface ISource
    {
        ElementFlag SourceElement { get; }
        
        ElementFlag ReceptorElement { get; }
        
        bool Active { get; set; } 
            
        void ReactToElement(ElementFlag elementFlag);
    }
}