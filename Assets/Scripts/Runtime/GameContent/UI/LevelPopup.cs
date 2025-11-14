using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.GameContent.UI
{
    public class LevelPopup : UIParent
    {
        #region Functions

        public void Bind(LevelDataSO mailLevel)
        {
            _levelName.text = mailLevel._mailLevel.LevelName;
            _levelSender.text = mailLevel._mailLevel.Sender;
            _levelDescription.text = mailLevel._mailLevel.Description;
            _levelData = mailLevel._mailLevel;
            _acceptButton.onClick.AddListener(OpenLevel);
            _closeButton.onClick.AddListener(Hide);
        }

        private void OpenLevel()
        {
            // SceneManager.LoadScene(_levelData.SceneName);
            Debug.Log("Loading level: " + _levelData.LevelName);
        }

        public override void Hide()
        {
            base.Hide();
            _levelName.text = "";
            _levelSender.text = "";
            _levelDescription.text = "";
            _levelData = null;
            _acceptButton.onClick.RemoveListener(OpenLevel);
            _closeButton.onClick.RemoveListener(Hide);
        }
        
        #endregion
        
        #region Fields
        
        private MailLevel _levelData;
        [SerializeField] private TextMeshProUGUI _levelName, _levelSender, _levelDescription;
        [SerializeField] private Button _acceptButton, _closeButton;

        #endregion
    }
}
