
namespace Runtime.Services.Game.GameContent.Actors.ActorControllers.States
{
    [Flags]
    public enum AIControllerState
    {
        None = 0,
        Start = 1,
        Idle = 2,
        Move = 4,
        Suspicious = 8,
        Chase = 16,
        Repair = 32,
        BBGrabbable = 64,
        Spotted = 128
    }
}


