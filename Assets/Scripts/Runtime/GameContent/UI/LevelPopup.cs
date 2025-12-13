using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Runtime.GameContent.UI
{
    public class LevelPopup : UIParent
    {
        #region Functions

        public void Bind(LevelDataSO mailLevel)
        {
            _levelName.text = mailLevel._mailLevel.LevelName;
            _levelSender.text = "From : " + mailLevel._mailLevel.Sender;
            _levelDescription.text = mailLevel._mailLevel.Description;
            _levelData = mailLevel._mailLevel;
            _objectiveText.text = "Objective: " + mailLevel._mailLevel.Objective;
            _acceptButton.onClick.AddListener(OpenLevel);
            _closeButton.onClick.AddListener(Hide);
        }

        private void OpenLevel()
        {
            SceneManager.LoadScene(_levelData.LevelName, LoadSceneMode.Single);
        }

        public override void Hide()
        {
            base.Hide();
            _levelName.text = "";
            _levelSender.text = "";
            _levelDescription.text = "";
            _levelData = null;
            _objectiveText.text = "";
            _acceptButton.onClick.RemoveListener(OpenLevel);
            _closeButton.onClick.RemoveListener(Hide);
        }
        
        #endregion
        
        #region Fields
        
        private MailLevel _levelData;
        [SerializeField] private TextMeshProUGUI _levelName, _levelSender, _levelDescription, _objectiveText;
        [SerializeField] private Button _acceptButton, _closeButton;

        #endregion
    }
}