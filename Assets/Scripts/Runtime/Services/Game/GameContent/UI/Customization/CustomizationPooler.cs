using System.Collections.Generic;
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

            for (int i = 0; i < customItems.Length; i++)
            {
                var btn = _pool[i];
                btn.gameObject.SetActive(true);
                btn.ResetForPool();
                Sprite icon = (customItems[i].ItemIcon != null && i < customItems.Length) ? customItems[i].ItemIcon : null;
                
                if (customItems[i].ItemMesh != null)
                    btn.SetDataMesh(customItems[i].ItemMesh, icon, customItems[i].Locked, i);
                else
                    btn.SetDataPrefab(customItems[i].ItemPrefab, icon, customItems[i].Locked, i);
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
        
        public void PopulateMaterials(List<Material> materials, UnityAction<CustomizeButton> onSelected = null)
        {
            if (_customizeButtonPrefab == null || _contentArea == null || materials == null) return;

            for (int i = _pool.Count; i < materials.Count; i++)
                CreatePooledButton(false);

            for (int i = 0; i < materials.Count; i++)
            {
                var btn = _pool[i];
                btn.gameObject.SetActive(true);
                btn.ResetForPool();

                Sprite icon = null;
                btn.SetDataMat(materials[i], icon, false, i);

                if (onSelected != null)
                    btn.OnChangeSkin.AddListener(onSelected);
                else
                    btn.OnChangeSkin.RemoveAllListeners();
            }

            for (int i = materials.Count; i < _pool.Count; i++)
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