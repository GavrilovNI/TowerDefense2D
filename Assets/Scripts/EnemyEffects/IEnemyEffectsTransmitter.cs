
namespace Game.EnemyEffects
{
    public interface IEnemyEffectsTransmitter<T> where T : IEnemyEffect
    {
        void AddEffect(T effect);
    }
}
