using Game.Enemies;
using Game.EnemyEffects.Effects.Abstract;
using UnityEngine;

namespace Game.EnemyEffects.Effects
{
    [CreateAssetMenu(fileName = nameof(InstantDamageEffect), menuName = EnemyEffect.DefaultMenuPath + nameof(InstantDamageEffect))]

    public class InstantDamageEffect : InstantEnemyEffect
    {
        [SerializeField, Min(0)]
        private float _damage = 1;

        public override void ApplyInstantly(Enemy enemy)
        {
            enemy.Damage(_damage);
        }
    }
}
