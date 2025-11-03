using Runtime.GameContent.Logics.LogicModels;

namespace Runtime.GameContent.Logics.Interfaces
{
    public interface ITransmission
    {
        ElementFlag Element { get; }
        
        void CheckOtherElement(ElementFlag elementFlag);
        
        void SetSelfElement(ElementFlag elementFlag);
    }
}