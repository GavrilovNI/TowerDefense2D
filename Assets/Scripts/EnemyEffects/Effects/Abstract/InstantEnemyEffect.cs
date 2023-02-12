using Game.Enemies;
using System.Collections;

namespace Game.EnemyEffects.Effects.Abstract
{
    public abstract class InstantEnemyEffect : EnemyEffect
    {
        public sealed override IEnumerator Apply(Enemy enemy)
        {
            ApplyInstantly(enemy);
            yield break;
        }

        public abstract void ApplyInstantly(Enemy enemy);
    }
}
