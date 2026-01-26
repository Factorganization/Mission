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
            SelectedMesh = mesh;
            _locked = locked;
            
            if (_price != null)
                _price.text = price.ToString();

            if (_image != null)
                _image.sprite = icon;

            if (_lockImage != null)
                _lockImage.gameObject.SetActive(_locked);
        }

        public void SetDataMat(CustomizeItem item, Material mat, Sprite icon, bool locked, int index, int price)
        {
            _customizeItem = item;
            ItemIndex = index;
            SelectedMaterial = mat;
            _locked = locked;

            if (_price != null)
                _price.text = price.ToString();
            
            if (_image != null)
                _image.sprite = icon;

            if (_lockImage != null)
                _lockImage.gameObject.SetActive(_locked);
        }

        public void UnlockItem()
        {
            if (ServiceLocator.Instance.Get<DataService>().DevilDollars < _customizeItem.ItemPrice)
            {
                MainMenuUI.Instance.PurchaseContainer.PurchaseText.text = "Not enough Devil Dollars!";
                return;
            }
            
            _locked = false;
            
            if (_lockImage != null)
                _lockImage.gameObject.SetActive(false);
            
            var a = ServiceLocator.Instance.Get<DataService>();
            
            CustomizeItemData data = new CustomizeItemData
            {
                ItemName = _customizeItem.ItemName,
                Locked = false
            };
            
            MainMenuUI.Instance.PurchaseContainer.Hide();
            MainMenuUI.Instance.PurchaseContainer.ConfirmButton.onClick.RemoveAllListeners();
            a.SubtractMoney(_customizeItem.ItemPrice);
            a.AddPurchaseItem(data);
            a.UpdatePurchaseItem(_customizeItem.ItemName, MainMenuUI.Instance.PlayerPreview);
            MainMenuUI.Instance.UpdateDataInfo.UpdateData();
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