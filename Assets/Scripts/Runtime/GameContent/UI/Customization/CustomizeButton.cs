using System;
using Runtime.GameContent.UI.Customization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class ChangeSkin : UnityEvent<CustomizeButton> { }

namespace Runtime.GameContent.UI.Customization
{
    public class CustomizeButton : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _image.sprite = _customizeItem.ItemIcon;
            
            if (_locked)
            {
                _lockImage.gameObject.SetActive(true);
            }
            else
            {
                _lockImage.gameObject.SetActive(false);
            }
        }
        
        public void OnSelected()
        {
            if (_locked) return;
            OnChangeSkin.Invoke(this);
        }

        #endregion
        
        #region Fields

        [SerializeField] private Image _image;
        [SerializeField] private Image _lockImage;
        
        [SerializeField] private CustomizeItem _customizeItem;
        [SerializeField] private bool _locked;
        
        public ChangeSkin OnChangeSkin = new ChangeSkin();

        #endregion
    }
}