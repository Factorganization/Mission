using System.Collections.Generic;
using Runtime.Services.Data;
using UnityEngine.Events;

namespace Runtime.Services.Game.GameContent.UI.Customization
{
    public class CustomizationPooler : MonoBehaviour
    {
        #region Functions
        
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_contentArea == null || _customizeButtonPrefab == null) return;
            for (int i = 0; i < _initialPoolSize; i++)
                CreatePooledButton(false);
        }
        
        private CustomizeButton CreatePooledButton(bool active = false)
        {
            var go = Instantiate(_customizeButtonPrefab, _contentArea.transform);
            go.SetActive(active);
            var btn = go.GetComponent<CustomizeButton>();
            if (btn == null)
            {
                Debug.LogError("Prefab missing CustomizeButton component");
                Destroy(go);
                return null;
            }
            _pool.Add(btn);
            return btn;
        }
        
        public void Populate(CustomizeItem[] customItems, UnityAction<CustomizeButton> onSelected = null)
        {
            if (_customizeButtonPrefab == null || _contentArea == null || customItems == null) return;

            for (int i = _pool.Count; i < customItems.Length; i++)
                CreatePooledButton(false);
            
            var a = ServiceLocator.Instance.Get<DataService>();

            for (int i = 0; i < customItems.Length; i++)
            {
                var btn = _pool[i];
                btn.gameObject.SetActive(true);
                btn.ResetForPool();
                Sprite icon = (customItems[i].ItemIcon != null && i < customItems.Length) ? customItems[i].ItemIcon : null;
                
                if (customItems[i].ItemMesh != null)
                    btn.SetDataMesh(customItems[i], 
                                    customItems[i].ItemMesh, 
                                    icon, 
                                    a.PurchasedItems[a.PurchasedItems.FindIndex(x => x.ItemName == customItems[i].ItemName)].Locked, 
                                    i, 
                                    customItems[i].ItemPrice);

                if (onSelected != null)
                {
                    btn.OnChangeSkin.AddListener(onSelected);
                }
                else
                    btn.OnChangeSkin.RemoveAllListeners();
            }

            for (int i = customItems.Length; i < _pool.Count; i++)
            {
                var btn = _pool[i];
                btn.ResetForPool();
                btn.gameObject.SetActive(false);
            }
        }
        
        public void PopulateMaterials(CustomizeItem[] customItems, UnityAction<CustomizeButton> onSelected = null)
        {
            if (_customizeButtonPrefab == null || _contentArea == null || customItems == null) return;

            for (int i = _pool.Count; i < customItems.Length; i++)
                CreatePooledButton(false);
            
            var a = ServiceLocator.Instance.Get<DataService>();

            for (int i = 0; i < customItems.Length; i++)
            {
                var btn = _pool[i];
                btn.gameObject.SetActive(true);
                btn.ResetForPool();

                Sprite icon = (customItems[i].ItemIcon != null && i < customItems.Length) ? customItems[i].ItemIcon : null;
                
                btn.SetDataMat(customItems[i], 
                               customItems[i].ItemMaterial, 
                               icon, 
                               a.PurchasedItems[a.PurchasedItems.FindIndex(x => x.ItemName == customItems[i].ItemName)].Locked, 
                            i, 
                               customItems[i].ItemPrice);

                if (onSelected != null)
                    btn.OnChangeSkin.AddListener(onSelected);
                else
                    btn.OnChangeSkin.RemoveAllListeners();
            }

            for (int i = customItems.Length; i < _pool.Count; i++)
            {
                var btn = _pool[i];
                btn.ResetForPool();
                btn.gameObject.SetActive(false);
            }
        }

        public void ClearPool()
        {
            foreach (var b in _pool)
            {
                if (b != null)
                {
                    b.ResetForPool();
                    Destroy(b.gameObject);
                }
            }
            _pool.Clear();
        }

        #endregion
        
        #region Fields
        
        [SerializeField] private GameObject _customizeButtonPrefab;
        [SerializeField] private GameObject _contentArea;
        [SerializeField] private int _initialPoolSize = 20;
        
        private readonly List<CustomizeButton> _pool = new List<CustomizeButton>();
        
        #endregion
    }
}