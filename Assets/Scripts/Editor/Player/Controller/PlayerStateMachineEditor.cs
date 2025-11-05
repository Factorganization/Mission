using Runtime.GameContent.Player.Controller.LocalMachine.View;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.Player.Controller
{
    [CustomEditor(typeof(PlayerStateMachine))]
    public class PlayerStateMachineEditor : UnityEditor.Editor
    {
        #region methodes

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            visualTree.CloneTree(root);

            return root;
        }

        #endregion

        #region fields

        public VisualTreeAsset visualTree;

        #endregion
    }
}