using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Logics.LogicInterfaces;
using Runtime.GameContent.Logics.LogicModels;
using UnityEngine;

namespace Runtime.GameContent.Actors.ActorViews
{
    public class NoInteractObjectView : ActorView, INoInteract
    {
        #region proprties
        
        public Transform Transform => transform;
        
        public ElementFlag Flag1 { get; set; }

        public ElementFlag Flag2 => flag;
        
        public ElementFlag Flag3 { get; set; }
        
        public bool Active
        {
            get => true;
            set { }
        }

        public VFXReferences VFX => vfxReferences;
        
        public void CheckOtherElement(IElementHolder holder)
        {
            throw new System.NotImplementedException();
        }
        
        #endregion

        #region fields

        [SerializeField] private ElementFlag flag;
        
        [SerializeField] private VFXReferences vfxReferences;

        #endregion
    }
}