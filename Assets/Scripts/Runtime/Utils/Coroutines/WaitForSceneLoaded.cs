using Runtime.Services.Scene;

namespace Runtime.Utils.Coroutines;

public class WaitForSceneLoaded : CustomYieldInstruction
{
    public override bool keepWaiting => !ServiceLocator.Instance.Get<SceneService>().Loaded;
}