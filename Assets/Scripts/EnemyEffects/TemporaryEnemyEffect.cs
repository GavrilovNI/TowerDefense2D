using Game.Enemies;
using System;
using System.Collections;
using UnityEngine;

namespace Game.EnemyEffects
{
    public abstract class TemporaryEnemyEffect : ContinuousEnemyEffect
    {
        [SerializeField, Min(0f)]
        private float _durationInSeconds = 1f;

        public TemporaryEnemyEffect(float durationInSeconds)
        {
            _durationInSeconds = durationInSeconds;
        }

        protected sealed override IEnumerator ApplyContinuously(Enemy enemy)
        {
            float secondsLeft = _durationInSeconds;

            while(secondsLeft > 0)
            {
                float elapsedTimeInSeconds = Mathf.Min(secondsLeft, Time.deltaTime);
                secondsLeft -= elapsedTimeInSeconds;
                ApplyByElapsedTime(enemy, elapsedTimeInSeconds);
                yield return null;
            }
        }

        protected abstract void ApplyByElapsedTime(Enemy enemy, float elapsedTimeInSeconds);
    }
}
