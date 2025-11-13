using UnityEngine;
using Runtime.GameContent.Logics.LogicModels;

namespace Runtime.GameContent.Logics.LogicInterfaces
{
    public interface IElementHolder
    {
        /// <summary>
        /// Transform of the Element Holder
        /// </summary>
        public Transform Transform { get; }
        
        /// <summary>
        /// Define element that can affect this object or that type of source
        /// </summary>
        public ElementFlag Flag1 { get; }
        
        /// <summary>
        /// Defines element held by a transmitter or the element that affect a source 
        /// </summary>
        public ElementFlag Flag2 { get; }
        
        /// <summary>
        /// Defines if an object is active and can transmit any element
        /// </summary>
        public bool Active { get; }
        
        /// <summary>
        /// Interact with another object that can hold an element, can call the same function from the other object
        /// </summary>
        /// <param name="elementFlag">element flags Flag1 from the other object</param>
        public void CheckOtherElement(ElementFlag elementFlag);
    }
}