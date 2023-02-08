using Game.Enemies;
using System;
using UnityEngine;

namespace Game.EnemyEffects
{
    [CreateAssetMenu(fileName = nameof(DamageOverTimeEffect), menuName = "ScriptableObjects/EnemyEffects/" + nameof(DamageOverTimeEffect))]
    public class DamageOverTimeEffect : TemporaryEnemyEffect
    {
        [SerializeField, Min(0f)]
        private float _damagePerSecond = 1f;

        public DamageOverTimeEffect() : this(1, 1)
        {

        }

        public DamageOverTimeEffect(float durationInSeconds, float damagePerSecond) : base(durationInSeconds)
        {
            _damagePerSecond = damagePerSecond;
        }

        protected override void ApplyByElapsedTime(Enemy enemy, float elapsedTimeInSeconds)
        {
            float damage = _damagePerSecond * elapsedTimeInSeconds;
            enemy.Damage(damage);
        }
    }
}
