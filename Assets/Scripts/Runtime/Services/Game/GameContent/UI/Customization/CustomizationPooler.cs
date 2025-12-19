using System.Collections.Generic;
using UnityEngine.Events;

namespace Runtime.Services.GameService.GameContent.UI.Customization
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
        
        public void PopulatePrefab(CustomizeItem[] prefabs, UnityAction<CustomizeButton> onSelected = null)
        {
            if (_customizeButtonPrefab == null || _contentArea == null || prefabs == null) return;

            for (int i = _pool.Count; i < prefabs.Length; i++)
                CreatePooledButton(false);

            for (int i = 0; i < prefabs.Length; i++)
            {
                var btn = _pool[i];
                btn.gameObject.SetActive(true);
                btn.ResetForPool();
                Sprite icon = (prefabs[i].ItemIcon != null && i < prefabs.Length) ? prefabs[i].ItemIcon : null;
                btn.SetData(prefabs[i].ItemPrefab, icon, prefabs[i].Locked, i);
                if (onSelected != null)
                    btn.OnChangeSkin.AddListener(onSelected);
                else
                    btn.OnChangeSkin.RemoveAllListeners();
            }

            for (int i = prefabs.Length; i < _pool.Count; i++)
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