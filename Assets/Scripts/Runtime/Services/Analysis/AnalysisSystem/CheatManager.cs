using Runtime.Service;
using Runtime.Services.Game.GameContent.Actors.ActorViews;
using Runtime.Services.Game.GameSystems;
using Runtime.Services.Scene;

namespace Runtime.Services.Analysis.AnalysisSystem
{
    public class CheatManager : MonoBehaviour
    {
        #region methodes

        public async void LoadSceneGroup()
        {
            var s = ServiceLocator.Instance.Get<SceneService>();
            if (_id >= s.Count)
                return;
            
            await s.LoadSceneGroup(_id);
        }

        public void SetId(string id)
        {
            if (int.TryParse(id, out var i))
                _id = i;
        }

        public void PlayerNoClip()
        {
            if (GameManager.Instance is not null)
                GameManager.Instance.Player.PlayerModel.col.enabled = !GameManager.Instance.Player.PlayerModel.col.enabled;
        }

        public void SetAi()
        {
            var ais = FindObjectsByType<AIStateMachine>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var ai in ais)
                ai.gameObject.SetActive(!ai.gameObject.activeSelf);
        }

        public void ResetAllElements()
        {
            if (LevelGenerator.Generator is null)
                return;

            foreach (var e in LevelGenerator.Generator.ElementHolders)
            {
                e.Flag3 = 0;
            }
        }

        public void ResetAllMissions()
        {
            if (LevelGenerator.Generator is null)
                return;

            foreach (var e in LevelGenerator.Generator.ElementHolders)
            {
                e.Flag3 = 0;
                for (var i = 0; i < e.MissionDone.Length; i++)
                {
                    e.MissionDone[i] = false;
                }
            }

            if (MissionManager.Manager is null)
                return;
        
            MissionManager.Manager.ResetMissions();
        }

        #endregion

        #region fields

        private int _id;

        #endregion
    }
}