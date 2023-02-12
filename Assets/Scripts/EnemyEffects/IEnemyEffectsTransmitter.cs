using System.Collections.Generic;

namespace Game.EnemyEffects.Effects.Abstract
{
    public interface IEnemyEffectsTransmitter<T> where T : EnemyEffect
    {
        void AddEffect(T effect);
        void AddEffects(IEnumerable<T> effect);
    }
}
