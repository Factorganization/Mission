using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Modals.PopupLevel
{
    public class PopupLevel : UIView
    {
        #region Functions

        public PopupLevel(VisualElement root)
        {
            _hideOnAwake = true;
            _isOverlay = true;
            
            Initialize(root);
        }
        
        protected override void RegisterButtonCallbacks()
        {
            _closeButton.RegisterCallback<ClickEvent>(Hide);
        }
        
        public void SetMissionDetails(string title, string sender, string description)
        {
            _titleLabel.text = title;
            _senderLabel.text = sender;
            _descLabel.text = description;
            _acceptButton.RegisterCallback<ClickEvent, LevelDataSO>((evt, levelData) => 
                AcceptMission(levelData), _levelData);
        }

        protected override void SetVisualElements()
        {
            _titleLabel = Root.Q<Label>("title");
            _senderLabel = Root.Q<Label>("sender");
            _descLabel = Root.Q<Label>("description");
            
            _acceptButton = Root.Q<Button>("accept-button");
            _closeButton = Root.Q<Button>("close-button");
        }

        public void SetLevelData(LevelDataSO levelData)
        {
            _levelData = levelData;
        }
        
        private void AcceptMission(LevelDataSO levelData)
        {
            // Change Scene to level scene
            Debug.Log("Accepted mission for level: " + levelData._mailLevel.LevelName);
        }
        
        public override void Dispose()
        {
            UnregisterButtonCallbacks();
        }

        private void UnregisterButtonCallbacks()
        {
            _closeButton.UnregisterCallback<ClickEvent>(Hide);
        }
        
        private void Hide(ClickEvent clickEvent)
        {
            Hide();
            Root.pickingMode = PickingMode.Ignore;
        }

        #endregion
        
        #region Fields

        private LevelDataSO _levelData;
        private Button _acceptButton;
        private Button _closeButton;
        private Label _titleLabel;
        private Label _senderLabel;
        private Label _descLabel;

        #endregion
    }
}