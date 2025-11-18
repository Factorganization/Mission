using Runtime.Utils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.GameContent.UI.Customization
{
    public class CustomizationPage : UIParent, IDraggeable
    {
       #region Functions

       private void Start()
       {
           Initialize();
       }

       private void Initialize()
       {
           _closeButton.onClick.AddListener(() => Hide());
       }
       
       public override void Show() 
       {
           base.Show();
           //_animator.Play("OpenCustomization"); 
       }
       
       #endregion
       
       #region Fields
       
       [SerializeField] private GameObject _characterPreview;
       [SerializeField] private Button _closeButton;
       [SerializeField] private Animation _animator;
       
       #endregion
    }
}