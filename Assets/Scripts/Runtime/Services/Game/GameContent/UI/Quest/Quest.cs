using TMPro;

namespace Runtime.Services.Game.GameContent.UI.Quest
{
    public class Quest : MonoBehaviour
    {
        #region Functions

        public void Bind(QuestData questData)
        {
            _quest = questData;
            _questDescription.text = ". " + _quest.description + " " +_quest.currentProgress + "/" + _quest.objective;
        }

        public void UpdateQuest()
        {
            if (_quest != null) 
                _questDescription.text = _quest.description + _quest.currentProgress + "/" + _quest.objective;
        } 

        #endregion

        #region Fields

        [SerializeField] private QuestData _quest;
        [SerializeField] private TextMeshProUGUI _questDescription;
        
        #endregion
    }
}