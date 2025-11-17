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
            
            if (_image != null && _customizeItem != null)
                _image.sprite = _customizeItem.ItemIcon;

            if (_lockImage != null)
                _lockImage.gameObject.SetActive(_locked);
        }
        
        public void OnSelected()
        {
            if (_locked) return;
            OnChangeSkin.Invoke(this);
        }
        
        public void SetData(CustomizeItem item, bool locked)
        {
            _customizeItem = item;
            _locked = locked;

            if (item.ItemIcon != null)
                _image.sprite = item != null ? item.ItemIcon : null;

            if (_lockImage != null)
                _lockImage.gameObject.SetActive(_locked);
        }
        
        public void ResetForPool()
        {
            // remove listeners and clear data
            OnChangeSkin.RemoveAllListeners();
            _customizeItem = null;
            _locked = false;
            if (_image != null) _image.sprite = null;
            if (_lockImage != null) _lockImage.gameObject.SetActive(false);
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