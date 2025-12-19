using Runtime.Services.GameService.GameContent.Logics.LogicModels.ElementModels;
using Runtime.Services.GameService.GameContent.Logics.LogicModels.MissionModels;
using TMPro;

namespace Runtime.Services.GameService.GameSystems
{
    public class MissionManager : MonoBehaviour
    {
        #region properties

        public static MissionManager Manager { get; private set; }

        #endregion

        #region methodes

        private void Awake()
        {
            if (Manager is not null)
                Debug.LogWarning("MissionManager already instantiated");
            
            Manager = this;
        }

        private void Start()
        {
            _currentMissionsCount = new int[missions.Length];
            for (var i = 0; i < missions.Length; i++)
            {
                _currentMissionsCount[i] = missions[i].number;
            }
            SetText();
        }

        public void TryGetMission(MissionModel mission)
        {
            var i = FindMission(missions, mission);

            if (i == -1)
                return;

            if (_currentMissionsCount[i] > 0)
                _currentMissionsCount[i]--;

            SetText();
            CheckEndGame();
        }

        private static int FindMission(MissionModel[] missions, MissionModel mission)
        {
            for (var i = 0; i < missions.Length; i++)
            {
                if (missions[i] == mission)
                    return i;
            }
            return -1;
        }

        private void CheckEndGame()
        {
            foreach (var c in _currentMissionsCount)
            {
                if (c > 0)
                    return;
            }
        }

        private void SetText()
        {
            text.text = "";
            for (var i = 0; i < missions.Length; i++)
            {
                var m = missions[i];
                if (m.mission is MissionType.None)
                    continue;
                
                if (_currentMissionsCount[i] > 0)
                    text.text += "<color=red>";
                else
                    text.text += "<color=green>";
                
                if (m.mission is MissionType.Action)
                {
                    text.text += "Destroy ";
                    text.text += $"{m.number} ";
                    text.text += $"{Enum.GetName(typeof(ObjectType), m.objectType)!.Split('_')[^1]} ";
                    text.text += $"in the {Enum.GetName(typeof(RoomType), m.room)} ";
                    text.text += $": {m.number - _currentMissionsCount[i]}/{m.number}";
                }
                else if (m.mission is MissionType.ElementAffection)
                {
                    text.text += "Set ";
                    text.text += $"{m.number} ";
                    text.text += $"{Enum.GetName(typeof(ObjectType), m.objectType)!.Split('_')[^1]} ";
                    var s = "";
                    if ((m.toApply & ElementFlag.CanBeWet) != 0)
                        s += "under water";
                    if ((m.toApply & ElementFlag.CanBurn) != 0)
                    {
                        if (s != "")
                            s += " or ";
                        s += "under fire";
                    }
                    if ((m.toApply & ElementFlag.CanConduct) != 0)
                    {
                        if (s != "")
                            s += " or ";
                        s += "in electricity";
                    }

                    if ((m.toApply & ElementFlag.CanExplode) != 0)
                    {
                        if (s != "")
                            s += " or ";
                        s += "in explosion (wtf is this sentence)";
                    }

                    text.text += $"{s} ";
                    text.text += $"in the {Enum.GetName(typeof(RoomType), m.room)} ";
                    text.text += $": {m.number - _currentMissionsCount[i]}/{m.number}";
                }
                
                text.text += "</color>";
                text.text += "\n";
            }
        }

        #endregion

        #region fields

        [SerializeField] private MissionModel[] missions;
        
        [SerializeField] private TMP_Text text;

        private int[] _currentMissionsCount;

        #endregion
    }
}