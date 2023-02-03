
namespace Game.DragDrop
{
    public interface IObjectGetter<T>
    {
        bool CanGet();
        T Get();
    }
}
