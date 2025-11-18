namespace Runtime.Utils.UI
{
    public interface IDraggeable
    {
        public virtual void OnBeginDrag() { }

        public virtual void OnDrag() { }

        public virtual void OnEndDrag() { }
    }
}   