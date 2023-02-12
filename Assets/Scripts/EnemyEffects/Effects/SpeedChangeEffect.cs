using Game.Enemies;
using Game.EnemyEffects.Effects.Abstract;
using Game.Paths;
using UnityEngine;

namespace Game.EnemyEffects.Effects
{
    [CreateAssetMenu(fileName = nameof(SpeedChangeEffect), menuName = EnemyEffect.DefaultMenuPath + nameof(SpeedChangeEffect))]

    public class SpeedChangeEffect : StartEndEnemyEffect
    {
        [SerializeField, Min(0.001f)]
        private float _speedChangeRatio;

        public SpeedChangeEffect() : this(1, 1)
        {

        }

        public SpeedChangeEffect(float durationInSeconds, float speedChangeRatio) : base(durationInSeconds)
        {
            _speedChangeRatio = speedChangeRatio;
        }

        protected override void OnEffectStart(Enemy enemy)
        {
            PathFollower pathFollower = enemy.GetComponent<PathFollower>();
            if(pathFollower != null)
                pathFollower.Speed *= _speedChangeRatio;
        }

        protected override void OnEffectEnd(Enemy enemy)
        {
            PathFollower pathFollower = enemy.GetComponent<PathFollower>();
            if(pathFollower != null)
                pathFollower.Speed /= _speedChangeRatio;
        }
    }
}
