using Game.Enemies;
using Game.EnemyEffects.Effects.Abstract;
using Game.Extensions.Unity;
using System.Collections.Generic;
using UnityEngine;
using static Game.EnemyEffects.EffectsApplier;

namespace Game.Turrets.Weapons
{
    public class TargettedEffectTransmitter : WeaponWithEffects<EnemyEffect>
    {
        [SerializeField]
        protected Enemy? Target;

        private readonly List<Canceler> _appliedEffectCancelers = new();

        public void SetTarget(Enemy? target)
        {
            if(System.Object.ReferenceEquals(Target, target))
                return;

            OnUpdatingTarget();

            Target = target;

            if(Target.IsNull())
                Target = null;

            OnTargetUpdated();
        }

        protected override void ApplyEffects(Enemy target)
        {
            var effectsApplier = target.EffectsApplier;
            foreach(var effect in Effects)
            {
                var canceler = effectsApplier.AddEffectPermanently(effect);
                _appliedEffectCancelers.Add(canceler);
            }
        }

        protected virtual void StopAllEffects()
        {
            foreach(var effectCanceler in _appliedEffectCancelers)
                effectCanceler.Cancel();

            _appliedEffectCancelers.Clear();
        }

        protected virtual void OnUpdatingTarget()
        {
            StopAllEffects();
        }

        protected virtual void OnTargetUpdated()
        {
            if(Target != null)
                ApplyEffects(Target);
        }

        private void Start()
        {
            SetTarget(Target);
        }

        private void OnDestroy()
        {
            SetTarget(null);
        }
    }
}
