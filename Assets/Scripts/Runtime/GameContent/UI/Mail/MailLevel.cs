using UnityEngine;

[CreateAssetMenu(fileName = "MailLevel", menuName = "Scriptable Objects/MailLevel")]
public class MailLevel : ScriptableObject
{
    #region Fields

    public string Subject;
    public string LevelName;
    public string Sender;
    public string Description;
    public string Objective;

    #endregion
}
