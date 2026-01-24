using Runtime.Service;
using Runtime.Services.Data;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.Customization
{
    [Serializable]
    public class ChangeSkin : UnityEvent<CustomizeButton> { }
    
    [Serializable]
    public class UnlockItemEvent : UnityEvent<CustomizeButton> { }

    public class CustomizeButton : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {

            if (_image != null && _customizeItem != null)
                _image.sprite = _customizeItem.ItemIcon;

            if (_lockImage != null)
                _lockImage.gameObject.SetActive(_locked);
        }

        public void OnSelected()
        {
            OnChangeSkin.Invoke(this);
        }

        public void SetDataMesh(CustomizeItem item, Mesh mesh, Sprite icon, bool locked, int index, int price)
        {
            _customizeItem = item;
            ItemIndex = index;
            _locked = locked;
            SelectedMesh = mesh;
            
            if (_price != null)
                _price.text = price.ToString();

            if (_image != null)
                _image.sprite = icon;

            if (_lockImage != null)
                _lockImage.gameObject.SetActive(_locked);
        }

        public void SetDataMat(Material mat, Sprite icon, bool locked, int index)
        {
            _customizeItem = null;
            ItemIndex = index;
            _locked = locked;
            SelectedMaterial = mat;

            if (_image != null)
                _image.sprite = icon;

            if (_lockImage != null)
                _lockImage.gameObject.SetActive(_locked);
        }

        public void UnlockItem()
        {
            if (ServiceLocator.Instance.Get<DataService>().DevilDollars < _customizeItem.ItemPrice)
            {
                // Play can't buy anim
                Debug.Log("Not enough Devil Dollars!");
                return;
            }
            
            _customizeItem.Locked = false;
            if (_lockImage != null)
                _lockImage.gameObject.SetActive(false);
            MainMenuUI.Instance.PurchaseContainer.Hide();
            ServiceLocator.Instance.Get<DataService>().DevilDollars -= _customizeItem.ItemPrice;
        }

        public void ResetForPool()
        {
            OnChangeSkin.RemoveAllListeners();
            _customizeItem = null;
            _locked = false;
            if (_image != null) _image.sprite = null;
            if (_lockImage != null) _lockImage.gameObject.SetActive(false);
        }

        #endregion

        #region Fields

        [SerializeField] private Image _image;
        [SerializeField] private Image _lockImage;

        [SerializeField] private TextMeshProUGUI _price;
        
        [SerializeField] private CustomizeItem _customizeItem;
        [SerializeField] private bool _locked;

        public ChangeSkin OnChangeSkin = new ChangeSkin();

        public CustomizeItem CustomizeItem => _customizeItem;
        public Material SelectedMaterial { get; private set; }
        public Mesh SelectedMesh { get; private set; }
        public int ItemIndex { get; private set; }

        #endregion
    }
}