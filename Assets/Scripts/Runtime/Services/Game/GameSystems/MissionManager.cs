using System.Collections.Generic;
using Runtime.Services.Game.GameContent.Actors.ActorViews;
using Runtime.Services.Game.GameContent.Logics.LogicModels.MissionModels;
using TMPro;

namespace Runtime.Services.Game.GameSystems
{
    public class MissionManager : MonoBehaviour
    {
        #region properties

        public static MissionManager Manager { get; private set; }

        public int TempDD => _tempDD;

        #endregion

        #region methodes

		#region unity events

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

				if (missions[i].mission is MissionType.ElementPresence)
					_presenceMissions.Add(i, missions[i]);
			}
			SetText();
            _tempDD = 0;
        }

		private void Update()
        {
			_missionTimer += Time.deltaTime;

			if (_missionTimer > 0.5f)
				return;

			_missionTimer = 0;

			foreach (var m in _presenceMissions)
			{
				if (_currentMissionsCount[m.Key] == 0)
					continue;

                _currentMissionsCount[m.Key] = m.Value.number;
				var i = 0;

				foreach (var e in LevelGenerator.Generator.ElementHolders)
				{
					if (m.Value == new MissionModel(MissionType.ElementPresence, e.ObjectType, e.Flag3, e.RoomType))
						i++;
				}

				_currentMissionsCount[m.Key] -= i;
				if (_currentMissionsCount[m.Key] < 0)
					_currentMissionsCount[m.Key] = 0;
			}

			SetText();
			CheckEndGame();
		}

		#endregion

		#region missions callbacks

        public void ResetMissions()
        {
            for (var i = 0; i < missions.Length; i++)
            {
                _currentMissionsCount[i] = missions[i].number;
            }
        }

        public void WinAllMissions()
        {
            for (var i = 0; i < missions.Length; i++)
            {
                _currentMissionsCount[i] = 0;
            }
            CheckEndGame();
        }
        
        public bool TryGetAndSetMission(MissionModel mission)
        {
            var l = FindMission(missions, mission);

            if (l.Count == 0)
                return false;

			foreach (var i in l)
			{
				if (_currentMissionsCount[i] > 0)
					_currentMissionsCount[i]--;

                else
                {
                    _doneMissions++;
                    var t = GameManager.Instance.Timer.RemainingTime;

                    var dd = GetDD(_doneMissions);
                    var p = GetPercentage(t);

                    _tempDD += dd + (int)(dd * p);
                }
			}

			if (onBoardingMode)
                SetTextOnBoard();
            else
                SetText();
			CheckEndGame();
			return true;
        }

        public List<int> TryGetMissions(MissionModel mission)
        {
            return FindMission(missions, mission);
        }

        public bool TrySetMission(MissionModel mission, int number)
        {
            return false;
        }

        private static List<int> FindMission(MissionModel[] missions, MissionModel mission)
        {
			List<int> l = new();

            for (var i = 0; i < missions.Length; i++)
            {
                if (missions[i] == mission)
                    l.Add(i);
            }

            return l;
        }

        private void CheckEndGame()
        {
            foreach (var c in _currentMissionsCount)
            {
                if (c > 0)
                    return;
            }
            
            endGame.OnActivate();
        }

		private void SetTextOnBoard()
        {
            for (var i = 0; i < _currentMissionsCount.Length; i++)
            {
                var c = _currentMissionsCount[i];
                var t = missionTexts[i];
                
                
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
                
                /*if (m.mission is MissionType.Action)
                {
                    text.text += "Destroy ";
                    text.text += $"{m.number} ";
                    text.text += $"{Enum.GetName(typeof(ObjectType), m.objectType)!.Split('_')[^1]} ";
                    text.text += $"in the {Enum.GetName(typeof(RoomType), m.room)} ";
                    text.text += $": {m.number - _currentMissionsCount[i]}/{m.number}";
                }*/
                /*else if (m.mission is MissionType.ElementAffection)
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
                }*/
				/*else if (m.mission is MissionType.ElementPresence)
                {
                    text.text += "Have ";
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
                        s += "in explosion (still not a good sentence)";
                    }

                    text.text += $"{s} ";
                    text.text += $"in the {Enum.GetName(typeof(RoomType), m.room)} ";
                    text.text += $": {m.number - _currentMissionsCount[i]}/{m.number}";
                }*/
                text.text += $": {m.number - _currentMissionsCount[i]}/{m.number}";
                text.text += "</color>";
                text.text += "\n";
            }
        }

        private static int GetDD(int missions) => missions switch
        {
            1 => 100,
            2 => 200,
            3 => 200,
            4 => 300,
            5 => 250,
            6 => 300,
            7 => 250,
            8 => 400,
            9 => 450,
            10 => 500,
            > 10 => 500,
            _ => 0
        };

        private static float GetPercentage(float time) => time switch
        {
            > 300 => 1f,
            > 270 => 0.9f,
            > 240 => 0.8f,
            > 210 => 0.7f,
            > 180 => 0.6f,
            > 150 => 0.5f,
            > 120 => 0.4f,
            > 90 => 0.3f,
            > 60 => 0.2f,
            _ => 0,
        };
        
		#endregion

        #endregion

        #region fields

        [SerializeField] private EndGameView endGame;
        
        [SerializeField] private MissionModel[] missions;

        [SerializeField] private TMP_Text text;
        
        [SerializeField] private TMP_Text[] missionTexts;

        [SerializeField] private bool onBoardingMode;

		private readonly Dictionary<int, MissionModel> _presenceMissions = new();

        private int[] _currentMissionsCount;

		private float _missionTimer;

        private int _doneMissions;

        private int _tempDD;

        #endregion
    }
}