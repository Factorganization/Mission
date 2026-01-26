using Runtime.Services.Data;
using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.Customization
{
    public class CustomizationView : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
            
            var b = ServiceLocator.Instance.Get<DataService>();
            
            if (b.PurchasedItems == null)
                return;
            
            b.PurchasedItems = _characterPreview.CustomItems;
            
            CustomizationEvent(CustomizationPlayer.BodyPartType.Hair);
        }

        private void Initialize()
        {
             if (_customizationColors == null)
                 return;
            
             _hornsButton.onClick.AddListener( () =>
             {
                 CustomizationEvent(CustomizationPlayer.BodyPartType.Horns);
             });
            
             _hairButton.onClick.AddListener(() =>
             {
                 CustomizationEvent(CustomizationPlayer.BodyPartType.Hair);
             });
             
             _eyesButton.onClick.AddListener(() =>
             {
                 if (_customizationColors != null)
                     _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Eyes);

                 var mats= _characterPreview.GetItem(CustomizationPlayer.BodyPartType.Eyes);
                 _customizationPooler.PopulateMaterials(mats, (btn) =>
                 {
                     var mat = btn.SelectedMaterial;
                     
                     if (!btn.CustomizeItem.Locked)
                     {
                         _characterPreview.ApplyMaterialToBodyPart(CustomizationPlayer.BodyPartType.Eyes, mat);
                     }
                     else
                     {
                         MainMenuUI.Instance.PurchaseContainer.Show();
                         MainMenuUI.Instance.PurchaseContainer.ConfirmButton.onClick.AddListener(btn.UnlockItem);
                         MainMenuUI.Instance.PurchaseContainer.ConfirmButton.onClick.AddListener(Initialize);
                     }
                 });
             });

             _bodyButton.onClick.AddListener(() =>
             {
                 CustomizationEvent(CustomizationPlayer.BodyPartType.Body);
             });
            
             _tailButton.onClick.AddListener(() =>
             {
                 CustomizationEvent(CustomizationPlayer.BodyPartType.Tail);
             });
        }

        private void CustomizationEvent(CustomizationPlayer.BodyPartType bodyPart)
        {
            if (_customizationColors != null)
                _customizationColors.SetCurrentBodyPart(bodyPart);
            
            _customizationColors.SetCurrentBodyPart(bodyPart);
            var meshes= _characterPreview.GetItem(bodyPart);
            var a = ServiceLocator.Instance.Get<DataService>();
            _customizationPooler.Populate(meshes, (btn) =>
            {
                if (!a.PurchasedItems[a.PurchasedItems.FindIndex(x => x.ItemName == btn.CustomizeItem.ItemName)].Locked)
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