using Runtime.GameContent.Logics.LogicInterfaces;

namespace Runtime.GameContent.Logics.LogicModels
{
    public struct ElementInteractionData
    {
		/// <summary>
		/// Init element holder pair
		/// </summary>
		/// <param name="holder1">IS holding element</param>
		/// <param name="holder2">CAN hold element</param>
        public ElementInteractionData(IElementHolder holder1, IElementHolder holder2)
        {
            this.holder1 = holder1;
            this.holder2 = holder2;
        }

		/// <summary>
		/// IS holding element
		/// </summary>
        public IElementHolder holder1;

		/// <summary>
		/// CAN hold element
		/// </summary>
        public IElementHolder holder2;
    }
}