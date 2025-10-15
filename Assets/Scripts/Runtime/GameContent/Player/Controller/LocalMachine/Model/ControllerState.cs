namespace Runtime.GameContent.Player.Controller.LocalMachine.Model
{
    [System.Flags]
    public enum ControllerState
    {
        None = 0,
        Idle = 1,
        Move = 2,
        Jump = 4,
        Fall = 8,
        Interact = 16,
        Possess = 32,
        Menu = 64,
        Locked = 128
    }
}