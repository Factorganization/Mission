using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.GameContent.UI.Customization
{
    public class CustomizationView : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _headButton.onClick.AddListener(() =>
            {
                _customizationPooler.Populate(_headItems);
            });
            
            _tailButton.onClick.AddListener(() =>
            {
                _customizationPooler.Populate(_tailItems);
            });
            
            _eyesButton.onClick.AddListener(() =>
            {
                _customizationPooler.Populate(_eyesItems);
            });
            
            _bodyButton.onClick.AddListener(() =>
            {
                _customizationPooler.Populate(_bodyItems);
            });
            
            _hornsButton.onClick.AddListener(() =>
            {
                _customizationPooler.Populate(_hornsItems);
            });
        }
        
        #endregion

        #region Fields

        [SerializeField] private Button _headButton, _tailButton, _eyesButton, _bodyButton, _hornsButton;
        [SerializeField] private GameObject _contentArea; 
        
        [SerializeField] List<CustomizeItem> _headItems, _tailItems, _eyesItems, _bodyItems, _hornsItems;
        [SerializeField] CustomizationPooler _customizationPooler;
        [SerializeField] private CustomizationPlayer _characterPreview;
        
        #endregion

    }
}