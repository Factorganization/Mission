using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.Customization
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
             if (_customizationColors == null)
                 return;
            
             _hornsButton.onClick.AddListener( () =>
             {
                 if (_customizationColors != null)
                     _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Horns);
                 var meshes = _characterPreview.GetItem(CustomizationPlayer.BodyPartType.Horns);
                 _customizationPooler.Populate(meshes, (btn) =>
                 {
                     _characterPreview.SetBodyPartMesh(CustomizationPlayer.BodyPartType.Horns, btn.ItemIndex);
                 });
             });
            
             _hairButton.onClick.AddListener(() =>
             {
                 if (_customizationColors != null)
                     _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Hair);
                 var meshes= _characterPreview.GetItem(CustomizationPlayer.BodyPartType.Hair);
                 _customizationPooler.Populate(meshes, (btn) =>
                 {
                     _characterPreview.SetBodyPartMesh(CustomizationPlayer.BodyPartType.Hair, btn.ItemIndex);
                 });
             });
             
             _eyesButton.onClick.AddListener(() =>
             {
                 if (_customizationColors != null)
                     _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Eyes);

                 var mats = _customizationColors.GetMaterialsForBodyPart(CustomizationPlayer.BodyPartType.Eyes);
                 _customizationPooler.PopulateMaterials(mats, (btn) =>
                 {
                     var mat = btn.SelectedMaterial;
                     if (mat != null)
                         _characterPreview.ApplyMaterialToBodyPart(CustomizationPlayer.BodyPartType.Eyes, mat);
                 });
             });

             _bodyButton.onClick.AddListener(() =>
             {
                 if (_customizationColors != null)
                     _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Body);
                 var meshes = _characterPreview.GetItem(CustomizationPlayer.BodyPartType.Body);
                 _customizationPooler.Populate(meshes, (btn) =>
                 {
                     _characterPreview.SetBodyPartMesh(CustomizationPlayer.BodyPartType.Body, btn.ItemIndex);
                 });
             });
            
             _tailButton.onClick.AddListener(() =>
             {
                 if (_customizationColors != null)
                     _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Tail);
                 var meshes = _characterPreview.GetItem(CustomizationPlayer.BodyPartType.Tail);
                 _customizationPooler.Populate(meshes, (btn) =>
                 {
                     _characterPreview.SetBodyPartMesh(CustomizationPlayer.BodyPartType.Tail, btn.ItemIndex);
                 });
             });
        }
        
        #endregion

        #region Fields

        [SerializeField] private Button _hairButton, _tailButton, _eyesButton, _bodyButton, _hornsButton;
        [SerializeField] private GameObject _contentArea; 
        
        [SerializeField] CustomizationPooler _customizationPooler;
        [SerializeField] private CustomizationPlayer _characterPreview;
        [SerializeField] private CustomizationColors _customizationColors;
        
        #endregion

    }
}