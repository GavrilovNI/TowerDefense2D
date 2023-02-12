using Game.Enemies;
using Game.EnemyEffects.Effects.Abstract;
using UnityEngine;

namespace Game.EnemyEffects.Effects
{
    [CreateAssetMenu(fileName = nameof(DamageOverTimeEffect), menuName = EnemyEffect.DefaultMenuPath + nameof(DamageOverTimeEffect))]
    public class DamageOverTimeEffect : TickableEnemyEffect
    {
        [SerializeField, Min(0f)]
        private float _damagePerSecond;

        public DamageOverTimeEffect() : this(1, 1)
        {

        }

        public DamageOverTimeEffect(float durationInSeconds, float damagePerSecond) : base(durationInSeconds)
        {
            _damagePerSecond = damagePerSecond;
        }

        protected override void Tick(Enemy enemy, float elapsedTimeInSeconds)
        {
            float damage = _damagePerSecond * elapsedTimeInSeconds;
            enemy.Damage(damage);
        }
    }
}
