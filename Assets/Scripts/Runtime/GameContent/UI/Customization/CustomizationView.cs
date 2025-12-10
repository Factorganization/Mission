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
            _hairButton.onClick.AddListener(() =>
            {
                var prefabs = _characterPreview.GetPrefabs(CustomizationPlayer.BodyPartType.Hair);
                _customizationPooler.PopulatePrefab(prefabs, (btn) =>
                {
                    _characterPreview.SetBodyPartPrefab(CustomizationPlayer.BodyPartType.Hair, btn.ItemIndex);
                });
            });
            
            _tailButton.onClick.AddListener(() =>
            {
                var prefabs = _characterPreview.GetPrefabs(CustomizationPlayer.BodyPartType.Tail);
                _customizationPooler.PopulatePrefab(prefabs, (btn) =>
                {
                    _characterPreview.SetBodyPartPrefab(CustomizationPlayer.BodyPartType.Tail, btn.ItemIndex);
                });
            });
            
            _eyesButton.onClick.AddListener(() =>
            {
                var prefabs = _characterPreview.GetPrefabs(CustomizationPlayer.BodyPartType.Eyes);
                _customizationPooler.PopulatePrefab(prefabs, (btn) =>
                {
                    _characterPreview.SetBodyPartPrefab(CustomizationPlayer.BodyPartType.Eyes, btn.ItemIndex);
                });
            });
            
            _bodyButton.onClick.AddListener(() =>
            {
                var prefabs = _characterPreview.GetPrefabs(CustomizationPlayer.BodyPartType.Body);
                _customizationPooler.PopulatePrefab(prefabs, (btn) =>
                {
                    _characterPreview.SetBodyPartPrefab(CustomizationPlayer.BodyPartType.Body, btn.ItemIndex);
                });
            });
            
            _hornsButton.onClick.AddListener(() =>
            {
                var prefabs = _characterPreview.GetPrefabs(CustomizationPlayer.BodyPartType.Horns);
                _customizationPooler.PopulatePrefab(prefabs, (btn) =>
                {
                    _characterPreview.SetBodyPartPrefab(CustomizationPlayer.BodyPartType.Horns, btn.ItemIndex);
                });
            });
        }
        
        #endregion

        #region Fields

        [SerializeField] private Button _hairButton, _tailButton, _eyesButton, _bodyButton, _hornsButton;
        [SerializeField] private GameObject _contentArea; 
        
        [SerializeField] CustomizationPooler _customizationPooler;
        [SerializeField] private CustomizationPlayer _characterPreview;
        
        #endregion

    }
}