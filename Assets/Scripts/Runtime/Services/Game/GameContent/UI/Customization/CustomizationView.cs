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
            _hairButton.onClick.AddListener(() =>
            {
                if (_customizationColors != null)
                    _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Hair);
                var prefabs = _characterPreview.GetPrefabs(CustomizationPlayer.BodyPartType.Hair);
                _customizationPooler.PopulatePrefab(prefabs, (btn) =>
                {
                    _characterPreview.SetBodyPartPrefab(CustomizationPlayer.BodyPartType.Hair, btn.ItemIndex);
                });
            });
            
            _tailButton.onClick.AddListener(() =>
            {
                if (_customizationColors != null)
                    _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Tail);
                var prefabs = _characterPreview.GetPrefabs(CustomizationPlayer.BodyPartType.Tail);
                _customizationPooler.PopulatePrefab(prefabs, (btn) =>
                {
                    _characterPreview.SetBodyPartPrefab(CustomizationPlayer.BodyPartType.Tail, btn.ItemIndex);
                });
            });
            
            _eyesButton.onClick.AddListener(() =>
            {
                if (_customizationColors != null)
                    _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Eyes);
                var prefabs = _characterPreview.GetPrefabs(CustomizationPlayer.BodyPartType.Eyes);
                _customizationPooler.PopulatePrefab(prefabs, (btn) =>
                {
                    _characterPreview.SetBodyPartPrefab(CustomizationPlayer.BodyPartType.Eyes, btn.ItemIndex);
                });
            });
            
            _bodyButton.onClick.AddListener(() =>
            {
                if (_customizationColors != null)
                    _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Body);
                var prefabs = _characterPreview.GetPrefabs(CustomizationPlayer.BodyPartType.Body);
                _customizationPooler.PopulatePrefab(prefabs, (btn) =>
                {
                    _characterPreview.SetBodyPartPrefab(CustomizationPlayer.BodyPartType.Body, btn.ItemIndex);
                });
            });
            
            _hornsButton.onClick.AddListener(() =>
            {
                if (_customizationColors != null)
                    _customizationColors.SetCurrentBodyPart(CustomizationPlayer.BodyPartType.Horns);
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
        [SerializeField] private CustomizationColors _customizationColors;
        
        #endregion

    }
}