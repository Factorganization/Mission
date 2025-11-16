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
        /// Define element that IS affecting this object
        /// </summary>
        public ElementFlag Flag1 { get; set; }
        
        /// <summary>
        /// Defines element that CAN affect this object
        /// </summary>
        public ElementFlag Flag2 { get; }
        
        /// <summary>
        /// Defines if an object is active and can transmit any element
        /// </summary>
        public bool Active { get; set; }
        
        /// <summary>
        /// Graph feedbacks of the elements 
        /// </summary>
        public VFXReferences VFX { get; }
        
        /// <summary>
        /// Interact with another object that can hold an element, can call the same function from the other object
        /// </summary>
        /// <param name="holder">other object holding elements</param>
        public void CheckOtherElement(IElementHolder holder);
    }
}