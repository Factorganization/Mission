using Runtime.Services.GameService.GameContent.UI.Mail;

namespace Runtime.Services.GameService.GameContent.UI;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "ScriptableObjects/LevelDataSO", order = 1)]
public class LevelDataSO : ScriptableObject
{
    public MailLevel _mailLevel;
    public MailLevel[] _allLevels;
}