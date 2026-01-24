namespace Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model
{
    [Flags]
    public enum ControllerState
    {
        None = 0,
        Start = 1,
        Idle = 2,
        Move = 4,
        Jump = 8,
        Fall = 16,
        Interact = 32,
        Possess = 64,
        Menu = 128,
        Locked = 256
    }
}