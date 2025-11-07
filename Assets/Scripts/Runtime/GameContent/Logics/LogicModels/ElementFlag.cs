namespace Runtime.GameContent.Logics.LogicModels
{
    /// <summary>
    /// BitMask element descriptor that can be held by objects
    /// </summary>
    [System.Flags]
    public enum ElementFlag
    {
        CanBeWet = 1,
        CanBurn = 2,
        CanConduct = 4,
        CanExplode = 8
    }
}