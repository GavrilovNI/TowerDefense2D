using Game.Enemies;
using System.Collections;
using UnityEngine;

namespace Game.EnemyEffects.Effects.Abstract
{
    public abstract class StartEndEnemyEffect : EnemyEffect
    {
        [SerializeField, Min(0f)]
        private float _durationInSeconds;

        public StartEndEnemyEffect(float durationInSeconds)
        {
            _durationInSeconds = durationInSeconds;
        }

        public sealed override IEnumerator Apply(Enemy enemy)
        {
            OnEffectStart(enemy);
            yield return new WaitForSeconds(_durationInSeconds);
            OnEffectEnd(enemy);
        }

        protected abstract void OnEffectStart(Enemy enemy);

        protected abstract void OnEffectEnd(Enemy enemy);

        public override void OnCancelled(Enemy enemy) => OnEffectEnd(enemy);
    }
}
