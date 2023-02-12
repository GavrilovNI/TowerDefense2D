using Game.Enemies;
using Game.EnemyEffects.Effects.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.EnemyEffects
{
    public class EffectsApplier
    {
        public readonly Enemy Enemy;

        private readonly Dictionary<Coroutine, EnemyEffect> _runningEffects = new();

        public EffectsApplier(Enemy enemy)
        {
            Enemy = enemy;
        }

        protected bool IsEffectRunning(Coroutine effectCoroutine)
        {
            return _runningEffects.ContainsKey(effectCoroutine);
        }

        public Canceler AddEffect(EnemyEffect effect)
        {
            return StartEffect(effect, ApplyEffect);

            IEnumerator ApplyEffect(EnemyEffect effect, Func<Coroutine> coroutineGetter)
            {
                yield return effect.Apply(Enemy);
                OnEffectFinished(coroutineGetter());
            }
        }

        public Canceler AddEffectPermanently(EnemyEffect effect)
        {
            return StartEffect(effect, ApplyEffectPermanently);

            IEnumerator ApplyEffectPermanently(EnemyEffect effect, Func<Coroutine> coroutineGetter)
            {
                while (true)
                {
                    int frameBeforeApply = Time.frameCount;
                    yield return effect.Apply(Enemy);
                    if(frameBeforeApply == Time.frameCount)
                        yield return null;
                }
            }
        }

        private Canceler StartEffect(EnemyEffect effect, Func<EnemyEffect, Func<Coroutine>, IEnumerator> effectRoutineGetter)
        {
            Canceler canceler = new(this);
            Coroutine coroutine = null;
            coroutine = Enemy.StartCoroutine(effectRoutineGetter(effect, () => coroutine));
            canceler.InitCoroutine(coroutine);

            _runningEffects.Add(coroutine, effect);
            return canceler;
        }

        protected void OnEffectFinished(Coroutine effectCoroutine)
        {
            _runningEffects.Remove(effectCoroutine);
        }

        protected void CancelEffect(Coroutine effectCoroutine)
        {
            Enemy.StopCoroutine(effectCoroutine);
            var effect = _runningEffects[effectCoroutine];
            effect.OnCancelled(Enemy);
            _runningEffects.Remove(effectCoroutine);
        }


        public class Canceler
        {
            public static readonly Canceler CanceledOnStart = new(null, null, true);

            public bool IsCanceled => _canceledManualy || _effectsApplier.IsEffectRunning(_coroutine) == false;
            public Coroutine Coroutine => _coroutine;


            private bool _canceledManualy;
            private EffectsApplier _effectsApplier;
            private Coroutine _coroutine;

            private Canceler(EffectsApplier effectsApplier, Coroutine coroutine, bool canceled)
            {
                _effectsApplier = effectsApplier;
                _coroutine = coroutine;
                _canceledManualy = canceled;
            }

            public Canceler(EffectsApplier effectsApplier, Coroutine coroutine) : this(effectsApplier, coroutine, false)
            {
            }

            public Canceler(EffectsApplier effectsApplier) : this(effectsApplier, null, false)
            {
            }

            public void InitCoroutine(Coroutine coroutine)
            {
                if(_canceledManualy || _coroutine != null)
                    throw new InvalidOperationException("Effect was canceled or coroutine was initialized");

                _coroutine = coroutine;
            }

            public void Cancel()
            {
                if(IsCanceled)
                    throw new InvalidOperationException("Already canceled");
                if(_coroutine == null)
                    throw new InvalidOperationException("Coroutine wasn't initialized");

                _canceledManualy = true;
                _effectsApplier.CancelEffect(_coroutine);

                _effectsApplier = null;
                _coroutine = null;
            }
        }
    }
}
