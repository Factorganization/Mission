using Runtime.GameContent.Logics.LogicModels.MissionModels;
using UnityEngine;

namespace Runtime.Management.GameManagement
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
        }

        public void TryGetMission(MissionModel mission)
        {
            var i = FindMission(missions, mission);
            
            if (i == -1)
                return;

            _currentMissionsCount[i]--;
            if (_currentMissionsCount[i] == 0)
                Debug.Log($"{missions[i].mission} done !");
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

        #endregion

        #region fields

        [SerializeField] private MissionModel[] missions;

        private int[] _currentMissionsCount;

        #endregion
    }
}