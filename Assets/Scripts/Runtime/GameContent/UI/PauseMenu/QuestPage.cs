using System.Collections.Generic;

namespace Runtime.GameContent.UI
{
    public class QuestPage : UIParent
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var questData in _questList)
            {
                var quest = Instantiate(_questPrefab);
                quest.transform.SetParent(_questDescription.transform, false);
                
                var questComponent = quest.GetComponent<Quest.Quest>();
                questComponent.Bind(questData);
                
                _quests.Add(questComponent);
            }
        }

        public override void Hide()
        {
            _quests.Clear();
        }

        #endregion

        #region Fields
        
        // Temporary quest data list for testing
        [SerializeField] private List<QuestData> _questList;
        
        [SerializeField] private GameObject _questPrefab;
        [SerializeField] private GameObject _questDescription;
        
        public List<Quest.Quest> _quests = new List<Quest.Quest>();
        
        #endregion
    }
}