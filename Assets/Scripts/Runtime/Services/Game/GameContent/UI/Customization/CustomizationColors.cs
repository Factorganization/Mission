using System.Collections.Generic;
using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.Customization
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
            if (_colorButton1 != null) _colorButton1.onClick.AddListener(() => ApplyColor(0));
            if (_colorButton2 != null) _colorButton2.onClick.AddListener(() => ApplyColor(1));
            if (_colorButton3 != null) _colorButton3.onClick.AddListener(() => ApplyColor(2));
            if (_colorButton4 != null) _colorButton4.onClick.AddListener(() => ApplyColor(3));
        }

        private void ApplyColor(int colorIndex)
        {
            if (_characterPreview == null) return;

            var mats = GetMatsForCurrentBodyPart();
            if (mats == null || mats.Count == 0) return;

            int clamped = Mathf.Clamp(colorIndex, 0, mats.Count - 1);
            var mat = mats[clamped];
            if (mat == null) return;

            _characterPreview.ApplyMaterialToBodyPart(_currentBodyPart, mat);
        }
        
        public void SetCurrentBodyPart(CustomizationPlayer.BodyPartType bodyPartType)
        {
            _currentBodyPart = bodyPartType;
        }
        
        public List<Material> GetMaterialsForBodyPart(CustomizationPlayer.BodyPartType bodyPartType)
        {
            switch (bodyPartType)
            {
                case CustomizationPlayer.BodyPartType.Hair:
                    return _hairMats ?? _colorsMats;
                case CustomizationPlayer.BodyPartType.Tail:
                    return _tailMats ?? _colorsMats;
                case CustomizationPlayer.BodyPartType.Eyes:
                    return _eyesMats ?? _colorsMats;
                case CustomizationPlayer.BodyPartType.Body:
                    return _bodyMats ?? _colorsMats;
                case CustomizationPlayer.BodyPartType.Horns:
                    return _skinMats ?? _colorsMats;
                default:
                    return _colorsMats;
            }
        }
        
        private List<Material> GetMatsForCurrentBodyPart()
        {
            return GetMaterialsForBodyPart(_currentBodyPart);
        }

        #endregion

        #region Fields
        [SerializeField] private Button _colorButton1, _colorButton2, _colorButton3, _colorButton4;
        [SerializeField] private List<Material> _colorsMats, _hairMats, _tailMats, _eyesMats, _bodyMats, _skinMats;
        [SerializeField] private CustomizationPlayer _characterPreview;

        private CustomizationPlayer.BodyPartType _currentBodyPart = CustomizationPlayer.BodyPartType.Hair;
        #endregion
    }
}