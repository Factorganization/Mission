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
                var meshes = _characterPreview.GetMeshes(CustomizationPlayer.BodyPartType.Head);
                _customizationPooler.PopulateMeshes(meshes, null, (btn) =>
                {
                    _characterPreview.SetBodyPartMesh(CustomizationPlayer.BodyPartType.Head, btn.ItemIndex);
                });
            });
            
            _tailButton.onClick.AddListener(() =>
            {
                var meshes = _characterPreview.GetMeshes(CustomizationPlayer.BodyPartType.Tail);
                _customizationPooler.PopulateMeshes(meshes, null, (btn) =>
                {
                    _characterPreview.SetBodyPartMesh(CustomizationPlayer.BodyPartType.Tail, btn.ItemIndex);
                });
            });
            
            _eyesButton.onClick.AddListener(() =>
            {
                var meshes = _characterPreview.GetMeshes(CustomizationPlayer.BodyPartType.Eyes);
                _customizationPooler.PopulateMeshes(meshes, null, (btn) =>
                {
                    _characterPreview.SetBodyPartMesh(CustomizationPlayer.BodyPartType.Eyes, btn.ItemIndex);
                });
            });
            
            _bodyButton.onClick.AddListener(() =>
            {
                var meshes = _characterPreview.GetMeshes(CustomizationPlayer.BodyPartType.Body);
                _customizationPooler.PopulateMeshes(meshes, null, (btn) =>
                {
                    _characterPreview.SetBodyPartMesh(CustomizationPlayer.BodyPartType.Body, btn.ItemIndex);
                });
            });
            
            _hornsButton.onClick.AddListener(() =>
            {
                var meshes = _characterPreview.GetMeshes(CustomizationPlayer.BodyPartType.Horns);
                _customizationPooler.PopulateMeshes(meshes, null, (btn) =>
                {
                    _characterPreview.SetBodyPartMesh(CustomizationPlayer.BodyPartType.Horns, btn.ItemIndex);
                });
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