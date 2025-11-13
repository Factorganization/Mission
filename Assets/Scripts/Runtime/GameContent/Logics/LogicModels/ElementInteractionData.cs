using Runtime.GameContent.Logics.LogicInterfaces;

namespace Runtime.GameContent.Logics.LogicModels
{
    public struct ElementInteractionData
    {
        public ElementInteractionData(IElementHolder holder1, IElementHolder holder2)
        {
            Holder1 = holder1;
            Holder2 = holder2;
        }

        public IElementHolder Holder1;
        
        public IElementHolder Holder2;
    }
}