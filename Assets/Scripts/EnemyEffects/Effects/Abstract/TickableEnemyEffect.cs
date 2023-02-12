using Game.Enemies;
using System.Collections;
using UnityEngine;

namespace Game.EnemyEffects.Effects.Abstract
{
    public abstract class TickableEnemyEffect : EnemyEffect
    {
        [SerializeField, Min(0f)]
        private float _durationInSeconds;

        public TickableEnemyEffect(float durationInSeconds)
        {
            _durationInSeconds = durationInSeconds;
        }

        public sealed override IEnumerator Apply(Enemy enemy)
        {
            float secondsLeft = _durationInSeconds;

            if(secondsLeft == 0)
                yield break;

            while(secondsLeft > 0)
            {
                float elapsedTimeInSeconds = Mathf.Min(secondsLeft, Time.deltaTime);
                secondsLeft -= elapsedTimeInSeconds;
                Tick(enemy, elapsedTimeInSeconds);
                yield return null;
            }
        }

        protected abstract void Tick(Enemy enemy, float elapsedTimeInSeconds);

    }
}
