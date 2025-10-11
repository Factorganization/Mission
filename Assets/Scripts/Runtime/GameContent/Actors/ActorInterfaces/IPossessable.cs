namespace Runtime.GameContent.Actors.ActorInterfaces
{
    public interface IPossessable
    {
        public bool Possessed { get; set; }
        
        public void Action();
    }
}