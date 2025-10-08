namespace GameContent.Player.Controller.LocalMachine.Model
{
    public enum ControllerState : byte
    {
        Idle,
        Move,
        Fall,
        Interact,
        Possess,
        Menu,
        Locked
    }
}