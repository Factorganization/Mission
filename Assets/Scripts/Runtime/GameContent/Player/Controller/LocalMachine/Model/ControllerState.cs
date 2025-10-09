namespace Runtime.GameContent.Player.Controller.LocalMachine.Model
{
    public enum ControllerState : byte
    {
        Idle,
        Move,
        Jump,
        Fall,
        Interact,
        Possess,
        Menu,
        Locked
    }
}