using UnityEditor;
using UnityEngine;

namespace Runtime.GameContent.UI.Customization
{
    [CreateAssetMenu(fileName = "CustomizeItem", menuName = "Scriptable Objects/CustomizeItem")]
    public class CustomizeItem : ScriptableObject
    {
        #region Fields

        public string ItemName;
        public Sprite ItemIcon;
        public Mesh ItemMesh;
        public CustomizationPlayer.BodyPartType ItemBodyPart;
        public bool Locked;

        #endregion
    }
}
