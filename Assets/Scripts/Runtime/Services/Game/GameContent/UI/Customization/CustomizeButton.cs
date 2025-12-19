using UnityEngine.Events;
using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.Customization;

[Serializable]
public class ChangeSkin : UnityEvent<CustomizeButton> { }

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
        if (_locked) return;
        OnChangeSkin.Invoke(this);
    }
        
    public void SetData(GameObject prefab, Sprite icon, bool locked, int index)
    {
        _customizeItem = null;
        SelectedPrefab = prefab;
        ItemIndex = index;
        _locked = locked;

        if (_image != null)
            _image.sprite = icon;

        if (_lockImage != null)
            _lockImage.gameObject.SetActive(_locked);
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
        
    [SerializeField] private CustomizeItem _customizeItem;
    [SerializeField] private bool _locked;
        
    public ChangeSkin OnChangeSkin = new ChangeSkin();

    public GameObject SelectedPrefab { get; private set; }
    public int ItemIndex { get; private set; }

    #endregion
}