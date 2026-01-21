using Runtime.Service;
using Runtime.Services.Game.GameContent.Actors.ActorViews;
using Runtime.Services.Game.GameSystems;
using Runtime.Services.Scene;

namespace Runtime.Services.Analysis.AnalysisSystem;

public class CheatManager : MonoBehaviour
{
    #region methodes

    public async void LoadSceneGroup(string sceneGroup)
    {
        await ServiceLocator.Instance.Get<SceneService>().LoadSceneGroup(sceneGroup);
    }

    public void PlayerToPosition(Vector3 position)
    {
        if (GameManager.Instance is not null)
            GameManager.Instance.Player.PlayerModel.rb.position = position;
    }

    public void PlayerNoClip()
    {
        /*if (GameManager.Instance is not null)
            GameManager.Instance.Player.PlayerModel*/ //TODO le ptn de collider 
    }

    public void SetAi(bool active)
    {
        var ais = FindObjectsByType<AIStateMachine>(FindObjectsSortMode.None);

        foreach (var ai in ais)
            ai.gameObject.SetActive(active);
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
        
        /*foreach (var VARIABLE in MissionManager.Manager.)
        {
            
        }*/ //TODO choper les missions
    }

    #endregion
}