using Game.Enemies;
using System;
using System.Collections;
using UnityEngine;

namespace Game.EnemyEffects
{
    [SerializeField]
    public abstract class ContinuousEnemyEffect : EnemyEffect
    {
        public sealed override void Apply(Enemy enemy)
        {
            Apply(enemy, out _);
        }

        public void Apply(Enemy enemy, out Coroutine coroutine)
        {
            coroutine = enemy.StartCoroutine(ApplyContinuously(enemy));
        }

        public void ApplyPermantly(Enemy enemy, out Coroutine coroutine)
        {
            coroutine = enemy.StartCoroutine(ApplyPermantly(enemy));
        }

        protected abstract IEnumerator ApplyContinuously(Enemy enemy);

        private IEnumerator ApplyPermantly(Enemy enemy)
        {
            while(true)
            {
                int frameBeforeApply = Time.frameCount;
                yield return ApplyContinuously(enemy);
                if(frameBeforeApply == Time.frameCount)
                    yield return null;
            }
        }
    }
}
