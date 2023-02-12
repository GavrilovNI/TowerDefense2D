using System.Collections.Generic;

namespace Game.EnemyEffects
{
    public interface IEnemyEffectsTransmitter<T> where T : EnemyEffect
    {
        void AddEffect(T effect);
        void AddEffects(IEnumerable<T> effect);
    }
}
