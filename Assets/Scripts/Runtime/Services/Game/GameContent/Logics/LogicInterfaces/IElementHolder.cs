using Runtime.Services.Game.GameContent.Logics.LogicModels;
using Runtime.Services.Game.GameContent.Logics.LogicModels.ElementModels;
using Runtime.Services.Game.GameContent.Logics.LogicModels.MissionModels;

namespace Runtime.Services.Game.GameContent.Logics.LogicInterfaces
{
    public interface IElementHolder
    {
        /// <summary>
        /// Transform of the Element Holder
        /// </summary>
        public Transform Transform { get; }
        
        /// <summary>
        /// Collider of the element holder
        /// </summary>
        public BoxCollider Collider { get; }
        
        /// <summary>
        /// Define element that IS affecting this object
        /// </summary>
        public ElementFlag Flag1 { get; set; }
        
        /// <summary>
        /// Defines element that CAN affect this object
        /// </summary>
        public ElementFlag Flag2 { get; }
        
        /// <summary>
        /// Hidden supplementary Flag that can store temporary IS data
        /// </summary>
        public ElementFlag Flag3 { get; set; }
        
        /// <summary>
        /// The Current Room the Element holder is in
        /// </summary>
        public RoomType RoomType { get; set; }
        
        /// <summary>
        /// Defines if an object is active and can transmit any element
        /// </summary>
        public bool Active { get; set; }
        
        /// <summary>
        /// Max distance to apply element to another holder
        /// </summary>
        public float ElementApplicationDistance { get; }
        
        /// <summary>
        /// array of element length to indicate if object has done its mission for a specific element
        /// </summary>
        public bool[] MissionDone { get; }

        /// <summary>
        /// Element application durations
        /// </summary>
        public ElementDuration Durations { get; }
        
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