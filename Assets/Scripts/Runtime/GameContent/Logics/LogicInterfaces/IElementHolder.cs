using Runtime.GameContent.Logics.LogicModels;

namespace Runtime.GameContent.Logics.LogicInterfaces
{
    public interface IElementHolder
    {
        /// <summary>
        /// Define element that can affect this object or that type of source
        /// </summary>
        ElementFlag Flag1 { get; }
        
        /// <summary>
        /// Defines element held by a transmitter or the element that affect a source 
        /// </summary>
        ElementFlag Flag2 { get; set; }
        
        /// <summary>
        /// Defines if an object is active and can transmit any element
        /// </summary>
        bool Active { get; set; }
        
        /// <summary>
        /// Interact with another object that can hold an element, can call the same function from the other object
        /// </summary>
        /// <param name="elementFlag">element flags Flag1 from the other object</param>
        void CheckOtherElement(ElementFlag elementFlag);
    }
}