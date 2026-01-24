
namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States
{
    [Flags]
    public enum AIControllerState
    {
        None = 0,
        Idle = 1,
        Move = 2,
        Suspicious = 4,
        Chase = 8,
        Repair = 16,
        BBGrabbable = 32, 
        Spotted = 64
    }
}


