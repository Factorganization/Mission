using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.GameContent.UI.Customization
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
        
        public void Populate(List<CustomizeItem> items, UnityAction<CustomizeButton> onSelected = null)
        {
            if (_customizeButtonPrefab == null || _contentArea == null) return;
            
            for (int i = _pool.Count; i < items.Count; i++)
                CreatePooledButton(false);
            
            for (int i = 0; i < items.Count; i++)
            {
                Debug.Log("Populating item: " + items[i].ItemName);
                var btn = _pool[i];
                btn.gameObject.SetActive(true);
                btn.ResetForPool();
                btn.SetData(items[i], items[i].Locked);
                if (onSelected != null)
                    btn.OnChangeSkin.AddListener(onSelected);
                else
                    btn.OnChangeSkin.RemoveAllListeners();
            }
            
            for (int i = items.Count; i < _pool.Count; i++)
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