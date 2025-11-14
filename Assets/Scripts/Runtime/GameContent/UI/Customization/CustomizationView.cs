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
            
        }

        private void UpdateShop(Category category)
        {
            
        }

        #endregion

        #region Fields

        [SerializeField] private Button _headButton, _tailButton, _eyesButton, _bodyButton, _hornsButton;
        [SerializeField] private GameObject _contentArea; 
        [SerializeField] private GameObject _customizeButtonPrefab;
        
        #endregion

    }
}