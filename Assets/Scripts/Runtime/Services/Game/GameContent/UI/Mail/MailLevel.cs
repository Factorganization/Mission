namespace Runtime.Services.Game.GameContent.UI.Mail
{
    [CreateAssetMenu(fileName = "MailLevel", menuName = "Scriptable Objects/MailLevel")]
    public class MailLevel : ScriptableObject
    {
        #region Fields

        public string Subject;
        public string LevelName;
        public string LevelSceneName;
        public string Sender;
        
        [TextArea]
        public string Description;
        
        public int ThresholdScore;
        public bool isMailUnlocked;
        public bool isMailNew;

        #endregion
    }
}