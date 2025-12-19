namespace Runtime.Services.Game.GameContent.Logics.LogicModels.ElementModels
{
    /// <summary>
    /// BitMask element descriptor that can be held by objects
    /// </summary>
    [Flags]
    public enum ElementFlag
    {
        CanBeWet = 1,
        CanBurn = 2,
        CanConduct = 4,
        CanExplode = 8
    }
}