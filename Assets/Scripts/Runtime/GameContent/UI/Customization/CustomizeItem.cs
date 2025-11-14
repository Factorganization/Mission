using UnityEngine;

namespace Runtime.GameContent.UI.Customization
{
    public enum Category
    {
        Head,
        Body,
        Eyes,
        Horns,
        Tail
    }
    
    [CreateAssetMenu(fileName = "CustomizeItem", menuName = "Scriptable Objects/CustomizeItem")]
    public class CustomizeItem : ScriptableObject
    {
        #region Fields

        public string ItemName;
        public Sprite ItemIcon;
        public GameObject ItemPrefab;
        public Category ItemCategory;

        #endregion
    }
}
