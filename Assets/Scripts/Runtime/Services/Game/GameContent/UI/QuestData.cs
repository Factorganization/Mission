namespace Runtime.Services.GameService.GameContent.UI;

[CreateAssetMenu(fileName = "MissionDetails", menuName = "Scriptable Objects/MissionDetails")]
public class QuestData : ScriptableObject
{
    public int currentProgress;
    public int objective;
    public string description;
}