namespace Runtime.GameContent.Logics.LogicModels
{
    [System.Flags]
    public enum ElementFlag
    {
        CanWet = 1,
        CanBurn = 2,
        CanConduct = 4,
        CanExplode = 8
    }
}