using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.Customization
{
    public class CustomizationView : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
            
            CustomizationEvent(CustomizationPlayer.BodyPartType.Hair);
        }

        private void Initialize()
        {
             if (_customizationColors == null)
                 return;
            
             _hornsButton.onClick.AddListener( () =>
             {
                 if (_customizationColors != null)
                     _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Horns);
                 CustomizationEvent(CustomizationPlayer.BodyPartType.Horns);
             });
            
             _hairButton.onClick.AddListener(() =>
             {
                 if (_customizationColors != null)
                     _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Hair);
                 CustomizationEvent(CustomizationPlayer.BodyPartType.Hair);
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
                 CustomizationEvent(CustomizationPlayer.BodyPartType.Body);
             });
            
             _tailButton.onClick.AddListener(() =>
             {
                 if (_customizationColors != null)
                     _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Tail);
                 CustomizationEvent(CustomizationPlayer.BodyPartType.Tail);
             });
        }

        private void CustomizationEvent(CustomizationPlayer.BodyPartType bodyPart)
        {
            _customizationColors.SetCurrentBodyPart(bodyPart);
            var meshes= _characterPreview.GetItem(bodyPart);
            _customizationPooler.Populate(meshes, (btn) =>
            {
                if (!btn.CustomizeItem.Locked)
                {
                    _characterPreview.SetBodyPartMesh(bodyPart, btn.ItemIndex);
                    if (_customizationColors != null)
                    {
                        if (bodyPart == CustomizationPlayer.BodyPartType.Hair ||
                            bodyPart == CustomizationPlayer.BodyPartType.Body)
                        {
                            _customizationColors.SetCurrentItemIndex(btn.ItemIndex);
                            _customizationColors.ApplyColor(0);
                        }
                    }
                }
                else
                {
                    MainMenuUI.Instance.PurchaseContainer.Show();
                    MainMenuUI.Instance.PurchaseContainer.ConfirmButton.onClick.AddListener(btn.UnlockItem);
                    MainMenuUI.Instance.PurchaseContainer.ConfirmButton.onClick.AddListener(Initialize);
                }
            });
            
            _scrollbar.value = 1;
        }
        
        #endregion

        #region Fields

        [SerializeField] private Button _hairButton, _tailButton, _eyesButton, _bodyButton, _hornsButton;
        [SerializeField] private GameObject _contentArea; 
        
        [SerializeField] CustomizationPooler _customizationPooler;
        [SerializeField] private CustomizationPlayer _characterPreview;
        [SerializeField] private CustomizationColors _customizationColors;
        
        [SerializeField] private Scrollbar _scrollbar;
        
        #endregion

    }
}