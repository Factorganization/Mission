namespace Runtime.Services.Game.GameContent.UI.Customization
{
    [CreateAssetMenu(fileName = "CustomizeItem", menuName = "Scriptable Objects/CustomizeItem")]
    public class CustomizeItem : ScriptableObject
    {
        #region Fields

        public string ItemName;
        public Sprite ItemIcon;
        public Mesh ItemMesh;
        public int ItemPrice;
        public Material ItemMaterial;
        public bool Locked;

        #endregion
    }
}
