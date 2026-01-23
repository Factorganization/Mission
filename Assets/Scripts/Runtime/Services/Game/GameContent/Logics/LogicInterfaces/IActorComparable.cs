namespace Runtime.Services.Game.GameContent.Logics.LogicInterfaces;

public interface IActorComparable
{
    public int Id { get; }
    
    public void SetId();
}