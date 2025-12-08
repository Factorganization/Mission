using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.GameContent.UI.Customization
{
    public class CustomizationColors : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            
        }

        public void ChangeMeshColor()
        {
            
        }
        
        #endregion
        
        #region Fields
        [SerializeField] private Button _colorButton1, _colorButton2, _colorButton3, _colorButton4;
        
        [SerializeField] private List<Material> _colorsMats;
        #endregion
    }

}