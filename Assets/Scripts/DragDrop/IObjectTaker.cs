
namespace Game.DragDrop
{
    public interface IObjectTaker<T>
    {
        bool CanTake(T obj);
        void Take(T obj);
    }
}
