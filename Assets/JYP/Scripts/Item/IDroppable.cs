public interface IDroppable
{
    public DropState State { get; protected set; }
    
    void Drop();

    public enum DropState
    {
        None = -1,
        Drop,
        PickingUp,
    }
}